using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BudgetCapture.BLL;
using BudgetCapture.DAL;
using System.Configuration;
using System.IO;

namespace BudgetCapture.Admin
{
    public partial class ExistingStaff : System.Web.UI.Page
    {
        private static AppUser usr=null;private static BudgetYear budYr=null;
        private string AdminRole = ConfigurationManager.AppSettings["adminRole"].ToString();
        private string deptIniRole = ConfigurationManager.AppSettings["DeptIniRole"].ToString();
        private string deptApprRole = ConfigurationManager.AppSettings["DeptApprverRole"].ToString();
        private string PBMgrRole = ConfigurationManager.AppSettings["PBMgrRole"].ToString();
        private string PBOffRole = ConfigurationManager.AppSettings["PBOffRole"].ToString();
        private string EDRole = ConfigurationManager.AppSettings["EDRole"].ToString();
        private string MDRole = ConfigurationManager.AppSettings["MDRole"].ToString();
        private string budgetType = ConfigurationManager.AppSettings["SalBen"].ToString();
        decimal priceTotal = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            error.Visible = false; success.Visible = false;
            //dvAdd.Visible = false; 
            dvAppr.Visible = false;
            try
            {
            if (Session["user"] != null)
            {
                usr = (AppUser)Session["user"];
                budYr=(BudgetYear)Session["budgetYr"];
                List<TrackApproval> getTrackApproval = null;
                getTrackApproval = LookUpBLL.GetTrackApproval(usr.DepartmentID.Value, budYr.ID).ToList();
                if (User.IsInRole(deptApprRole) && budYr.IsActive==true)
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
                //        dvAdmin.Visible = false;
                //    }
                //    else
                //    {
                //        dvAdmin.Visible = true;
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
                    Utility.BindGrade(ddlgrade);
                    Utility.BindDept(ddlDeptFilter);
                  //  BindGrid();
                    lbDept.Text = usr.Department.Name.ToUpper();
                    lbyr.Text = budYr.Year;
                    if (User.IsInRole(AdminRole) || User.IsInRole(PBMgrRole) || User.IsInRole(PBOffRole))
                    {
                        dvAdminScr.Visible = true;
                    }
                    if (Request.QueryString["deptId"] != null && Request.QueryString["batchId"] != null && Request.QueryString["Id"] != null)
                    {
                        BindGridATC();
                        ddlDeptFilter.Visible = false;
                        dvAdd.Visible = false; dvAppr.Visible = false; dvAdmin.Visible = false; dvATC.Visible = false;
                        gvDept.Visible = false; gvATC.Visible = true;
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
                    gvDept.DataSource = CommonBLL.GetExistStaffList(budYr.ID,dep);
                    gvDept.DataBind();
                }
                else
                {
                    gvDept.DataSource = CommonBLL.GetExistStaffList(budYr.ID);
                    gvDept.DataBind();
                }
            }
            else if (User.IsInRole(EDRole) || User.IsInRole(MDRole))
            {
                if (Request.QueryString["deptId"] != null)
                {
                    int deptId = int.Parse(Request.QueryString["deptId"].ToString());
                    gvDept.DataSource = CommonBLL.GetExistStaffList(budYr.ID, deptId);
                    gvDept.DataBind();
                }
            }
            else
            {
                gvDept.DataSource = CommonBLL.GetExistStaffList(budYr.ID, usr.DepartmentID.Value);
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
                    gvATC.DataSource = CommonBLL.GetExistingStaffBudgetATCList(batchId);
                    gvATC.DataBind();
                }
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            DAL.ExistingStaff stf = null;
            try
            {
                usr = (AppUser)Session["user"]; budYr = (BudgetYear)Session["budgetYr"];
                Grade gd = LookUpBLL.GetGrade(int.Parse(ddlgrade.SelectedValue));
                if (hid.Value == "Update")
                {
                    bool rst = false; int tot = 0;
                    stf = CommonBLL.GetExistingStaff(Convert.ToInt32(txtID.Text));
                    if (stf != null)
                    {
                        if (!int.TryParse(txtTot.Text, out tot))
                        {
                            error.Visible = true;
                            error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button>Total must be numeric!!.";
                            return;
                        }
                        stf.TotalNumber = tot;
                        stf.GradeID = int.Parse(ddlgrade.SelectedValue);
                      
                        if (stf.Status == (int)Utility.BudgetItemStatus.Pending_ReAlignment)
                        {
                            stf.Status = (int)Utility.BudgetItemStatus.Pending_ReAlignment;
                        }
                        else
                        {
                            stf.DepartmentID = usr.DepartmentID;
                            stf.Status = (int)Utility.BudgetItemStatus.Pending_HOD_Approval;
                        }
                        if (gd != null)
                        {
                            ManningCas mCas = Utility.ComputeManningCas(gd);
                            if (mCas != null)
                            {
                                stf.CasCost = tot*mCas.CasTotal;
                                stf.ManningCost = tot*mCas.ManningTotal;
                            }
                            stf.TotalCost = tot * (mCas.CasTotal + mCas.ManningTotal);
                        }
                        stf.BudgetYrID = budYr.ID;
                        rst = CommonBLL.UpdateExistingStaff(stf);
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
                    int tot = 0;
                    if (!int.TryParse(txtTot.Text, out tot))
                    {
                        error.Visible = true;
                        error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button>Total must be numeric!!.";
                        return;
                    }
                    bool result = false;
                     stf = new DAL.ExistingStaff();
                     stf.GradeID = int.Parse(ddlgrade.SelectedValue);
                     stf.DepartmentID = usr.DepartmentID;
                     stf.AddedBy = User.Identity.Name;
                     stf.DateAdded = DateTime.Now;
                     stf.TotalNumber = tot;
                     stf.BudgetYrID = budYr.ID;
                     stf.ATCApproved = false;
                     stf.ATCAmount = 0;
                     stf.Status = (int)Utility.BudgetItemStatus.Pending_HOD_Approval;
                     if (gd != null)
                     {
                         ManningCas mCas = Utility.ComputeManningCas(gd);
                         if (mCas != null)
                         {
                             stf.CasCost = tot * mCas.CasTotal;
                             stf.ManningCost = tot * mCas.ManningTotal;
                             stf.TotalCost = tot * (mCas.CasTotal + mCas.ManningTotal);
                             result = CommonBLL.AddExistingStaff(stf);
                             if (result)
                             {
                                 BindGrid();
                                 txtTot.Text = "";
                                 ddlgrade.SelectedValue = "";
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
                         else
                         {
                             error.Visible = true;
                             error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button>Record could Not added. Ensure that Salary Grade items has been set up properly. Kindly try again. If error persist contact Administrator!!.";
                         }
                        
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
        protected void gvDept_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "del")
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                   // int key = Convert.ToInt32(gvDept.DataKeys[index].Value.ToString());
                    DAL.ExistingStaff estf=CommonBLL.GetExistingStaff(index);
                    CommonBLL.DeleteExistingRecord(estf);
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
                    DAL.ExistingStaff estf = CommonBLL.GetExistingStaff(index);
                    txtID.Text = estf.ID.ToString();
                    txtTot.Text = estf.TotalNumber.ToString();
                    ddlgrade.SelectedValue = estf.GradeID.ToString();
                    btnSubmit.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                error.Visible = true;
                error.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button> An error occurred. kindly try again!!!";
                Utility.WriteError("Error: " + ex.InnerException);
            }
        }
        protected void gvDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //hid.Value = "Update";
                //dvID.Visible = false;
                //btnSubmit.Text = "Update";

                //GridViewRow row = gvDept.SelectedRow;
                //txtID.Text = (row.FindControl("lbRecID") as Label).Text;
                //txtTot.Text = row.Cells[2].Text.ToLower();
                //ddlgrade.SelectedValue = (row.FindControl("lbID") as Label).Text; 
                ////ddlDir.SelectedItem.Text = HttpUtility.HtmlDecode(row.Cells[2].Text).ToLower();
            }
            catch (Exception ex)
            {
                //error.Visible = true;
                //error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> An error occured. Kindly try again. If error persist contact Administrator!!.";
                //Utility.WriteError("Error: " + ex.InnerException);
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

        protected void lnkAddNew_Click(object sender, EventArgs e)
        {
            try
            {
                hid.Value = "";
                dvID.Visible = false;
                btnSubmit.Text = "Add";
                txtTot.Text = "";
                ddlgrade.SelectedValue = "";
            }
            catch
            {
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
                    Label st = e.Row.FindControl("lbStatus") as Label;
                    CheckBox ck = e.Row.FindControl("chkRow") as CheckBox;
                    Label atcst = e.Row.FindControl("lbatcflg") as Label;
                    ImageButton imgEdit = e.Row.FindControl("imgBtnEdit") as ImageButton;
                    ImageButton imgDel = e.Row.FindControl("imgBtnDel") as ImageButton;
                    Label lbsTot = e.Row.FindControl("lbAmt") as Label;
                    Label lbATCAmt = e.Row.FindControl("lbATCAmt") as Label;
                    Label lbOutAmt = e.Row.FindControl("lbOutAtcAmt") as Label;
                    TextBox txtAtcAmount = e.Row.FindControl("txtAtcAmount") as TextBox;
                    Label lbusr = e.Row.FindControl("lbusr") as Label;
                    //quantityTotal += Convert.ToInt32(lbQty.Text);
                    priceTotal += Convert.ToDecimal(lbsTot.Text);
                    if (User.IsInRole(deptIniRole) && budYr.IsActive==true)
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
                    if (User.IsInRole(deptApprRole)&& budYr.IsActive==true)
                    {
                        ck.Visible = true;
                        if (st.Text == ((int)Utility.BudgetItemStatus.HOD_Approved).ToString() || st.Text == ((int)Utility.BudgetItemStatus.Rejected).ToString() )
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
                        DAL.ExistingStaff exStf = CommonBLL.GetExistingStaff(recID);
                        if (exStf != null && exStf.Status == (int)Utility.BudgetItemStatus.Pending_HOD_Approval)
                        {
                            exStf.Status = (int)Utility.BudgetItemStatus.Rejected;
                           // exStf.ApprovedBy = usr.FullName;
                            //exStf.DateApproved = DateTime.Now;
                            CommonBLL.UpdateExistingStaff(exStf);
                            budgetInputer = UserBLL.GetUserByUserName(exStf.AddedBy);
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
            catch(Exception ex)
            {
                error.Visible = true;
                error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> An error occured. Kindly try again. If error persist contact Administrator!!.";
                Utility.WriteError("Error: " + ex.InnerException);
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
                        DAL.ExistingStaff exStf = CommonBLL.GetExistingStaff(recID);
                        if (exStf != null && exStf.Status == (int)Utility.BudgetItemStatus.Pending_HOD_Approval)
                        {
                            exStf.Status = (int)Utility.BudgetItemStatus.HOD_Approved;
                            exStf.ApprovedBy = usr.FullName;
                            exStf.DateApproved = DateTime.Now;
                            CommonBLL.UpdateExistingStaff(exStf);
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
                if(string.IsNullOrEmpty(txtcomment.Text))
                {
                    modalErr.Visible = true;
                    modalErr.InnerText = "Comment is required!!!";
                    mpeAppr.Show();
                    return;
                }
                bool isset = false; AppUser budgetInputer =new AppUser();
                foreach (GridViewRow row in gvDept.Rows)
                {
                    if (((CheckBox)row.FindControl("chkRow")).Checked)
                    {
                        Label lbID = row.FindControl("lbRecID") as Label;
                        int recID = int.Parse(lbID.Text);
                        DAL.ExistingStaff exStf = CommonBLL.GetExistingStaff(recID);
                        if (exStf != null && exStf.Status == (int)Utility.BudgetItemStatus.HOD_Approved)
                        {
                            exStf.Status = (int)Utility.BudgetItemStatus.Returned_For_Correction;
                            CommonBLL.UpdateExistingStaff(exStf);
                            budgetInputer = UserBLL.GetUserByUserName(exStf.AddedBy);
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
                        body = body.Replace("@BudgetElement", "Existing Staff");
                        body = body.Replace("@add_Comment", txtcomment.Text); //Replace the values from DB or any other source to personalize each mail.  
                        f1.Close();
                    }
                    string rst = "";
                    try
                    {
                        rst = Utility.SendMail(budgetInputer.Email, from,hodEmail, subject, body);
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
        protected void gvDept_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void btnClr_Click(object sender, EventArgs e)
        {
            ddlDeptFilter.SelectedValue = "";
            BindGrid();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlDeptFilter.SelectedValue != "")
                {
                    int dept = int.Parse(ddlDeptFilter.SelectedValue);
                    usr = (AppUser)Session["user"]; budYr = (BudgetYear)Session["budgetYr"];
                    if (User.IsInRole(AdminRole) || User.IsInRole(PBMgrRole) || User.IsInRole(PBOffRole))
                    {
                        List<TrackApproval> getTrackApproval = null;
                        getTrackApproval = LookUpBLL.GetTrackApproval(dept, budYr.ID).ToList();
                        if (getTrackApproval != null && getTrackApproval.Count() > 0)//if selected department has been pushed for approval
                        {
                            if (getTrackApproval.FirstOrDefault().status == (int)Utility.BudgetItemStatus.Pending_ReAlignment)
                            {
                                dvAdd.Visible = true;
                                btnSubmit.Enabled = false;
                            }
                        }
                        gvDept.DataSource = CommonBLL.GetExistStaffList(budYr.ID, dept);
                        gvDept.DataBind();
                    }
                }
            }
            catch
            {
            }
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
                      //  DAL.ExistingStaff exStf = CommonBLL.GetExistingStaff(recID);
                        DAL.ExistingStaff exStf = CommonBLL.GetExistingStaff(recID);
                        if (exStf != null && exStf.Status == (int)Utility.BudgetItemStatus.HOD_Approved)
                        {
                            exStf.Status = (int)Utility.BudgetItemStatus.Pending_ED_Approval;
                            CommonBLL.UpdateExistingStaff(exStf);
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
                        body = body.Replace("@BudgetElement", "Salary&Benefits Budget");
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
                batch = "SA-" + batch; decimal atcAmount = 0;

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
                        DAL.ExistingStaff expPro = CommonBLL.GetExistingStaff(recID);
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
                            CommonBLL.UpdateExistingStaff(expPro);
                            CommonBLL.AddExistingBudgetATC(new ExistingStaffBudgetATC() { ATCBatchId = batch, BudgetItemId = recID, BudgetItem = expPro.Grade.Name, ATCAmount = amt, DepartmentId = expPro.DepartmentID.Value, DateAdded = DateTime.Now, Status = (int)Utility.ATCStatus.Pending_PBManager_Approval });
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
                                body = body.Replace("@BudgetElement", "Salary&Benefit Budget");
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
                    DAL.ExistingStaff exStf = CommonBLL.GetExistingStaff(recID);
                    ExistingStaffBudgetATC atcitm = CommonBLL.GetExistingStaffBudgetATC(rwID);
                    if (exStf != null && exStf.ATCStatus == (int)Utility.ATCStatus.Pending_PBManager_Approval)
                    {
                        exStf.ATCStatus = (int)Utility.ATCStatus.Pending_MD_Approval;
                        atcitm.Status = (int)Utility.ATCStatus.Pending_MD_Approval;
                        CommonBLL.UpdateExistingStaff(exStf);
                        CommonBLL.UpdateExistingStaffBudgetATC(atcitm);
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
                        body = body.Replace("@BudgetElement", "Salary&Benefits Budget");
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
                    DAL.ExistingStaff exStf = CommonBLL.GetExistingStaff(recID);
                    ExistingStaffBudgetATC atcitm = CommonBLL.GetExistingStaffBudgetATC(rwID);
                    if (exStf != null && exStf.ATCStatus == (int)Utility.ATCStatus.Pending_MD_Approval)
                    {
                        exStf.ATCStatus = (int)Utility.ATCStatus.MD_Approved;
                        atcitm.Status = (int)Utility.ATCStatus.MD_Approved;
                        CommonBLL.UpdateExistingStaff(exStf);
                        CommonBLL.UpdateExistingStaffBudgetATC(atcitm);
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
                        body = body.Replace("@BudgetElement", "Salary&Benefit Budget");
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
                    DAL.ExistingStaff exStf = CommonBLL.GetExistingStaff(recID);
                    ExistingStaffBudgetATC atcitm = CommonBLL.GetExistingStaffBudgetATC(rwID);
                    if (exStf.ATCStatus == (int)Utility.ATCStatus.Pending_PBManager_Approval || exStf.ATCStatus == (int)Utility.ATCStatus.Pending_MD_Approval)
                    {
                        exStf.ATCStatus = (int)Utility.ATCStatus.Declined;
                        exStf.ATCApproved = false;
                        exStf.ATCAmount -= atcitm.ATCAmount;
                        atcitm.Status = (int)Utility.ATCStatus.Declined;
                        CommonBLL.UpdateExistingStaff(exStf);
                        CommonBLL.UpdateExistingStaffBudgetATC(atcitm);
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
                    btnATCForward.Enabled = false;
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