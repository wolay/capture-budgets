using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using BudgetCapture.BLL;
using BudgetCapture.DAL;

namespace BudgetCapture
{
    public partial class GssHome : System.Web.UI.Page
    {
        CultureInfo culture = new CultureInfo("en-GB");
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                error.Visible = false; success.Visible = false;
                if (!IsPostBack)
                {

                    //if (User.IsInRole(CommSupRole))
                    //{
                    //    dvAppr.Visible = true;
                    //}
                    Utility.BindGssCustomer(ddlCustFilter);
                    DateTime from = new DateTime(0001, 1, 1);
                    DateTime to = DateTime.Now.AddYears(1);
                    BindGrid(from, to);
                }
            }
            catch (Exception ex)
            {
                error.Visible = true;
                error.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button> An error occurred. kindly try again!!!";
                Utility.WriteError("Error: " + ex.Message);
            }
        }
        private void BindGrid(DateTime from, DateTime to, int cust = 0)
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
            gvCommon.DataSource = GasSalesBLL.GetAggregateSalesList(from, to, cust);
            gvCommon.DataBind();
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime from; DateTime to; int cust = 0;
                if (!DateTime.TryParseExact(txtFilterDateFrom.Text, "dd/MM/yyyy", culture, DateTimeStyles.None, out from))
                {
                    from = new DateTime(0001, 1, 1);
                }
                if (!DateTime.TryParseExact(txtFilterDateTo.Text, "dd/MM/yyyy", culture, DateTimeStyles.None, out to))
                {
                    to = DateTime.Now.AddYears(1);
                }
                if (ddlCustFilter.SelectedValue != "")
                {
                    cust = int.Parse(ddlCustFilter.SelectedValue);
                }
                BindGrid(from, to, cust);
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnAll_Click(object sender, EventArgs e)
        {
            txtFilterDateFrom.Text = "";
            txtFilterDateTo.Text = "";
            ddlCustFilter.SelectedValue = "";
            DateTime from; DateTime to; int cust = 0;
            from = new DateTime(0001, 1, 1);
            to = DateTime.Now.AddYears(1);
            BindGrid(from, to, cust);
        }

        protected void gvCommon_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                 DateTime from; DateTime to;
                if (!DateTime.TryParseExact(txtFilterDateFrom.Text, "dd/MM/yyyy", culture, DateTimeStyles.None, out from))
                {
                    from = new DateTime(0001, 1, 1);

                }
                if (!DateTime.TryParseExact(txtFilterDateTo.Text, "dd/MM/yyyy", culture, DateTimeStyles.None, out to))
                {
                    to = DateTime.Now.AddYears(1);
                }
                gvCommon.PageIndex = e.NewPageIndex;
                BindGrid(from, to);
            }
            catch
            {
            }
        }

        protected void gvDept_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
    }
}