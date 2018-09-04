using BudgetCapture.BLL;
using BudgetCapture.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BudgetCapture
{
    public partial class DirectBudgetPg : System.Web.UI.Page
    {
        private static AppUser usr = null; private static BudgetYear budYr = null;
        private string AdminRole = ConfigurationManager.AppSettings["adminRole"].ToString();
        private string deptIniRole = ConfigurationManager.AppSettings["DeptIniRole"].ToString();
        private string deptApprRole = ConfigurationManager.AppSettings["DeptApprverRole"].ToString();
        private string PBMgrRole = ConfigurationManager.AppSettings["PBMgrRole"].ToString();
        private string PBOffRole = ConfigurationManager.AppSettings["PBOffRole"].ToString();
        private string EDRole = ConfigurationManager.AppSettings["EDRole"].ToString();
        private string MDRole = ConfigurationManager.AppSettings["MDRole"].ToString();
        private string budgetType = ConfigurationManager.AppSettings["Direct"].ToString();
        decimal priceTotal = 0;
        int quantityTotal = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            error.Visible = false; success.Visible = false;
           // dvAdd.Visible = false; 
            dvAppr.Visible = false;
            try
            {
                if (Session["user"] != null)
                {
                    usr = (AppUser)Session["user"];
                    budYr = (BudgetYear)Session["budgetYr"];
                    List<TrackApproval> getTrackApproval = null;
                    getTrackApproval = LookUpBLL.GetTrackApproval(usr.DepartmentID.Value, budYr.ID).ToList();
                    if (User.IsInRole(deptApprRole) && budYr.IsActive == true)
                    {
                        if (getTrackApproval != null && getTrackApproval.Count() > 0)//if selected department has been pushed for approval
                        {

                            dvAppr.Visible = false;
                            if (getTrackApproval.FirstOrDefault().status == (int)Utility.BudgetItemStatus.MD_Approved)
                            {
                                dvATC.Visible = true;
                            }
                            if (getTrackApproval.FirstOrDefault().status == (int)Utility.BudgetItemStatus.Returned_For_Correction)
                            {
                                dvAppr.Visible = true;
                            }
                        }
                        else
                        {
                            dvAppr.Visible = true;
                        }
                    }
                    if (User.IsInRole(deptIniRole) && budYr.IsActive == true)
                    {
                        if (getTrackApproval != null && getTrackApproval.Count() > 0)//if selected department has been pushed for approval
                        {
                            dvAdd.Visible = false;
                            if (getTrackApproval.FirstOrDefault().status == (int)Utility.BudgetItemStatus.Returned_For_Correction)
                            {
                                dvAdd.Visible = true;
                            }
                        }
                        else
                        {
                            dvAdd.Visible = true;
                        }
                    }
                    //if (User.IsInRole(AdminRole) || User.IsInRole(PBMgrRole) || User.IsInRole(PBOffRole) && budYr.IsActive == true)
                    //{
                    //    if (getTrackApproval != null && getTrackApproval.Count() > 0)//if selected department has been pushed for approval
                    //    {
                    //        if (getTrackApproval.FirstOrDefault().status == (int)Utility.BudgetItemStatus.Pending_ReAlignment)
                    //        {
                    //            dvAdd.Visible = true;
                    //        }
                    //    }
                        

                    //}

                }
                else
                {
                    Response.Redirect("Login.aspx", false);
                    return;
                }

                if (!IsPostBack)
                {
                    //Utility.BindMonth(ddlMonth);
                    Utility.BindDirectType(ddlCat);
                    Utility.BindDirectType(ddlExpTyp);
                    Utility.BindDept(ddlDeptFilter);
                    lbDept.Text = usr.Department.Name.ToUpper();
                    lbyr.Text = budYr.Year;
                  
                    if (User.IsInRole(AdminRole)||User.IsInRole(PBMgrRole) || User.IsInRole(PBOffRole))
                    {
                        ddlDeptFilter.Visible = true;
                    }
                    if (Request.QueryString["deptId"] != null && Request.QueryString["batchId"] != null && Request.QueryString["Id"] != null)
                    {
                        BindGridATC();
                        ddlDeptFilter.Visible = false;
                        dvAdd.Visible = false; dvAppr.Visible = false; dvAdmin.Visible = false; dvATC.Visible = false;
                        dvFilterHeader.Visible = false; gvDept.Visible = false; gvATC.Visible = true;
                        int headerId = int.Parse(Request.QueryString["Id"].ToString());
                        ATCRequestHeader atcRec = CommonBLL.GetATCHeader(headerId);
                        if (atcRec.Status == (int)Utility.ATCStatus.Pending_PBManager_Approval && User.IsInRole(PBMgrRole))
                        {
                            dvATCApproval.Visible = true;
                            btnATCForward.Visible = true;
                            btnATCReject.Visible = true;
                            btnATCMDApproval.Visible = false;
                        }
                        if (atcRec.Status == (int)Utility.ATCStatus.Pending_MD_Approval && User.IsInRole(MDRole))
                        {
                            dvATCApproval.Visible = true;
                            btnATCMDApproval.Visible = true;
                            btnATCReject.Visible = true;
                            btnATCForward.Visible = false;
                        }
                        if (atcRec.Status == (int)Utility.ATCStatus.MD_Approved || atcRec.Status == (int)Utility.ATCStatus.Declined)
                        {
                            dvATCApproval.Visible = false;
                        }
                    }
                    else
                    {
                        BindGrid();
                    }
                   
                }
            }
            catch (Exception ex)
            {
                error.Visible = true;
                error.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button> An error occurred. kindly try again!!!";
                Utility.WriteError("Error: " + ex.Message);
            }
        }
        private void BindGrid()
        {
            usr = (AppUser)Session["user"]; budYr = (BudgetYear)Session["budgetYr"];
            if (User.IsInRole(AdminRole) || User.IsInRole(PBMgrRole) || User.IsInRole(PBOffRole))
            {
                if (ddlDeptFilter.SelectedValue != "")
                {
                    int dep = int.Parse(ddlDeptFilter.SelectedValue);
                    gvDept.DataSource = CommonBLL.GetDirectBudgetList(budYr.ID,dep);
                    gvDept.DataBind();
                }
                else
                {
                    gvDept.DataSource = CommonBLL.GetDirectBudgetList(budYr.ID);
                    gvDept.DataBind();
                }
            }
            else if (User.IsInRole(EDRole) || User.IsInRole(MDRole))
            {
                if (Request.QueryString["deptId"] != null)
                {
                    int deptId = int.Parse(Request.QueryString["deptId"].ToString());
                    gvDept.DataSource = CommonBLL.GetDirectBudgetList(budYr.ID,deptId);
                    gvDept.DataBind();
                }
            }
            else
            {
                gvDept.DataSource = CommonBLL.GetDirectBudgetList(budYr.ID, usr.DepartmentID.Value);
                gvDept.DataBind();
            }
        }
        private void BindGridATC()
        {
            usr = (AppUser)Session["user"]; budYr = (BudgetYear)Session["budgetYr"];
            if (User.IsInRole(AdminRole) || User.IsInRole(PBMgrRole) || User.IsInRole(deptApprRole) || User.IsInRole(EDRole) || User.IsInRole(MDRole))
            {
                if (Request.QueryString["deptId"] != null && Request.QueryString["batchId"] != null)
                {
                    int deptId = int.Parse(Request.QueryString["deptId"].ToString());
                    string batchId = Request.QueryString["batchId"].ToString();
                    gvATC.DataSource = CommonBLL.GetDirectBudgetATCList(batchId);
                    gvATC.DataBind();
                }
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            DirectBudget astBud = null; DirectExpenseItem item = null;
            try
            {
                usr = (AppUser)Session["user"]; budYr = (BudgetYear)Session["budgetYr"];
                if (hid.Value == "Update")
                {
                    bool rst = false; decimal amt = 0;
                    // int month = int.Parse(ddlMonth.SelectedValue); 
                    int extTot = 0;
                    item = LookUpBLL.GetDirectItem(int.Parse(ddlAsset.SelectedValue));
                    astBud = CommonBLL.GetDirectBudget(Convert.ToInt32(txtID.Text));
                    if (astBud != null)
                    {
                        if (!decimal.TryParse(txtAmt.Text, out amt))
                        {
                            error.Visible = true;
                            error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button>Amount must be numeric!!.";
                            return;
                        }
                        ;
                        astBud.DirectItemID = int.Parse(ddlAsset.SelectedValue);
                        if (astBud.Status == (int)Utility.BudgetItemStatus.Pending_ReAlignment)
                        {
                            astBud.Status = (int)Utility.BudgetItemStatus.Pending_ReAlignment;
                        }
                        else
                        {
                            astBud.DepartmentID = usr.DepartmentID;
                            astBud.Status = (int)Utility.BudgetItemStatus.Pending_HOD_Approval;
                        }
                        // astBud.MonthID = month;
                        astBud.BudgetYrID = budYr.ID;

                        astBud.Amount = amt;
                        astBud.Justification = txtJust.Text;

                        rst = CommonBLL.UpdateDirectExpense(astBud);
                        if (rst != false)
                        {
                            BindGrid();
                            success.Visible = true;
                            success.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> Record updated successfully!!.";
                            return;
                        }
                    }
                    else
                    {
                        error.Visible = true;
                        error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button>Record could Not updated. Kindly try again. If error persist contact Administrator!!.";
                    }
                }
                else
                {
                    decimal amt = 0; int extTot = 0;
                    if (!decimal.TryParse(txtAmt.Text, out amt))
                    {
                        error.Visible = true;
                        error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button>Amount must be numeric!!.";
                        return;
                    }

                    bool result = false;
                    astBud = new DirectBudget();
                    //int month = int.Parse(ddlMonth.SelectedValue);
                    item = LookUpBLL.GetDirectItem(int.Parse(ddlAsset.SelectedValue));
                    astBud.DirectItemID = int.Parse(ddlAsset.SelectedValue);
                    astBud.DepartmentID = usr.DepartmentID;
                    astBud.AddeBy = User.Identity.Name;
                    astBud.DateAdded = DateTime.Now;
                    astBud.Amount = amt;
                    astBud.BudgetYrID = budYr.ID;
                    //  astBud.MonthID = month;
                    astBud.Justification = txtJust.Text;
                    astBud.ATCApproved = false;
                    astBud.ATCAmount = 0;
                    //if (!string.IsNullOrEmpty(txtExtQty.Text))
                    //{
                    //    if (int.TryParse(txtExtQty.Text, out extTot))
                    //    {
                    //        astBud.ExistingQuantity = extTot;
                    //    }
                    //}
                    {
                        astBud.Status = (int)Utility.BudgetItemStatus.Pending_HOD_Approval;
                    }

                    result = CommonBLL.AddDirectExpense(astBud);
                    if (result)
                    {
                        BindGrid();
                        txtAmt.Text = "";
                        ddlCat.SelectedValue = "";
                        txtJust.Text = "";
                        ddlAsset.Items.Clear();
                        //ddlMonth.SelectedValue = "";
                        success.Visible = true;
                        success.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> Record added successfully!!.";
                        return;
                    }
                    else
                    {
                        error.Visible = true;
                        error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button>Record could Not added. Kindly try again. If error persist contact Administrator!!.";
                    }
                }
            }
            catch (Exception ex)
            {
                error.Visible = true;
                error.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button> An error occurred. kindly try again!!!";
                Utility.WriteError("Error: " + ex.Message);
            }
        }

        protected void gvDept_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDept.PageIndex = e.NewPageIndex;
                BindGrid();
            }
            catch (Exception ex)
            {
                Utility.WriteError("Error: " + ex.InnerException);
            }
        }

        protected void gvDept_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "del")
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    // int key = Convert.ToInt32(gvDept.DataKeys[index].Value.ToString());
                    DirectBudget estf = CommonBLL.GetDirectBudget(index);
                    CommonBLL.DeleteDirectExpense(estf);
                    BindGrid();
                    success.Visible = true;
                    success.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> Record deleted successfully!!.";
                    return;

                }
                if (e.CommandName == "edt")
                {
                    hid.Value = "Update";
                    dvID.Visible = false;
                    btnSubmit.Text = "Update";
                    //GridViewRow row = gvDept.SelectedRow;
                    int index = int.Parse(e.CommandArgument.ToString());
                    DirectBudget estf = CommonBLL.GetDirectBudget(index);
                    txtID.Text = estf.ID.ToString();
                    ddlCat.SelectedValue = estf.DirectExpenseItem.DirectTypeID.Value.ToString();
                    BindDirectItemList(estf.DirectExpenseItem.DirectTypeID.Value);
                    ddlAsset.SelectedValue = estf.DirectItemID.Value.ToString();
                    txtAmt.Text = estf.Amount.ToString();
                    // txtExtQty.Text = estf.ExistingQuantity.ToString();
                    txtJust.Text = estf.Justification;
                    ddlCat.Enabled = false;
                    ddlAsset.Enabled = false;
                    btnSubmit.Enabled = true;
                    // ddlMonth.SelectedValue = estf.MonthID.ToString();
                }
            }
            catch (Exception ex)
            {
                error.Visible = true;
                error.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button> An error occurred. kindly try again!!!";
                Utility.WriteError("Error: " + ex.InnerException);
            }
        }

        protected void gvDept_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                CheckBox chkheader = null; budYr = (BudgetYear)Session["budgetYr"];
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    chkheader = e.Row.FindControl("chkHeader") as CheckBox;
                    Label atcAmtHeader = e.Row.FindControl("AtcAmtHeader") as Label;
                    Label lbOutHeader = e.Row.FindControl("lbOutAmtHeader") as Label;  
                    if (User.IsInRole(deptApprRole) && budYr.IsActive == true)
                    {
                        chkheader.Visible = true;
                        lbOutHeader.Visible = true;
                        atcAmtHeader.Visible = true;
                    }
                    if (User.IsInRole(AdminRole) || User.IsInRole(PBMgrRole) || User.IsInRole(PBOffRole) && budYr.IsActive == true)
                    {
                        chkheader.Visible = true;
                    }
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    // Label lbQty = e.Row.FindControl("lbQty") as Label;
                    Label lbsTot = e.Row.FindControl("lbAmt") as Label;
                    //quantityTotal += Convert.ToInt32(lbQty.Text);
                    priceTotal += Convert.ToDecimal(lbsTot.Text);
                    Label st = e.Row.FindControl("lbStatus") as Label;
                    CheckBox ck = e.Row.FindControl("chkRow") as CheckBox;
                    Label atcst = e.Row.FindControl("lbatcflg") as Label;
                    ImageButton imgEdit = e.Row.FindControl("imgBtnEdit") as ImageButton;
                    ImageButton imgDel = e.Row.FindControl("imgBtnDel") as ImageButton;

                    Label lbATCAmt = e.Row.FindControl("lbATCAmt") as Label;
                    Label lbOutAmt = e.Row.FindControl("lbOutAtcAmt") as Label;
                    TextBox txtAtcAmount = e.Row.FindControl("txtAtcAmount") as TextBox;

                    Label lbusr = e.Row.FindControl("lbusr") as Label;

                    if (User.IsInRole(deptIniRole) && budYr.IsActive == true)
                    {
                        if (st.Text == ((int)Utility.BudgetItemStatus.Rejected).ToString() || st.Text == ((int)Utility.BudgetItemStatus.Pending_HOD_Approval).ToString() || st.Text == ((int)Utility.BudgetItemStatus.Returned_For_Correction).ToString())
                        {
                            imgDel.Visible = true;
                            imgEdit.Visible = true;
                        }
                    }
                    if (lbusr.Text.Trim() != User.Identity.Name)
                    {
                        imgDel.Visible = false;
                        imgEdit.Visible = false;
                    }
                    if (User.IsInRole(deptApprRole) && budYr.IsActive == true)
                    {
                        ck.Visible = true;
                        if (st.Text == ((int)Utility.BudgetItemStatus.HOD_Approved).ToString() || st.Text == ((int)Utility.BudgetItemStatus.Rejected).ToString())
                        {
                            //ck.Enabled = false;
                            ck.Visible = false;
                        }
                        if (atcst.Text == ((int)Utility.ATCStatus.Pending_PBManager_Approval).ToString() || atcst.Text == ((int)Utility.ATCStatus.Pending_MD_Approval).ToString() || atcst.Text == ((int)Utility.ATCStatus.MD_Approved).ToString())
                        {
                            ck.Visible = false;
                        }
                        if (st.Text == ((int)Utility.BudgetItemStatus.MD_Approved).ToString())
                        {
                            lbOutAmt.Visible = true;

                            decimal amt = decimal.Parse(lbsTot.Text);
                            decimal atcamt = decimal.Parse(lbATCAmt.Text);
                            if (atcamt < amt)
                            {
                                txtAtcAmount.Visible = true;
                                ck.Visible = true;
                            }
                            else
                            {
                                ck.Visible = false;
                            }
                        }
                    }
                    if (User.IsInRole(AdminRole) || User.IsInRole(PBMgrRole) || User.IsInRole(PBOffRole) && budYr.IsActive == true)
                    {
                        ck.Visible = true;
                        if (st.Text == ((int)Utility.BudgetItemStatus.HOD_Approved).ToString() || st.Text == ((int)Utility.BudgetItemStatus.Pending_ED_Approval).ToString() || st.Text == ((int)Utility.BudgetItemStatus.Pending_MD_Approval).ToString())
                        {
                            ck.Visible = true;
                        }
                        if (st.Text == ((int)Utility.BudgetItemStatus.Returned_For_Correction).ToString())
                        {
                            ck.Visible = false;
                        }
                        if (atcst.Text == ((int)Utility.ATCStatus.Pending_PBManager_Approval).ToString() || atcst.Text == ((int)Utility.ATCStatus.Pending_MD_Approval).ToString() || atcst.Text == ((int)Utility.ATCStatus.MD_Approved).ToString())
                        {
                            ck.Visible = false;
                        }
                        if (st.Text == ((int)Utility.BudgetItemStatus.Pending_ReAlignment).ToString())
                        {
                            imgEdit.Visible = true;
                            imgDel.Visible = true;
                            ck.Visible = false;
                        }
                    }
                }
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    //Label lbQty = e.Row.FindControl("lbSumQty") as Label;
                    //lbQty.Text = quantityTotal.ToString();
                    Label lbsTot = e.Row.FindControl("lbSubTotalFt") as Label;
                    lbsTot.Text = priceTotal.ToString("N");
                }
            }
            catch (Exception ex)
            {
                error.Visible = true;
                error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> An error occured. Kindly try again. If error persist contact Administrator!!.";
                Utility.WriteError("Error: " + ex.InnerException);
            }
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            try
            {
                AppUser usr = null;
                if (Session["user"] != null)
                {
                    usr = (AppUser)Session["user"];
                }
                else
                {
                    Response.Redirect("../Login.aspx", false);
                    return;
                }
                if (string.IsNullOrEmpty(txtcomment.Text))
                {
                    modalErr.Visible = true;
                    modalErr.InnerText = "Comment is required!!!";
                    mpeAppr.Show();
                    return;
                }
                bool isset = false; AppUser budgetInputer = new AppUser();
                foreach (GridViewRow row in gvDept.Rows)
                {
                    if (((CheckBox)row.FindControl("chkRow")).Checked)
                    {
                        Label lbID = row.FindControl("lbRecID") as Label;
                        int recID = int.Parse(lbID.Text);
                        DirectBudget expPro = CommonBLL.GetDirectBudget(recID);
                        if (expPro != null && expPro.Status == (int)Utility.BudgetItemStatus.HOD_Approved)
                        {
                            expPro.Status = (int)Utility.BudgetItemStatus.Returned_For_Correction;
                            CommonBLL.UpdateDirectExpense(expPro);
                            budgetInputer = UserBLL.GetUserByUserName(expPro.AddeBy);
                            isset = true;
                        }
                    }
                }
                if (isset)
                {

                    BindGrid();
                    //sending mail
                    string body = "";
                    string from = ConfigurationManager.AppSettings["exUser"].ToString();
                    string siteUrl = ConfigurationManager.AppSettings["siteUrl"].ToString();
                    string appLogo = ConfigurationManager.AppSettings["appLogoUrl"].ToString();
                    string hodEmail = UserBLL.GetApproverEmailByDept(budgetInputer.DepartmentID.Value);
                    string subject = "Budget Item Correction Notification";
                    string FilePath = Server.MapPath("EmailTemplates/");
                    if (File.Exists(FilePath + "ReturnBudget.htm"))
                    {
                        FileStream f1 = new FileStream(FilePath + "ReturnBudget.htm", FileMode.Open);
                        StreamReader sr = new StreamReader(f1);
                        body = sr.ReadToEnd();
                        body = body.Replace("@add_appLogo", appLogo);
                        body = body.Replace("@siteUrl", siteUrl);
                        body = body.Replace("@BudgetElement", "Direct Budget");
                        body = body.Replace("@add_Comment", txtcomment.Text); //Replace the values from DB or any other source to personalize each mail.  
                        f1.Close();
                    }
                    string rst = "";
                    try
                    {
                        rst = Utility.SendMail(budgetInputer.Email, from, hodEmail, subject, body);
                    }
                    catch { }
                    if (rst.Contains("Successful"))
                    {
                        success.Visible = true;
                        success.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button>Selected Record(s) has been successfully Returned for correction.Notification has been sent to Initiator";
                        return;
                    }
                    else
                    {
                        success.Visible = true;
                        success.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button>Selected Record(s) has been successfully Returned for correction.Notification could NOT be sent at this time";
                        return;
                    }

                }
                else
                {
                    BindGrid();
                    error.Visible = true;
                    error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> An error occured. Either no record is selected OR some of selected record(s) could not be approved.If error persist contact Administrator!!.";
                }
            }
            catch (Exception ex)
            {
                error.Visible = true;
                error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> An error occured. Kindly try again. If error persist contact Administrator!!.";
                Utility.WriteError("Error: " + ex.Message);
            }
        }
        protected void btnApprv_Click(object sender, EventArgs e)
        {
            try
            {
                AppUser usr = null;
                if (Session["user"] != null)
                {
                    usr = (AppUser)Session["user"];
                }
                else
                {
                    Response.Redirect("../Login.aspx", false);
                    return;
                }
                bool isset = false;
                foreach (GridViewRow row in gvDept.Rows)
                {
                    if (((CheckBox)row.FindControl("chkRow")).Checked)
                    {
                        Label lbID = row.FindControl("lbRecID") as Label;
                        int recID = int.Parse(lbID.Text);
                        DirectBudget expPro = CommonBLL.GetDirectBudget(recID);
                        if (expPro.Status == (int)Utility.BudgetItemStatus.Pending_HOD_Approval || expPro.Status == (int)Utility.BudgetItemStatus.Returned_For_Correction)
                        {
                            expPro.Status = (int)Utility.BudgetItemStatus.HOD_Approved;
                            expPro.ApprovedBy = usr.FullName;
                            expPro.DateApproved = DateTime.Now;
                            CommonBLL.UpdateDirectExpense(expPro);
                            isset = true;
                        }
                    }
                }
                if (isset)
                {
                    BindGrid();
                    success.Visible = true;
                    success.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button>Selected Record(s) has been successfully approved.";
                    return;
                }
                else
                {
                    BindGrid();
                    error.Visible = true;
                    error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> An error occured some of selected record(s) could not be approved.If error persist contact Administrator!!.";
                }
            }
            catch (Exception ex)
            {
                error.Visible = true;
                error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> An error occured. Kindly try again. If error persist contact Administrator!!.";
                Utility.WriteError("Error: " + ex.InnerException);
            }
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtRejComment.Text))
                {
                    modalErr.Visible = true;
                    modalErr.InnerText = "Comment is required!!!";
                    ModalPopupExtender1.Show();
                    return;
                }
                bool isset = false; AppUser budgetInputer = new AppUser();
                foreach (GridViewRow row in gvDept.Rows)
                {
                    if (((CheckBox)row.FindControl("chkRow")).Checked)
                    {
                        Label lbID = row.FindControl("lbRecID") as Label;
                        int recID = int.Parse(lbID.Text);
                        DirectBudget expPro = CommonBLL.GetDirectBudget(recID);
                        if (expPro != null && expPro.Status == (int)Utility.BudgetItemStatus.Pending_HOD_Approval)
                        {
                            expPro.Status = (int)Utility.BudgetItemStatus.Rejected;
                            // exStf.ApprovedBy = usr.FullName;
                            //exStf.DateApproved = DateTime.Now;
                            CommonBLL.UpdateDirectExpense(expPro);
                            budgetInputer = UserBLL.GetUserByUserName(expPro.AddeBy);
                            isset = true;
                        }
                    }
                }

                if (isset)
                {
                    BindGrid();
                    //sending mail
                    string body = "";
                    string from = ConfigurationManager.AppSettings["exUser"].ToString();
                    string siteUrl = ConfigurationManager.AppSettings["siteUrl"].ToString();
                    string appLogo = ConfigurationManager.AppSettings["appLogoUrl"].ToString();
                    //string hodEmail = UserBLL.GetApproverEmailByDept(budgetInputer.DepartmentID.Value);
                    string subject = "Budget Item Rejection Notification";
                    string FilePath = Server.MapPath("EmailTemplates/");
                    if (File.Exists(FilePath + "RejectBudget.htm"))
                    {
                        FileStream f1 = new FileStream(FilePath + "RejectBudget.htm", FileMode.Open);
                        StreamReader sr = new StreamReader(f1);
                        body = sr.ReadToEnd();
                        body = body.Replace("@add_appLogo", appLogo);
                        body = body.Replace("@siteUrl", siteUrl);
                        body = body.Replace("@BudgetElement", "Direct Budget");
                        body = body.Replace("@add_Comment", txtRejComment.Text); //Replace the values from DB or any other source to personalize each mail.  
                        f1.Close();
                    }
                    string rst = "";
                    try
                    {
                        rst = Utility.SendMail(budgetInputer.Email, from, "", subject, body);
                    }
                    catch { }
                    //BindGrid();
                    if (rst.Contains("Successful"))
                    {
                        success.Visible = true;
                        success.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button>Selected Record(s) has been successfully rejected.";
                        return;
                    }
                    else
                    {
                        success.Visible = true;
                        success.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button>Selected Record(s) has been successfully Returned for correction.Notification could NOT be sent at this time";
                        return;
                    }
                }
                else
                {
                    BindGrid();
                    error.Visible = true;
                    error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> An error occured some of selected record(s) could not be approved.If error persist contact Administrator!!.";
                }
            }
            catch (Exception ex)
            {
                error.Visible = true;
                error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> An error occured. Kindly try again. If error persist contact Administrator!!.";
                Utility.WriteError("Error: " + ex.InnerException);
            }
        }
        protected void lnkAddNew_Click(object sender, EventArgs e)
        {
            try
            {
                hid.Value = "";
                dvID.Visible = false;
                btnSubmit.Text = "Add";
                txtAmt.Text = "";
                txtJust.Text = "";
                // txtExtQty.Text = "";
                ddlAsset.Enabled = true;
                ddlCat.Enabled = true;
                ddlAsset.Items.Clear();
                ddlCat.Items.Clear();
                // ddlMonth.Items.Clear();
                // Utility.BindMonth(ddlMonth);
                Utility.BindDirectType(ddlCat);
            }
            catch (Exception ex)
            {
            }
        }

        protected string GetStatus(object o)
        {
            try
            {
                return Utility.GetBudgetStatus(o);
            }
            catch
            {
                return "";
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                usr = (AppUser)Session["user"]; budYr = (BudgetYear)Session["budgetYr"];
                var q = CommonBLL.GetDirectBudgetList(budYr.ID);
                if (ddlExpTyp.SelectedValue != "")
                {
                    int AstypID = int.Parse(ddlExpTyp.SelectedValue);
                    q = q.Where(p => p.DirectExpenseItem.DirectTypeID.Value == AstypID);
                }
                if (User.IsInRole(AdminRole) || User.IsInRole(PBMgrRole) || User.IsInRole(PBOffRole))
                {
                    if (ddlDeptFilter.SelectedValue != "")
                    {
                        int dep = int.Parse(ddlDeptFilter.SelectedValue);
                        q = q.Where(p => p.DepartmentID == dep);
                        List<TrackApproval> getTrackApproval = null;
                        getTrackApproval = LookUpBLL.GetTrackApproval(dep, budYr.ID).ToList();
                        if (getTrackApproval != null && getTrackApproval.Count() > 0)//if selected department has been pushed for approval
                        {
                            if (getTrackApproval.FirstOrDefault().status == (int)Utility.BudgetItemStatus.Pending_ReAlignment)
                            {
                                dvAdd.Visible = true;
                                btnSubmit.Enabled = false;
                            }
                        }
                    }
                    gvDept.DataSource = q.ToList();
                    gvDept.DataBind();
                }
                else
                {
                    gvDept.DataSource = q.Where(p => p.DepartmentID == usr.DepartmentID).ToList();
                    gvDept.DataBind();
                }

            }
            catch (Exception ex)
            {
                error.Visible = true;
                error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> An error occured. Kindly try again. If error persist contact Administrator!!.";
                Utility.WriteError("Error: " + ex.InnerException);
            }
        }

        protected void btnClr_Click(object sender, EventArgs e)
        {
            ddlExpTyp.SelectedValue = "";
            ddlDeptFilter.SelectedValue = "";
            BindGrid();
        }

        protected void ddlCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlAsset.Items.Clear();
            if (ddlCat.SelectedValue != "")
            {
                BindDirectItemList(int.Parse(ddlCat.SelectedValue));
            }
        }

        private void BindDirectItemList(int DirectTypeID)
        {
            ddlAsset.Items.Clear();
            ddlAsset.DataTextField = "Name";
            ddlAsset.DataValueField = "ID";
            ddlAsset.DataSource = LookUpBLL.GetDirectItemListLookUp(DirectTypeID);
            ddlAsset.DataBind();
        }
        protected void btnForward_Click(object sender, EventArgs e)
        {
            try
            {
                AppUser usr = null;
                if (Session["user"] != null)
                {
                    usr = (AppUser)Session["user"];
                }
                else
                {
                    Response.Redirect("../Login.aspx", false);
                    return;
                }
                bool isset = false;
                foreach (GridViewRow row in gvDept.Rows)
                {
                    if (((CheckBox)row.FindControl("chkRow")).Checked)
                    {
                        Label lbID = row.FindControl("lbRecID") as Label;
                        int recID = int.Parse(lbID.Text);
                        DirectBudget expPro = CommonBLL.GetDirectBudget(recID);
                        if (expPro != null && expPro.Status == (int)Utility.BudgetItemStatus.HOD_Approved)
                        {
                            expPro.Status = (int)Utility.BudgetItemStatus.Pending_ED_Approval;
                            CommonBLL.UpdateDirectExpense(expPro);
                            // budgetInputer = UserBLL.GetUserByUserName(expPro.AddeBy);
                            isset = true;
                        }
                    }
                }
                if (isset)
                {
                    BindGrid();
                    string body = "";
                    string from = ConfigurationManager.AppSettings["exUser"].ToString();
                    string siteUrl = ConfigurationManager.AppSettings["siteUrl"].ToString();
                    string appLogo = ConfigurationManager.AppSettings["appLogoUrl"].ToString();
                    string hodEmail = Utility.GetUsersEmailAdd(EDRole);
                    string subject = "Budget Item Approval Notification";
                    string FilePath = Server.MapPath("EmailTemplates/");
                    if (File.Exists(FilePath + "ApproveBudget.htm"))
                    {
                        FileStream f1 = new FileStream(FilePath + "ApproveBudget.htm", FileMode.Open);
                        StreamReader sr = new StreamReader(f1);
                        body = sr.ReadToEnd();
                        body = body.Replace("@add_appLogo", appLogo);
                        body = body.Replace("@siteUrl", siteUrl);
                        body = body.Replace("@BudgetElement", "Direct Budget");
                        //body = body.Replace("@add_Comment", txtcomment.Text); //Replace the values from DB or any other source to personalize each mail.  
                        f1.Close();
                    }
                    string rst = "";
                    try
                    {
                        rst = Utility.SendMail(hodEmail, from, "", subject, body);
                    }
                    catch { }
                    if (rst.Contains("Successful"))
                    {
                        success.Visible = true;
                        success.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button>Selected Approved Record(s) has been successfully Forwarded. Notification has been sent to the ED";
                        return;
                    }
                    else
                    {
                        success.Visible = true;
                        success.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button>Selected Approved Record(s) has been successfully Forwarded.";
                        return;
                    }
                }
                else
                {
                    BindGrid();
                    error.Visible = true;
                    error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> An error occured some of selected record(s) could not be process. Ensure Records are selected.If error persist contact Administrator!!.";
                }
            }
            catch (Exception ex)
            {
                error.Visible = true;
                error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> An error occured. Kindly try again. If error persist contact Administrator!!.";
                Utility.WriteError("Error: " + ex.InnerException);
            }
        }

        protected void btnATC_Click(object sender, EventArgs e)
        {
            try
            {
                AppUser usr = null; BudgetYear budYr = null;
                int btype = int.Parse(budgetType);
                if (Session["user"] != null)
                {
                    usr = (AppUser)Session["user"];
                    budYr = (BudgetYear)Session["budgetYr"];
                }
                else
                {
                    Response.Redirect("Login.aspx", false);
                    return;
                }
                bool isset = false;
                string batch = CommonBLL.GenerateBatchID(budYr.ID, usr.DepartmentID.Value, btype);
                batch = "DI-" + batch; decimal atcAmount = 0;
                foreach (GridViewRow row in gvDept.Rows)
                {
                    if (((CheckBox)row.FindControl("chkRow")).Checked)
                    {
                        Label lbAmt = row.FindControl("lbAmt") as Label;
                        Label lbATCAmt = row.FindControl("lbATCAmt") as Label;
                        TextBox txAmt = row.FindControl("txtAtcAmount") as TextBox;
                        if (!string.IsNullOrEmpty(txAmt.Text))
                        {
                            decimal bAmt = decimal.Parse(lbAmt.Text);
                            decimal atcamt = decimal.Parse(lbATCAmt.Text);
                            decimal curAtc = decimal.Parse(txAmt.Text);
                            if ((atcamt + curAtc) > bAmt)
                            {
                                error.Visible = true;
                                error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button>ATC Amount inputted is more than budget amount. Kindly review your input";
                                return;
                            }
                        }
                        else
                        {
                            error.Visible = true;
                            error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button>Input textbox for check item cannot be empty. Kindly review your input";
                            return;
                        }
                    }
                }

                foreach (GridViewRow row in gvDept.Rows)
                {
                    if (((CheckBox)row.FindControl("chkRow")).Checked)
                    {
                        Label lbID = row.FindControl("lbRecID") as Label;
                        TextBox txAmt = row.FindControl("txtAtcAmount") as TextBox;
                        Label lbATCAmt = row.FindControl("lbATCAmt") as Label;
                        int recID = int.Parse(lbID.Text);
                        DirectBudget expPro = CommonBLL.GetDirectBudget(recID);
                        if (expPro != null && expPro.Status == (int)Utility.BudgetItemStatus.MD_Approved)
                        {
                           decimal totATc = 0;
                            decimal amt = decimal.Parse(txAmt.Text);
                            decimal oAtcAmt = 0;
                            if (decimal.TryParse(lbATCAmt.Text, out oAtcAmt))
                            {
                                totATc = oAtcAmt + amt;
                            }
                            else
                            {
                                totATc = amt;
                            }
                            atcAmount += amt;
                            expPro.ATCBatchID = batch;
                            expPro.ATCApproved = false;
                            expPro.ATCStatus = (int)Utility.ATCStatus.Pending_PBManager_Approval;
                            expPro.ATCAmount = totATc;
                            CommonBLL.UpdateDirectExpense(expPro);
                            CommonBLL.AddDirectExpenseATC(new DirectBudgetATC() { ATCBatchId = batch, BudgetItemId = recID, BudgetItem = expPro.DirectExpenseItem.Name, ATCAmount = amt, DepartmentId = expPro.DepartmentID.Value, DateAdded = DateTime.Now, Status = (int)Utility.ATCStatus.Pending_PBManager_Approval });
                            // budgetInputer = UserBLL.GetUserByUserName(expPro.AddeBy);
                            isset = true;
                        }
                    }
                }
                        if (isset)
                        {
                            CommonBLL.AddATCHeader(new ATCRequestHeader() { BatchID = batch, DepartmentID = usr.DepartmentID.Value, BudgetTypeId = btype, BudgetYrID = budYr.ID, Amount = atcAmount, RequestBy = usr.FullName, RequestDate = DateTime.Now, Status = (int)Utility.ATCStatus.Pending_PBManager_Approval });
                            BindGrid();
                            string body = "";
                            string from = ConfigurationManager.AppSettings["exUser"].ToString();
                            string siteUrl = ConfigurationManager.AppSettings["siteUrl"].ToString(); 
                            string appLogo = ConfigurationManager.AppSettings["appLogoUrl"].ToString();
                            string hodEmail = Utility.GetUsersEmailAdd(PBMgrRole);
                            string subject = "ATC Approval Notification";
                            string FilePath = Server.MapPath("EmailTemplates/");
                            if (File.Exists(FilePath + "ATCApproval.htm"))
                            {
                                FileStream f1 = new FileStream(FilePath + "ATCApproval.htm", FileMode.Open);
                                StreamReader sr = new StreamReader(f1);
                                body = sr.ReadToEnd();
                                body = body.Replace("@add_appLogo", appLogo);
                                body = body.Replace("@siteUrl", siteUrl);
                                body = body.Replace("@BudgetElement", "Direct Budget");
                                //body = body.Replace("@add_Comment", txtcomment.Text); //Replace the values from DB or any other source to personalize each mail.  
                                f1.Close();
                            }
                            string rst = "";
                            try
                            {
                                rst = Utility.SendMail(hodEmail, from, "", subject, body);
                            }
                            catch { }
                            if (rst.Contains("Successful"))
                            {
                                success.Visible = true;
                                success.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button>Selected Approved Record(s) has been successfully Forwarded to Planning&Budgeting manager. Notification has been sent to the P&B Manager";
                                return;
                            }
                            else
                            {
                                success.Visible = true;
                                success.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button>Selected Approved Record(s) has been successfully Forwarded to Planning & Budgeting Manager.";
                                return;
                            }
                        }
                        else
                        {
                            BindGrid();
                            error.Visible = true;
                            error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> An error occured some of selected record(s) could not be process. Ensure Records are selected.If error persist contact Administrator!!.";
                        }
    
            }
            catch (Exception ex)
            {
                error.Visible = true;
                error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> An error occured. Kindly try again. If error persist contact Administrator!!.";
                Utility.WriteError("Error: " + ex.Message);
            }
        }

        protected void btnATCForward_Click(object sender, EventArgs e)
        {
            try
            {
                AppUser usr = null;
                if (Session["user"] != null)
                {
                    usr = (AppUser)Session["user"];
                }
                else
                {
                    Response.Redirect("../Login.aspx", false);
                    return;
                }
                bool isset = false;
                foreach (GridViewRow row in gvATC.Rows)
                {
                    //if (((CheckBox)row.FindControl("chkRow")).Checked)
                    //{
                    Label lbrecID = row.FindControl("lbRecID") as Label;
                    Label lbID = row.FindControl("lbID") as Label;
                    int recID = int.Parse(lbrecID.Text);
                    int rwID = int.Parse(lbID.Text);
                    
                    DirectBudget expPro = CommonBLL.GetDirectBudget(recID);
                    DirectBudgetATC atcitm = CommonBLL.GetDirectBudgetATC(rwID);
                    if (expPro != null && expPro.ATCStatus == (int)Utility.ATCStatus.Pending_PBManager_Approval)
                    {
                        expPro.ATCStatus = (int)Utility.ATCStatus.Pending_MD_Approval;
                        atcitm.Status = (int)Utility.ATCStatus.Pending_MD_Approval;
                        CommonBLL.UpdateDirectExpense(expPro);
                        CommonBLL.UpdateDirectExpenseATC(atcitm);
                        // budgetInputer = UserBLL.GetUserByUserName(expPro.AddeBy);
                        isset = true;
                    }
                    //}
                }
                if (isset)
                {
                    int headerId = int.Parse(Request.QueryString["Id"].ToString());
                    ATCRequestHeader atcRec = CommonBLL.GetATCHeader(headerId);
                    atcRec.Status = (int)Utility.ATCStatus.Pending_MD_Approval;
                    CommonBLL.UpdateATCHeader(atcRec);
                    btnATCForward.Enabled = false;
                    btnATCReject.Enabled = false;
                    BindGridATC();
                    dvAdmin.Visible = false;
                    string body = "";
                    string from = ConfigurationManager.AppSettings["exUser"].ToString();
                    string siteUrl = ConfigurationManager.AppSettings["siteUrl"].ToString();
                    string appLogo = ConfigurationManager.AppSettings["appLogoUrl"].ToString();
                    string hodEmail = Utility.GetUsersEmailAdd(MDRole);
                    string subject = "ATC Approval Notification";
                    string FilePath = Server.MapPath("EmailTemplates/");
                    if (File.Exists(FilePath + "ATCApproval.htm"))
                    {
                        FileStream f1 = new FileStream(FilePath + "ATCApproval.htm", FileMode.Open);
                        StreamReader sr = new StreamReader(f1);
                        body = sr.ReadToEnd();
                        body = body.Replace("@add_appLogo", appLogo);
                        body = body.Replace("@siteUrl", siteUrl);
                        body = body.Replace("@BudgetElement", "Direct Budget");
                        body = body.Replace("@add_Dept", atcRec.Department.Name);
                        //body = body.Replace("@add_Comment", txtcomment.Text); //Replace the values from DB or any other source to personalize each mail.  
                        f1.Close();
                    }
                    string rst = "";
                    try
                    {
                        rst = Utility.SendMail(hodEmail, from, "", subject, body);
                    }
                    catch { }
                    if (rst.Contains("Successful"))
                    {
                        success.Visible = true;
                        success.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button>BudgetItem(s) has been successfully Forwarded. Notification has been sent to the MD";
                        return;
                    }
                    else
                    {
                        success.Visible = true;
                        success.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button>BudgetItem(s) has been successfully Forwarded.";
                        return;
                    }
                }
                else
                {
                    BindGridATC();
                    error.Visible = true;
                    error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> An error occured some of selected record(s) could not be process. Ensure Records are selected.If error persist contact Administrator!!.";
                }
            }
            catch (Exception ex)
            {
                error.Visible = true;
                error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> An error occured. Kindly try again. If error persist contact Administrator!!.";
                Utility.WriteError("Error: " + ex.InnerException);
            }
        }
        protected void btnATCMDApproval_Click(object sender, EventArgs e)
        {
            try
            {
                AppUser usr = null;
                if (Session["user"] != null)
                {
                    usr = (AppUser)Session["user"];
                }
                else
                {
                    Response.Redirect("../Login.aspx", false);
                    return;
                }
                bool isset = false;
                foreach (GridViewRow row in gvATC.Rows)
                {

                    Label lbrecID = row.FindControl("lbRecID") as Label;
                    Label lbID = row.FindControl("lbID") as Label;
                    int recID = int.Parse(lbrecID.Text);
                    int rwID = int.Parse(lbID.Text);

                    DirectBudget expPro = CommonBLL.GetDirectBudget(recID);
                    DirectBudgetATC atcitm = CommonBLL.GetDirectBudgetATC(rwID);
                    if (expPro != null && expPro.ATCStatus == (int)Utility.ATCStatus.Pending_MD_Approval)
                    {
                        expPro.ATCStatus = (int)Utility.ATCStatus.MD_Approved;
                        atcitm.Status = (int)Utility.ATCStatus.MD_Approved;
                        CommonBLL.UpdateDirectExpense(expPro);
                        CommonBLL.UpdateDirectExpenseATC(atcitm);
                        // budgetInputer = UserBLL.GetUserByUserName(expPro.AddeBy);
                        isset = true;
                    }

                }
                if (isset)
                {
                    int headerId = int.Parse(Request.QueryString["Id"].ToString());
                    ATCRequestHeader atcRec = CommonBLL.GetATCHeader(headerId);
                    atcRec.Status = (int)Utility.ATCStatus.MD_Approved;

                    CommonBLL.UpdateATCHeader(atcRec);
                    btnATCMDApproval.Enabled = false;
                    btnATCReject.Enabled = false;
                    btnATCForward.Enabled = false;
                    BindGridATC();
                    dvAdmin.Visible = false;
                    string body = "";
                    string from = ConfigurationManager.AppSettings["exUser"].ToString();
                    string siteUrl = ConfigurationManager.AppSettings["siteUrl"].ToString();
                    string appLogo = ConfigurationManager.AppSettings["appLogoUrl"].ToString();
                    string hodEmail = Utility.GetUsersEmailAdd(deptApprRole, atcRec.DepartmentID.Value);
                    string subject = "ATC Approval Notification";
                    string FilePath = Server.MapPath("EmailTemplates/");
                    if (File.Exists(FilePath + "ATCApprovalNotification.htm"))
                    {
                        FileStream f1 = new FileStream(FilePath + "ATCApprovalNotification.htm", FileMode.Open);
                        StreamReader sr = new StreamReader(f1);
                        body = sr.ReadToEnd();
                        body = body.Replace("@add_appLogo", appLogo);
                        body = body.Replace("@siteUrl", siteUrl);
                        body = body.Replace("@BudgetElement", "Direct Budget");
                        body = body.Replace("@add_Dept", atcRec.Department.Name);
                        //body = body.Replace("@add_Comment", txtcomment.Text); //Replace the values from DB or any other source to personalize each mail.  
                        f1.Close();
                    }
                    string rst = "";
                    try
                    {
                        rst = Utility.SendMail(hodEmail, from, "", subject, body);
                    }
                    catch { }
                    if (rst.Contains("Successful"))
                    {
                        success.Visible = true;
                        success.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button>BudgetItem(s) has been successfully Approved. Notification has been sent to the HOD of requesting department";
                        return;
                    }
                    else
                    {
                        success.Visible = true;
                        success.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button>BudgetItem(s) has been successfully Approved.";
                        return;
                    }
                }
                else
                {
                    BindGridATC();
                    error.Visible = true;
                    error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> An error occured some of selected record(s) could not be process. Ensure Records are selected.If error persist contact Administrator!!.";
                }
            }
            catch (Exception ex)
            {
                error.Visible = true;
                error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> An error occured. Kindly try again. If error persist contact Administrator!!.";
                Utility.WriteError("Error: " + ex.InnerException);
            }
        }

        protected void btnATCReject_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtATCcmt.Text))
                {
                    lbatcmsg.Visible = true;
                    lbatcmsg.InnerText = "Comment is required!!!";
                    mpeATC.Show();
                    return;
                }
                AppUser usr = null;
                if (Session["user"] != null)
                {
                    usr = (AppUser)Session["user"];
                }
                else
                {
                    Response.Redirect("../Login.aspx", false);
                    return;
                }
                bool isset = false;
                foreach (GridViewRow row in gvATC.Rows)
                {

                    Label lbrecID = row.FindControl("lbRecID") as Label;
                    Label lbID = row.FindControl("lbID") as Label;
                    int recID = int.Parse(lbrecID.Text);
                    int rwID = int.Parse(lbID.Text);

                    DirectBudget expPro = CommonBLL.GetDirectBudget(recID);
                    DirectBudgetATC atcitm = CommonBLL.GetDirectBudgetATC(rwID);
                    if (expPro.ATCStatus == (int)Utility.ATCStatus.Pending_PBManager_Approval || expPro.ATCStatus == (int)Utility.ATCStatus.Pending_MD_Approval)
                    {
                        expPro.ATCStatus = (int)Utility.ATCStatus.Declined;
                        expPro.ATCApproved = false;
                        expPro.ATCAmount -= atcitm.ATCAmount;
                        atcitm.Status = (int)Utility.ATCStatus.Declined;
                        CommonBLL.UpdateDirectExpense(expPro);
                        CommonBLL.UpdateDirectExpenseATC(atcitm);
                        // budgetInputer = UserBLL.GetUserByUserName(expPro.AddeBy);
                        isset = true;
                    }

                }
                if (isset)
                {
                    int headerId = int.Parse(Request.QueryString["Id"].ToString());
                    ATCRequestHeader atcRec = CommonBLL.GetATCHeader(headerId);
                    atcRec.Status = (int)Utility.ATCStatus.Declined;

                    CommonBLL.UpdateATCHeader(atcRec);
                    btnATCMDApproval.Enabled = false;
                    btnATCReject.Enabled = false;
                    BindGridATC();
                    dvAdmin.Visible = false;
                    string body = "";
                    string from = ConfigurationManager.AppSettings["exUser"].ToString();
                    string siteUrl = ConfigurationManager.AppSettings["siteUrl"].ToString();
                    string appLogo = ConfigurationManager.AppSettings["appLogoUrl"].ToString();
                    string hodEmail = Utility.GetUsersEmailAdd(deptApprRole, atcRec.DepartmentID.Value);
                    string subject = "ATC Rejection Notification";
                    string FilePath = Server.MapPath("EmailTemplates/");
                    if (File.Exists(FilePath + "ReturnATCRequest.htm"))
                    {
                        FileStream f1 = new FileStream(FilePath + "ReturnATCRequest.htm", FileMode.Open);
                        StreamReader sr = new StreamReader(f1);
                        body = sr.ReadToEnd();
                        body = body.Replace("@add_appLogo", appLogo);
                        body = body.Replace("@siteUrl", siteUrl);
                        body = body.Replace("@BudgetElement", "Direct Budget");
                        body = body.Replace("@add_Comment", txtATCcmt.Text);
                        //body = body.Replace("@add_Comment", txtcomment.Text); //Replace the values from DB or any other source to personalize each mail.  
                        f1.Close();
                    }
                    string rst = "";
                    try
                    {
                        rst = Utility.SendMail(hodEmail, from, "", subject, body);
                    }
                    catch { }
                    if (rst.Contains("Successful"))
                    {
                        success.Visible = true;
                        success.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button> ATC request for BudgetItem(s) has been declined successfully . Notification has been sent to the HOD of requesting department";
                        return;
                    }
                    else
                    {
                        success.Visible = true;
                        success.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button>ATC request for BudgetItem(s) has been declined successfully.";
                        return;
                    }
                }
                else
                {
                    BindGridATC();
                    error.Visible = true;
                    error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> An error occured some of selected record(s) could not be process.If error persist contact Administrator!!.";
                }
            }
            catch (Exception ex)
            {
                error.Visible = true;
                error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> An error occured. Kindly try again. If error persist contact Administrator!!.";
                Utility.WriteError("Error: " + ex.InnerException);
            }
        }

        protected string GetATCStatus(object o)
        {
            try
            {
                return Utility.GetATCStatus(o);
            }
            catch
            {
                return "";
            }
        }
        protected string GetOutStandingATC(object amt, object atcamt)
        {
            try
            {
                decimal amtd = decimal.Parse(amt.ToString());
                decimal atcamtd = decimal.Parse(atcamt.ToString());
                return (amtd - atcamtd).ToString("N");
            }
            catch
            {
                return "";
            }
        }

        protected void gvATC_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
    }
}