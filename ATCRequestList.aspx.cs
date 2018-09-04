using BudgetCapture.BLL;
using BudgetCapture.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BudgetCapture
{
    public partial class ATCRequestList : System.Web.UI.Page
    {
        private static AppUser usr = null; private static BudgetYear budYr = null;
        private string AdminRole = ConfigurationManager.AppSettings["adminRole"].ToString();
        private string deptIniRole = ConfigurationManager.AppSettings["DeptIniRole"].ToString();
        private string deptApprRole = ConfigurationManager.AppSettings["DeptApprverRole"].ToString();
        private string PBMgrRole = ConfigurationManager.AppSettings["PBMgrRole"].ToString();
        private string PBOffRole = ConfigurationManager.AppSettings["PBOffRole"].ToString();
        private string EDRole = ConfigurationManager.AppSettings["EDRole"].ToString();
        private string MDRole = ConfigurationManager.AppSettings["MDRole"].ToString();
        decimal amtTot = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            error.Visible = false; success.Visible = false;
            try
            {
                // hidEDPending.Value = ""; hidMDPending.Value = "";
                //  btnCorrect.Visible = false;
                //  btnMD.Visible = false;
                //dvExco.Visible = false;
                if (Session["user"] != null)
                {
                    usr = (AppUser)Session["user"];
                    budYr = (BudgetYear)Session["budgetYr"];

                }
                else
                {
                    Response.Redirect("Login.aspx", false);
                    return;
                }
                if (!IsPostBack)
                {
                    lbDept.Text = usr.Department.Name.ToUpper();
                    lbyr.Text = budYr.Year;
                    int budyrID = budYr.ID;
                    int deptID = usr.DepartmentID.Value;
                    
                    if (User.IsInRole(AdminRole) || User.IsInRole(PBMgrRole) || User.IsInRole(PBOffRole))
                    {
                        dvFilter.Visible = true;
                        Utility.BindDept(ddlDept);
                        List<Department> deptList = DepartmentBLL.GetDeptList();
                    }
                    if (User.IsInRole(EDRole) || User.IsInRole(MDRole))
                    {
                        dvFilter.Visible = true;
                        Utility.BindDept(ddlDept);
                        List<Department> deptList = DepartmentBLL.GetDeptList();
                        lbDept.Visible = false;
                    }

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
            usr = (AppUser)Session["user"]; budYr = (BudgetYear)Session["budgetYr"];
            if (User.IsInRole(AdminRole) || User.IsInRole(PBMgrRole) || User.IsInRole(PBOffRole) || User.IsInRole(EDRole) || User.IsInRole(MDRole))
            {
                gvDept.DataSource = CommonBLL.GetATCRequestList(budYr.ID);
                gvDept.DataBind();
            }
            else if(User.IsInRole(deptApprRole))
            {
                gvDept.DataSource = CommonBLL.GetATCRequestList(budYr.ID, usr.DepartmentID.Value);
                gvDept.DataBind();
            }
        }

        protected void gvDept_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void gvDept_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName == "edt")
                {
                    ATCRequestHeader header=CommonBLL.GetATCHeader(int.Parse(e.CommandArgument.ToString()));

                    string budgetType = header.BudgetMenuItem.Code;
                    string batchId=header.BatchID;
                    int deptId = header.DepartmentID.Value;
                    if (budgetType == "MO")
                    {
                        Response.Redirect(string.Format("MovableAsset.aspx?deptId={0}&&batchId={1}&&Id={2}", deptId, batchId, header.ID), false);
                    }
                    if (budgetType == "SA")
                    {
                        Response.Redirect(string.Format("ExistingStaffpg.aspx?deptId={0}&&batchId={1}&&Id={2}", deptId, batchId, header.ID), false);
                    }
                    if (budgetType == "ID")
                    {
                        Response.Redirect(string.Format("IndirectBudgetPg.aspx?deptId={0}&&batchId={1}&&Id={2}", deptId, batchId, header.ID), false);
                    }
                    if (budgetType == "DI")
                    {
                        Response.Redirect(string.Format("DirectBudgetPg.aspx?deptId={0}&&batchId={1}&&Id={2}", deptId, batchId,header.ID), false);
                    }
                    if (budgetType == "CA")
                    {
                        Response.Redirect(string.Format("CapexBudgetPg.aspx?deptId={0}&&batchId={1}&&Id={2}", deptId,batchId,header.ID),false);
                    }
            
                     
                }
            }
            catch (Exception ex)
            {
                error.Visible = true;
                error.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button> An error occurred. kindly try again!!!";
                Utility.WriteError("Error: " + ex.InnerException);
            }
        }
        protected string GetStatus(object o)
        {
            try
            {
                return Utility.GetATCStatus(o);
            }
            catch
            {
                return "";
            }
        }

        protected void btnMD_Click(object sender, EventArgs e)
        {

        }

        protected void btnReject_Click(object sender, EventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                budYr = (BudgetYear)Session["budgetYr"];
                if (ddlDept.SelectedValue != "")
                {
                    int deptId=int.Parse(ddlDept.SelectedValue);
                    gvDept.DataSource = CommonBLL.GetATCRequestList(budYr.ID, deptId);
                    gvDept.DataBind();
                }
            }
            catch
            {
            }
        }

        protected void btnClr_Click(object sender, EventArgs e)
        {
            try
            {
                ddlDept.SelectedValue = "";
                BindGrid();
            }
            catch
            {
            }
        }

        protected void gvDept_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                // budYr = (BudgetYear)Session["budgetYr"];
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    Label lbsTot = e.Row.FindControl("lbamtpApp") as Label;
                    amtTot += decimal.Parse(lbsTot.Text);
                     
                }
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    //Label lbQty = e.Row.FindControl("lbSumQty") as Label;
                    //lbQty.Text = quantityTotal.ToString();
                    Label lbsTot = e.Row.FindControl("lbSubTotalFt") as Label;
                    lbsTot.Text = amtTot.ToString("N");
                }
            }
            catch (Exception ex)
            {
            }
        }

    }
}