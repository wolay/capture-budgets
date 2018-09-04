using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BudgetCapture.BLL;
using BudgetCapture.DAL;
using System.Globalization;

namespace BudgetCapture.Admin
{
    public partial class ManageBudgetYr : System.Web.UI.Page
    {
        CultureInfo culture = new CultureInfo("en-GB");
        protected void Page_Load(object sender, EventArgs e)
        {
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
            gvDir.DataSource = LookUpBLL.GetBudgetYearList();
            gvDir.DataBind();
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (hid.Value == "Update")
                {
                    BudgetYear mak = null; bool rst = false; int year = 0;
                    mak = LookUpBLL.GetBudgetYear(Convert.ToInt32(txtID.Text));
                    if (mak != null)
                    {
                        if (!int.TryParse(txtDept.Text, out year))
                        {
                            error.Visible = true;
                            error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button>Year must be numeric!!.";
                            return;
                        }
                        mak.Year = year.ToString();
                        mak.StartDate = DateTime.Parse(txtDate.Text, culture);
                        mak.EndDate = DateTime.Parse(txtEDate.Text, culture);
                        if (chkActive.Checked)
                        {
                            LookUpBLL.DeactivateAllBudgetYear();
                            mak.IsActive = true;
                        }
                        else
                        {
                            mak.IsActive = false;
                        }
                        rst = LookUpBLL.UpdateBudgetYear(mak);
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
                    int mak = 0;
                   
                    if(!int.TryParse(txtDept.Text,out mak))
                    {
                        error.Visible = true;
                        error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button>Year must be numeric!!.";
                        return;
                    }
                    bool result = false;
                    BudgetYear byr = new BudgetYear();
                    byr.Year = mak.ToString();
                    byr.StartDate = DateTime.Parse(txtDate.Text, culture);
                    byr.EndDate = DateTime.Parse(txtEDate.Text, culture);
                    if (chkActive.Checked)
                    {
                        LookUpBLL.DeactivateAllBudgetYear();
                        byr.IsActive = true;
                    }
                    else
                    {
                        byr.IsActive = false;
                    }
                    result = LookUpBLL.AddBudgetYear(byr);
                    if (result)
                    {
                        BindGrid();
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
                dvMsg.InnerText = "Update Budget Year :";
                btnSubmit.Text = "Update";
                string recID="";
                GridViewRow row = gvDir.SelectedRow;
                 recID= (row.FindControl("lbID") as Label).Text;
                 BudgetYear bYear=LookUpBLL.GetBudgetYear(int.Parse(recID));
                if(bYear!=null)
                {
                    txtID.Text = recID;
                    txtDept.Text = bYear.Year;
                    if(bYear.StartDate.HasValue)
                    {
                        txtDate.Text = bYear.StartDate.Value.ToString("dd/MM/yyyy");
                    }
                    if (bYear.EndDate.HasValue)
                    {
                        txtEDate.Text = bYear.EndDate.Value.ToString("dd/MM/yyyy");
                    }
                    if (bYear.IsActive.HasValue && bYear.IsActive == true)
                    {
                        chkActive.Checked = true;
                    }
                    else
                    {
                        chkActive.Checked = false;
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