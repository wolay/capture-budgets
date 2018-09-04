using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BudgetCapture.BLL;
using BudgetCapture.DAL;
using System.Web.Security;
using System.Configuration;

namespace BudgetCapture
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Utility.BindBudgetYear(ddlBudgetYr);
                }

            }
            catch(Exception ex)
            {
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
            string rolname = ""; string ursname = ""; string pwd = "";
            bool a = false; bool b = false; bool c = false; bool d = false;
            bool f = false; bool g = false; bool h = false;
            bool gss_a = false; bool gss_cmo = false; bool gss_cms = false;
            string val = ""; bool gss_wdo = false; bool gss_wds = false;
            ursname = username.Value.Trim();
            pwd = password.Value.Trim();
            ADAuth.Service ADSvr = new ADAuth.Service();
            //val = ADSvr.ADValidateUser(ursname, pwd);
            val = "true";
            if (val.ToLower().Contains("true"))
            {
                AppUser usr = new AppUser();
                usr = UserBLL.GetUserByUserName(ursname);
                if (usr != null)
                {
                    if (!usr.isActive.Value)
                    {
                        error.Visible = true;
                        error.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button> Sorry you profile is NOT active.";
                        return;
                    }
                    Session["user"] = usr;
                }
                else
                {
                    error.Visible = true;
                    error.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button> You do NOT have access to this application!!!";
                    return;
                }
                
                FormsAuthentication.SetAuthCookie(ursname, false);
                string[] rol = Roles.GetRolesForUser(ursname);
                if (rol.Length != 0)
                {
                    rolname = rol[0];

                    if (rolname == ConfigurationManager.AppSettings["adminRole"].ToString())
                    { a = true; }
                    if (rolname == ConfigurationManager.AppSettings["DeptIniRole"].ToString())
                    { b = true; }
                    if (rolname == ConfigurationManager.AppSettings["DeptApprverRole"].ToString())
                    { c = true; }

                    if (rolname == ConfigurationManager.AppSettings["PBOffRole"].ToString())
                    { d = true; }
                    if (rolname == ConfigurationManager.AppSettings["PBMgrRole"].ToString())
                    { f = true; }
                    if (rolname == ConfigurationManager.AppSettings["EDRole"].ToString())
                    { g = true; }
                    if (rolname == ConfigurationManager.AppSettings["MDRole"].ToString())
                    { h = true; }
                    if (rolname == ConfigurationManager.AppSettings["GssAdminRole"].ToString())
                    { gss_a = true; }
                    if (rolname == ConfigurationManager.AppSettings["CommOffRole"].ToString())
                    { gss_cmo = true; }
                    if (rolname == ConfigurationManager.AppSettings["CommSupRole"].ToString())
                    { gss_cms = true; }
                    if (rolname == ConfigurationManager.AppSettings["WoDOffRole"].ToString())
                    { gss_wdo = true; }
                    if (rolname == ConfigurationManager.AppSettings["WoDSupRole"].ToString())
                    { gss_wds = true; }
                    if (a || b || c|| d || f|| g|| h)
                    {
                        //Response.Redirect("Default.aspx", false);
                        mpeAppr.Show();
                        return;
                    }
                    if (gss_a || gss_cmo || gss_cms || gss_wdo || gss_wds)
                    {
                        Response.Redirect("GssHome.aspx", false);
                        return;
                    }
                    else
                    {
                        Response.Redirect("Login.aspx", false);
                        return;
                    }
                }
                
            }
            else
            {
                error.Visible = true;
                error.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button> Incorrect username or password. Kindly verify your credentials!!!";
            }
            }
            catch (Exception ex)
            {
                error.Visible = true;
                error.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button> An error occurred. kindly try again!!!";
                Utility.WriteError("Error: " + ex.InnerException+ "\\stackTrace"+ ex.StackTrace);
                return;
            }
        }

        protected void btnAppr_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlBudgetYr.SelectedValue != "")
                {
                    BudgetYear byr = new BudgetYear();
                    byr = LookUpBLL.GetBudgetYear(int.Parse(ddlBudgetYr.SelectedValue));
                    Session["budgetYr"] = byr;
                    Response.Redirect("Default.aspx", false);
                    return;
                }
                else
                {
                    modalErr.Visible = true;
                    modalErr.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button> Kindly select Budget year and Proceed";
                    mpeAppr.Show();
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}