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
    public partial class ManageAsset : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Utility.BindAssetType(ddlDir);
                    Utility.BindAssetType(ddlAstTyp);
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
            gvCommon.DataSource = LookUpBLL.GetAssetList();
            gvCommon.DataBind();
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (hid.Value == "Update")
                {
                    Asset ast = null; bool rst = false; decimal uprice = 0;
                    ast = LookUpBLL.GetAsset(Convert.ToInt32(txtID.Text));
                    if (ast != null)
                    {
                        
                        if (!decimal.TryParse(txtPrice.Text, out uprice))
                        {
                            error.Visible = true;
                            error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button>Unitprice must be numeric!!.";
                            return;
                        }
                        ast.AssetName = txtAsset1.Text;
                        ast.AssetTypeID = int.Parse(ddlDir.SelectedValue);
                        ast.UnitPrice =uprice ;
                        ast.DelFlg = "N";
                        rst = LookUpBLL.UpdateAsset(ast);
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
                    asstNme = txtAsset1.Text;
                    if (!decimal.TryParse(txtPrice.Text, out uprice))
                    {
                        error.Visible = true;
                        error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button>Unitprice must be numeric!!.";
                        return;
                    }
                    bool result = false;
                    Asset ast = new Asset();
                    ast.AssetName = asstNme; 
                    ast.AssetTypeID = int.Parse(ddlDir.SelectedValue);
                    ast.UnitPrice = uprice;
                    ast.DelFlg = "N";
                    result = LookUpBLL.AddAsset(ast);
                    if (result)
                    {
                        BindGrid();
                        txtAsset1.Text = "";
                        ddlDir.SelectedValue = "";
                        txtPrice.Text = "";
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
                dvMsg.InnerText = "Update Asset :";
                btnSubmit.Text = "Update";

                GridViewRow row = gvCommon.SelectedRow; string recID = "";
                recID = (row.FindControl("lbID") as Label).Text; ;
                Asset ast = LookUpBLL.GetAsset(int.Parse(recID));
                if (ast != null)
                {
                    txtID.Text = recID;
                    txtAsset1.Text = ast.AssetName;
                    txtPrice.Text = ast.UnitPrice.Value.ToString("N");
                    ddlDir.SelectedValue = (row.FindControl("lbDir") as Label).Text; ;
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
                    Asset ast = LookUpBLL.GetAsset(key);
                    ast.DelFlg = "Y";
                    LookUpBLL.UpdateAsset(ast);
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
                    gvCommon.DataSource = LookUpBLL.GetAssetLookup(astTypID);
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