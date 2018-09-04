using BudgetCapture.BLL;
using BudgetCapture.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BudgetCapture.GSS
{
    public partial class CaptureGasSales : System.Web.UI.Page
    {
        CultureInfo culture = new CultureInfo("en-GB");
        private string WoDOffRole = ConfigurationManager.AppSettings["WoDOffRole"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                error.Visible = false; success.Visible = false;
                if (!IsPostBack)
                {
                    Utility.BindGssCustomer(ddlDir);
                    Utility.BindGssCustomer(ddlCustFilter);
                    BindGrid(DateTime.Now.AddDays(40));
                    if (User.IsInRole(WoDOffRole))
                    {
                        dvAdd.Visible = true;
                        
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
        private void BindGrid(DateTime filter,int cust=0)
        {
            AppUser usr = null;
            if (Session["user"] != null)
            {
                usr = (AppUser)Session["user"];
            }
            else
            {
                Response.Redirect("../Login.aspx", false);
                return;
            }
            gvCommon.DataSource = GasSalesBLL.GetMySalesList(usr.ID, (int)Utility.SalesStatus.Pending_Capture_Approval,filter,cust);
            gvCommon.DataBind();
        }
        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<GSS_Customer> SearchCustomers(string prefixText, int count)
        {
            try
            {
                return LookUpBLL.SearchCustomer(prefixText);
            }
            catch
            {
                return null;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            AppUser usr = null;
            if (Session["user"] != null)
            {
                usr = (AppUser)Session["user"];
            }
            else
            {
                Response.Redirect("../Login.aspx", false);
                return;
            }
            try
            {
                decimal totVol = 0; DateTime sdate;
                if (hid.Value == "Update")
                {
                    GSS_SalesTbl ast = null; bool rst = false;
                    ast = GasSalesBLL.GetGasSale(Convert.ToInt32(txtID.Text));
                    if (ast != null)
                    {
                        ast.CustomerID = int.Parse(ddlDir.SelectedValue);
                        if (!decimal.TryParse(txtVol.Text, out totVol))
                        {
                            error.Visible = true;
                            error.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button>Volume must be numeric!!.";
                            return;
                        }
                        ast.CapturedVolumeSale = totVol;
                        if (!DateTime.TryParseExact(txtDate.Text, "dd/MM/yyyy", culture, DateTimeStyles.None, out sdate))
                        {
                            error.Visible = true;
                            error.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button>Invalid date!!.";
                            return;
                        }
                        ast.DateCaptured = sdate;
                        ast.CapturedBy = usr.ID;
                        ast.Status = (int)Utility.SalesStatus.Pending_Capture_Approval;
                        //ast.RecoveryTypeID = int.Parse(ddlDir.SelectedValue);
                        rst = GasSalesBLL.UpdateSalesData(ast);
                        if (rst != false)
                        {
                            BindGrid(DateTime.Now.AddDays(40));
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
                    string asstNme = ""; bool result = false;
                    GSS_SalesTbl ast = new GSS_SalesTbl();
                    if (ast != null)
                    {
                        ast.CustomerID = int.Parse(ddlDir.SelectedValue);
                        if (!decimal.TryParse(txtVol.Text, out totVol))
                        {
                            error.Visible = true;
                            error.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button>Volume must be numeric!!.";
                            return;
                        }
                        ast.CapturedVolumeSale = totVol;
                        if (!DateTime.TryParseExact(txtDate.Text, "dd/MM/yyyy", culture, DateTimeStyles.None, out sdate))
                        {
                            error.Visible = true;
                            error.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button>Invalid date!!.";
                            return;
                        }
                        ast.DateCaptured =  sdate;
                        ast.CapturedBy = usr.ID;
                        ast.Status = (int)Utility.SalesStatus.Pending_Capture_Approval;
                        // ast.RecoveryTypeID = int.Parse(ddlDir.SelectedValue);

                        result = GasSalesBLL.AddSalesData(ast);
                        if (result)
                        {
                            BindGrid(DateTime.Now.AddDays(40));
                            ddlDir.SelectedValue = "";
                            txtVol.Text = "";
                            txtDate.Text = "";
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
            }
            catch (Exception ex)
            {
                error.Visible = true;
                error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> An error occured. Kindly try again. If error persist contact Administrator!!.";
                Utility.WriteError("Error: " + ex.Message);
            }
        }

        protected void gvCommon_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvCommon.PageIndex = e.NewPageIndex;
                BindGrid(DateTime.Now.AddDays(40));
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
                    // int key = Convert.ToInt32(gvDept.DataKeys[index].Value.ToString());
                    GSS_SalesTbl estf = GasSalesBLL.GetGasSale(index);
                    GasSalesBLL.DeleteGasSales(estf);
                    BindGrid(DateTime.Now.AddDays(40));
                    success.Visible = true;
                    success.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> Record deleted successfully!!.";
                    return;

                }
            }catch(Exception ex){
            }
        }

        protected void gvCommon_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                hid.Value = "Update";
                dvID.Visible = true;
                dvMsg.InnerText = "Update Sales Data:";
                btnSubmit.Text = "Update";

                GridViewRow row = gvCommon.SelectedRow; string recID = "";
                recID = (row.FindControl("lbRecID") as Label).Text;
                GSS_SalesTbl ast = GasSalesBLL.GetGasSale(int.Parse(recID));
                if (ast != null)
                {
                    txtID.Text = recID;
                    ddlDir.SelectedValue = ast.CustomerID.Value.ToString();
                    txtVol.Text = ast.CapturedVolumeSale.Value.ToString();
                    txtDate.Text = ast.DateCaptured.Value.ToString(culture);
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
        protected string GetStatus(object o)
        {
            try
            {
                return Utility.GetSalesStatus(o);
            }
            catch
            {
                return "";
            }
        }

        protected void btnClr_Click(object sender, EventArgs e)
        {
            try
            {
                ddlCustFilter.SelectedValue = "";
                txtFilterDate.Text = "";
                BindGrid(DateTime.Now.AddDays(40));
            }catch(Exception ex)
            {
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                int cust = 0; DateTime filter = DateTime.Now.AddDays(40); bool u = false;
                if (ddlCustFilter.SelectedValue != "")
                {
                    cust = int.Parse(ddlCustFilter.SelectedValue);
                }
                if (!DateTime.TryParseExact(txtFilterDate.Text, "dd/MM/yyyy", culture, DateTimeStyles.None, out filter))
                {
                     filter = DateTime.Now.AddDays(40);
                }
                BindGrid(filter, cust);
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