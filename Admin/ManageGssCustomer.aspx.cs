using BudgetCapture.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BudgetCapture.Admin
{
    public partial class ManageGssCustomer : System.Web.UI.Page
    {
        private string CommOffRole = ConfigurationManager.AppSettings["CommOffRole"].ToString();
        private string CommSupRole = ConfigurationManager.AppSettings["CommSupRole"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["success"] != null)
                {
                    int val = int.Parse(Request.QueryString["success"].ToString());
                    if (val == 1)
                    {
                        success.Visible = true;
                        success.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> Record added successfully!!.";
                       // return;
                    }
                    if (val == 2)
                    {
                        success.Visible = true;
                        success.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> Record updated successfully!!.";
                        // return;
                    }
                }
                if (!IsPostBack)
                {
                    BindGrid();
                    Utility.BindPipelineSection(ddlSection);
                    Utility.BindCustomerType(ddlType);
                }
            }
            catch (Exception ex)
            {
            }
        }
        private void BindGrid(int all=0)
        {
            int CustId = 0; string Custname = ""; int custtype = 0; int custsection = 0;
            if (Request.QueryString["newRecordId"] != null && all==0)
            {
               CustId = int.Parse(Request.QueryString["newRecordId"].ToString());
            }
            if(!string.IsNullOrEmpty(txtStaffID.Text))
            {
                CustId = int.Parse(txtStaffID.Text);
            }
            if (!string.IsNullOrEmpty(txtfName.Text))
            {
                Custname = txtfName.Text;
            }
            if (ddlSection.SelectedValue != "")
            {
                custsection = int.Parse(ddlSection.SelectedValue);
            }
            if (ddlSection.SelectedValue != "")
            {
                custtype = int.Parse(ddlType.SelectedValue);
            }
            gvDept.DataSource = LookUpBLL.GetGssCustomerListFilter(CustId, Custname, custtype, custsection);
            gvDept.DataBind();
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                BindGrid();
            }
            catch (Exception ex)
            {
            }
        }
        protected string GetPaymentTerm(object o)
        {
            try
            {
                return Utility.GetPaymentTerm(o);
            }
            catch
            {
                return "";
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
                Utility.WriteError("Error: " + ex.Message);
            }
        }

        protected void gvDept_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "View")
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    Response.Redirect("CustomerDetails.aspx?Id="+index.ToString(), false);
                     

                }
                if (e.CommandName == "edt")
                {
                    hid.Value = "Update";
                    //GridViewRow row = gvDept.SelectedRow;
                    int index = int.Parse(e.CommandArgument.ToString());
                    Response.Redirect("SetupCustomer.aspx?update=1&&Id="+index.ToString(), false);
                    return;
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
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    // Label lbQty = e.Row.FindControl("lbQty") as Label;
                  
                    ImageButton imgEdit = e.Row.FindControl("imgBtnEdit") as ImageButton;
                    ImageButton imgDel = e.Row.FindControl("imgBtnDel") as ImageButton;
                    if (User.IsInRole(CommOffRole)||User.IsInRole(CommSupRole))
                    {
                        //imgDel.Visible = true;
                        imgEdit.Visible = true;
                    }
                }

            }catch(Exception ex)
            {
            }
        }

        protected void btnAll_Click(object sender, EventArgs e)
        {
            try
            {
                ddlSection.SelectedValue="";
                ddlType.SelectedValue="";
                txtfName.Text="";txtStaffID.Text="";
                int all = 1;
                BindGrid(all);
            }catch(Exception ex)
            {

            }
        }
    }
}