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
    public partial class ManageDept : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Utility.BindDirectorate(ddlDir);
                    BindBudgetType();
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
        private void BindBudgetType()
        {
            chkBudgetType.DataTextField = "BudgetItem";
            chkBudgetType.DataValueField = "ID";
            chkBudgetType.DataSource = LookUpBLL.GetBudgetMenuList();
            chkBudgetType.DataBind();
        }
        private void BindGrid()
        {
            gvDept.DataSource = DepartmentBLL.GetDeptList().OrderBy(j=>j.DirectorateID).ToList();
            gvDept.DataBind();
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (hid.Value == "Update")
                {
                    Department dpt = null; bool rst = false;
                    dpt = DepartmentBLL.GetDepartment(Convert.ToInt32(txtID.Text));
                    if (dpt != null)
                    {
                        dpt.Name = txtDept.Text; dpt.DirectorateID = int.Parse(ddlDir.SelectedValue);
                        //dpt.Code = txtCode.Text;
                        rst = DepartmentBLL.Update(dpt);
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
                    string dept = ""; string code = "";
                    dept = txtDept.Text;
                   // code = txtCode.Text;
                    bool result = false;
                    Department dp = new Department();
                    dp.Name = dept.ToUpper(); dp.DirectorateID = int.Parse(ddlDir.SelectedValue);
                    dp.Code = code;
                    result = DepartmentBLL.AddDepartment(dp);
                    if (result)
                    {
                        BindGrid();
                        txtDept.Text = "";
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

        protected void gvDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                hid.Value = "Update";
                dvID.Visible = true;
                dvMsg.InnerText = "Update Department :";
                btnSubmit.Text = "Update";

                GridViewRow row = gvDept.SelectedRow;
                txtID.Text = (row.FindControl("lbID") as Label).Text; ;
                txtDept.Text = row.Cells[2].Text.ToUpper();
               // txtCode.Text = row.Cells[1].Text.ToUpper();
                ddlDir.SelectedValue = (row.FindControl("lbDir") as Label).Text; ;
                //ddlDir.SelectedItem.Text = HttpUtility.HtmlDecode(row.Cells[2].Text).ToLower();
            }
            catch (Exception ex)
            {
                error.Visible = true;
                error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> An error occured. Kindly try again. If error persist contact Administrator!!.";
                Utility.WriteError("Error: " + ex.InnerException);
            }
        }

        protected void gvDept_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDept.PageIndex = e.NewPageIndex;
                BindGrid();
            }
            catch
            {
            }
        }
        protected void gvDept_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "del")
                {
                    lnkBtn.Visible = true; bType.Visible = true;
                    chkBudgetType.Items.Clear();
                    BindBudgetType();
                    int index = int.Parse(e.CommandArgument.ToString());
                    int key = Convert.ToInt32(gvDept.DataKeys[index].Value.ToString());
                    hidbudget.Value = key.ToString();
                    Department dd = DepartmentBLL.GetDepartment(key);
                    lbDept.Text = dd.Name;
                    hidbudget.Value = key.ToString();
                    List<DeptBudgetItem> bd = LookUpBLL.GetDeptMenuItem(key).ToList();
                    if (bd.Count() > 0)
                    {
                        foreach (DeptBudgetItem d in bd)
                        {
                            foreach (ListItem l in chkBudgetType.Items)
                            {
                                if (d.MenuItemId.ToString() == l.Value)
                                {
                                    l.Selected = true;
                                }
                            }
                        }

                    }

                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void lnkBtn_Click(object sender, EventArgs e)
        {
            try
            {
                int deptId=int.Parse(hidbudget.Value);
                foreach(ListItem l in chkBudgetType.Items)
                {
                    int menId = int.Parse(l.Value);
                    DepartmentBudgetMenu dm = LookUpBLL.GetMenuForDept(deptId, menId);
                    if (l.Selected == true)
                    {
                        if (dm == null)
                            LookUpBLL.AddMeunItem(new DepartmentBudgetMenu { DepartmentID = deptId, MenuItemID = menId });
                    }
                    else
                    {
                        if(dm!=null)
                            LookUpBLL.DeleteDeptMenuItems(dm);
                    }
                }
                success.Visible = true;
                success.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> Record updated successfully!!.";
                return;
            }
            catch (Exception ex)
            {
            }
        }

        protected void chkBudgetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    int deptId=int.Parse(hidbudget.Value);
            //   // if (chkBudgetType.SelectedItem != null)
            //   // {

            //    if (chkBudgetType.SelectedItem.Selected)
            //    {
            //        int menId = int.Parse(chkBudgetType.SelectedValue);
            //        LookUpBLL.AddMeunItem(new DepartmentBudgetMenu { DepartmentID = deptId, MenuItemID = menId });
            //    }
            //    else
            //        //LookUpBLL.DeleteDeptMenuItems(new DepartmentBudgetMenu { DepartmentID = deptId, MenuItemID = menId });
            //   // }
            //}
            //catch
            //{
            //}
        }

    }
}