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
    public partial class ManageGradeSalElement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            error.Visible = false; success.Visible = false;
            try
            {
                if (!IsPostBack)
                {
                    Utility.BindGrade(ddlGrd);
                    Utility.BindSalaryElement(ddlEle);
                    Utility.BindGrade(ddlGradeFilter);
                    Utility.BindSalaryElement(ddlElemFilter);
                    BindGrid(0,0);
                }
            }
            catch (Exception ex)
            {
                error.Visible = true;
                error.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button> An error occurred. kindly try again!!!";
                Utility.WriteError("Error: " + ex.Message);
            }
        }
        private void BindGrid(int grade, int elem)
        {
            gvDir.DataSource = LookUpBLL.GetGradeSalElementList(grade,elem);
            gvDir.DataBind();
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            decimal Amt = 0; decimal costyr = 0;
            try
            {
                if (hid.Value == "Update")
                {
                    GradeSalaryElement grd = null; bool rst = false;
                    grd = LookUpBLL.GetGradeSalElement(Convert.ToInt32(hidGrd.Value),Convert.ToInt32(hidEle.Value));
                    if (grd != null)
                    {
                        if (!decimal.TryParse(txtAmt.Text, out Amt) )
                        {
                            error.Visible = true;
                            error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button>Cost must be numeric!!.";
                            return;
                        }
                        grd.SalElementID = int.Parse(ddlEle.SelectedValue);
                        grd.GradeId = int.Parse(ddlGrd.SelectedValue);
                        grd.Amount = Amt;
                        
                        if (chkActive.Checked)
                            grd.isActive = true;
                        else
                            grd.isActive = false;
                        rst = LookUpBLL.UpdateGradeSalElement(grd);
                        if (rst != false)
                        {
                            BindGrid(0,0);
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
                    if (!decimal.TryParse(txtAmt.Text, out Amt))
                    {
                        error.Visible = true;
                        error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button>Cost must be numeric!!.";
                        return;
                    } 
                    bool result = false;
                    GradeSalaryElement grd = new GradeSalaryElement();
                    grd.SalElementID = int.Parse(ddlEle.SelectedValue);
                    grd.GradeId = int.Parse(ddlGrd.SelectedValue);
                    grd.Amount = Amt;

                    if (chkActive.Checked)
                        grd.isActive = true;
                    else
                        grd.isActive = false;
                    result = LookUpBLL.AddGradeSalElement(grd);
                    if (result)
                    {
                        BindGrid(0,0);
                        ddlGrd.SelectedValue = "";
                        ddlEle.SelectedValue = "";
                        txtAmt.Text = "";
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
                dvMsg.InnerText = "Update Grade Salary Element Value :";
                btnSubmit.Text = "Update";

               // GridViewRow row = gvCommon.SelectedRow; 
                string grdId = ""; string elemId = "";
                grdId = gvDir.SelectedDataKey.Values[1].ToString() ;
                elemId = gvDir.SelectedDataKey.Values[0].ToString();
                GradeSalaryElement ast = LookUpBLL.GetGradeSalElement(int.Parse(grdId),int.Parse(elemId));
                if (ast != null)
                {
                   // txtID.Text = recID;
                    hidEle.Value = ast.SalElementID.ToString();
                    hidGrd.Value = ast.GradeId.ToString();
                    ddlEle.SelectedValue = ast.SalElementID.ToString();
                    ddlGrd.SelectedValue = ast.GradeId.ToString();
                    txtAmt.Text = ast.Amount.Value.ToString("N");
                    if (ast.isActive.HasValue && ast.isActive.Value)
                        chkActive.Checked = true;
                    else
                        chkActive.Checked = false;
                    //ddlDir.SelectedItem.Text = HttpUtility.HtmlDecode(row.Cells[2].Text).ToLower();
                }
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
                BindGrid(0,0);
            }
            catch
            {
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                int gdval = 0; int elemval = 0;
                if (ddlGradeFilter.SelectedValue != "")
                {
                    gdval = int.Parse(ddlGradeFilter.SelectedValue);
                }
                if (ddlElemFilter.SelectedValue != "")
                {
                    elemval = int.Parse(ddlElemFilter.SelectedValue);
                } 
                BindGrid(gdval,elemval) ;
                  
                //}
            }
            catch
            {
            }
        }

        protected void btnClr_Click(object sender, EventArgs e)
        {
            ddlElemFilter.SelectedValue = "";
            ddlGradeFilter.SelectedValue = "";
            BindGrid(0, 0);
        }

    }
}