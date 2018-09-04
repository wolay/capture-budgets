using BudgetCapture.BLL;
using BudgetCapture.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace BudgetCapture
{
    public partial class ReinvestmentPage : System.Web.UI.Page
    {
        private static AppUser usr = null; private static BudgetYear budYr = null;
        private string AdminRole = ConfigurationManager.AppSettings["adminRole"].ToString();
        private string deptIniRole = ConfigurationManager.AppSettings["DeptIniRole"].ToString();
        private string deptApprRole = ConfigurationManager.AppSettings["DeptApprverRole"].ToString();
        decimal priceTotal = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            error.Visible = false; success.Visible = false;
            dvAdd.Visible = false; dvAppr.Visible = false;
            try
            {
                if (Session["user"] != null)
                {
                    usr = (AppUser)Session["user"];
                    budYr = (BudgetYear)Session["budgetYr"];
                    if (User.IsInRole(deptApprRole) && budYr.IsActive == true)
                    {
                        dvAppr.Visible = true;
                    }
                    if (User.IsInRole(deptIniRole) && budYr.IsActive == true)
                    {
                        dvAdd.Visible = true;
                    }
                    if (User.IsInRole(AdminRole) && budYr.IsActive == true)
                    {
                        dvAdmin.Visible = true;
                    }
                }
                else
                {
                    Response.Redirect("Login.aspx", false);
                    return;
                }

                if (!IsPostBack)
                {
                    Utility.BindMonth(ddlMonth);
                    Utility.BindReinvestmentType(ddlCat);
                    Utility.BindReinvestmentType(ddlReinvestTyp);
                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                error.Visible = true;
                error.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button> An error occurred. kindly try again!!!";
                Utility.WriteError("Error: " + ex.InnerException);

            }
        }
        private void BindGrid()
        {
            usr = (AppUser)Session["user"]; budYr = (BudgetYear)Session["budgetYr"];
            if (User.IsInRole(AdminRole))
            {
                gvDept.DataSource = CommonBLL.GetReinvestmentList(budYr.ID);
                gvDept.DataBind();
            }
            else
            {
                gvDept.DataSource = CommonBLL.GetReinvestmentList(budYr.ID, usr.DepartmentID.Value);
                gvDept.DataBind();
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

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Reinvestment expPro = null;
            try
            {
                usr = (AppUser)Session["user"]; budYr = (BudgetYear)Session["budgetYr"];
                if (hid.Value == "Update")
                {
                    bool rst = false; decimal tot = 0; int month = int.Parse(ddlMonth.SelectedValue);
                    expPro = CommonBLL.GetReinvestment(Convert.ToInt32(txtID.Text));
                    if (expPro != null)
                    {
                        if (!decimal.TryParse(txtTot.Text, out tot))
                        {
                            error.Visible = true;
                            error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button>Amount must be numeric!!.";
                            return;
                        }
                        expPro.Amount = tot;
                        expPro.ReinvestmentID = int.Parse(ddlCat.SelectedValue);
                        expPro.DepartmentID = usr.DepartmentID;
                        expPro.Status = (int)Utility.BudgetItemStatus.Pending_Approval;
                        expPro.Month = month;
                        expPro.BudgetYrID = budYr.ID;
                        rst = CommonBLL.UpdateReinvestment(expPro);
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
                    decimal tot = 0;
                    if (!decimal.TryParse(txtTot.Text, out tot))
                    {
                        error.Visible = true;
                        error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button>Amount must be numeric!!.";
                        return;
                    }
                    bool result = false;
                    expPro = new Reinvestment(); int month = int.Parse(ddlMonth.SelectedValue);
                    expPro.ReinvestmentID = int.Parse(ddlCat.SelectedValue);
                    expPro.DepartmentID = usr.DepartmentID;
            
                    expPro.AddedBy = User.Identity.Name;
                    expPro.DateAdded = DateTime.Now;
                    expPro.Amount = tot;
                    expPro.BudgetYrID = budYr.ID;
                    expPro.Month = month;
                    expPro.Status = (int)Utility.BudgetItemStatus.Pending_Approval;

                    result = CommonBLL.AddReinvestment(expPro);
                    if (result)
                    {
                        BindGrid();
                        txtTot.Text = "";
                        ddlCat.SelectedValue = "";
                        ddlMonth.SelectedValue = "";
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                usr = (AppUser)Session["user"]; budYr = (BudgetYear)Session["budgetYr"];
                if (ddlReinvestTyp.SelectedValue != "")
                {
                    int expTy = int.Parse(ddlReinvestTyp.SelectedValue);
                    if (User.IsInRole(AdminRole))
                    {
                        gvDept.DataSource = CommonBLL.GetReinvestmentList(budYr.ID).Where(b => b.ReinvestmentID.Value == expTy).ToList();
                        gvDept.DataBind();
                    }
                    else
                    {
                        gvDept.DataSource = CommonBLL.GetReinvestmentList(budYr.ID, usr.DepartmentID.Value).Where(b => b.ReinvestmentID.Value == expTy).ToList();
                        gvDept.DataBind();
                    }
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
            try
            {
                ddlReinvestTyp.SelectedValue = "";
                BindGrid();
            }
            catch
            {
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
                
                ddlCat.SelectedValue = "";
                ddlMonth.SelectedValue = "";
            }
            catch
            {
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

        protected void gvDept_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                CheckBox chkheader = null; budYr = (BudgetYear)Session["budgetYr"];
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    chkheader = e.Row.FindControl("chkHeader") as CheckBox;
                    if (User.IsInRole(deptApprRole) && budYr.IsActive == true)
                    {
                        chkheader.Visible = true;
                    }
                    if (User.IsInRole(AdminRole) && budYr.IsActive == true)
                    {
                        chkheader.Visible = true;
                    }
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lbAmt = e.Row.FindControl("lbAmt") as Label;
                    priceTotal += Convert.ToDecimal(lbAmt.Text);

                    Label st = e.Row.FindControl("lbStatus") as Label;
                    CheckBox ck = e.Row.FindControl("chkRow") as CheckBox;

                    ImageButton imgEdit = e.Row.FindControl("imgBtnEdit") as ImageButton;
                    ImageButton imgDel = e.Row.FindControl("imgBtnDel") as ImageButton;


                    if (User.IsInRole(deptIniRole) && budYr.IsActive == true)
                    {
                        if (st.Text == ((int)Utility.BudgetItemStatus.Rejected).ToString() || st.Text == ((int)Utility.BudgetItemStatus.Pending_Approval).ToString() || st.Text == ((int)Utility.BudgetItemStatus.Returned_For_Correction).ToString())
                        {
                            imgDel.Visible = true;
                            imgEdit.Visible = true;
                        }
                    }
                    if (User.IsInRole(deptApprRole) && budYr.IsActive == true)
                    {
                        ck.Visible = true;
                        if (st.Text == ((int)Utility.BudgetItemStatus.Approved).ToString() || st.Text == ((int)Utility.BudgetItemStatus.Rejected).ToString())
                        {
                            //ck.Enabled = false;
                            ck.Visible = false;
                        }
                    }
                    if (User.IsInRole(AdminRole) && budYr.IsActive == true)
                    {
                        if (st.Text == ((int)Utility.BudgetItemStatus.Approved).ToString())
                        {
                            ck.Visible = true;
                        }
                        if (st.Text == ((int)Utility.BudgetItemStatus.Returned_For_Correction).ToString())
                        {
                            ck.Visible = false;
                        }
                    }
                }
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    Label lbSumAmt = e.Row.FindControl("lbSumAmt") as Label;

                    lbSumAmt.Text = priceTotal.ToString("N");
                }
            }
            catch (Exception ex)
            {
                error.Visible = true;
                error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> An error occured. Kindly try again. If error persist contact Administrator!!.";
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
                    Reinvestment estf = CommonBLL.GetReinvestment(index);
                    CommonBLL.DeleteReinvestment(estf);
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
                    Reinvestment estf = CommonBLL.GetReinvestment(index);
                    txtID.Text = estf.ID.ToString();
                     
                    txtTot.Text = estf.Amount.ToString();
                    ddlCat.SelectedValue = estf.ReinvestmentID.ToString();
                    ddlMonth.SelectedValue = estf.Month.ToString();
                }
            }
            catch (Exception ex)
            {
                error.Visible = true;
                error.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button> An error occurred. kindly try again!!!";
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
                        Reinvestment expPro = CommonBLL.GetReinvestment(recID);
                        if (expPro != null && expPro.Status == (int)Utility.BudgetItemStatus.Approved)
                        {
                            expPro.Status = (int)Utility.BudgetItemStatus.Returned_For_Correction;
                            CommonBLL.UpdateReinvestment(expPro);
                            budgetInputer = UserBLL.GetUserByUserName(expPro.AddedBy);
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
                        body = body.Replace("@BudgetElement", "Re-investment Projection");
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
                        Reinvestment expPro = CommonBLL.GetReinvestment(recID);
                        if (expPro != null && expPro.Status == (int)Utility.BudgetItemStatus.Pending_Approval)
                        {
                            expPro.Status = (int)Utility.BudgetItemStatus.Approved;
                            expPro.ApprovedBy = usr.FullName;
                            expPro.DateApproved = DateTime.Now;
                            CommonBLL.UpdateReinvestment(expPro);
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
                bool isset = false;
                foreach (GridViewRow row in gvDept.Rows)
                {
                    if (((CheckBox)row.FindControl("chkRow")).Checked)
                    {
                        Label lbID = row.FindControl("lbRecID") as Label;
                        int recID = int.Parse(lbID.Text);
                        Reinvestment expPro = CommonBLL.GetReinvestment(recID);
                        if (expPro != null && expPro.Status == (int)Utility.BudgetItemStatus.Pending_Approval)
                        {
                            expPro.Status = (int)Utility.BudgetItemStatus.Rejected;
                            // exStf.ApprovedBy = usr.FullName;
                            //exStf.DateApproved = DateTime.Now;
                            CommonBLL.UpdateReinvestment(expPro);
                            isset = true;
                        }
                    }
                }
                if (isset)
                {
                    BindGrid();
                    success.Visible = true;
                    success.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button>Selected Record(s) has been successfully rejected.";
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
    }
}