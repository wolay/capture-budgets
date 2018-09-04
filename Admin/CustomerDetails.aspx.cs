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
    public partial class CustomerDetails : System.Web.UI.Page
    {
        private string CommOffRole = ConfigurationManager.AppSettings["CommOffRole"].ToString();
        private string CommSupRole = ConfigurationManager.AppSettings["CommSupRole"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (User.IsInRole(CommOffRole) || User.IsInRole(CommSupRole))
                {
                    dvAdmin.Visible = true;
                }
                if(Request.QueryString["Id"]!=null)
                {
                    int Id = int.Parse(Request.QueryString["Id"].ToString());
                    hid.Value = Id.ToString();
                    gvheader.DataSource = LookUpBLL.GetGssCustomerById(Id);
                    gvheader.DataBind();
                }
            }
            catch(Exception ex)
            {
                error.Visible = true;
                error.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button> An error occurred. kindly try again!!!";
                Utility.WriteError("Error: " + ex.Message);
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

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(hid.Value);
                Response.Redirect("SetupCustomer.aspx?update=1&&Id=" + id.ToString(), false);
                return;
            }
            catch (Exception ex)
            {
            }
        }

    }
}