using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BudgetCapture.BLL;
using System.Web.Security;
using BudgetCapture.DAL;
using System.IO;

namespace BudgetCapture.Admin
{
    public partial class UserSetup : System.Web.UI.Page
    {
        private string AdminRole = ConfigurationManager.AppSettings["adminRole"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            error.Visible = false; success.Visible = false;
            try
            {
                if(!IsPostBack)
                {
                    Utility.BindDept(ddlDept);
                    Utility.BindDirectorate(ddlDir);
                    BindGrid();
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void BindGrid()
        {
            gvUsers.DataSource = UserBLL.GetUsersList();
            gvUsers.DataBind();
        }
        private void Reset()
        {
            ddlDir.SelectedValue = ""; txtEmail.Text = "";
            txtfName.Text = ""; ddlDept.SelectedValue = "";
            ddlRole.SelectedValue = ""; txtStaffID.Text = "";
        }
        protected void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                string staffID = txtStaffID.Text.Trim();
                MembershipUserCollection usercol = Membership.FindUsersByName(staffID);
                if (usercol.Count != 0)
                {
                    error.Visible = true;
                    error.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button> This user already exist, pls add another user";
                    return;
                }
                AppUser urs = new AppUser();
                urs = Utility.GetUser(staffID);
               // if (ursList.Count > 0)
               if (urs != null)
                {
                    dvDetail.Visible = true;
                    //foreach (var u in ursList)
                    {
                        txtfName.Text = urs.FullName;
                        txtEmail.Text = urs.Email;
                        //txtfName.Text = "Test user";
                        //txtEmail.Text = "test@mail.com";
                        //txtDir.Text = urs.Directorate;
                        //ddlDept.SelectedItem.Text = u.DepartmentName.Trim();
                        //ddlDept.
                    }
                }
              else
              {
                  Reset();
                  dvDetail.Visible = true;
                  txtfName.Enabled = true;
                  txtEmail.Enabled = true;
                  error.Visible = true;
                  error.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button> User was NOT found. However, You can manually add user details!!.";
              }
            }
            catch (Exception ex)
            {
                error.Visible = true;
                error.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button> An error occurred " + ex.Message + " . Kindly try again. If error persist contact Administrator!!.";
                Utility.WriteError("Error: " + ex.InnerException);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string usrname = txtStaffID.Text;
            try
            {
                string email = txtEmail.Text;
                string fname = txtfName.Text;
                AppUser usr = new AppUser();
                usr.FullName = fname;
                usr.StaffID = txtStaffID.Text.Trim();
                usr.DepartmentID = Convert.ToInt32(ddlDept.SelectedValue);
                usr.Email = email;
                usr.UserRole = ddlRole.SelectedValue;
                usr.DateAdded = DateTime.Now;
                if (User.Identity.IsAuthenticated)
                    usr.AddedBy = User.Identity.Name;
                usr.isActive = true;
                MembershipUser user = Membership.CreateUser(usrname, "Pass@word", email);
                if (UserBLL.AddUser(usr))
                {
                    Roles.AddUserToRole(usrname, ddlRole.SelectedValue);
                    
                    //sending mail
                    string body = "";
                    string from = ConfigurationManager.AppSettings["exUser"].ToString();
                    string siteUrl = ConfigurationManager.AppSettings["siteUrl"].ToString();
                    string appLogo = ConfigurationManager.AppSettings["appLogoUrl"].ToString();
                    string subject = "User Creation Notification";
                    string FilePath = Server.MapPath("EmailTemplates/");
                    if (File.Exists(FilePath + "NewUserCreated.htm"))
                    {
                        FileStream f1 = new FileStream(FilePath + "NewUserCreated.htm", FileMode.Open);
                        StreamReader sr = new StreamReader(f1);
                        body = sr.ReadToEnd();
                        body = body.Replace("@add_appLogo", appLogo);
                        body = body.Replace("@siteUrl", siteUrl);
                        body = body.Replace("@add_fname", fname);
                        body = body.Replace("@add_username", usrname); //Replace the values from DB or any other source to personalize each mail.  
                        f1.Close();
                    }
                    string rst = Utility.SendMail(email, from, "", subject, body);
                    if (rst.Contains("Successful"))
                    {
                        Reset(); BindGrid();
                        success.Visible = true;
                        success.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> User was successfully created!!. A mail has been sent to the user";
                        return;
                    }
                    else
                    {
                        Reset(); BindGrid();
                        success.Visible = true;
                        success.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> User was successfully created!!. A mail could NOT be sent to the user at this time";
                        return;
                    }
                }
                else
                {
                    Membership.DeleteUser(usrname);
                    error.Visible = true;
                    error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> User Account could not be created. Kindly try again. If error persist contact Administrator!!.";
                    return;
                }
            }
            catch (Exception ex)
            {
                Membership.DeleteUser(usrname);
                error.Visible = true;
                error.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button> An error occurred " + ex.Message + " . Kindly try again. If error persist contact Administrator!!.";
                Utility.WriteError("Error: " + ex.InnerException);
            }
        }

        protected void gvUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(gvUsers.SelectedDataKey.Value.ToString());
                Response.Redirect(string.Format("UserDetail.aspx?id={0}", id), false);
            }
            catch (Exception ex)
            {
                Utility.WriteError("Error: " + ex.Message);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(txtSea.Value))
                {
                    string srch = txtSea.Value;
                    gvUsers.DataSource = UserBLL.GetUserByName(srch);
                    gvUsers.DataBind();
                }
            }
            catch
            {
            }
        }
        protected void btnClr_Click(object sender, EventArgs e)
        {
            try
            {
                txtSea.Value = "";
                BindGrid();
            }
            catch
            {
            }
        }
    }
}