using BudgetCapture.BLL;
using BudgetCapture.DAL;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BudgetCapture.Reports
{
    public partial class ATCRequestProRpt : System.Web.UI.Page
    {
        private AppUser usr = null;
        ReportParameter[] rpt; CultureInfo culture = new CultureInfo("en-GB");
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    Response.Redirect("../Login.aspx", false);
                }
                if (Session["user"] != null)
                {
                    usr = (AppUser)Session["user"];
                }
                else
                {
                    Response.Redirect("../Login.aspx", false);
                    return;
                }
                if (!IsPostBack)
                {
                    Utility.BindBudgetYear(ddlbudyr);
                    Utility.BindDept(ddlDept);
                    Utility.BindATCStatus(ddlStatus);
                    lbMsg.Text = "";
                    int byrID = int.Parse(ddlbudyr.SelectedValue);
                    DBAccess db = new DBAccess();
                    db.AddParameter("budgetYrId", byrID);
                    rpt = new ReportParameter[3];
                    rpt[2] = new ReportParameter("status", "1");

                    rpt[1] = new ReportParameter("deptID", "1");
                    rpt[0] = new ReportParameter("budgetYrID", byrID.ToString());
                    if (ddlDept.SelectedValue != "")
                    {
                        rpt[1] = new ReportParameter("deptID", ddlDept.SelectedValue);
                        db.AddParameter("@deptId", int.Parse(ddlDept.SelectedValue));
                    }
                    if (ddlStatus.SelectedValue != "")
                    {
                        rpt[2] = new ReportParameter("status", ddlStatus.SelectedValue);
                        db.AddParameter("@status", int.Parse(ddlStatus.SelectedValue));
                    }
                    DataSet ds = new DataSet();
                    ds = db.ExecuteDataSet("bc_rpt_ATCRequest");

                    ReportDataSource datasource = new
                      ReportDataSource("dsBudget",
                      ds.Tables[0]);

                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.SetParameters(rpt);
                    ReportViewer1.LocalReport.DataSources.Add(datasource);

                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        lbMsg.Text = "Sorry, No record found!";
                    }

                    ReportViewer1.LocalReport.Refresh();
                    return;

                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                lbMsg.Text = "";
                int byrID = int.Parse(ddlbudyr.SelectedValue);
                DBAccess db = new DBAccess();
                db.AddParameter("budgetYrId", byrID);
                rpt = new ReportParameter[3];
                rpt[2] = new ReportParameter("status", "1");
                rpt[1] = new ReportParameter("deptID", "1");
                rpt[0] = new ReportParameter("budgetYrID", byrID.ToString());
                if (ddlDept.SelectedValue != "")
                {
                    rpt[1] = new ReportParameter("deptID", ddlDept.SelectedValue);
                    db.AddParameter("@deptId", int.Parse(ddlDept.SelectedValue));
                }
                if (ddlStatus.SelectedValue != "")
                {
                    rpt[2] = new ReportParameter("status", ddlStatus.SelectedValue);
                    db.AddParameter("@status", int.Parse(ddlStatus.SelectedValue));
                }
                DataSet ds = new DataSet();
                ds = db.ExecuteDataSet("bc_rpt_ATCRequest");

                ReportDataSource datasource = new
                  ReportDataSource("dsBudget",
                  ds.Tables[0]);

                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.SetParameters(rpt);
                ReportViewer1.LocalReport.DataSources.Add(datasource);

                if (ds.Tables[0].Rows.Count == 0)
                {
                    lbMsg.Text = "Sorry, No record found!";
                }

                ReportViewer1.LocalReport.Refresh();
                return;

            }
            catch (Exception ex)
            {
                lbMsg.Text = ex.Message;
            }
        }
    }
}