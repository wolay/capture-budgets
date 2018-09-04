using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BudgetCapture.BLL;
using BudgetCapture.DAL;

namespace BudgetCapture.Admin
{
    public partial class ManageGrades : System.Web.UI.Page
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
            gvDir.DataSource = LookUpBLL.GetGradeList();
            gvDir.DataBind();
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            decimal costmth = 0; decimal costyr = 0;
            try
            {
                if (hid.Value == "Update")
                {
                    Grade grd = null; bool rst = false; 
                    grd = LookUpBLL.GetGrade(Convert.ToInt32(txtID.Text));
                    if (grd != null)
                    {
                    //    if (!decimal.TryParse(txtCostMth.Text, out costmth) || !decimal.TryParse(txtCostYr.Text, out costyr))
                    //    {
                    //        error.Visible = true;
                    //        error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button>Cost must be numeric!!.";
                    //        return;
                    //    }
                        grd.Name =txtDept.Text.ToUpper();
                        grd.CostPerMonth = costmth;
                        grd.CostPerYear = costyr;
                        if (chkActive.Checked)
                            grd.isActive = true;
                        else
                            grd.isActive = false;
                        rst = LookUpBLL.UpdateGrade(grd);
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
                   // int mak = 0;
                    //if (!decimal.TryParse(txtCostMth.Text, out costmth) || !decimal.TryParse(txtCostYr.Text, out costyr))
                    //{
                    //    error.Visible = true;
                    //    error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button>Cost must be numeric!!.";
                    //    return;
                    //} 
                    bool result = false;
                    Grade grd = new Grade();
                    grd.Name = txtDept.Text.ToUpper();
                    grd.CostPerMonth = costmth;
                    grd.CostPerYear = costyr;
                    if(chkActive.Checked)
                    grd.isActive = true;
                    else
                    grd.isActive = false;
                    result = LookUpBLL.AddGrade(grd);
                    if (result)
                    {
                        BindGrid();
                        txtDept.Text = "";
                       // txtCostYr.Text = "";
                       // txtCostMth.Text = "";
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
                dvMsg.InnerText = "Update Grade :";
                btnSubmit.Text = "Update";

                GridViewRow row = gvDir.SelectedRow;
                txtID.Text = row.Cells[0].Text;
                txtDept.Text = row.Cells[1].Text.ToUpper();
              //  txtCostMth.Text = row.Cells[2].Text.ToUpper();
              //  txtCostYr.Text = row.Cells[3].Text.ToUpper();
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