using BudgetCapture.BLL;
using BudgetCapture.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BudgetCapture.GSS
{
    public partial class ApproveCertifiedVolume : System.Web.UI.Page
    {
        private string CommSupRole = ConfigurationManager.AppSettings["CommSupRole"].ToString();
        CultureInfo culture = new CultureInfo("en-GB");
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                error.Visible = false; success.Visible = false;
                if (!IsPostBack)
                {

                    if (User.IsInRole(CommSupRole))
                    {
                        dvAppr.Visible = true;
                    }
                    Utility.BindGssCustomer(ddlCustFilter);
                    BindGrid(DateTime.Now.AddDays(40));
                }
            }
            catch (Exception ex)
            {
                error.Visible = true;
                error.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button> An error occurred. kindly try again!!!";
                Utility.WriteError("Error: " + ex.Message);
            }
        }
        protected string GetStatus(object o)
        {
            try
            {
                return Utility.GetSalesStatus(o);
            }
            catch
            {
                return "";
            }
        }
        private void BindGrid(DateTime filter, int cust = 0)
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
            gvCommon.DataSource = GasSalesBLL.GetGasSalesList((int)Utility.SalesStatus.Pending_Certified_Approval, filter, cust);
            gvCommon.DataBind();
        }
        protected void gvCommon_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvCommon.PageIndex = e.NewPageIndex;
                BindGrid(DateTime.Now.AddDays(40));
            }
            catch
            {
            }
        }
        protected void gvCommon_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                CheckBox chkheader = null;
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    chkheader = e.Row.FindControl("chkHeader") as CheckBox;
                    if (User.IsInRole(CommSupRole))
                    {
                        chkheader.Visible = true;
                    }
                    if (User.IsInRole(CommSupRole))
                    {
                        chkheader.Visible = true;
                    }
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    // Label lbQty = e.Row.FindControl("lbQty") as Label;
                    Label st = e.Row.FindControl("lbStatus") as Label;
                    CheckBox ck = e.Row.FindControl("chkRow") as CheckBox;
                    if (User.IsInRole(CommSupRole))
                    {
                        ck.Visible = true;
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

        protected void btnApprv_Click(object sender, EventArgs e)
        {
            try
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
                    foreach (GridViewRow row in gvCommon.Rows)
                    {
                        if (((CheckBox)row.FindControl("chkRow")).Checked)
                        {
                            Label lbID = row.FindControl("lbRecID") as Label;
                            int recID = int.Parse(lbID.Text);
                            GSS_SalesTbl expPro = GasSalesBLL.GetGasSale(recID);
                            if (expPro != null && expPro.Status == (int)Utility.SalesStatus.Pending_Certified_Approval)
                            {
                                expPro.Status = (int)Utility.SalesStatus.Certified_Volume_Approved;
                                expPro.CertifiedApprovedby = usr.ID;

                                GasSalesBLL.UpdateSalesData(expPro);
                                isset = true;
                            }
                        }
                    }
                    if (isset)
                    {
                        BindGrid(DateTime.Now.AddDays(40));
                        success.Visible = true;
                        success.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button>Selected Record(s) has been successfully approved.";
                        return;
                    }
                    else
                    {
                        BindGrid(DateTime.Now.AddDays(40));
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
            catch (Exception ex)
            {
            }
        }
        protected void btnReject_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    bool isset = false;
                    foreach (GridViewRow row in gvCommon.Rows)
                    {
                        if (((CheckBox)row.FindControl("chkRow")).Checked)
                        {
                            Label lbID = row.FindControl("lbRecID") as Label;
                            int recID = int.Parse(lbID.Text);
                            GSS_SalesTbl expPro = GasSalesBLL.GetGasSale(recID);
                            if (expPro != null && expPro.Status == (int)Utility.SalesStatus.Pending_Certified_Approval)
                            {
                                expPro.Status = (int)Utility.SalesStatus.Captured_Volume_Approved;
                                // exStf.ApprovedBy = usr.FullName;
                                //exStf.DateApproved = DateTime.Now;
                                GasSalesBLL.UpdateSalesData(expPro);
                                isset = true;
                            }
                        }
                    }
                    if (isset)
                    {
                        BindGrid(DateTime.Now.AddDays(40));
                        success.Visible = true;
                        success.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button>Selected Record(s) has been successfully rejected.";
                        return;
                    }
                    else
                    {
                        BindGrid(DateTime.Now.AddDays(40));
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
            catch (Exception ex)
            {
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                int cust = 0; DateTime filter = DateTime.Now.AddDays(40); bool u = false;
                if (ddlCustFilter.SelectedValue != "")
                {
                    cust = int.Parse(ddlCustFilter.SelectedValue);
                }
                if (!DateTime.TryParseExact(txtFilterDate.Text, "dd/MM/yyyy", culture, DateTimeStyles.None, out filter))
                {
                    filter = DateTime.Now.AddDays(40);
                }
                BindGrid(filter, cust);
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
                ddlCustFilter.SelectedValue = "";
                txtFilterDate.Text = "";
                BindGrid(DateTime.Now.AddDays(40));
            }
            catch (Exception ex)
            {
            }
        }
    }
}