﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BudgetCapture.DAL;
using Microsoft.Reporting.WebForms;
using System.Globalization;
using BudgetCapture.BLL;
using System.Data;

namespace BudgetCapture.Reports
{
    public partial class DirectPro_ED : System.Web.UI.Page
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
                    Utility.BindDirectorate(ddlDir);

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
                rpt[2] = new ReportParameter("dirID", "1");
                rpt[1] = new ReportParameter("deptID", "1");
                rpt[0] = new ReportParameter("budgetYrID", byrID.ToString());
                if (ddlDept.SelectedValue != "")
                {
                    rpt[1] = new ReportParameter("deptID", ddlDept.SelectedValue);
                    db.AddParameter("@deptId", int.Parse(ddlDept.SelectedValue));
                }
                if (ddlDir.SelectedValue != "")
                {
                    rpt[2] = new ReportParameter("dirID", ddlDir.SelectedValue);
                    db.AddParameter("@directorateID", int.Parse(ddlDir.SelectedValue));
                }

                DataSet ds = new DataSet();
                ds = db.ExecuteDataSet("bc_rpt_Direct");

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