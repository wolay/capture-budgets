using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BudgetCapture.DAL;
using System.Configuration;
using BudgetCapture.BLL;

namespace BudgetCapture
{
    public partial class DisbursementPro : System.Web.UI.Page
    {
        private static AppUser usr = null; private static BudgetYear budYr = null;
        decimal priceTotal = 0;
        private string AdminRole = ConfigurationManager.AppSettings["adminRole"].ToString();
        private string deptIniRole = ConfigurationManager.AppSettings["DeptIniRole"].ToString();
        private string deptApprRole = ConfigurationManager.AppSettings["DeptApprverRole"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            this.error.Visible = false;
            this.success.Visible = false;
            this.dvAdd.Visible = false;
            this.dvAppr.Visible = false;
            try
            {
                if (this.Session["user"] != null)
                {
                    bool? nullable;
                    usr = (AppUser)this.Session["user"];
                    budYr = (BudgetYear)this.Session["budgetYr"];
                    if (base.User.IsInRole(this.deptApprRole) && ((nullable = budYr.IsActive).GetValueOrDefault() && nullable.HasValue))
                    {
                        this.dvAppr.Visible = true;
                    }
                    if (base.User.IsInRole(this.deptIniRole) && ((nullable = budYr.IsActive).GetValueOrDefault() && nullable.HasValue))
                    {
                        this.dvAdd.Visible = true;
                    }
                }
                else
                {
                    base.Response.Redirect("Login.aspx", false);
                    return;
                }
                if (!base.IsPostBack)
                {
                    Utility.BindMonth(this.ddlMonth);
                    Utility.BindDisbursementList(this.ddlAsset);
                    Utility.BindDept(this.ddlDeptFilter);
                    this.lbDept.Text = usr.Department.Name.ToUpper();
                    this.lbyr.Text = budYr.Year;
                    this.BindGrid();
                    if (base.User.IsInRole(this.AdminRole))
                    {
                        this.dvAdminScr.Visible = true;
                    }
                }
            }
            catch (Exception exception)
            {
                this.error.Visible = true;
                this.error.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button> An error occurred. kindly try again!!!";
                Utility.WriteError("Error: " + exception.InnerException);
            }

        }
        private void BindGrid()
        {
            usr = (AppUser)Session["user"]; budYr = (BudgetYear)Session["budgetYr"];
            if (User.IsInRole(AdminRole))
            {
                gvDept.DataSource = CommonBLL.GetDisbursementList(budYr.ID);
                gvDept.DataBind();
            }
            else
            {
                gvDept.DataSource = CommonBLL.GetDisbursementList(budYr.ID, usr.DepartmentID.Value);
                gvDept.DataBind();
            }
        }
        //protected void btnLoad_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string agrNo = txtobligor.Text.Trim();
        //        Obligor obl = Utility.GetObligor(agrNo);
        //        if (obl != null)
        //        {
        //            lbName.Text = "Obligor's Name: " + obl.FullName.ToUpper();
        //            lbName.ForeColor = System.Drawing.Color.RoyalBlue;
        //            btnSubmit.Enabled = true;

        //        }
        //        else
        //        {
        //            lbName.Text = "Not Found";
        //            lbName.ForeColor = System.Drawing.Color.Maroon;
        //            btnSubmit.Enabled = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        error.Visible = true;
        //        error.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button> An error occurred. kindly try again!!!";
        //        Utility.WriteError("Error: " + ex.Message);
        //    }
        //}

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            DisbursementProjection recpro = null;
            try
            {
                decimal num;
                int num2;
                usr = (AppUser)this.Session["user"];
                budYr = (BudgetYear)this.Session["budgetYr"];
                if (this.hid.Value == "Update")
                {
                    num = 0M;
                    num2 = int.Parse(this.ddlMonth.SelectedValue);
                    recpro = CommonBLL.GetDisbursement(Convert.ToInt32(this.txtID.Text));
                    if (recpro != null)
                    {
                        if (!decimal.TryParse(this.txtTot.Text, out num))
                        {
                            this.error.Visible = true;
                            this.error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button>Amount must be numeric!!.";
                        }
                        else
                        {
                            recpro.Amount = new decimal?(num);
                            recpro.AssetName = this.ddlAsset.SelectedItem.Text;
                            recpro.DepartmentId = usr.DepartmentID;
                            recpro.Status = 1;
                            recpro.Month = new int?(num2);
                            recpro.BudgetYrID = new int?(budYr.ID);
                            if (CommonBLL.UpdateDisbursement(recpro))
                            {
                                this.BindGrid();
                                this.success.Visible = true;
                                this.success.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> Record updated successfully!!.";
                            }
                        }
                    }
                    else
                    {
                        this.error.Visible = true;
                        this.error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button>Record could Not updated. Kindly try again. If error persist contact Administrator!!.";
                    }
                }
                else
                {
                    num = 0M;
                    if (!decimal.TryParse(this.txtTot.Text, out num))
                    {
                        this.error.Visible = true;
                        this.error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button>Quantity must be numeric!!.";
                    }
                    else
                    {
                        recpro = new DisbursementProjection();
                        num2 = int.Parse(this.ddlMonth.SelectedValue);
                        recpro.AssetName = this.ddlAsset.SelectedItem.Text;
                        recpro.DepartmentId = usr.DepartmentID;
                        recpro.AddedBy = base.User.Identity.Name;
                        recpro.DateAdded = new DateTime?(DateTime.Now);
                        recpro.Amount = new decimal?(num);
                        recpro.BudgetYrID = new int?(budYr.ID);
                        recpro.Month = new int?(num2);
                        recpro.Status = 1;
                        if (CommonBLL.AddDisbursement(recpro))
                        {
                            this.BindGrid();
                            this.txtTot.Text = "";
                            this.ddlAsset.SelectedValue = "";
                            this.lbName.Text = "";
                            this.btnSubmit.Enabled = false;
                            this.ddlMonth.SelectedValue = "";
                            this.success.Visible = true;
                            this.success.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> Record added successfully!!.";
                        }
                        else
                        {
                            this.error.Visible = true;
                            this.error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button>Record could Not added. Kindly try again. If error persist contact Administrator!!.";
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.error.Visible = true;
                this.error.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button> An error occurred. kindly try again!!!";
                Utility.WriteError("Error: " + exception.Message);
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
                bool? nullable;
                CheckBox box = null;
                budYr = (BudgetYear)this.Session["budgetYr"];
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    box = e.Row.FindControl("chkHeader") as CheckBox;
                    if (base.User.IsInRole(this.deptApprRole) && ((nullable = budYr.IsActive).GetValueOrDefault() && nullable.HasValue))
                    {
                        box.Visible = true;
                    }
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    int num;
                    Label label = e.Row.FindControl("lbAmt") as Label;
                    this.priceTotal += Convert.ToDecimal(label.Text);
                    Label label2 = e.Row.FindControl("lbStatus") as Label;
                    CheckBox box2 = e.Row.FindControl("chkRow") as CheckBox;
                    ImageButton button = e.Row.FindControl("imgBtnEdit") as ImageButton;
                    ImageButton button2 = e.Row.FindControl("imgBtnDel") as ImageButton;
                    if (base.User.IsInRole(this.deptIniRole) && ((nullable = budYr.IsActive).GetValueOrDefault() && nullable.HasValue))
                    {
                        num = 3;
                        if ((label2.Text == num.ToString()) || (label2.Text == (num = 1).ToString()))
                        {
                            button2.Visible = true;
                            button.Visible = true;
                        }
                    }
                    if (base.User.IsInRole(this.deptApprRole) && ((nullable = budYr.IsActive).GetValueOrDefault() && nullable.HasValue))
                    {
                        box2.Visible = true;
                        num = 2;
                        if ((label2.Text == num.ToString()) || (label2.Text == (num = 3).ToString()))
                        {
                            box2.Visible = false;
                        }
                    }
                }
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    Label label3 = e.Row.FindControl("lbSumAmt") as Label;
                    label3.Text = this.priceTotal.ToString("N");
                }
            }
            catch (Exception exception)
            {
                this.error.Visible = true;
                this.error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> An error occured. Kindly try again. If error persist contact Administrator!!.";
                Utility.WriteError("Error: " + exception.InnerException);
            }

        }
        protected void gvDept_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "del")
                {
                    CommonBLL.DeleteDisbursement(CommonBLL.GetDisbursement(int.Parse(e.CommandArgument.ToString())));
                    this.BindGrid();
                    this.success.Visible = true;
                    this.success.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> Record deleted successfully!!.";
                }
                else if (e.CommandName == "edt")
                {
                    this.hid.Value = "Update";
                    this.dvID.Visible = false;
                    this.btnSubmit.Text = "Update";
                    this.btnSubmit.Enabled = true;
                    DisbursementProjection disbursement = CommonBLL.GetDisbursement(int.Parse(e.CommandArgument.ToString()));
                    this.txtID.Text = disbursement.ID.ToString();
                    this.ddlAsset.SelectedItem.Text = disbursement.AssetName;
                    this.txtTot.Text = disbursement.Amount.ToString();
                    this.ddlMonth.SelectedValue = disbursement.Month.ToString();
                }
            }
            catch (Exception exception)
            {
                this.error.Visible = true;
                this.error.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button> An error occurred. kindly try again!!!";
                Utility.WriteError("Error: " + exception.InnerException);
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
                        DisbursementProjection expPro = CommonBLL.GetDisbursement(recID);
                        if (expPro != null && expPro.Status == (int)Utility.BudgetItemStatus.Pending_Approval)
                        {
                            expPro.Status = (int)Utility.BudgetItemStatus.Approved;
                            expPro.ApprovedBy = usr.FullName;
                            expPro.DateApproved = DateTime.Now;
                            CommonBLL.UpdateDisbursement(expPro);
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
                        DisbursementProjection expPro = CommonBLL.GetDisbursement(recID);
                        if (expPro != null && expPro.Status == (int)Utility.BudgetItemStatus.Pending_Approval)
                        {
                            expPro.Status = (int)Utility.BudgetItemStatus.Rejected;
                            // exStf.ApprovedBy = usr.FullName;
                            //exStf.DateApproved = DateTime.Now;
                            CommonBLL.UpdateDisbursement(expPro);
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
        protected void lnkAddNew_Click(object sender, EventArgs e)
        {
            try
            {
                this.hid.Value = "";
                this.dvID.Visible = false;
                this.btnSubmit.Text = "Add";
                this.txtTot.Text = "";
                this.btnSubmit.Enabled = false;
                this.ddlAsset.SelectedValue = "";
                this.lbName.Text = "";
                this.txtTot.Text = "";
                this.ddlMonth.Items.Clear();
                Utility.BindMonth(this.ddlMonth);
            }
            catch (Exception)
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
                if (ddlDeptFilter.SelectedValue != "")
                {
                    int dept = int.Parse(ddlDeptFilter.SelectedValue);
                    usr = (AppUser)Session["user"]; budYr = (BudgetYear)Session["budgetYr"];
                    if (User.IsInRole(AdminRole))
                    {
                        gvDept.DataSource = CommonBLL.GetDisbursementList(budYr.ID, dept);
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