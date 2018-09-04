using BudgetCapture.BLL;
using BudgetCapture.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BudgetCapture.Admin
{
    public partial class ManageCapexType : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            error.Visible = false; success.Visible = false;
            try
            {
                if (!IsPostBack)
                {
                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                error.Visible = true;
                error.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button> An error occurred. kindly try again!!!";
                Utility.WriteError("Error: " + ex.Message);
            }
        }
        private void BindGrid()
        {
            if (Session["budgetYr"] != null)
            {
                BudgetYear byr = (BudgetYear)Session["budgetYr"];
                gvDir.DataSource = LookUpBLL.GetCapexTypeList(byr.ID);
                gvDir.DataBind();
            }
            else
            {
                Response.Redirect("Login.aspx", false);
                return;
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (hid.Value == "Update")
                {
                    CapexType exp = null; bool rst = false;
                    exp = LookUpBLL.GetCapexType(Convert.ToInt32(txtID.Text));
                    if (exp != null)
                    {
                        exp.Name = txtDept.Text.ToUpper();
                        // exp.GLCode = txtcode.Text;
                        if (chkActive.Checked)
                            exp.isActive = true;
                        else
                            exp.isActive = false;
                        rst = LookUpBLL.UpdateCapexType(exp);
                        if (rst != false)
                        {
                            BindGrid();
                            success.Visible = true;
                            success.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> Record updated successfully!!.";
                            return;
                        }
                    }

                    else
                    {
                        error.Visible = true;
                        error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button>Record could Not updated. Kindly try again. If error persist contact Administrator!!.";
                    }
                }
                else
                {
                    BudgetYear byr = (BudgetYear)Session["budgetYr"];
                    bool result = false;
                    CapexType exp = new CapexType();
                    exp.Name = txtDept.Text.ToUpper();
                    exp.BudgetYearID = byr.ID;
                    // exp.GLCode = txtcode.Text;
                    if (chkActive.Checked)
                        exp.isActive = true;
                    else
                        exp.isActive = false;
                    result = LookUpBLL.AddCapexType(exp);
                    if (result)
                    {
                        BindGrid();
                        txtDept.Text = "";
                        //txtcode.Text = "";
                        success.Visible = true;
                        success.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> Record added successfully!!.";
                        return;
                    }
                    else
                    {
                        error.Visible = true;
                        error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button>Record could Not added. Kindly try again. If error persist contact Administrator!!.";
                    }
                }
            }
            catch (Exception ex)
            {
                error.Visible = true;
                error.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button> An error occurred. kindly try again!!!";
                Utility.WriteError("Error: " + ex.Message);
            }
        }

        protected void gvDir_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                hid.Value = "Update";
                dvID.Visible = true;
                dvMsg.InnerText = "Update DirectType :";
                btnSubmit.Text = "Update";

                GridViewRow row = gvDir.SelectedRow;
                txtID.Text = row.Cells[0].Text;
                //  txtcode.Text = row.Cells[1].Text.ToUpper();
                txtDept.Text = row.Cells[1].Text.ToUpper();
                if ((row.FindControl("cbCheckBox") as CheckBox).Checked)
                    chkActive.Checked = true;
                else
                    chkActive.Checked = false;
            }
            catch (Exception ex)
            {
                error.Visible = true;
                error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> An error occured. Kindly try again. If error persist contact Administrator!!.";
                Utility.WriteError("Error: " + ex.InnerException);
            }
        }

        protected void gvDir_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDir.PageIndex = e.NewPageIndex;
                BindGrid();
            }
            catch
            {
            }
        }
    }
}