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
    public partial class ManagePipelineSection : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    // Utility.BindRecoveryType(ddlDir);

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
            gvCommon.DataSource = LookUpBLL.GetPipelineSectionList(false);
            gvCommon.DataBind();
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                decimal totcap = 0;
                if (hid.Value == "Update")
                {
                    GSS_PipelineSection ast = null; bool rst = false; 
                    ast = LookUpBLL.GetpipelineSection(Convert.ToInt32(txtID.Text));
                    if (ast != null)
                    {
                        ast.Name = txtAsset1.Text;
                        if(!decimal.TryParse(txtcap.Text,out totcap))
                        {
                            error.Visible=true;
                            error.InnerHtml="<button type='button' class='close' data-dismiss='alert'>&times;</button>Total capacity must be numeric!!.";
                            return;
                        }
                        ast.TotalCapacity = totcap;
                        if (chkActive.Checked)
                            ast.isActive = true;
                        else
                            ast.isActive = false;
                        //ast.RecoveryTypeID = int.Parse(ddlDir.SelectedValue);
                        rst = LookUpBLL.UpdatePipelineSection(ast);
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
                    string asstNme = "";
                    asstNme = txtAsset1.Text;
                    if (!decimal.TryParse(txtcap.Text, out totcap))
                    {
                        error.Visible = true;
                        error.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button>Total capacity must be numeric!!.";
                        return;
                    }
                    bool result = false;
                    GSS_PipelineSection ast = new GSS_PipelineSection();
                    ast.Name = asstNme;
                    ast.TotalCapacity = totcap;
                    if (chkActive.Checked)
                        ast.isActive = true;
                    else
                        ast.isActive = false;
                    // ast.RecoveryTypeID = int.Parse(ddlDir.SelectedValue);

                    result = LookUpBLL.AddPipelineSection(ast);
                    if (result)
                    {
                        BindGrid();
                        txtAsset1.Text = "";
                        txtcap.Text = "";
                        // ddlDir.SelectedValue = "";

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
                error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> An error occured. Kindly try again. If error persist contact Administrator!!.";
                Utility.WriteError("Error: " + ex.InnerException);
            }
        }

         
        protected void gvCommon_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            
        }

        protected void gvCommon_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                hid.Value = "Update";
                dvID.Visible = true;
                dvMsg.InnerText = "Update Pipeline Section:";
                btnSubmit.Text = "Update";

                GridViewRow row = gvCommon.SelectedRow; string recID = "";
                recID = (row.FindControl("lbID") as Label).Text; 
                GSS_PipelineSection ast = LookUpBLL.GetpipelineSection(int.Parse(recID));
                if (ast != null)
                {
                    txtID.Text = recID;
                    txtAsset1.Text = ast.Name;
                    txtcap.Text = ast.TotalCapacity.Value.ToString();
                    if (ast.isActive.HasValue && ast.isActive.Value)
                        chkActive.Checked = true;
                    else
                        chkActive.Checked = false;
                    //ddlDir.SelectedValue = (row.FindControl("lbDir") as Label).Text; ;
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
        protected void gvCommon_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvCommon.PageIndex = e.NewPageIndex;
                BindGrid();
            }
            catch
            {
            }
        }
    }
}