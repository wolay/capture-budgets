﻿using BudgetCapture.BLL;
using BudgetCapture.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BudgetCapture.Admin
{
    public partial class ManageIndirectType : System.Web.UI.Page
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
            gvDir.DataSource = LookUpBLL.GetIndirectTypeList();
            gvDir.DataBind();
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (hid.Value == "Update")
                {
                    IndirectType exp = null; bool rst = false;
                    exp = LookUpBLL.GetIndirectType(Convert.ToInt32(txtID.Text));
                    if (exp != null)
                    {
                        exp.Name = txtDept.Text.ToUpper();
                        exp.GLCode = txtcode.Text;
                        if (chkActive.Checked)
                            exp.isActive = true;
                        else
                            exp.isActive = false;
                        rst = LookUpBLL.UpdateIndirectType(exp);
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
                    IndirectType exp = new IndirectType();
                    exp.Name = txtDept.Text.ToUpper();
                    exp.GLCode = txtcode.Text;
                    if (chkActive.Checked)
                        exp.isActive = true;
                    else
                        exp.isActive = false;
                    result = LookUpBLL.AddIndirectType(exp);
                    if (result)
                    {
                        BindGrid();
                        txtDept.Text = "";
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
                dvMsg.InnerText = "Update IndirectType :";
                btnSubmit.Text = "Update";

                GridViewRow row = gvDir.SelectedRow;
                txtID.Text = row.Cells[0].Text;
                txtcode.Text = row.Cells[1].Text.ToUpper();
                txtDept.Text = row.Cells[2].Text.ToUpper();
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