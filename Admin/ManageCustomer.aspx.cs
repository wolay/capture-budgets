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
    public partial class ManageCustomer : System.Web.UI.Page
    {
        private static BudgetYear budYr = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Utility.BindCustomerType(ddlDir);
                    Utility.BindCustomerType(ddlOblTyp);
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
            gvCommon.DataSource = LookUpBLL.GetCustomerList();
            gvCommon.DataBind();
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                budYr = (BudgetYear)Session["budgetYr"];
                if (hid.Value == "Update")
                {
                    Customer ast = null; bool rst = false;
                    ast = LookUpBLL.GetCustomer(Convert.ToInt32(txtID.Text));
                    if (ast != null)
                    {
                        ast.FullName = txtOblg.Text;
                        ast.Code = txtAgr.Text;
                        ast.CustomerTypeID = int.Parse(ddlDir.SelectedValue);
                       // ast.BudgetYearID = budYr.ID;
                        rst = LookUpBLL.UpdateCustomer(ast);
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
                    
                    bool result = false;
                    Customer ast = new Customer();
                    ast.FullName = txtOblg.Text;
                    ast.CustomerTypeID = int.Parse(ddlDir.SelectedValue);
                    ast.Code = txtAgr.Text;
                   // ast.BudgetYearID = budYr.ID;
                    result = LookUpBLL.AddCustomer(ast);
                    if (result)
                    {
                        BindGrid();
                        txtOblg.Text = "";
                        txtAgr.Text = "";
                        ddlDir.SelectedValue = "";

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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (ddlOblTyp.SelectedValue != "")
            {
                int tyId=int.Parse(ddlOblTyp.SelectedValue);
                gvCommon.DataSource = LookUpBLL.GetCustomerList().Where(k=>k.CustomerTypeID==tyId).ToList();
                gvCommon.DataBind();
            }
        }

        protected void btnClr_Click(object sender, EventArgs e)
        {
            ddlOblTyp.SelectedValue = "";
            BindGrid();
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

        protected void gvCommon_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                hid.Value = "Update";
                dvID.Visible = true;
                dvMsg.InnerText = "Update Customer:";
                btnSubmit.Text = "Update";

                GridViewRow row = gvCommon.SelectedRow; string recID = "";
                recID = (row.FindControl("lbID") as Label).Text; ;
                Customer ast = LookUpBLL.GetCustomer(int.Parse(recID));
                if (ast != null)
                {
                    txtID.Text = recID;
                    txtOblg.Text = ast.FullName;
                    txtAgr.Text = ast.Code;
                    ddlDir.SelectedValue = ast.CustomerTypeID.ToString();

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
    }
}