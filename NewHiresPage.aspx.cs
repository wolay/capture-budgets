using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BudgetCapture.DAL;
using BudgetCapture.BLL;
using System.IO;

namespace BudgetCapture
{
    public partial class NewHiresPage : System.Web.UI.Page
    {
        private static AppUser usr = null; private static BudgetYear budYr = null;
        private string AdminRole = ConfigurationManager.AppSettings["adminRole"].ToString();
        private string deptIniRole = ConfigurationManager.AppSettings["DeptIniRole"].ToString();
        private string deptApprRole = ConfigurationManager.AppSettings["DeptApprverRole"].ToString();
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
                    Utility.BindGrade(ddlgrade);
                    Utility.BindMonth(ddlMonth);
                    Utility.BindDept(ddlDeptFilter);
                    lbDept.Text = usr.Department.Name.ToUpper();
                    lbyr.Text = budYr.Year;
                    BindGrid();
                    if (User.IsInRole(AdminRole))
                    {
                        dvAdminScr.Visible = true;
                    }
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
                gvDept.DataSource = CommonBLL.GetNewHireList(budYr.ID);
                gvDept.DataBind();
            }
            else
            {
                gvDept.DataSource = CommonBLL.GetNewHireList(budYr.ID, usr.DepartmentID.Value);
                gvDept.DataBind();
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            NewHire stf = null;
            try
            {
                usr = (AppUser)Session["user"]; budYr = (BudgetYear)Session["budgetYr"];
                Grade gd = LookUpBLL.GetGrade(int.Parse(ddlgrade.SelectedValue));
                if (hid.Value == "Update")
                {
                    bool rst = false; int tot = 0; int month = int.Parse(ddlMonth.SelectedValue);
                    stf = CommonBLL.GetNewHire(Convert.ToInt32(txtID.Text));
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
                        stf.DepartmentID = usr.DepartmentID;
                        stf.Status = (int)Utility.BudgetItemStatus.Pending_Approval;
                        stf.MonthYear = month;
                        if (gd != null)
                            stf.TotalCost = tot * gd.CostPerMonth*Utility.GetOutstandingMonth(month);
                        stf.BudgetYrID = budYr.ID;
                        rst = CommonBLL.UpdateNewHire(stf);
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
                    stf = new NewHire(); int month=int.Parse(ddlMonth.SelectedValue);
                    stf.GradeID = int.Parse(ddlgrade.SelectedValue);
                    stf.DepartmentID = usr.DepartmentID;
                    stf.AddedBy = User.Identity.Name;
                    stf.DateAdded = DateTime.Now;
                    stf.TotalNumber = tot;
                    stf.BudgetYrID = budYr.ID;
                    stf.MonthYear=int.Parse(ddlMonth.SelectedValue);
                    stf.Status = (int)Utility.BudgetItemStatus.Pending_Approval;
                    if (gd != null)
                        stf.TotalCost = tot * gd.CostPerMonth * Utility.GetOutstandingMonth(month);
                    result = CommonBLL.AddNewHire(stf);
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
            }
            catch (Exception ex)
            {
                error.Visible = true;
                error.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button> An error occurred. kindly try again!!!";
                Utility.WriteError("Error: " + ex.Message);
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
                        NewHire exStf = CommonBLL.GetNewHire(recID);
                        if (exStf != null && exStf.Status == (int)Utility.BudgetItemStatus.Approved)
                        {
                            exStf.Status = (int)Utility.BudgetItemStatus.Returned_For_Correction;
                            CommonBLL.UpdateNewHire(exStf);
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
                        body = body.Replace("@BudgetElement", "New Hires");
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
                         NewHire exStf = CommonBLL.GetNewHire(recID);
                        if (exStf != null && exStf.Status == (int)Utility.BudgetItemStatus.Pending_Approval)
                        {
                            exStf.Status = (int)Utility.BudgetItemStatus.Approved;
                            exStf.ApprovedBy = usr.FullName;
                            exStf.DateApproved = DateTime.Now;
                            CommonBLL.UpdateNewHire(exStf);
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
                        NewHire exStf = CommonBLL.GetNewHire(recID);
                        if (exStf != null && exStf.Status == (int)Utility.BudgetItemStatus.Pending_Approval)
                        {
                            exStf.Status = (int)Utility.BudgetItemStatus.Rejected;
                            // exStf.ApprovedBy = usr.FullName;
                            //exStf.DateApproved = DateTime.Now;
                            CommonBLL.UpdateNewHire(exStf);
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

        protected void gvDept_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "del")
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    // int key = Convert.ToInt32(gvDept.DataKeys[index].Value.ToString());
                    NewHire estf = CommonBLL.GetNewHire(index);
                    CommonBLL.DeleteNewHire(estf);
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
                    NewHire estf = CommonBLL.GetNewHire(index);
                    txtID.Text = estf.ID.ToString();
                    txtTot.Text = estf.TotalNumber.ToString();
                    ddlgrade.SelectedValue = estf.GradeID.ToString();
                    ddlMonth.SelectedValue = estf.MonthYear.ToString();
                }
            }
            catch (Exception ex)
            {
                error.Visible = true;
                error.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button> An error occurred. kindly try again!!!";
                Utility.WriteError("Error: " + ex.InnerException);
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
            }
            catch (Exception ex)
            {
                error.Visible = true;
                error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> An error occured. Kindly try again. If error persist contact Administrator!!.";
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
                ddlMonth.SelectedValue = "";
            }
            catch
            {
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlDeptFilter.SelectedValue != "")
                {
                    int dept = int.Parse(ddlDeptFilter.SelectedValue);
                    usr = (AppUser)Session["user"]; budYr = (BudgetYear)Session["budgetYr"];
                    if (User.IsInRole(AdminRole))
                    {
                        gvDept.DataSource = CommonBLL.GetNewHireList(budYr.ID, dept);
                        gvDept.DataBind();
                    }
                }
            }
            catch
            {
            }
        }

        protected void btnClr_Click(object sender, EventArgs e)
        {
            ddlDeptFilter.SelectedValue = "";
            BindGrid();
        }
    }
}