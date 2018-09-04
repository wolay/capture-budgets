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
    public partial class ManageSalaryElement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                  //  Utility.BindAssetType(ddlDir);
                    Utility.BindSalBenCategory(ddlCat);
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
            gvDir.DataSource = LookUpBLL.GetSalaryElementList();
            gvDir.DataBind();
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (hid.Value == "Update")
                {
                    SalaryElement ast = null; bool rst = false; decimal uprice = 0;
                    ast = LookUpBLL.GetSalaryElement(Convert.ToInt32(txtID.Text));
                    if (ast != null)
                    {

                        ast.Elements = txtName.Text.ToUpper();
                        ast.CategoryId = int.Parse(ddlCat.SelectedValue);
                        ast.Code = txtDept.Text;
                        if (chkActive.Checked)
                            ast.isActive = true;
                        else
                            ast.isActive = false;
                        rst = LookUpBLL.UpdateSalaryElement(ast);
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
                    string asstNme = ""; decimal uprice = 0;
                    asstNme = txtName.Text;
                     
                    bool result = false;
                    SalaryElement ast = new SalaryElement();
                    ast.Code = txtDept.Text;
                    ast.Elements = asstNme;
                    ast.CategoryId = int.Parse(ddlCat.SelectedValue);
                    if (chkActive.Checked)
                        ast.isActive = true;
                    else
                        ast.isActive = false;
                    
                    result = LookUpBLL.AddSalaryElement(ast);
                    if (result)
                    {
                        BindGrid();
                        txtName.Text = "";
                        ddlCat.SelectedValue = "";
                        txtDept.Text = "";
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

        protected void gvDir_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                hid.Value = "Update";
                dvID.Visible = true;
                dvMsg.InnerText = "Update Salary Element :";
                btnSubmit.Text = "Update";

                //GridViewRow row = gvCommon.SelectedRow; 
                string recID = "";
                recID = gvDir.SelectedDataKey.Value.ToString();
                //recID = (row.FindControl("lbID") as Label).Text; ;
                SalaryElement ast = LookUpBLL.GetSalaryElement(int.Parse(recID));
                if (ast != null)
                {
                    txtID.Text = recID;
                    txtName.Text = ast.Elements;
                    txtDept.Text = ast.Code;
                    ddlCat.SelectedValue = ast.CategoryId.ToString();
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
                BindGrid();
            }
            catch
            {
            }
        }
    }
}