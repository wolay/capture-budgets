using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BudgetCapture.BLL;
using BudgetCapture.DAL;
using System.Configuration;
using System.IO;
using System.Data.OleDb;
using System.Data;
using System.Reflection;
using System.Runtime.InteropServices;

namespace BudgetCapture
{
    public partial class RestructureProjectionPage : System.Web.UI.Page
    {
        private static AppUser usr = null; private static BudgetYear budYr = null;
        decimal priceTotal = 0;
        private string AdminRole = ConfigurationManager.AppSettings["adminRole"].ToString();
        private string deptIniRole = ConfigurationManager.AppSettings["DeptIniRole"].ToString();
        private string deptApprRole = ConfigurationManager.AppSettings["DeptApprverRole"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            error.Visible = false; success.Visible = false;
            dvAdd.Visible = false; dvAppr.Visible = false;
            try
            {
                if (Session["user"] != null)
                {
                    usr = (AppUser)Session["user"];
                    budYr = (BudgetYear)Session["budgetYr"];
                    if (User.IsInRole(deptApprRole) && budYr.IsActive == true)
                    {
                        dvAppr.Visible = true;
                    }
                    if (User.IsInRole(deptIniRole) && budYr.IsActive == true)
                    {
                        dvAdd.Visible = true;
                    }
                    if (User.IsInRole(AdminRole) && budYr.IsActive == true)
                    {
                        dvAdmin.Visible = true;
                    }
                }
                else
                {
                    Response.Redirect("Login.aspx", false);
                    return;
                }

                if (!IsPostBack)
                {
                    Utility.BindMonth(ddlMonth);
                   // Utility.BindObligorToRestructure(ddlObligor);
                    Utility.BindDept(ddlDeptFilter);
                   // Utility.BindLoanType(ddlLoanType);
                    lbDept.Text = usr.Department.Name.ToUpper();
                    lbyr.Text = budYr.Year;
                    BindGrid();
                    if (User.IsInRole(AdminRole))
                    {
                        dvAdminScr.Visible = true;
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
        private void BindGrid()
        {
            usr = (AppUser)Session["user"]; budYr = (BudgetYear)Session["budgetYr"];
            if (User.IsInRole(AdminRole))
            {
                gvDept.DataSource = CommonBLL.GetRestructureList(budYr.ID);
                gvDept.DataBind();
            }
            else
            {
                gvDept.DataSource = CommonBLL.GetRestructureList(budYr.ID, usr.DepartmentID.Value);
                gvDept.DataBind();
            }
        }
        protected void rboList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.rboList.SelectedValue == "1")
            {
                this.dvSingle.Visible = true;
                this.dvBatch.Visible = false;
            }
            if (this.rboList.SelectedValue == "2")
            {
                this.dvSingle.Visible = false;
                this.dvBatch.Visible = true;
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string filename = "";
            try
            {
                this.error.Visible = false;
                string str2 = "";
                string str3 = "";
                string month = "";
                double num = 0.0;
                int num2 = 0;
                OleDbConnection connection = new OleDbConnection();
                OleDbCommand selectCommand = new OleDbCommand();
                OleDbDataAdapter adapter = new OleDbDataAdapter();
                DataSet dataSet = new DataSet();
                Microsoft.Office.Interop.Excel.Application o = new  Microsoft.Office.Interop.Excel.Application();
                object obj2 = Missing.Value;
                string name = "";
                if (this.xlsUpload.HasFile)
                {
                    string extension = Path.GetExtension(this.xlsUpload.FileName.ToString());
                    string cmdText = null;
                    string connectionString = "";
                    if ((extension.Trim().ToLower() == ".xls") || (extension.Trim().ToLower() == ".xlsx"))
                    {
                        if (File.Exists(base.Server.MapPath("ExcelUpload/" + this.xlsUpload.FileName.ToString())))
                        {
                            this.error.Visible = true;
                            this.error.InnerText = "File already exist on the server.kindly upload another file";
                        }
                        else
                        {
                            this.xlsUpload.SaveAs(base.Server.MapPath("ExcelUpload/" + this.xlsUpload.FileName.ToString()));
                            filename = base.Server.MapPath("ExcelUpload/" + this.xlsUpload.FileName.ToString());
                            Microsoft.Office.Interop.Excel.Workbook workbook = o.Workbooks.Open(filename, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                            foreach (Microsoft.Office.Interop.Excel.Worksheet worksheet in workbook.Worksheets)
                            {
                                name = worksheet.Name;
                                break;
                            }
                            workbook.Close(false, obj2, obj2);
                            while (Marshal.ReleaseComObject(workbook) > 0)
                            {
                            }
                            workbook = null;
                            o.Quit();
                            Marshal.ReleaseComObject(o);
                            o = null;
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                            if (extension.Trim().ToLower() == ".xls")
                            {
                                connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filename + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                            }
                            else if (extension.Trim().ToLower() == ".xlsx")
                            {
                                connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                            }
                            else
                            {
                                File.Delete(filename);
                                return;
                            }
                            cmdText = "SELECT * FROM [" + name + "$]";
                            connection = new OleDbConnection(connectionString);
                            if (connection.State == ConnectionState.Closed)
                            {
                                connection.Open();
                            }
                            selectCommand = new OleDbCommand(cmdText, connection);
                            adapter = new OleDbDataAdapter(selectCommand);
                            dataSet = new DataSet();
                            adapter.Fill(dataSet);
                            bool flag = false;
                            DataTable table = new DataTable();
                            DataColumn column = new DataColumn("AgrNo", Type.GetType("System.String"));
                            DataColumn column2 = new DataColumn("Name", Type.GetType("System.String"));
                            DataColumn column3 = new DataColumn("Amt", Type.GetType("System.Double"));
                            DataColumn column4 = new DataColumn("Month", Type.GetType("System.String"));
                            table.Columns.Add(column);
                            table.Columns.Add(column2);
                            table.Columns.Add(column3);
                            table.Columns.Add(column4);
                            foreach (DataRow row in dataSet.Tables[0].Rows)
                            {
                                str2 = "";
                                str3 = "";
                                num = 0.0;
                                month = "";
                                flag = false;
                                str2 = row[0].ToString();
                                str3 = row[1].ToString();
                                if (str2.ToUpper().Contains("END OF FILE"))
                                {
                                    flag = true;
                                    break;
                                }
                                if ((!string.IsNullOrEmpty(str3) && (str3.Length > 7)) && str3.Substring(1, 7).Contains("9999999"))
                                {
                                    flag = true;
                                    break;
                                }
                                if (!string.IsNullOrEmpty(row[2].ToString()))
                                {
                                    double result = 0.0;
                                    if (!double.TryParse(row[2].ToString().Trim(), out result))
                                    {
                                        this.error.Visible = true;
                                        this.error.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button>Suspected wrong input file format.  Amount must not be numeric";
                                        connection.Close();
                                        return;
                                    }
                                    num = double.Parse(row[2].ToString().Trim());
                                }
                                month = row[3].ToString().Trim();
                                if (this.getMonthNo(month) == 0)
                                {
                                    this.error.Visible = true;
                                    this.error.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button>The input file format is wrong. One of the month value is in wrong format.Check the input file and try again";
                                    return;
                                }
                                num2 = this.getMonthNo(month);
                                if (((str2.Trim().Length < 1) || (month.Trim().Length < 1)) || (str3.Trim().Length < 1))
                                {
                                    this.error.Visible = true;
                                    this.error.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button>The input file format is wrong. One of the row is empty.Check the input file and try again";
                                    return;
                                }
                                DataRow row2 = table.NewRow();
                                row2[column] = str2;
                                row2[column2] = str3;
                                row2[column3] = num;
                                row2[column4] = num2;
                                table.Rows.Add(row2);
                            }
                            if ((table != null) && (table.Rows.Count > 0))
                            {
                                bool flag3 = false;
                                foreach (DataRow row in table.Rows)
                                {
                                    RestructureProjection recpro = new RestructureProjection
                                    {
                                        ObligorName = row[1].ToString(),
                                        AgreementNo = row[0].ToString(),
                                        DepartmentId = usr.DepartmentID,
                                        AddedBy = base.User.Identity.Name,
                                        DateAdded = new DateTime?(DateTime.Now),
                                        Amount = new decimal?(decimal.Parse(row[2].ToString())),
                                        BudgetYrID = new int?(budYr.ID),
                                        Month = new int?(int.Parse(row[3].ToString())),
                                        Status = 1,
                                       // LoanType=ddlLoanType.SelectedValue
                                    };
                                    flag3 = CommonBLL.AddRestructure(recpro);
                                }
                                if (flag3)
                                {
                                    this.BindGrid();
                                    this.success.Visible = true;
                                    this.success.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> Record updated successfully!!.";
                                }
                                else
                                {
                                    this.error.Visible = true;
                                    this.error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button>Record could Not updated. Kindly try again. If error persist contact Administrator!!.";
                                }
                            }
                        }
                    }
                    else
                    {
                        this.error.Visible = true;
                        this.error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> An error occured. File not valid";
                    }
                }
                else
                {
                    this.error.Visible = true;
                    this.error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> An error occured. File not valid";
                }
            }
            catch (Exception exception)
            {
                if (File.Exists(filename))
                {
                    this.error.Visible = true;
                }
                this.error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> An error occured. Kindly try again. If error persist contact Administrator!!.";
                Utility.WriteError("Error: " + exception.Message);
            }
        }
        private int getMonthNo(string month)
        {
            if (month.ToLower() == "jan")
            {
                return 1;
            }
            if (month.ToLower() == "feb")
            {
                return 2;
            }
            if (month.ToLower() == "mar")
            {
                return 3;
            }
            if (month.ToLower() == "apr")
            {
                return 4;
            }
            if (month.ToLower() == "may")
            {
                return 5;
            }
            if (month.ToLower() == "jun")
            {
                return 6;
            }
            if (month.ToLower() == "jul")
            {
                return 7;
            }
            if (month.ToLower() == "aug")
            {
                return 8;
            }
            if (month.ToLower() == "sep")
            {
                return 9;
            }
            if (month.ToLower() == "oct")
            {
                return 10;
            }
            if (month.ToLower() == "nov")
            {
                return 11;
            }
            if (month.ToLower() == "dec")
            {
                return 12;
            }
            return 0;
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                string agrNo = txtobligor.Text.Trim();
                Obligor obl = Utility.GetObligor(agrNo);
                if (obl != null)
                {
                    lbName.Text = "Obligor's Name: " + obl.FullName.ToUpper();
                    lbName.ForeColor = System.Drawing.Color.RoyalBlue;
                    btnSubmit.Enabled = true;

                }
                else
                {
                    lbName.Text = "Not Found";
                    lbName.ForeColor = System.Drawing.Color.Maroon;
                    btnSubmit.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                error.Visible = true;
                error.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button> An error occurred. kindly try again!!!";
                Utility.WriteError("Error: " + ex.Message);
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            RestructureProjection recPro = null;   
          
            try
            {
                usr = (AppUser)Session["user"]; budYr = (BudgetYear)Session["budgetYr"];
                if (hid.Value == "Update")
                {
                    bool rst = false; decimal tot = 0; int month = int.Parse(ddlMonth.SelectedValue);

                    recPro = CommonBLL.GetRestructure(Convert.ToInt32(txtID.Text));
                    if (recPro != null)
                    {
                        if (!decimal.TryParse(txtTot.Text, out tot))
                        {
                            error.Visible = true;
                            error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button>Amount must be numeric!!.";
                            return;
                        }
                        recPro.Amount = tot;
                        recPro.ObligorName = lbName.Text;
                        recPro.AgreementNo = txtobligor.Text;
                       // recPro.ObligorID = int.Parse(ddlObligor.SelectedValue);
                        recPro.DepartmentId = usr.DepartmentID;
                        recPro.Status = (int)Utility.BudgetItemStatus.Pending_Approval;
                        recPro.Month = month;
                        recPro.BudgetYrID = budYr.ID;
                        //recPro.LoanType = ddlLoanType.SelectedValue;
                        rst = CommonBLL.UpdateRestructure(recPro);
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
                    decimal tot = 0;
                    if (!decimal.TryParse(txtTot.Text, out tot))
                    {
                        error.Visible = true;
                        error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button>Quantity must be numeric!!.";
                        return;
                    }
                    bool result = false;
                    recPro = new RestructureProjection(); int month = int.Parse(ddlMonth.SelectedValue);
                   // recPro.ObligorID = int.Parse(ddlObligor.SelectedValue);
                    recPro.ObligorName = lbName.Text.Split(new char[] { ':' })[1].TrimStart();
                    recPro.AgreementNo = txtobligor.Text;
                    recPro.DepartmentId = usr.DepartmentID;
                    recPro.AddedBy = User.Identity.Name;
                    recPro.DateAdded = DateTime.Now;
                    recPro.Amount = tot;
                    recPro.BudgetYrID = budYr.ID;
                    recPro.Month = month;
                    //recPro.LoanType = ddlLoanType.SelectedValue;
                    recPro.Status = (int)Utility.BudgetItemStatus.Pending_Approval;
                    result = CommonBLL.AddRestructure(recPro);
                    if (result)
                    {
                        BindGrid();
                        txtTot.Text = "";
                        txtobligor.Text = "";
                        lbName.Text = "";
                        btnSubmit.Enabled = false;
                       // ddlObligor.SelectedValue="";
                        ddlMonth.SelectedValue = "";
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

        protected void gvDept_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDept.PageIndex = e.NewPageIndex;
                BindGrid();
            }
            catch (Exception ex)
            {
                Utility.WriteError("Error: " + ex.InnerException);
            }
        }

        protected void gvDept_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                CheckBox chkheader = null; budYr = (BudgetYear)Session["budgetYr"];
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    chkheader = e.Row.FindControl("chkHeader") as CheckBox;
                    if (User.IsInRole(deptApprRole) && budYr.IsActive == true)
                    {
                        chkheader.Visible = true;
                    }
                    if (User.IsInRole(AdminRole) && budYr.IsActive == true)
                    {
                        chkheader.Visible = true;
                    }
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lbAmt = e.Row.FindControl("lbAmt") as Label;
                        priceTotal += Convert.ToDecimal(lbAmt.Text);
                  
                    Label st = e.Row.FindControl("lbStatus") as Label;
                    CheckBox ck = e.Row.FindControl("chkRow") as CheckBox;

                    ImageButton imgEdit = e.Row.FindControl("imgBtnEdit") as ImageButton;
                    ImageButton imgDel = e.Row.FindControl("imgBtnDel") as ImageButton;


                    if (User.IsInRole(deptIniRole) && budYr.IsActive == true)
                    {
                        if (st.Text == ((int)Utility.BudgetItemStatus.Rejected).ToString() || st.Text == ((int)Utility.BudgetItemStatus.Pending_Approval).ToString() || st.Text == ((int)Utility.BudgetItemStatus.Returned_For_Correction).ToString())
                        {
                            imgDel.Visible = true;
                            imgEdit.Visible = true;
                        }
                    }
                    if (User.IsInRole(deptApprRole) && budYr.IsActive == true)
                    {
                        ck.Visible = true;
                        if (st.Text == ((int)Utility.BudgetItemStatus.Approved).ToString() || st.Text == ((int)Utility.BudgetItemStatus.Rejected).ToString())
                        {
                            //ck.Enabled = false;
                            ck.Visible = false;
                        }
                    }
                    if (User.IsInRole(AdminRole) && budYr.IsActive == true)
                    {
                        if (st.Text == ((int)Utility.BudgetItemStatus.Approved).ToString())
                        {
                            ck.Visible = true;
                        }
                        if (st.Text == ((int)Utility.BudgetItemStatus.Returned_For_Correction).ToString())
                        {
                            ck.Visible = false;
                        }
                    }
                }
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    Label lbSumAmt = e.Row.FindControl("lbSumAmt") as Label;

                    lbSumAmt.Text = priceTotal.ToString("N");
                }
            }
            catch (Exception ex)
            {
                error.Visible = true;
                error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> An error occured. Kindly try again. If error persist contact Administrator!!.";
                Utility.WriteError("Error: " + ex.InnerException);
            }
        }

        protected void gvDept_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "del")
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    // int key = Convert.ToInt32(gvDept.DataKeys[index].Value.ToString());
                    RestructureProjection estf = CommonBLL.GetRestructure(index);
                    CommonBLL.DeleteRestructure(estf);
                    BindGrid();
                    success.Visible = true;
                    success.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> Record deleted successfully!!.";
                    return;

                }
                if (e.CommandName == "edt")
                {
                    hid.Value = "Update";
                    dvID.Visible = false;
                    btnSubmit.Text = "Update";
                    btnLoad.Visible = false;
                    btnSubmit.Enabled = true;
                    //GridViewRow row = gvDept.SelectedRow;
                    int index = int.Parse(e.CommandArgument.ToString());
                    RestructureProjection estf = CommonBLL.GetRestructure(index);
                    txtID.Text = estf.ID.ToString();
                    txtobligor.Text = estf.AgreementNo;
                    lbName.Text = estf.ObligorName;
                    txtobligor.Enabled = false;
                    //ddlLoanType.SelectedValue = estf.LoanType;
                    //ddlObligor.SelectedValue = estf.ObligorID.Value.ToString();
                    txtTot.Text = estf.Amount.ToString();
                    // ddlCat.Enabled = false;
                    //ddlAsset.Enabled = false;
                    ddlMonth.SelectedValue = estf.Month.ToString();
                }
            }
            catch (Exception ex)
            {
                error.Visible = true;
                error.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button> An error occurred. kindly try again!!!";
                Utility.WriteError("Error: " + ex.InnerException);
            }
        }
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            try
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
                if (string.IsNullOrEmpty(txtcomment.Text))
                {
                    modalErr.Visible = true;
                    modalErr.InnerText = "Comment is required!!!";
                    mpeAppr.Show();
                    return;
                }
                bool isset = false; AppUser budgetInputer = new AppUser();
                foreach (GridViewRow row in gvDept.Rows)
                {
                    if (((CheckBox)row.FindControl("chkRow")).Checked)
                    {
                        Label lbID = row.FindControl("lbRecID") as Label;
                        int recID = int.Parse(lbID.Text);
                        RestructureProjection expPro = CommonBLL.GetRestructure(recID);
                        if (expPro != null && expPro.Status == (int)Utility.BudgetItemStatus.Approved)
                        {
                            expPro.Status = (int)Utility.BudgetItemStatus.Returned_For_Correction;
                            CommonBLL.UpdateRestructure(expPro);
                            budgetInputer = UserBLL.GetUserByUserName(expPro.AddedBy);
                            isset = true;
                        }
                    }
                }
                if (isset)
                {

                    BindGrid();
                    //sending mail
                    string body = "";
                    string from = ConfigurationManager.AppSettings["exUser"].ToString();
                    string siteUrl = ConfigurationManager.AppSettings["siteUrl"].ToString();
                    string appLogo = ConfigurationManager.AppSettings["appLogoUrl"].ToString();
                    string hodEmail = UserBLL.GetApproverEmailByDept(budgetInputer.DepartmentID.Value);
                    string subject = "Budget Item Correction Notification";
                    string FilePath = Server.MapPath("EmailTemplates/");
                    if (File.Exists(FilePath + "ReturnBudget.htm"))
                    {
                        FileStream f1 = new FileStream(FilePath + "ReturnBudget.htm", FileMode.Open);
                        StreamReader sr = new StreamReader(f1);
                        body = sr.ReadToEnd();
                        body = body.Replace("@add_appLogo", appLogo);
                        body = body.Replace("@siteUrl", siteUrl);
                        body = body.Replace("@BudgetElement", "Restructure Projection");
                        body = body.Replace("@add_Comment", txtcomment.Text); //Replace the values from DB or any other source to personalize each mail.  
                        f1.Close();
                    }
                    string rst = "";
                    try
                    {
                        rst = Utility.SendMail(budgetInputer.Email, from, hodEmail, subject, body);
                    }
                    catch { }
                    if (rst.Contains("Successful"))
                    {
                        success.Visible = true;
                        success.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button>Selected Record(s) has been successfully Returned for correction.Notification has been sent to Initiator";
                        return;
                    }
                    else
                    {
                        success.Visible = true;
                        success.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button>Selected Record(s) has been successfully Returned for correction.Notification could NOT be sent at this time";
                        return;
                    }

                }
                else
                {
                    BindGrid();
                    error.Visible = true;
                    error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> An error occured. Either no record is selected OR some of selected record(s) could not be approved.If error persist contact Administrator!!.";
                }
            }
            catch (Exception ex)
            {
                error.Visible = true;
                error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> An error occured. Kindly try again. If error persist contact Administrator!!.";
                Utility.WriteError("Error: " + ex.Message);
            }
        }
        protected void btnApprv_Click(object sender, EventArgs e)
        {
            try
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
                bool isset = false;
                foreach (GridViewRow row in gvDept.Rows)
                {
                    if (((CheckBox)row.FindControl("chkRow")).Checked)
                    {
                        Label lbID = row.FindControl("lbRecID") as Label;
                        int recID = int.Parse(lbID.Text);
                        RestructureProjection expPro = CommonBLL.GetRestructure(recID);
                        if (expPro != null && expPro.Status == (int)Utility.BudgetItemStatus.Pending_Approval)
                        {
                            expPro.Status = (int)Utility.BudgetItemStatus.Approved;
                            expPro.ApprovedBy = usr.FullName;
                            expPro.DateApproved = DateTime.Now;
                            CommonBLL.UpdateRestructure(expPro);
                            isset = true;
                        }
                    }
                }
                if (isset)
                {
                    BindGrid();
                    success.Visible = true;
                    success.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button>Selected Record(s) has been successfully approved.";
                    return;
                }
                else
                {
                    BindGrid();
                    error.Visible = true;
                    error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> An error occured some of selected record(s) could not be approved.If error persist contact Administrator!!.";
                }
            }
            catch (Exception ex)
            {
                error.Visible = true;
                error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> An error occured. Kindly try again. If error persist contact Administrator!!.";
                Utility.WriteError("Error: " + ex.InnerException);
            }
        }
        protected void btnReject_Click(object sender, EventArgs e)
        {
            try
            {
                bool isset = false;
                foreach (GridViewRow row in gvDept.Rows)
                {
                    if (((CheckBox)row.FindControl("chkRow")).Checked)
                    {
                        Label lbID = row.FindControl("lbRecID") as Label;
                        int recID = int.Parse(lbID.Text);
                        RestructureProjection expPro = CommonBLL.GetRestructure(recID);
                        if (expPro != null && expPro.Status == (int)Utility.BudgetItemStatus.Pending_Approval)
                        {
                            expPro.Status = (int)Utility.BudgetItemStatus.Rejected;
                            // exStf.ApprovedBy = usr.FullName;
                            //exStf.DateApproved = DateTime.Now;
                            CommonBLL.UpdateRestructure(expPro);
                            isset = true;
                        }
                    }
                }
                if (isset)
                {
                    BindGrid();
                    success.Visible = true;
                    success.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button>Selected Record(s) has been successfully rejected.";
                    return;
                }
                else
                {
                    BindGrid();
                    error.Visible = true;
                    error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> An error occured some of selected record(s) could not be approved.If error persist contact Administrator!!.";
                }
            }
            catch (Exception ex)
            {
                error.Visible = true;
                error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> An error occured. Kindly try again. If error persist contact Administrator!!.";
                Utility.WriteError("Error: " + ex.InnerException);
            }
        }
        protected void lnkAddNew_Click(object sender, EventArgs e)
        {
            try
            {
                hid.Value = "";
                dvID.Visible = false;
                btnSubmit.Text = "Add";
                txtTot.Text = "";
                btnSubmit.Enabled = false;
                btnLoad.Visible = true;
                txtobligor.Text = "";
                txtobligor.Enabled = true;
                lbName.Text = "";
                txtTot.Text = "";
               // ddlObligor.Enabled = true;
               // ddlObligor.Items.Clear();
                ddlMonth.Items.Clear();
                Utility.BindMonth(ddlMonth);
                //Utility.BindObligorToRestructure(ddlObligor);
            }
            catch (Exception ex)
            {
            }
        }

        protected string GetStatus(object o)
        {
            try
            {
                return Utility.GetBudgetStatus(o);
            }
            catch
            {
                return "";
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                bool isCatSet = false; bool isTypeSet = false; string cat = ""; int dept=0;
                if (ddlCatFilter.SelectedValue != "")
                {
                    cat = ddlCatFilter.SelectedValue;
                    isCatSet = true;
                }
               
                usr = (AppUser)Session["user"]; budYr = (BudgetYear)Session["budgetYr"];
                if (User.IsInRole(AdminRole))
                {
                    var q = CommonBLL.GetRestructureList(budYr.ID);
                    if (isCatSet)
                    {
                        q = q.Where(p => p.LoanType == cat);
                    }
                    if (ddlDeptFilter.SelectedValue != "")
                    {
                        dept = int.Parse(ddlDeptFilter.SelectedValue);
                        q = q.Where(p => p.DepartmentId == dept);

                    }
                    gvDept.DataSource = q.ToList();
                    gvDept.DataBind();
                }
                else
                {
                    var q = CommonBLL.GetRestructureList(budYr.ID,usr.DepartmentID.Value);
                    if (isCatSet)
                    {
                        q = q.Where(p => p.LoanType == cat);
                    }
                    gvDept.DataSource = q.ToList();
                    gvDept.DataBind();
                }
            }
            catch
            {
            }
        }

        protected void btnClr_Click(object sender, EventArgs e)
        {
            ddlDeptFilter.SelectedValue = "";
            BindGrid();
        }
    }
}