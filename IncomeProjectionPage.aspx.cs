using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BudgetCapture.DAL;
using BudgetCapture.BLL;
using System.Data;
using System.IO;

namespace BudgetCapture
{
    public partial class IncomeProjectionPage : System.Web.UI.Page
    {
        private static AppUser usr = null; private static BudgetYear budYr = null;
        decimal priceTotal = 0;
        private string AdminRole = ConfigurationManager.AppSettings["adminRole"].ToString();
        private string deptIniRole = ConfigurationManager.AppSettings["DeptIniRole"].ToString();
        private string deptApprRole = ConfigurationManager.AppSettings["DeptApprverRole"].ToString();
        private string rentalIncon = ConfigurationManager.AppSettings["RentalIncome"].ToString();
        private string forfeitedAsset = ConfigurationManager.AppSettings["ForfeitedAsset"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            error.Visible = false; success.Visible = false;
            dvAdd.Visible = false; //dvAppr.Visible = false;
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
                        dvSelect.Visible = true;
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
                    Utility.BindIncomeType(ddlIncomeType);
                    Utility.BindIncomeType(ddlIncTyp);
                    BindProprtyList(ddlProperty);
                    lbDept.Text = usr.Department.Name.ToUpper();
                    lbyr.Text = budYr.Year;
                    FirstGridViewRow();
                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                error.Visible = true;
                error.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button> An error occurred. kindly try again!!!";
                Utility.WriteError("Error: " + ex.InnerException);
            }
        }

        protected void ddlIncomeType_SelectedIndexChanged(object sender, EventArgs e)
        {
 
            dvProperty.Visible = false; dvDetail.Visible = false;
            if (ddlIncomeType.SelectedValue != "")
            {
                if (ddlIncomeType.SelectedValue == rentalIncon)
                {
                    dvRentalIncome.Visible = true;
                    dvAdd.Visible = false;
                }
                else
                {
                    dvRentalIncome.Visible = false;
                    dvAdd.Visible = true;
                    if (ddlIncomeType.SelectedValue == forfeitedAsset)
                    {
                        dvProperty.Visible = true;
                        dvRentalIncome.Visible=false;
                    }
                    else
                    {
                        dvDetail.Visible = true;
                        dvProperty.Visible=false;
                    }
                }
            }
        }
        private void BindGrid()
        {
            usr = (AppUser)Session["user"]; budYr = (BudgetYear)Session["budgetYr"];
            if (User.IsInRole(AdminRole))
            {
                gvDept.DataSource = CommonBLL.GetIncomeList(budYr.ID);
                gvDept.DataBind();
            }
            else
            {
                gvDept.DataSource = CommonBLL.GetIncomeList(budYr.ID, usr.DepartmentID.Value);
                gvDept.DataBind();
            }
        }
        private void BindProprtyList(DropDownList ddlist)
        {
            budYr = (BudgetYear)Session["budgetYr"];
            ddlist.DataTextField = "Name";
            ddlist.DataValueField = "ID";
            ddlist.DataSource = LookUpBLL.GetPropertyLookup(budYr.ID);
            ddlist.DataBind();
        }

        protected void gvInputCapture_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            SetRowData();
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["CurrentTable"];
                DataRow drCurrentRow = null;
                int rowIndex = Convert.ToInt32(e.RowIndex);
                if (dt.Rows.Count > 1)
                {
                    dt.Rows.Remove(dt.Rows[rowIndex]);
                    drCurrentRow = dt.NewRow();
                    ViewState["CurrentTable"] = dt;
                    gvInputCapture.DataSource = dt;
                    gvInputCapture.DataBind();

                    for (int i = 0; i < gvInputCapture.Rows.Count - 1; i++)
                    {
                        gvInputCapture.Rows[i].Cells[0].Text = Convert.ToString(i + 1);
                    }
                    SetPreviousData();
                }
            }
        }

        private void AddNewRow()
        {
            int rowIndex = 0;

            if (ViewState["CurrentTable"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                DataRow drCurrentRow = null;
                if (dtCurrentTable.Rows.Count > 0)
                {
                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {
                        DropDownList DrpPro = (DropDownList)gvInputCapture.Rows[rowIndex].Cells[1].FindControl("ddlPro");
                        TextBox TextBoxjan = (TextBox)gvInputCapture.Rows[rowIndex].Cells[2].FindControl("txtjan");
                        TextBox TextBoxfeb = (TextBox)gvInputCapture.Rows[rowIndex].Cells[3].FindControl("txtfeb");
                        TextBox TextBoxmar = (TextBox)gvInputCapture.Rows[rowIndex].Cells[4].FindControl("txtmar");
                        TextBox TextBoxapr = (TextBox)gvInputCapture.Rows[rowIndex].Cells[5].FindControl("txtapr");
                        TextBox TextBoxmay = (TextBox)gvInputCapture.Rows[rowIndex].Cells[6].FindControl("txtmay");
                        TextBox TextBoxjun = (TextBox)gvInputCapture.Rows[rowIndex].Cells[7].FindControl("txtjun");
                        TextBox TextBoxjul = (TextBox)gvInputCapture.Rows[rowIndex].Cells[8].FindControl("txtjul");
                        TextBox TextBoxaug = (TextBox)gvInputCapture.Rows[rowIndex].Cells[9].FindControl("txtaug");
                        TextBox TextBoxsep = (TextBox)gvInputCapture.Rows[rowIndex].Cells[10].FindControl("txtsep");
                        TextBox TextBoxoct = (TextBox)gvInputCapture.Rows[rowIndex].Cells[11].FindControl("txtoct");
                        TextBox TextBoxnov = (TextBox)gvInputCapture.Rows[rowIndex].Cells[12].FindControl("txtnov");
                        TextBox TextBoxdec = (TextBox)gvInputCapture.Rows[rowIndex].Cells[13].FindControl("txtdec");
                        drCurrentRow = dtCurrentTable.NewRow();
                        drCurrentRow["RowNum"] = i + 1;
                        BindProprtyList(DrpPro);//bind the property list
                        dtCurrentTable.Rows[i - 1]["Property"] = DrpPro.SelectedValue;
                        dtCurrentTable.Rows[i - 1]["Col1"] = TextBoxjan.Text;
                        dtCurrentTable.Rows[i - 1]["Col2"] = TextBoxfeb.Text;
                        dtCurrentTable.Rows[i - 1]["Col3"] = TextBoxmar.Text;
                        dtCurrentTable.Rows[i - 1]["Col4"] = TextBoxapr.Text;
                        dtCurrentTable.Rows[i - 1]["Col5"] = TextBoxmay.Text;
                        dtCurrentTable.Rows[i - 1]["Col6"] = TextBoxjun.Text;
                        dtCurrentTable.Rows[i - 1]["Col7"] = TextBoxjul.Text;
                        dtCurrentTable.Rows[i - 1]["Col8"] = TextBoxaug.Text;
                        dtCurrentTable.Rows[i - 1]["Col9"] = TextBoxsep.Text;
                        dtCurrentTable.Rows[i - 1]["Col10"] = TextBoxoct.Text;
                        dtCurrentTable.Rows[i - 1]["Col11"] = TextBoxnov.Text;
                        dtCurrentTable.Rows[i - 1]["Col12"] = TextBoxdec.Text;
                        rowIndex++;
                    }
                    dtCurrentTable.Rows.Add(drCurrentRow);
                    ViewState["CurrentTable"] = dtCurrentTable;

                    gvInputCapture.DataSource = dtCurrentTable;
                    gvInputCapture.DataBind();

                    //TextBox txn = (TextBox)gvInputCapture.Rows[rowIndex].Cells[1].FindControl("txtName");
                    //txn.Focus();
                    // txn.Focus;
                }
            }
            else
            {
                Response.Write("ViewState is null");
            }
            SetPreviousData();
        }
        private void SetPreviousData()
        {
            int rowIndex = 0;
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["CurrentTable"];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DropDownList DrpPro = (DropDownList)gvInputCapture.Rows[rowIndex].Cells[1].FindControl("ddlPro");
                        TextBox TextBoxjan = (TextBox)gvInputCapture.Rows[rowIndex].Cells[2].FindControl("txtjan");
                        TextBox TextBoxfeb = (TextBox)gvInputCapture.Rows[rowIndex].Cells[3].FindControl("txtfeb");
                        TextBox TextBoxmar = (TextBox)gvInputCapture.Rows[rowIndex].Cells[4].FindControl("txtmar");
                        TextBox TextBoxapr = (TextBox)gvInputCapture.Rows[rowIndex].Cells[5].FindControl("txtapr");
                        TextBox TextBoxmay = (TextBox)gvInputCapture.Rows[rowIndex].Cells[6].FindControl("txtmay");
                        TextBox TextBoxjun = (TextBox)gvInputCapture.Rows[rowIndex].Cells[7].FindControl("txtjun");
                        TextBox TextBoxjul = (TextBox)gvInputCapture.Rows[rowIndex].Cells[8].FindControl("txtjul");
                        TextBox TextBoxaug = (TextBox)gvInputCapture.Rows[rowIndex].Cells[9].FindControl("txtaug");
                        TextBox TextBoxsep = (TextBox)gvInputCapture.Rows[rowIndex].Cells[10].FindControl("txtsep");
                        TextBox TextBoxoct = (TextBox)gvInputCapture.Rows[rowIndex].Cells[11].FindControl("txtoct");
                        TextBox TextBoxnov = (TextBox)gvInputCapture.Rows[rowIndex].Cells[12].FindControl("txtnov");
                        TextBox TextBoxdec = (TextBox)gvInputCapture.Rows[rowIndex].Cells[13].FindControl("txtdec");
                        // drCurrentRow["RowNumber"] = i + 1;
                        BindProprtyList(DrpPro);
                        gvInputCapture.Rows[i].Cells[0].Text = Convert.ToString(i + 1);
                        DrpPro.SelectedValue = dt.Rows[i]["Property"].ToString();
                        string t = dt.Rows[i]["Property"].ToString();
                        //DrpPro.ClearSelection();
                       // DrpPro.Items.FindByText(dt.Rows[i]["Col1"].ToString()).Selected = true;
                        TextBoxjan.Text = dt.Rows[i]["Col1"].ToString();
                        TextBoxfeb.Text = dt.Rows[i]["Col2"].ToString();
                        TextBoxmar.Text = dt.Rows[i]["Col3"].ToString();
                        TextBoxapr.Text = dt.Rows[i]["Col4"].ToString();
                        TextBoxmay.Text = dt.Rows[i]["Col5"].ToString();
                        TextBoxjun.Text = dt.Rows[i]["Col6"].ToString();
                        TextBoxjul.Text = dt.Rows[i]["Col7"].ToString();
                        TextBoxaug.Text = dt.Rows[i]["Col8"].ToString();
                        TextBoxsep.Text = dt.Rows[i]["Col9"].ToString();
                        TextBoxoct.Text = dt.Rows[i]["Col10"].ToString();
                        TextBoxnov.Text = dt.Rows[i]["Col11"].ToString();
                        TextBoxdec.Text = dt.Rows[i]["Col12"].ToString();
                        rowIndex++;
                    }
                }
            }
        }
        private void SetRowData()
        {
            int rowIndex = 0;

            if (ViewState["CurrentTable"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                DataRow drCurrentRow = null;
                if (dtCurrentTable.Rows.Count > 0)
                {
                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {
                        DropDownList DrpPro = (DropDownList)gvInputCapture.Rows[rowIndex].Cells[1].FindControl("ddlPro");
                        TextBox TextBoxjan = (TextBox)gvInputCapture.Rows[rowIndex].Cells[2].FindControl("txtjan");
                        TextBox TextBoxfeb = (TextBox)gvInputCapture.Rows[rowIndex].Cells[3].FindControl("txtfeb");
                        TextBox TextBoxmar = (TextBox)gvInputCapture.Rows[rowIndex].Cells[4].FindControl("txtmar");
                        TextBox TextBoxapr = (TextBox)gvInputCapture.Rows[rowIndex].Cells[5].FindControl("txtapr");
                        TextBox TextBoxmay = (TextBox)gvInputCapture.Rows[rowIndex].Cells[6].FindControl("txtmay");
                        TextBox TextBoxjun = (TextBox)gvInputCapture.Rows[rowIndex].Cells[7].FindControl("txtjun");
                        TextBox TextBoxjul = (TextBox)gvInputCapture.Rows[rowIndex].Cells[8].FindControl("txtjul");
                        TextBox TextBoxaug = (TextBox)gvInputCapture.Rows[rowIndex].Cells[9].FindControl("txtaug");
                        TextBox TextBoxsep = (TextBox)gvInputCapture.Rows[rowIndex].Cells[10].FindControl("txtsep");
                        TextBox TextBoxoct = (TextBox)gvInputCapture.Rows[rowIndex].Cells[11].FindControl("txtoct");
                        TextBox TextBoxnov = (TextBox)gvInputCapture.Rows[rowIndex].Cells[12].FindControl("txtnov");
                        TextBox TextBoxdec = (TextBox)gvInputCapture.Rows[rowIndex].Cells[13].FindControl("txtdec");
                        drCurrentRow = dtCurrentTable.NewRow();
                        drCurrentRow["RowNum"] = i + 1;
                        dtCurrentTable.Rows[i - 1]["Property"] = DrpPro.SelectedValue;
                        dtCurrentTable.Rows[i - 1]["Col1"] = TextBoxjan.Text;
                        dtCurrentTable.Rows[i - 1]["Col2"] = TextBoxfeb.Text;
                        dtCurrentTable.Rows[i - 1]["Col3"] = TextBoxmar.Text;
                        dtCurrentTable.Rows[i - 1]["Col4"] = TextBoxapr.Text;
                        dtCurrentTable.Rows[i - 1]["Col5"] = TextBoxmay.Text;
                        dtCurrentTable.Rows[i - 1]["Col6"] = TextBoxjun.Text;
                        dtCurrentTable.Rows[i - 1]["Col7"] = TextBoxjul.Text;
                        dtCurrentTable.Rows[i - 1]["Col8"] = TextBoxaug.Text;
                        dtCurrentTable.Rows[i - 1]["Col9"] = TextBoxsep.Text;
                        dtCurrentTable.Rows[i - 1]["Col10"] = TextBoxoct.Text;
                        dtCurrentTable.Rows[i - 1]["Col11"] = TextBoxnov.Text;
                        dtCurrentTable.Rows[i - 1]["Col12"] = TextBoxdec.Text;
                        rowIndex++;
                    }

                    ViewState["CurrentTable"] = dtCurrentTable;
                    //grvStudentDetails.DataSource = dtCurrentTable;
                    //grvStudentDetails.DataBind();
                }
            }
            else
            {
                Response.Write("ViewState is null");
            }
            //SetPreviousData();
        }
        protected void ButtonAdd_Click(object sender, EventArgs e)
        {
            AddNewRow();
        }
        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            try
            {
                SetRowData();
                DataTable table = ViewState["CurrentTable"] as DataTable;
                usr = (AppUser)Session["user"]; budYr = (BudgetYear)Session["budgetYr"];
                bool isSave = false;
                if (table != null)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        decimal amt = 0; int month = 0; int propID = 0;
                        IncomeProjection income = new IncomeProjection();
                        string dlProp = row.ItemArray[1] as string;
                        string txjan = row.ItemArray[2] as string;
                        string txfeb = row.ItemArray[3] as string;
                        string txmar = row.ItemArray[4] as string;
                        string txapr = row.ItemArray[5] as string;
                        string txmay = row.ItemArray[6] as string;
                        string txjun = row.ItemArray[7] as string;
                        string txjul = row.ItemArray[8] as string;
                        string txaug = row.ItemArray[9] as string;
                        string txsep = row.ItemArray[10] as string;
                        string txoct = row.ItemArray[11] as string;
                        string txnov = row.ItemArray[12] as string;
                        string txdec = row.ItemArray[13] as string;

                        if (dlProp != null)
                        {
                            propID = int.Parse(dlProp); 
                        }
                        if (!string.IsNullOrEmpty(txjan)) { amt = decimal.Parse(txjan); month = 1; }
                        if (!string.IsNullOrEmpty(txfeb)) { amt = decimal.Parse(txfeb); month = 2; }
                        if (!string.IsNullOrEmpty(txmar)) { amt = decimal.Parse(txmar); month = 3; }
                        if (!string.IsNullOrEmpty(txapr)) { amt = decimal.Parse(txapr); month = 4; }
                        if (!string.IsNullOrEmpty(txmay)) { amt = decimal.Parse(txmay); month = 5; }
                        if (!string.IsNullOrEmpty(txjun)) { amt = decimal.Parse(txjun); month = 6; }
                        if (!string.IsNullOrEmpty(txjul)) { amt = decimal.Parse(txjul); month = 7; }
                        if (!string.IsNullOrEmpty(txaug)) { amt = decimal.Parse(txaug); month = 8; }
                        if (!string.IsNullOrEmpty(txsep)) { amt = decimal.Parse(txsep); month = 9; }
                        if (!string.IsNullOrEmpty(txoct)) { amt = decimal.Parse(txoct); month = 10; }
                        if (!string.IsNullOrEmpty(txnov)) { amt = decimal.Parse(txnov); month = 11; }
                        if (!string.IsNullOrEmpty(txdec)) { amt = decimal.Parse(txdec); month = 12; }

                        income.PropertyID = propID;
                        income.IncomeTypeId = int.Parse(ddlIncomeType.SelectedValue);
                        income.DepartmentId = usr.DepartmentID;
                        income.AddeBy = User.Identity.Name;
                        income.DateAdded = DateTime.Now;
                        income.Amount = amt;
                        income.BudgetYrID = budYr.ID;
                        income.Month = month;
                        income.Status = (int)Utility.BudgetItemStatus.Pending_Approval;

                        bool result = CommonBLL.AddIncomePro(income);
                        if(result)
                        {
                            isSave = true;
                        }else
                        {
                            isSave = false;
                            break;
                        }

                    }

                    if (isSave)
                    {
                        BindGrid();
                        ViewState["CurrentTable"] = null;
                        dvRentalIncome.Visible = false;
                        ddlIncomeType.SelectedValue = "";
                        success.Visible = true;
                        success.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> All Record(s) has been added successfully!!.";
                        return;
                    }else
                    {
                        error.Visible = true;
                        error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button>An error occured while saving the inputted data.Kindly review your input and try again.If error persist contact Administrator!!.";
                        return; 
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
        protected void lnkAddNew_Click(object sender, EventArgs e)
        {
            try
            {
                //hid.Value = "";
                dvID.Visible = false;
                btnSubmit.Text = "Add";
                txtTot.Text = "";
                ddlProperty.Enabled = true;
                ddlProperty.Items.Clear();
                ddlMonth.Items.Clear();
                Utility.BindMonth(ddlMonth);
                BindProprtyList(ddlProperty);
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            IncomeProjection income = null;
            try
            {
                usr = (AppUser)Session["user"]; budYr = (BudgetYear)Session["budgetYr"];
                if (hid.Value == "Update")
                {
                    bool rst = false; decimal tot = 0; int month = int.Parse(ddlMonth.SelectedValue);
                    income = CommonBLL.GetIncomePro(Convert.ToInt32(txtID.Text));
                    if (income != null)
                    {
                        if (!decimal.TryParse(txtTot.Text, out tot))
                        {
                            error.Visible = true;
                            error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button>Amount must be numeric!!.";
                            return;
                        }
                        income.Amount = tot;
                        income.IncomeTypeId = int.Parse(ddlIncomeType.SelectedValue);
                        if (ddlIncomeType.SelectedValue == forfeitedAsset)
                        {
                            income.PropertyID = int.Parse(ddlProperty.SelectedValue);
                        }
                        else
                        {
                            income.Details = txtDetail.Text;
                        }
                        income.DepartmentId = usr.DepartmentID;
                        income.Status = (int)Utility.BudgetItemStatus.Pending_Approval;
                        income.Month = month;
                        income.BudgetYrID = budYr.ID;
                        rst = CommonBLL.UpdateIncomePro(income);
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
                        error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button>Amount must be numeric!!.";
                        return;
                    }
                    bool result = false;
                    income = new IncomeProjection(); int month = int.Parse(ddlMonth.SelectedValue);
                    income.IncomeTypeId = int.Parse(ddlIncomeType.SelectedValue);
                    income.DepartmentId = usr.DepartmentID;
                    if (ddlIncomeType.SelectedValue == forfeitedAsset)
                    {
                        income.PropertyID = int.Parse(ddlProperty.SelectedValue);
                    }else
                    {
                        income.Details = txtDetail.Text;
                    }
                    income.AddeBy = User.Identity.Name;
                    income.DateAdded = DateTime.Now;
                    income.Amount = tot;
                    income.BudgetYrID = budYr.ID;
                    income.Month = month;
                    income.Status = (int)Utility.BudgetItemStatus.Pending_Approval;

                    result = CommonBLL.AddIncomePro(income);
                    if (result)
                    {
                        BindGrid();
                        txtTot.Text = "";
                        dvDetail.Visible = false;
                        dvProperty.Visible = false;
                        ddlIncomeType.SelectedValue = "";
                        ddlMonth.SelectedValue = "";
                        ddlProperty.SelectedValue = "";
                        txtDetail.Text = "";
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

        private void FirstGridViewRow()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("RowNum", typeof(string)));
            dt.Columns.Add(new DataColumn("Property", typeof(string)));
            dt.Columns.Add(new DataColumn("Col1", typeof(string)));
            dt.Columns.Add(new DataColumn("Col2", typeof(string)));
            dt.Columns.Add(new DataColumn("Col3", typeof(string)));
            dt.Columns.Add(new DataColumn("Col4", typeof(string)));
            dt.Columns.Add(new DataColumn("Col5", typeof(string)));
            dt.Columns.Add(new DataColumn("Col6", typeof(string)));
            dt.Columns.Add(new DataColumn("Col7", typeof(string)));
            dt.Columns.Add(new DataColumn("Col8", typeof(string)));
            dt.Columns.Add(new DataColumn("Col9", typeof(string)));
            dt.Columns.Add(new DataColumn("Col10", typeof(string)));
            dt.Columns.Add(new DataColumn("Col11", typeof(string)));
            dt.Columns.Add(new DataColumn("Col12", typeof(string)));
            dr = dt.NewRow();
            dr["RowNum"] = 1;
            dr["Property"] = string.Empty;
            dr["Col1"] = string.Empty;
            dr["Col2"] = string.Empty;
            dr["Col3"] = string.Empty;
            dr["Col4"] = string.Empty;
            dr["Col5"] = string.Empty;
            dr["Col6"] = string.Empty;
            dr["Col7"] = string.Empty;
            dr["Col8"] = string.Empty;
            dr["Col9"] = string.Empty;
            dr["Col10"] = string.Empty;
            dr["Col11"] = string.Empty;
            dr["Col12"] = string.Empty;
            dt.Rows.Add(dr);

            ViewState["CurrentTable"] = dt;


            gvInputCapture.DataSource = dt;
            gvInputCapture.DataBind();

            DropDownList ddl1 = (DropDownList)gvInputCapture.Rows[0].Cells[1].FindControl("ddlPro");
            BindProprtyList(ddl1);
            Button btnAdd = (Button)gvInputCapture.FooterRow.Cells[5].FindControl("ButtonAdd");
            Page.Form.DefaultFocus = btnAdd.ClientID;

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                usr = (AppUser)Session["user"]; budYr = (BudgetYear)Session["budgetYr"];
                if (ddlIncTyp.SelectedValue != "")
                {
                    int expTy = int.Parse(ddlIncTyp.SelectedValue);
                    if (User.IsInRole(AdminRole))
                    {
                        gvDept.DataSource = CommonBLL.GetIncomeList(budYr.ID).Where(b => b.IncomeTypeId.Value == expTy).ToList();
                        gvDept.DataBind();
                    }
                    else
                    {
                        gvDept.DataSource = CommonBLL.GetIncomeList(budYr.ID, usr.DepartmentID.Value).Where(b => b.IncomeTypeId.Value == expTy).ToList();
                        gvDept.DataBind();
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

        protected void btnClr_Click(object sender, EventArgs e)
        {
            try
            {
                ddlIncTyp.SelectedValue = "";
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
                    Label lbIncType = e.Row.FindControl("lbID") as Label;
                    Label lbProp = e.Row.FindControl("lbProp") as Label;
                    Label lbDetail = e.Row.FindControl("lbDetail") as Label;
                    if (lbIncType.Text == forfeitedAsset || lbIncType.Text==rentalIncon)
                    {
                        lbProp.Visible = true;
                    }
                    else
                    {
                        lbDetail.Visible = true;
                    }
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

        protected void gvDept_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "del")
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    // int key = Convert.ToInt32(gvDept.DataKeys[index].Value.ToString());
                    IncomeProjection estf = CommonBLL.GetIncomePro(index);
                    CommonBLL.DeleteIncomePro(estf);
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
                    dvAdd.Visible=true;
                    dvRentalIncome.Visible=false;
                    //GridViewRow row = gvDept.SelectedRow;
                    int index = int.Parse(e.CommandArgument.ToString());
                    IncomeProjection estf = CommonBLL.GetIncomePro(index);
                    txtID.Text = estf.ID.ToString();
                    if(estf.IncomeTypeId.ToString()==forfeitedAsset || estf.IncomeTypeId.ToString()==rentalIncon)
                    {
                        dvProperty.Visible = true;
                        dvDetail.Visible = false;
                        ddlProperty.SelectedValue = estf.PropertyID.Value.ToString();
                    }
                    else
                    {
                        dvDetail.Visible = true;
                        dvProperty.Visible = false;
                        txtDetail.Text = estf.Details;
                    }
                    txtTot.Text = estf.Amount.ToString();
                    ddlIncomeType.SelectedValue = estf.IncomeTypeId.ToString();
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
                        IncomeProjection expPro = CommonBLL.GetIncomePro(recID);
                        if (expPro != null && expPro.Status == (int)Utility.BudgetItemStatus.Approved)
                        {
                            expPro.Status = (int)Utility.BudgetItemStatus.Returned_For_Correction;
                            CommonBLL.UpdateIncomePro(expPro);
                            budgetInputer = UserBLL.GetUserByUserName(expPro.AddeBy);
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
                        body = body.Replace("@BudgetElement", "Inflow Projection");
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
                        IncomeProjection expPro = CommonBLL.GetIncomePro(recID);
                        if (expPro != null && expPro.Status == (int)Utility.BudgetItemStatus.Pending_Approval)
                        {
                            expPro.Status = (int)Utility.BudgetItemStatus.Approved;
                            expPro.ApprovedBy = usr.FullName;
                            expPro.ApprovedDate = DateTime.Now;
                            CommonBLL.UpdateIncomePro(expPro);
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
                        IncomeProjection expPro = CommonBLL.GetIncomePro(recID);
                        if (expPro != null && expPro.Status == (int)Utility.BudgetItemStatus.Pending_Approval)
                        {
                            expPro.Status = (int)Utility.BudgetItemStatus.Rejected;
                            // exStf.ApprovedBy = usr.FullName;
                            //exStf.DateApproved = DateTime.Now;
                            CommonBLL.UpdateIncomePro(expPro);
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
    }
}