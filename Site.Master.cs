using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BudgetCapture.DAL;
using BudgetCapture.BLL;

namespace BudgetCapture
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        private string AdminRole = ConfigurationManager.AppSettings["adminRole"].ToString();
        private string PBMgrRole = ConfigurationManager.AppSettings["PBMgrRole"].ToString();
        private string PBOffRole = ConfigurationManager.AppSettings["PBOffRole"].ToString();
        private string EDRole = ConfigurationManager.AppSettings["EDRole"].ToString();
        private string MDRole = ConfigurationManager.AppSettings["MDRole"].ToString();
        private string DeptApproverRole = ConfigurationManager.AppSettings["DeptApprverRole"].ToString();
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
                        if (HttpContext.Current.User.IsInRole(AdminRole) || HttpContext.Current.User.IsInRole(PBMgrRole) || HttpContext.Current.User.IsInRole(PBOffRole) || HttpContext.Current.User.IsInRole(EDRole) || HttpContext.Current.User.IsInRole(MDRole))
                        {
                            dvAdmin.Visible = true;
                            dvAdminAll.Visible = true;
                            rListAdmin.DataSource = LookUpBLL.GetBudgetMenuList();
                            rListAdmin.DataBind();
                            dvATCMenu.Visible = true;
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
                            if (!HttpContext.Current.User.IsInRole(AdminRole) )
                            {
                                int deptId = userdata.DepartmentID.Value;
                                rListMenu.DataSource = LookUpBLL.GetDeptMenuItem(deptId);
                                rListMenu.DataBind();
                                dvUsers.Visible = true;
                            }
                            if (HttpContext.Current.User.IsInRole(DeptApproverRole))
                            {
                                dvATCMenu.Visible = true;
                            }
                            if (HttpContext.Current.User.IsInRole(EDRole) || HttpContext.Current.User.IsInRole(MDRole))
                            {
                                dvAdmin.Visible = false;
                                dvAdminAll.Visible = false;
                                //rListAdmin.DataSource = LookUpBLL.GetBudgetMenuList();
                                //rListAdmin.DataBind();
                                dvUsers.Visible = false;
                                //dvAsset.Visible = true;
                                //dvCredit.Visible = true;
                                // dvFinSer.Visible = true;
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
        
            }catch(Exception ex)
            {
            }
        }
    }
}
