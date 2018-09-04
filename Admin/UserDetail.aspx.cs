using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BudgetCapture.BLL;
using BudgetCapture.DAL;
using System.Web.Security;

namespace BudgetCapture.Admin
{
    public partial class UserDetail : System.Web.UI.Page
    {
        private string AdminRole = ConfigurationManager.AppSettings["adminRole"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.IsInRole(AdminRole))
            {
                //Response.Redirect("../AccessDenied.aspx", false);
                //return;
            }
            if (!IsPostBack)
            {
                try
                {
                    if (Request.QueryString["id"] != null)
                    {
                        int Id = Convert.ToInt32(Request.QueryString["id"].ToString());
                        hid.Value = Id.ToString();
                        BindGrid(Id);
                        Utility.BindDept(ddlDept);
                    }
                    else
                    {
                        Response.Redirect("UserSetup.aspx", false);
                    }
                }
                catch (Exception ex)
                {
                    error.Visible = true;
                    error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> An error occured. Kindly try again. If error persist contact Administrator!!.";
                    Utility.WriteError("Error: " + ex.Message);
                }
            }
        }
        private void BindGrid(int Id)
        {
            gvheader.DataSource = UserBLL.GetUserByID(Id);
            gvheader.DataBind();
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(hid.Value);
                Label lbDeptID = gvheader.Rows[0].FindControl("lbDept") as Label;
                Label lbstaID = gvheader.Rows[0].FindControl("lbInit") as Label;
                string usrname = lbstaID.Text;
                AppUser usr = UserBLL.GetUserByUserName(usrname);
                if (ddlRole.SelectedValue != "")
                {
                    Label lbRole = gvheader.Rows[0].FindControl("lbrole") as Label;
                    Roles.RemoveUserFromRole(usrname, lbRole.Text);
                    Roles.AddUserToRole(usrname, ddlRole.SelectedValue);
                    usr.UserRole = ddlRole.SelectedValue;
                }
                if (ddlDept.SelectedValue != "")
                {
                    usr.DepartmentID = int.Parse(ddlDept.SelectedValue);
                }
                if(User.Identity.IsAuthenticated)
                usr.ModifiedBy = User.Identity.Name;
                usr.LastModified = DateTime.Now;
                if (UserBLL.UpdateUser(usr))
                {
                    //Roles.AddUserToRole(usrname, ddlRole.SelectedValue);
                    BindGrid(id);
                    success.Visible = true;
                    success.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button> Update was successful";
                }
                else
                {
                    error.Visible = true;
                    error.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button> An error occured. Kindly try again";
                }
            }
            catch (Exception ex)
            {
                error.Visible = true;
                error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> An error occured. Kindly try again. If error persist contact Administrator!!.";
                Utility.WriteError("Error: " + ex.Message);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Label lbstaID = gvheader.Rows[0].FindControl("lbEbl") as Label;
                bool status = bool.Parse(lbstaID.Text.Trim());
                int id = Convert.ToInt32(hid.Value);
                Label lbUsr = gvheader.Rows[0].FindControl("lbInit") as Label;
                string usrname = lbUsr.Text;
                AppUser usr = UserBLL.GetUserByUserName(usrname);
                 if(User.Identity.IsAuthenticated)
                usr.ModifiedBy = User.Identity.Name;
                usr.LastModified = DateTime.Now;
                if (status)
                {
                    usr.isActive = false;
                }
                else
                {
                    usr.isActive = true;
                }
              //  usr.isActive = status;
                if (UserBLL.UpdateUser(usr))
                {
                    //bool rst = UserBLL.UpdateUserById(id, status);
                    // if (rst)
                    BindGrid(id);
                    success.Visible = true;
                    success.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button> Update was successful";
                }
                else
                {
                    error.Visible = true;
                    error.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button> An error occured. Kindly try again";
                }
            }
            catch (Exception ex)
            {
                error.Visible = true;
                error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> An error occured. Kindly try again. If error persist contact Administrator!!.";
                Utility.WriteError("Error: " + ex.Message);
            }
        }
    }
}