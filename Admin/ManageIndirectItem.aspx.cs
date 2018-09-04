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
    public partial class ManageIndirectItem : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Utility.BindIndirectType(ddlDir);
                    Utility.BindIndirectType(ddlAstTyp);
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
            gvCommon.DataSource = LookUpBLL.GetIndirectItemList();
            gvCommon.DataBind();
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (hid.Value == "Update")
                {
                    IndirectBudgetItem ast = null; bool rst = false; decimal uprice = 0;
                    ast = LookUpBLL.GetIndirectItem(Convert.ToInt32(txtID.Text));
                    if (ast != null)
                    {
                        ast.Name = txtName.Text;
                        ast.IndirectTypeID = int.Parse(ddlDir.SelectedValue);
                        ast.Code = txtcode.Text;
                        ast.DelFlg = "N";
                        rst = LookUpBLL.UpdateIndirectItem(ast);
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
                    IndirectBudgetItem ast = new IndirectBudgetItem();
                    ast.Name = txtName.Text;
                    ast.IndirectTypeID = int.Parse(ddlDir.SelectedValue);
                    ast.Code = txtcode.Text;
                    ast.DelFlg = "N";
                    result = LookUpBLL.AddIndirectItem(ast);
                    if (result)
                    {
                        BindGrid();
                        txtName.Text = "";
                        ddlDir.SelectedValue = "";
                        txtcode.Text = "";
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

        protected void gvCommon_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                hid.Value = "Update";
                dvID.Visible = true;
                dvMsg.InnerText = "Update IndirectItem :";
                btnSubmit.Text = "Update";

                GridViewRow row = gvCommon.SelectedRow; string recID = "";
                recID = (row.FindControl("lbID") as Label).Text; ;
                IndirectBudgetItem ast = LookUpBLL.GetIndirectItem(int.Parse(recID));
                if (ast != null)
                {
                    txtID.Text = recID;
                    txtName.Text = ast.Name;
                    txtcode.Text = ast.Code;
                    ddlDir.SelectedValue = ast.IndirectTypeID.ToString();
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

        protected void gvCommon_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "del")
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    int key = Convert.ToInt32(gvCommon.DataKeys[index].Value.ToString());
                    IndirectBudgetItem ast = LookUpBLL.GetIndirectItem(key);
                    ast.DelFlg = "Y";
                    LookUpBLL.UpdateIndirectItem(ast);
                    BindGrid();
                    success.Visible = true;
                    success.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> Record deleted successfully!!.";
                    return;
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlAstTyp.SelectedValue != "")
                {
                    int astTypID = int.Parse(ddlAstTyp.SelectedValue);
                    gvCommon.DataSource = LookUpBLL.GetIndirectItemListLookUp(astTypID);
                    gvCommon.DataBind();
                }
            }
            catch
            {
            }
        }

        protected void btnClr_Click(object sender, EventArgs e)
        {
            ddlAstTyp.SelectedValue = "";
            BindGrid();
        }
    }
}