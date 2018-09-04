using BudgetCapture.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BudgetCapture
{
    public partial class SiteGSS : System.Web.UI.MasterPage
    {
        private string GssAdminRole = ConfigurationManager.AppSettings["GssAdminRole"].ToString();
        private string CommOffRole = ConfigurationManager.AppSettings["CommOffRole"].ToString();
        private string CommSupRole = ConfigurationManager.AppSettings["CommSupRole"].ToString();
        private string WoDOffRole = ConfigurationManager.AppSettings["WoDOffRole"].ToString();
        private string WoDSupRole = ConfigurationManager.AppSettings["WoDSupRole"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    Response.Redirect("~/Login.aspx", false);
                    return;
                }
                if (Session["user"] == null)
                {
                    Response.Redirect("~/Login.aspx", false);
                    return;
                }
                if (!IsPostBack)
                {
                    if (HttpContext.Current.User.Identity.IsAuthenticated)
                    {
                        if (HttpContext.Current.User.IsInRole(GssAdminRole))
                        {
                            dvAdmin.Visible = true;
                            
                            //dvAsset.Visible = true;
                            //dvCredit.Visible = true;
                            // dvFinSer.Visible = true;
                        }
                        if (HttpContext.Current.User.IsInRole(CommOffRole) || HttpContext.Current.User.IsInRole(CommSupRole))
                        {
                            dvComm.Visible = true;
                            //dvAsset.Visible = true;
                            //dvCredit.Visible = true;
                            // dvFinSer.Visible = true;
                        }
                        if (HttpContext.Current.User.IsInRole(WoDOffRole) || HttpContext.Current.User.IsInRole(WoDSupRole))
                        {
                            dvOps.Visible = true;
                            //dvAsset.Visible = true;
                            //dvCredit.Visible = true;
                            // dvFinSer.Visible = true;
                        }
                        Label lblfnm = (Label)LoginView1.FindControl("lbFName");
                        Label lblRole = (Label)LoginView1.FindControl("lbRole");
                        AppUser userdata = new AppUser();
                        if (Session["user"] != null)
                        {
                            userdata = (AppUser)Session["user"];
                            // userdata=UserBLL.GetUserByUserName(HttpContext.Current.User.Identity.Name);
                            lblfnm.Text = userdata.FullName;
                            lblRole.Text = userdata.UserRole;
                            if (!HttpContext.Current.User.IsInRole(GssAdminRole))
                            {
                                //int deptId = userdata.DepartmentID.Value;
                                //rListMenu.DataSource = LookUpBLL.GetDeptMenuItem(deptId);
                                //rListMenu.DataBind();
                                //dvUsers.Visible = true;
                            }
                             
                        }
                        else
                        {
                            Response.Redirect("~/Login.aspx", false);
                            return;
                        }
                    }
                    else
                    {
                        Response.Redirect("~/Login.aspx", false);
                        return;
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }
    }
}