using BudgetCapture.BLL;
using BudgetCapture.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BudgetCapture
{
    public partial class _Default : System.Web.UI.Page
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
                    List<DeptBudgetItem> budMenuLst = LookUpBLL.GetDeptMenuItem(deptID).ToList();
                    if (budMenuLst.Count() > 0)
                    {
                        foreach (DeptBudgetItem dm in budMenuLst)
                        {
                            if (dm.Code == "CA")//for capex
                                Utility.ComputeBudgetSummaryCapexProjection(budyrID, deptID);
                            if (dm.Code == "DI")
                                Utility.ComputeBudgetSummaryDirectProjection(budyrID, deptID);
                            if (dm.Code == "ID")
                                Utility.ComputeBudgetSummaryIndirectProjection(budyrID, deptID);
                            if (dm.Code == "MO")
                                Utility.ComputeBudgetSummaryFixedAsset(budyrID, deptID);
                            if (dm.Code == "RE")
                                Utility.ComputeBudgetSummaryRevenue(budyrID, deptID);
                            
                            if (dm.Code == "SA")
                                Utility.ComputeBudgetSummaryExistingStff(budyrID, deptID);
                        }
                    }
                    if (User.IsInRole(AdminRole) || User.IsInRole(PBMgrRole) || User.IsInRole(PBOffRole))
                    {
                        dvFilter.Visible = true;
                        Utility.BindDept(ddlDept);
                        List<Department> deptList = DepartmentBLL.GetDeptList();
                    }
                    if (User.IsInRole(MDRole))
                    {
                        dvFilter.Visible = true;
                        Utility.BindDept(ddlDept);
                        List<Department> deptList = DepartmentBLL.GetDeptList();
                        lbDept.Visible = false;
                    }
                    if (User.IsInRole(EDRole))
                    {
                        dvFilter.Visible = true;
                        Utility.BindDept(ddlDept, usr.Department.DirectorateID.Value);
                        List<Department> deptList = DepartmentBLL.GetDeptList(usr.Department.DirectorateID.Value);
                        lbDept.Visible = false;
                    }
                    BindGrid();
                    if (User.IsInRole(deptApprRole))
                    {
                        List<TrackApproval> getTrackApproval = null;
                        getTrackApproval = LookUpBLL.GetTrackApproval(usr.DepartmentID.Value, budYr.ID).ToList();
                        if (getTrackApproval != null && getTrackApproval.Count() > 0)
                        {
                            TrackApproval tt = getTrackApproval.FirstOrDefault();
                            if (tt.status == (int)Utility.BudgetItemStatus.Returned_For_Correction)
                            {
                                dvDept.Visible = true;
                                dvExco.Visible = false;
                                //hidEDPending.Value = "1";
                            }
                        }
                        else
                        {
                            dvDept.Visible = true;
                            dvExco.Visible = false;
                            //hidEDPending.Value = "1";
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                error.Visible = true;
                error.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button> An error occurred. kindly try again!!!";
                Utility.WriteError("Error: " + ex.Message);
            }
        }
        private void BindGrid()
        {
            usr = (AppUser)Session["user"]; budYr = (BudgetYear)Session["budgetYr"];
            if (User.IsInRole(AdminRole) || User.IsInRole(PBMgrRole) || User.IsInRole(PBOffRole) || User.IsInRole(MDRole))
            {
                gvDept.DataSource = CommonBLL.GetBudgetSummaryList(budYr.ID);
                gvDept.DataBind();
            }
            else if (User.IsInRole(EDRole))
            {
                gvDept.DataSource = CommonBLL.GetBudgetSummaryList(budYr.ID,0, usr.Department.DirectorateID.Value);
                gvDept.DataBind();
            }
            else
            {
                gvDept.DataSource = CommonBLL.GetBudgetSummaryList(budYr.ID, usr.DepartmentID.Value);
                gvDept.DataBind();
            }
        }

        protected void gvDept_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                 // budYr = (BudgetYear)Session["budgetYr"];
                  if (e.Row.RowType == DataControlRowType.DataRow)
                  {
                      Label lbDept = e.Row.FindControl("lbDept") as Label;
                      Label lbsTot = e.Row.FindControl("lblyear") as Label;
                      amtTot+=decimal.Parse(lbsTot.Text);
                      if (User.IsInRole(AdminRole) || User.IsInRole(PBMgrRole) || User.IsInRole(PBOffRole))
                      {
                          lbDept.Visible = true;
                      }
                      LinkButton imgEdit = e.Row.FindControl("imgBtnEdit") as LinkButton;
                      if (User.IsInRole(EDRole) || User.IsInRole(MDRole))
                      {
                          //if(hidEDPending.Value=="1")
                          imgEdit.Visible = true;
                      }
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

        protected void gvDept_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDept.PageIndex = e.NewPageIndex;
                BindGrid();
            }
            catch (Exception ex)
            {
                Utility.WriteError("Error: " + ex.Message);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                usr = (AppUser)Session["user"]; budYr = (BudgetYear)Session["budgetYr"];
                if(ddlDept.SelectedValue!="")
                {
                    int deptId = int.Parse(ddlDept.SelectedValue); budYr = (BudgetYear)Session["budgetYr"];
                    int budyrID = budYr.ID;
                    if (User.IsInRole(EDRole))
                    {
                        List<TrackApproval> getTrackApproval = null;
                        getTrackApproval = LookUpBLL.GetTrackApproval(deptId, budYr.ID).ToList();
                        if (getTrackApproval != null && getTrackApproval.Count() > 0)
                        {
                            TrackApproval tt = getTrackApproval.FirstOrDefault();
                            if (tt.status == (int)Utility.BudgetItemStatus.Pending_ED_Approval)
                            {
                                dvExco.Visible = true;
                                btnCorrect.Visible = true;
                                hidEDPending.Value = "1";
                            }
                            if (tt.status == (int)Utility.BudgetItemStatus.Pending_MD_Approval)
                            {
                                dvExco.Visible = true;
                                btnCorrect.Visible = false;
                                btnReturn.Visible = true;
                                btnReturn.Text = "Return For Correction After Review";
                                hidEDPending.Value = "1";
                            }
                            //dvExco.Visible = false;
                            //btnCorrect.Visible = false;
                            //hidEDPending.Value = "1";
                        }
                         
                        List<DeptBudgetItem> budMenuLst = LookUpBLL.GetDeptMenuItem(deptId).ToList();
                        if (budMenuLst.Count() > 0)
                        {
                            foreach (DeptBudgetItem dm in budMenuLst)
                            {
                                if (dm.Code == "CA")//for capex
                                    Utility.ComputeBudgetSummaryCapexProjection(budyrID, deptId);
                                if (dm.Code == "DI")
                                    Utility.ComputeBudgetSummaryDirectProjection(budyrID, deptId);
                                if (dm.Code == "ID")
                                    Utility.ComputeBudgetSummaryIndirectProjection(budyrID, deptId);
                                if (dm.Code == "MO")
                                    Utility.ComputeBudgetSummaryFixedAsset(budyrID, deptId);
                                if (dm.Code == "RE")
                                    Utility.ComputeBudgetSummaryRevenue(budyrID, deptId);

                                if (dm.Code == "SA")
                                    Utility.ComputeBudgetSummaryExistingStff(budyrID, deptId);
                            }
                        }
                    }
                    gvDept.DataSource = CommonBLL.GetBudgetSummaryList(budYr.ID).Where(d=>d.DepartmentID==deptId).ToList();
                    gvDept.DataBind();
                    lbmsg.Text = "";
                    lbmsg.Text = ddlDept.SelectedItem.Text + " Department Budgets";
                    
                    if (User.IsInRole(MDRole))
                      {
                          List<TrackApproval> getTrackApproval = null;
                          getTrackApproval = LookUpBLL.GetTrackApproval(deptId, budYr.ID).ToList();
                          if (getTrackApproval != null && getTrackApproval.Count() > 0)//if selected department has been pushed for approval
                          {
                              TrackApproval tt = getTrackApproval.FirstOrDefault();
                              if (tt.status == (int)Utility.BudgetItemStatus.Pending_MD_Approval)
                              {
                                  dvExco.Visible = true;
                                  btnMD.Visible = true;
                                  hidMDPending.Value = "1";
                              }
                              if (tt.status == (int)Utility.BudgetItemStatus.Pending_MD_Final_Approval)
                              {
                                  dvExco.Visible = true;
                                  btnMD.Visible = false;
                                  btnReject.Visible = false;
                                  btnMDFinal.Visible = true;
                                  hidMDPending.Value = "1";
                              }

                          }
                          else
                          {
                              dvExco.Visible = false;
                              btnMD.Visible = false;
                          }
                    }
                    if (User.IsInRole(AdminRole) || User.IsInRole(PBMgrRole) || User.IsInRole(PBOffRole))
                    {
                        List<TrackApproval> getTrackApproval = null;
                        getTrackApproval = LookUpBLL.GetTrackApproval(deptId, budYr.ID).ToList();
                        if (getTrackApproval != null && getTrackApproval.Count() > 0)//if selected department has been pushed for approval
                        {
                            TrackApproval tt = getTrackApproval.FirstOrDefault();
                            if (tt.status == (int)Utility.BudgetItemStatus.Pending_ReAlignment)
                            {
                                dvPB.Visible = true;
                                btnMD.Visible = false;
                                hidMDPending.Value = "1";

                                List<DeptBudgetItem> budMenuLst = LookUpBLL.GetDeptMenuItem(deptId).ToList();
                                if (budMenuLst.Count() > 0)
                                {
                                    foreach (DeptBudgetItem dm in budMenuLst)
                                    {
                                        if (dm.Code == "CA")//for capex
                                            Utility.ComputeBudgetSummaryCapexProjection(budyrID, deptId);
                                        if (dm.Code == "DI")
                                            Utility.ComputeBudgetSummaryDirectProjection(budyrID, deptId);
                                        if (dm.Code == "ID")
                                            Utility.ComputeBudgetSummaryIndirectProjection(budyrID, deptId);
                                        if (dm.Code == "MO")
                                            Utility.ComputeBudgetSummaryFixedAsset(budyrID, deptId);
                                        if (dm.Code == "RE")
                                            Utility.ComputeBudgetSummaryRevenue(budyrID, deptId);

                                        if (dm.Code == "SA")
                                            Utility.ComputeBudgetSummaryExistingStff(budyrID, deptId);
                                    }
                                }

                            }
                            gvDept.DataSource = CommonBLL.GetBudgetSummaryList(budYr.ID).Where(d => d.DepartmentID == deptId).ToList();
                            gvDept.DataBind();
                            lbPBmsg.Text = "";
                            lbPBmsg.Text = ddlDept.SelectedItem.Text + " Department Budgets";
                        }
                        else
                        {
                            dvExco.Visible = false;
                            btnMD.Visible = false;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                error.Visible = true;
                error.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button> An error occurred. kindly try again!!!";
                Utility.WriteError("Error: " + ex.Message);
            }
        }

        protected void btnClr_Click(object sender, EventArgs e)
        {
            ddlDept.SelectedValue = "";
            BindGrid();
            dvExco.Visible = false;
            btnCorrect.Visible = false;
            btnMD.Visible = false;
        }

        protected void btnCorrect_Click(object sender, EventArgs e)
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
                int deptID = int.Parse(ddlDept.SelectedValue);
                List<DeptBudgetItem> budMenuLst = LookUpBLL.GetDeptMenuItem(deptID).ToList();
                if (budMenuLst.Count() > 0)
                {
                    foreach (DeptBudgetItem dm in budMenuLst)
                    {
                        if (dm.Code == "CA")//for capex
                            Utility.UpdateBudgetCapex((int)Utility.BudgetItemStatus.Pending_MD_Approval, deptID, usr.FullName, 1);
                        if (dm.Code == "DI")
                            Utility.UpdateBudgetDirect((int)Utility.BudgetItemStatus.Pending_MD_Approval, deptID, usr.FullName, 1);
                        if (dm.Code == "ID")
                            Utility.UpdateBudgetIndirect((int)Utility.BudgetItemStatus.Pending_MD_Approval, deptID, usr.FullName, 1);
                        if (dm.Code == "MO")
                            Utility.UpdateBudgetMovable((int)Utility.BudgetItemStatus.Pending_MD_Approval, deptID, usr.FullName,1);
                        if (dm.Code == "RE")
                            Utility.UpdateBudgetRevenue((int)Utility.BudgetItemStatus.Pending_MD_Approval, deptID, usr.FullName, 1);
                        if (dm.Code == "SA")
                            Utility.UpdateBudgetExistingSf((int)Utility.BudgetItemStatus.Pending_MD_Approval, deptID, usr.FullName, 1);
                     }
                    BudgetYear budYr = (BudgetYear)Session["budgetYr"];
                    List<TrackApproval> getTrackApproval = null;
                    getTrackApproval = LookUpBLL.GetTrackApproval(deptID, budYr.ID).ToList();
                    if (getTrackApproval != null && getTrackApproval.Count() > 0)
                    {
                        TrackApproval appTrack = getTrackApproval.FirstOrDefault();
                        appTrack.status = (int)Utility.BudgetItemStatus.Pending_MD_Approval;
                        appTrack.ActionBy = usr.FullName;
                        LookUpBLL.UpdateApprovalStatus(appTrack);
                        dvExco.Visible = false;
                        success.Visible = true;
                        success.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button>Budget has been successfully approved.";
                        return;
                    }
                    else
                    {
                       LookUpBLL.AddApprovaltrack(new TrackApproval() { BudgetYr = budYr.ID, DepartmentID = deptID, ActionBy = usr.FullName, dateApproved = DateTime.Now, status = (int)Utility.BudgetItemStatus.Pending_MD_Approval });
                       dvExco.Visible = false;
                       success.Visible = true;
                       success.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button>Budget has been successfully approved.";
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

        protected void btnMD_Click(object sender, EventArgs e)
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
                int deptID = int.Parse(ddlDept.SelectedValue);
                List<DeptBudgetItem> budMenuLst = LookUpBLL.GetDeptMenuItem(deptID).ToList();
                if (budMenuLst.Count() > 0)
                {
                    foreach (DeptBudgetItem dm in budMenuLst)
                    {
                        if (dm.Code == "CA")//for capex
                            Utility.UpdateBudgetCapex((int)Utility.BudgetItemStatus.Pending_ReAlignment, deptID, usr.FullName, 2);
                        if (dm.Code == "DI")
                            Utility.UpdateBudgetDirect((int)Utility.BudgetItemStatus.Pending_ReAlignment, deptID, usr.FullName, 2);
                        if (dm.Code == "ID")
                            Utility.UpdateBudgetIndirect((int)Utility.BudgetItemStatus.Pending_ReAlignment, deptID, usr.FullName, 2);
                        if (dm.Code == "MO")
                            Utility.UpdateBudgetMovable((int)Utility.BudgetItemStatus.Pending_ReAlignment, deptID, usr.FullName, 2);
                        if (dm.Code == "RE")
                            Utility.UpdateBudgetRevenue((int)Utility.BudgetItemStatus.Pending_ReAlignment, deptID, usr.FullName, 2);
                        if (dm.Code == "SA")
                            Utility.UpdateBudgetExistingSf((int)Utility.BudgetItemStatus.Pending_ReAlignment, deptID, usr.FullName, 2);
                    }
                    BudgetYear budYr = (BudgetYear)Session["budgetYr"];
                    List<TrackApproval> getTrackApproval = null;
                    getTrackApproval = LookUpBLL.GetTrackApproval(deptID, budYr.ID).ToList();
                    if (getTrackApproval != null && getTrackApproval.Count() > 0)
                    {
                        TrackApproval appTrack = getTrackApproval.FirstOrDefault();
                        appTrack.status = (int)Utility.BudgetItemStatus.Pending_ReAlignment;
                        appTrack.ActionBy = usr.FullName;
                        LookUpBLL.UpdateApprovalStatus(appTrack);
                        dvExco.Visible = false;
                        success.Visible = true;
                        success.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button>Budget has been successfully approved.";
                        return;
                    }
                    else
                    {
                        LookUpBLL.AddApprovaltrack(new TrackApproval() { BudgetYr = budYr.ID, DepartmentID = deptID, ActionBy = usr.FullName, dateApproved = DateTime.Now, status = (int)Utility.BudgetItemStatus.Pending_ReAlignment });
                        dvExco.Visible = false;
                        success.Visible = true;
                        success.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button>Budget has been successfully approved.";
                        return;
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

        protected void gvDept_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                
                if (e.CommandName == "edt")
                {
                    string budgetType=e.CommandArgument.ToString();
                    int deptId = int.Parse(ddlDept.SelectedValue);
                    if(budgetType=="Movable Asset")
                    {
                        Response.Redirect(string.Format("MovableAsset.aspx?deptId={0}", deptId),false);
                    }
                    if (budgetType == "Salary & Benefit")
                    {
                        Response.Redirect(string.Format("ExistingStaffpg.aspx?deptId={0}", deptId),false);
                    }
                    if (budgetType == "Indirect Budget")
                    {
                        Response.Redirect(string.Format("IndirectBudgetPg.aspx?deptId={0}", deptId), false);
                    }
                    if (budgetType == "Direct Budget")
                    {
                        Response.Redirect(string.Format("DirectBudgetPg.aspx?deptId={0}", deptId));
                    }
                    if (budgetType == "Capex")
                    {
                        Response.Redirect(string.Format("CapexBudgetPg.aspx?deptId={0}", deptId));
                    }
                    if (budgetType == "Revenue Projection")
                    {
                        Response.Redirect(string.Format("RevenueProjectionPage.aspx?deptId={0}", deptId));
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
                 int deptID = int.Parse(ddlDept.SelectedValue);
                List<DeptBudgetItem> budMenuLst = LookUpBLL.GetDeptMenuItem(deptID).ToList();
                if (budMenuLst.Count() > 0)
                {
                    foreach (DeptBudgetItem dm in budMenuLst)
                    {
                        if (dm.Code == "CA")//for capex
                            Utility.UpdateBudgetCapex((int)Utility.BudgetItemStatus.Returned_For_Correction, deptID, usr.FullName, 4);
                        if (dm.Code == "DI")
                            Utility.UpdateBudgetDirect((int)Utility.BudgetItemStatus.Returned_For_Correction, deptID, usr.FullName, 4);
                        if (dm.Code == "ID")
                            Utility.UpdateBudgetIndirect((int)Utility.BudgetItemStatus.Returned_For_Correction, deptID, usr.FullName, 4);
                        if (dm.Code == "MO")
                            Utility.UpdateBudgetMovable((int)Utility.BudgetItemStatus.Returned_For_Correction, deptID, usr.FullName, 4);
                        if (dm.Code == "RE")
                            Utility.UpdateBudgetRevenue((int)Utility.BudgetItemStatus.Returned_For_Correction, deptID, usr.FullName, 4);
                        if (dm.Code == "SA")
                            Utility.UpdateBudgetExistingSf((int)Utility.BudgetItemStatus.Returned_For_Correction, deptID, usr.FullName, 4);
                    }
                    BudgetYear budYr = (BudgetYear)Session["budgetYr"];
                    List<TrackApproval> getTrackApproval = null;
                    getTrackApproval = LookUpBLL.GetTrackApproval(deptID, budYr.ID).ToList();
                    if (getTrackApproval != null && getTrackApproval.Count() > 0)
                    {
                        TrackApproval appTrack = getTrackApproval.FirstOrDefault();
                        appTrack.status = (int)Utility.BudgetItemStatus.Returned_For_Correction;
                        appTrack.ActionBy = usr.FullName;
                        LookUpBLL.UpdateApprovalStatus(appTrack);
                        dvExco.Visible = false;
                        //success.Visible = true;
                        //success.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button>Budget has been successfully returned to HOD.";
                        //return;
                    }
                    else
                    {
                        LookUpBLL.AddApprovaltrack(new TrackApproval() { BudgetYr = budYr.ID, DepartmentID = deptID, ActionBy = usr.FullName, dateApproved = DateTime.Now, status = (int)Utility.BudgetItemStatus.Returned_For_Correction });
                        dvExco.Visible = false;
                        //success.Visible = true;
                        //success.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button>Budget has been successfully approved.";
                        //return;
                    }
                    isset = true;
                }
                if (isset)
                {

                    BindGrid();
                    //sending mail
                    string body = "";
                    string from = ConfigurationManager.AppSettings["exUser"].ToString();
                    string siteUrl = ConfigurationManager.AppSettings["siteUrl"].ToString();
                    string appLogo = ConfigurationManager.AppSettings["appLogoUrl"].ToString();
                    string hodEmail = Utility.GetUsersEmailAdd(deptApprRole,deptID);
                    string subject = "Budget Item Correction Notification";
                    string FilePath = Server.MapPath("EmailTemplates/");
                    if (File.Exists(FilePath + "ReturnBudget.htm"))
                    {
                        FileStream f1 = new FileStream(FilePath + "ReturnBudget.htm", FileMode.Open);
                        StreamReader sr = new StreamReader(f1);
                        body = sr.ReadToEnd();
                        body = body.Replace("@add_appLogo", appLogo);
                        body = body.Replace("@siteUrl", siteUrl);
                        body = body.Replace("@BudgetElement", ddlDept.SelectedItem.Text+ " Department");
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
                        success.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button>Action was successful. Notification has been sent to HOD";
                        return;
                    }
                    else
                    {
                        success.Visible = true;
                        success.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button>Action was successful.Notification could NOT be sent at this time";
                        return;
                    }

                }
                else
                {
                    BindGrid();
                    error.Visible = true;
                    error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> An error occured. kindly try again.If error persist contact Administrator!!.";
                }
            }
            catch (Exception ex)
            {
                error.Visible = true;
                error.InnerHtml = " <button type='button' class='close' data-dismiss='alert'>&times;</button> An error occured. Kindly try again. If error persist contact Administrator!!.";
                Utility.WriteError("Error: " + ex.Message);
            }
        }

        protected void btnForward_Click(object sender, EventArgs e)
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
                int deptID = usr.DepartmentID.Value;
                List<DeptBudgetItem> budMenuLst = LookUpBLL.GetDeptMenuItem(deptID).ToList();
                if (budMenuLst.Count() > 0)
                {
                    foreach (DeptBudgetItem dm in budMenuLst)
                    {
                        if (dm.Code == "CA")//for capex
                            Utility.UpdateBudgetCapex((int)Utility.BudgetItemStatus.Pending_ED_Approval, deptID, usr.FullName, 3);
                        if (dm.Code == "DI")
                            Utility.UpdateBudgetDirect((int)Utility.BudgetItemStatus.Pending_ED_Approval, deptID, usr.FullName, 3);
                        if (dm.Code == "ID")
                            Utility.UpdateBudgetIndirect((int)Utility.BudgetItemStatus.Pending_ED_Approval, deptID, usr.FullName, 3);
                        if (dm.Code == "MO")
                            Utility.UpdateBudgetMovable((int)Utility.BudgetItemStatus.Pending_ED_Approval, deptID, usr.FullName, 3);
                        if (dm.Code == "RE")
                            Utility.UpdateBudgetRevenue((int)Utility.BudgetItemStatus.Pending_ED_Approval, deptID, usr.FullName, 3);
                        if (dm.Code == "SA")
                            Utility.UpdateBudgetExistingSf((int)Utility.BudgetItemStatus.Pending_ED_Approval, deptID, usr.FullName, 3);
                    }
                    BudgetYear budYr = (BudgetYear)Session["budgetYr"];
                    List<TrackApproval> getTrackApproval = null;
                    getTrackApproval = LookUpBLL.GetTrackApproval(deptID, budYr.ID).ToList();
                    if (getTrackApproval != null && getTrackApproval.Count() > 0)
                    {
                        TrackApproval appTrack = getTrackApproval.FirstOrDefault();
                        appTrack.status = (int)Utility.BudgetItemStatus.Pending_ED_Approval;
                        appTrack.ActionBy = usr.FullName;
                        LookUpBLL.UpdateApprovalStatus(appTrack);
                        dvDept.Visible = false;
                        success.Visible = true;
                        success.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button>Department Budget has been successfully forwarded to the ED.";
                        return;
                    }
                    else
                    {
                        LookUpBLL.AddApprovaltrack(new TrackApproval() { BudgetYr = budYr.ID, DepartmentID = deptID, ActionBy = usr.FullName, dateApproved = DateTime.Now, status = (int)Utility.BudgetItemStatus.Pending_ED_Approval });
                        dvDept.Visible = false;
                        success.Visible = true;
                        success.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button>Department Budget has been successfully forwarded to the ED.";
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

        protected void btnPBForward_Click(object sender, EventArgs e)
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
                int deptID = int.Parse(ddlDept.SelectedValue); 
                List<DeptBudgetItem> budMenuLst = LookUpBLL.GetDeptMenuItem(deptID).ToList();
                if (budMenuLst.Count() > 0)
                {
                    foreach (DeptBudgetItem dm in budMenuLst)
                    {
                        if (dm.Code == "CA")//for capex
                            Utility.UpdateBudgetCapex((int)Utility.BudgetItemStatus.Pending_MD_Final_Approval, deptID, usr.FullName, 3);
                        if (dm.Code == "DI")
                            Utility.UpdateBudgetDirect((int)Utility.BudgetItemStatus.Pending_MD_Final_Approval, deptID, usr.FullName, 3);
                        if (dm.Code == "ID")
                            Utility.UpdateBudgetIndirect((int)Utility.BudgetItemStatus.Pending_MD_Final_Approval, deptID, usr.FullName, 3);
                        if (dm.Code == "MO")
                            Utility.UpdateBudgetMovable((int)Utility.BudgetItemStatus.Pending_MD_Final_Approval, deptID, usr.FullName, 3);
                        if (dm.Code == "RE")
                            Utility.UpdateBudgetRevenue((int)Utility.BudgetItemStatus.Pending_MD_Final_Approval, deptID, usr.FullName, 3);
                        if (dm.Code == "SA")
                            Utility.UpdateBudgetExistingSf((int)Utility.BudgetItemStatus.Pending_MD_Final_Approval, deptID, usr.FullName, 3);
                    }
                    BudgetYear budYr = (BudgetYear)Session["budgetYr"];
                    List<TrackApproval> getTrackApproval = null;
                    getTrackApproval = LookUpBLL.GetTrackApproval(deptID, budYr.ID).ToList();
                    if (getTrackApproval != null && getTrackApproval.Count() > 0)
                    {
                        TrackApproval appTrack = getTrackApproval.FirstOrDefault();
                        appTrack.status = (int)Utility.BudgetItemStatus.Pending_MD_Final_Approval;
                        appTrack.ActionBy = usr.FullName;
                        LookUpBLL.UpdateApprovalStatus(appTrack);
                        dvDept.Visible = false;
                        success.Visible = true;
                        success.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button>Department Budget has been successfully forwarded to the MD.";
                        return;
                    }
                    else
                    {
                        LookUpBLL.AddApprovaltrack(new TrackApproval() { BudgetYr = budYr.ID, DepartmentID = deptID, ActionBy = usr.FullName, dateApproved = DateTime.Now, status = (int)Utility.BudgetItemStatus.Pending_MD_Final_Approval });
                        dvDept.Visible = false;
                        success.Visible = true;
                        success.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button>Department Budget has been successfully forwarded to the MD.";
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

        protected void btnMDFinal_Click(object sender, EventArgs e)
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
                int deptID = int.Parse(ddlDept.SelectedValue);
                List<DeptBudgetItem> budMenuLst = LookUpBLL.GetDeptMenuItem(deptID).ToList();
                if (budMenuLst.Count() > 0)
                {
                    foreach (DeptBudgetItem dm in budMenuLst)
                    {
                        if (dm.Code == "CA")//for capex
                            Utility.UpdateBudgetCapex((int)Utility.BudgetItemStatus.MD_Approved, deptID, usr.FullName, 2);
                        if (dm.Code == "DI")
                            Utility.UpdateBudgetDirect((int)Utility.BudgetItemStatus.MD_Approved, deptID, usr.FullName, 2);
                        if (dm.Code == "ID")
                            Utility.UpdateBudgetIndirect((int)Utility.BudgetItemStatus.MD_Approved, deptID, usr.FullName, 2);
                        if (dm.Code == "MO")
                            Utility.UpdateBudgetMovable((int)Utility.BudgetItemStatus.MD_Approved, deptID, usr.FullName, 2);
                        if (dm.Code == "RE")
                            Utility.UpdateBudgetRevenue((int)Utility.BudgetItemStatus.MD_Approved, deptID, usr.FullName, 2);
                        if (dm.Code == "SA")
                            Utility.UpdateBudgetExistingSf((int)Utility.BudgetItemStatus.MD_Approved, deptID, usr.FullName, 2);
                    }
                    BudgetYear budYr = (BudgetYear)Session["budgetYr"];
                    List<TrackApproval> getTrackApproval = null;
                    getTrackApproval = LookUpBLL.GetTrackApproval(deptID, budYr.ID).ToList();
                    if (getTrackApproval != null && getTrackApproval.Count() > 0)
                    {
                        TrackApproval appTrack = getTrackApproval.FirstOrDefault();
                        appTrack.status = (int)Utility.BudgetItemStatus.MD_Approved;
                        appTrack.ActionBy = usr.FullName;
                        LookUpBLL.UpdateApprovalStatus(appTrack);
                        dvExco.Visible = false;
                        success.Visible = true;
                        success.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button>Budget has been successfully approved.";
                        return;
                    }
                    else
                    {
                        LookUpBLL.AddApprovaltrack(new TrackApproval() { BudgetYr = budYr.ID, DepartmentID = deptID, ActionBy = usr.FullName, dateApproved = DateTime.Now, status = (int)Utility.BudgetItemStatus.MD_Approved });
                        dvExco.Visible = false;
                        success.Visible = true;
                        success.InnerHtml = "<button type='button' class='close' data-dismiss='alert'>&times;</button>Budget has been successfully approved.";
                        return;
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
    }
}
