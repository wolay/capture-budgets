using BudgetCapture.BLL;
using BudgetCapture.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BudgetCapture.Admin
{
    public partial class SetupCustomer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            error.Visible = false; success.Visible = false;
            if(!IsPostBack){
                if (Request.QueryString["update"] != null && Request.QueryString["Id"] != null)
                {
                    int upd = int.Parse(Request.QueryString["update"].ToString());
                    if (upd == 1)
                    {
                        int Id = int.Parse(Request.QueryString["Id"].ToString());
                        GSS_Customer c = LookUpBLL.GetGssCustomer(Id);
                        if (c != null)
                        {
                            hid.Value = "Update";
                            btnSubmit.Text = "Update";
                            dvID.Visible = true;
                            txtID.Text = c.ID.ToString();
                            txtAsset1.Text = c.CompanyName;
                            ddlDir.SelectedValue = c.CustomerTypeID.ToString();
                            txtaddr.Text = c.CompanyAddress;
                            txtperson.Text = c.ContactPerson;
                            txtPhone.Text = c.MobileNo;
                            txtEmail.Text = c.Email;
                            ddlSection.SelectedValue = c.PipelineSectionID.ToString();
                            ddlTerm.SelectedValue = c.PaymentTermFlg.ToString();
                            ddlVat.SelectedValue = c.VATFlg.ToString();
                            txtPrice.Text = c.UnitPrice.Value.ToString();
                            txtMinOrder.Text = c.MinimumOrderVolume.Value.ToString();
                            if (c.CapitalRecovery.Value)
                            {
                                chkCap.Checked = true;
                            }
                            else
                            {
                                chkCap.Checked = false;
                            }
                            if (c.isActive.Value)
                            {
                                chkActive.Checked = true;
                            }
                            else
                            {
                                chkActive.Checked = false;
                            }
                        }
                    }
                }
            }
            if (!IsPostBack)
            {
                Utility.BindCustomerType(ddlDir);
                Utility.BindPipelineSection(ddlSection);
                Utility.BindPaymentTerm(ddlTerm);

            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
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
                string compName = ""; int custtype = 0; string Addr = ""; decimal uPrice = 0; decimal minOrder = 0; string email = "";
                string mobileno = ""; int pipeSection = 0; int paymtFlg = 0; int vatFlg = 0; string contactPer = "";
                if (hid.Value == "Update")
                {
                    if (!decimal.TryParse(txtPrice.Text, out uPrice) || !decimal.TryParse(txtMinOrder.Text, out minOrder))
                    {
                        error.Visible = true;
                        error.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button>UnitPrice and minimum order must be numeric!!.";
                        return;
                    }
                   int Id = int.Parse(Request.QueryString["Id"].ToString());
                    GSS_Customer c = LookUpBLL.GetGssCustomer(Id);
                    c.CompanyName = txtAsset1.Text;
                    c.CustomerTypeID = int.Parse(ddlDir.SelectedValue);
                    c.CompanyAddress = txtaddr.Text;
                    c.ContactPerson = txtperson.Text;
                    c.MobileNo = txtPhone.Text;
                    c.Email = txtEmail.Text;
                    c.PipelineSectionID = int.Parse(ddlSection.SelectedValue);
                    c.PaymentTermFlg = int.Parse(ddlTerm.SelectedValue);
                    c.VATFlg = int.Parse(ddlVat.SelectedValue);
                    c.UnitPrice = uPrice;
                    c.MinimumOrderVolume = minOrder;
                     c.LastUpdatedBy=usr.FullName;
                    c.LastUpdatedDate=DateTime.Now;
                    if (chkCap.Checked)
                    {
                        c.CapitalRecovery = true;
                    }
                    else
                    {
                        c.CapitalRecovery = false;
                    }
                    if (chkActive.Checked)
                    {
                        c.isActive = true;
                    }
                    else
                    {
                        c.isActive = false;
                    }

                    if (LookUpBLL.UpdateGssCustomer(c))
                    {
                        ClearInput();
                        Response.Redirect("ManageGssCustomer.aspx?success=2&&newRecordId=" + c.ID.ToString(), false);
                    }
                    else
                    {
                        error.Visible = true;
                        error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button>Record could Not added. Kindly try again. If error persist contact Administrator!!.";
                    }
                }
                else
                {
                    if (!decimal.TryParse(txtPrice.Text, out uPrice) || !decimal.TryParse(txtMinOrder.Text, out minOrder))
                    {
                        error.Visible = true;
                        error.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button>UnitPrice and minimum order must be numeric!!.";
                        return;
                    }
                    GSS_Customer c = new GSS_Customer();
                    c.CompanyName = txtAsset1.Text;
                    c.CustomerTypeID = int.Parse(ddlDir.SelectedValue);
                    c.CompanyAddress = txtaddr.Text;
                    c.ContactPerson = txtperson.Text;
                    c.MobileNo = txtPhone.Text;
                    c.Email = txtEmail.Text;
                    c.PipelineSectionID = int.Parse(ddlSection.SelectedValue);
                    c.PaymentTermFlg = int.Parse(ddlTerm.SelectedValue);
                    c.VATFlg = int.Parse(ddlVat.SelectedValue);
                    c.UnitPrice = uPrice;
                    c.MinimumOrderVolume = minOrder;
                    c.AddedBy = usr.FullName;
                    c.DateAdded = DateTime.Now;
                    if (chkCap.Checked)
                    {
                        c.CapitalRecovery = true;
                    }
                    else
                    {
                        c.CapitalRecovery = false;
                    }
                    if (chkActive.Checked)
                    {
                        c.isActive = true;
                    }
                    else
                    {
                        c.isActive = false;
                    }

                    if (LookUpBLL.AddGssCustomer(c))
                    {
                        ClearInput();
                        Response.Redirect("ManageGssCustomer.aspx?success=1&&newRecordId=" + c.ID.ToString(), false);
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
        private void ClearInput()
        {
            txtaddr.Text = ""; txtAsset1.Text = ""; txtEmail.Text = ""; txtMinOrder.Text = "";
            txtperson.Text = ""; txtPhone.Text = ""; txtPrice.Text = ""; ddlDir.SelectedValue = "";
            ddlSection.SelectedValue = ""; ddlTerm.SelectedValue = ""; ddlVat.SelectedValue = "";
        }
    }
}