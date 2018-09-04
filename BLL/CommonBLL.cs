using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BudgetCapture.DAL;

namespace BudgetCapture.BLL
{
    public class CommonBLL
    {
        public static bool AddExistingStaff(ExistingStaff estf)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    db.ExistingStaffs.Add(estf);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Utility.WriteError("Error Msg: " + ex.Message);
                throw ex;
            }
        }
        public static bool UpdateExistingStaff(ExistingStaff staff)
        {
            try
            {
                bool rst = false;
                using (var db = new BudgetCaptureDBEntities())
                {
                    db.ExistingStaffs.Attach(staff);
                    db.Entry(staff).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    rst = true;
                }
                return rst;
            }
            catch (Exception ex)
            {
                //Utility.WriteError("Error Msg: " + ex.InnerException);
                throw ex;
            }
        }
        public static bool DeleteExistingRecord(ExistingStaff staff)
        {
            try
            {
                bool rst = false;
                using (var db = new BudgetCaptureDBEntities())
                {
                    db.Entry(staff).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                    rst = true;
                }
                return rst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static IEnumerable<ExistingStaff> GetExistStaffList(int budgetYrID,int deptId=0, string batchId="")
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    var result = db.ExistingStaffs.Include("BudgetYear").Include("Grade").Include("Department").Where(d => d.BudgetYrID == budgetYrID).OrderByDescending(o => o.Grade.CostPerMonth).AsQueryable();
                    if (deptId != 0)
                    {
                        result = result.Where(r => r.DepartmentID == deptId).AsQueryable();
                    }
                    if (!string.IsNullOrEmpty(batchId))
                    {
                        result = result.Where(r => r.ATCBatchID == batchId).AsQueryable();
                    }
                    return result.ToList();
                    //if (deptId == 0)
                    //{
                    //    return db.ExistingStaffs.Include("BudgetYear").Include("Grade").Include("Department").Where(d=>d.BudgetYrID==budgetYrID).OrderByDescending(o => o.Grade.CostPerMonth).ToList();
                    //}
                    //else
                    //{
                    //    return db.ExistingStaffs.Include("BudgetYear").Include("Grade").Include("Department").Where(j => j.DepartmentID == deptId && j.BudgetYrID==budgetYrID).OrderByDescending(o => o.Grade.CostPerMonth).ToList();
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static ExistingStaff GetExistingStaff(int Id)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    return db.ExistingStaffs.Include("Grade").Where(p => p.ID == Id).FirstOrDefault(); ;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        

        public static bool AddExpenseProjection(ExpenseProjection expPro)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    db.ExpenseProjections.Add(expPro);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Utility.WriteError("Error Msg: " + ex.Message);
                throw ex;
            }
        }
        public static bool UpdateExpenseProjection(ExpenseProjection expPro)
        {
            try
            {
                bool rst = false;
                using (var db = new BudgetCaptureDBEntities())
                {
                    db.ExpenseProjections.Attach(expPro);
                    db.Entry(expPro).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    rst = true;
                }
                return rst;
            }
            catch (Exception ex)
            {
                // Utility.WriteError("Error Msg: " + ex.Message);
                throw ex;
            }
        }
        public static bool DeleteExpensePro(ExpenseProjection expPro)
        {
            try
            {
                bool rst = false;
                using (var db = new BudgetCaptureDBEntities())
                {
                    db.Entry(expPro).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                    rst = true;
                }
                return rst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static ExpenseProjection GetExpensePRojection(int Id)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    return db.ExpenseProjections.Find(Id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static IEnumerable<ExpenseProjection> GetExpenseProjectionList(int budgetYrID, int deptId = 0, int expType=0)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    if (deptId == 0)
                    {
                        return db.ExpenseProjections.Include("MonthOfYear").Include("BudgetYear").Include("ExpenseType").Include("Department").Where(d => d.BudgetYrID == budgetYrID).OrderBy(o => o.Department.Name).ToList();
                    }
                    else
                    {
                        return db.ExpenseProjections.Include("MonthOfYear").Include("BudgetYear").Include("ExpenseType").Include("Department").Where(j => j.DepartmentID == deptId && j.BudgetYrID == budgetYrID).OrderBy(o => o.Department.Name).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool AddCapex(AssetBudget astBud)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    db.AssetBudgets.Add(astBud);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Utility.WriteError("Error Msg: " + ex.Message);
                throw ex;
            }
        }
        public static bool UpdateCapex(AssetBudget astBud)
        {
            try
            {
                bool rst = false;
                using (var db = new BudgetCaptureDBEntities())
                {
                    db.AssetBudgets.Attach(astBud);
                    db.Entry(astBud).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    rst = true;
                }
                return rst;
            }
            catch (Exception ex)
            {
                // Utility.WriteError("Error Msg: " + ex.Message);
                throw ex;
            }
        }
        public static bool DeleteCapex(AssetBudget astBud)
        {
            try
            {
                bool rst = false;
                using (var db = new BudgetCaptureDBEntities())
                {
                    db.Entry(astBud).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                    rst = true;
                }
                return rst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static AssetBudget GetAssetBudget(int Id)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    return db.AssetBudgets.Include("Asset").Where(k=>k.ID==Id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static IEnumerable<AssetBudget> GetAssetBudgetList(int budgetYrID, int deptId = 0, string batchId = "")
        {
            try
            {

                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    var result = db.AssetBudgets.Include("MonthOfYear").Include("BudgetYear").Include("Asset.AssetType").Include("Department").Where(d => d.BudgetYrID == budgetYrID).OrderBy(o => o.Department.Name).AsQueryable();
                    if (deptId != 0)
                    {
                        result = result.Where(r => r.DepartmentID == deptId).AsQueryable();
                    }
                    if (!string.IsNullOrEmpty(batchId))
                    {
                        result = result.Where(r => r.ATCBatchID == batchId).AsQueryable();
                    }
                    return result.ToList();
                    //if (deptId == 0)
                    //{
                    //    return db.AssetBudgets.Include("MonthOfYear").Include("BudgetYear").Include("Asset.AssetType").Include("Department").Where(d => d.BudgetYrID == budgetYrID).OrderBy(o => o.Department.Name).ToList();
                    //}
                    //else
                    //{
                    //    return db.AssetBudgets.Include("MonthOfYear").Include("BudgetYear").Include("Asset.AssetType").Include("Department").Where(j => j.DepartmentID == deptId && j.BudgetYrID == budgetYrID).OrderBy(o => o.Department.Name).ToList();
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool AddRevenue(RevenueProjection recpro)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    db.RevenueProjections.Add(recpro);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Utility.WriteError("Error Msg: " + ex.Message);
                throw ex;
            }
        }
        public static bool UpdateRevenue(RevenueProjection recpro)
        {
            try
            {
                bool rst = false;
                using (var db = new BudgetCaptureDBEntities())
                {
                    db.RevenueProjections.Attach(recpro);
                    db.Entry(recpro).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    rst = true;
                }
                return rst;
            }
            catch (Exception ex)
            {
                // Utility.WriteError("Error Msg: " + ex.Message);
                throw ex;
            }
        }
        public static bool DeleteRevenue(RevenueProjection recpro)
        {
            try
            {
                bool rst = false;
                using (var db = new BudgetCaptureDBEntities())
                {
                    db.Entry(recpro).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                    rst = true;
                }
                return rst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static RevenueProjection GetRevenue(int Id)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    return db.RevenueProjections.Include("CustomerType").Where(k => k.ID == Id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static IEnumerable<RevenueProjection> GetRevenueList(int budgetYrID, int deptId = 0)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    if (deptId == 0)
                    {
                        return db.RevenueProjections.Include("MonthOfYear").Include("BudgetYear").Include("CustomerType").Include("Department").Where(d => d.BudgetYrID == budgetYrID).OrderBy(o => o.Department.Name).ToList();
                    }
                    else
                    {
                        return db.RevenueProjections.Include("MonthOfYear").Include("BudgetYear").Include("CustomerType").Include("Department").Where(j => j.DepartmentID == deptId && j.BudgetYrID == budgetYrID).OrderBy(o => o.Department.Name).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static IEnumerable<BudgetSummaryView> GetBudgetSummaryList(int budgetYrID, int deptId = 0,int dirID=0)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    
                        var result= db.BudgetSummaryViews.Include("BudgetYear").Include("Department").Where(d => d.BudgetYrID == budgetYrID).OrderBy(o => o.Department.Name).AsQueryable();
                        if (deptId != 0)
                        {
                            result = result.Where(j => j.DepartmentID == deptId).OrderBy(o => o.BudgetElement).AsQueryable();
                        }
                        if (dirID != 0)
                        {
                            result = result.Where(j => j.Department.DirectorateID == dirID).OrderBy(o => o.BudgetElement).AsQueryable();
                        }
                        return result.ToList();
                    //else
                    //{
                    //    return db.BudgetSummaryViews.Include("BudgetYear").Include("Department").Where(j => j.DepartmentID == deptId && j.BudgetYrID == budgetYrID).OrderBy(o => o.BudgetElement).ToList();
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static bool AddIndirectExpense(IndirectBudget astBud)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    db.IndirectBudgets.Add(astBud);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Utility.WriteError("Error Msg: " + ex.Message);
                throw ex;
            }
        }
        public static bool UpdateIndirectExpense(IndirectBudget astBud)
        {
            try
            {
                bool rst = false;
                using (var db = new BudgetCaptureDBEntities())
                {
                    db.IndirectBudgets.Attach(astBud);
                    db.Entry(astBud).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    rst = true;
                }
                return rst;
            }
            catch (Exception ex)
            {
                // Utility.WriteError("Error Msg: " + ex.Message);
                throw ex;
            }
        }
        public static bool DeleteIndirectExpense(IndirectBudget astBud)
        {
            try
            {
                bool rst = false;
                using (var db = new BudgetCaptureDBEntities())
                {
                    db.Entry(astBud).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                    rst = true;
                }
                return rst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static IndirectBudget GetIndirectBudget(int Id)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    return db.IndirectBudgets.Include("IndirectBudgetItem").Where(k => k.ID == Id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static IEnumerable<IndirectBudget> GetIndirectBudgetList(int budgetYrID, int deptId = 0, string batchId="")
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    var result = db.IndirectBudgets.Include("BudgetYear").Include("IndirectBudgetItem.IndirectType").Include("Department").Where(d => d.BudgetYrID == budgetYrID).OrderBy(o => o.Department.Name).AsQueryable();
                    if (deptId != 0)
                    {
                        result = result.Where(r => r.DepartmentID == deptId).AsQueryable();
                    }
                    if (!string.IsNullOrEmpty(batchId))
                    {
                        result = result.Where(r => r.ATCBatchID == batchId).AsQueryable();
                    }
                    return result.ToList();
                    //if (deptId == 0)
                    //{
                    //    return db.IndirectBudgets.Include("BudgetYear").Include("IndirectBudgetItem.IndirectType").Include("Department").Where(d => d.BudgetYrID == budgetYrID).OrderBy(o => o.Department.Name).ToList();
                    //}
                    //else
                    //{
                    //    return db.IndirectBudgets.Include("BudgetYear").Include("IndirectBudgetItem.IndirectType").Include("Department").Where(j => j.DepartmentID == deptId && j.BudgetYrID == budgetYrID).OrderBy(o => o.Department.Name).ToList();
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool AddDirectExpense(DirectBudget astBud)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    db.DirectBudgets.Add(astBud);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Utility.WriteError("Error Msg: " + ex.Message);
                throw ex;
            }
        }
        public static bool UpdateDirectExpense(DirectBudget astBud)
        {
            try
            {
                bool rst = false;
                using (var db = new BudgetCaptureDBEntities())
                {
                    db.DirectBudgets.Attach(astBud);
                    db.Entry(astBud).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    rst = true;
                }
                return rst;
            }
            catch (Exception ex)
            {
                // Utility.WriteError("Error Msg: " + ex.Message);
                throw ex;
            }
        }
        public static bool DeleteDirectExpense(DirectBudget astBud)
        {
            try
            {
                bool rst = false;
                using (var db = new BudgetCaptureDBEntities())
                {
                    db.Entry(astBud).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                    rst = true;
                }
                return rst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DirectBudget GetDirectBudget(int Id)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    return db.DirectBudgets.Include("DirectExpenseItem").Where(k => k.ID == Id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static IEnumerable<DirectBudget> GetDirectBudgetList(int budgetYrID, int deptId = 0, string batchId = "")
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    var result=db.DirectBudgets.Include("BudgetYear").Include("DirectExpenseItem.DirectType").Include("Department").Where(d => d.BudgetYrID == budgetYrID).OrderBy(o => o.Department.Name).AsQueryable();
                    if (deptId != 0)
                    {
                        result = result.Where(r => r.DepartmentID == deptId).AsQueryable();
                    }
                    if (!string.IsNullOrEmpty(batchId))
                    {
                        result = result.Where(r => r.ATCBatchID == batchId).AsQueryable();
                    }
                    return result.ToList();
                    //if (deptId == 0)
                    //{
                    //    return db.DirectBudgets.Include("BudgetYear").Include("DirectExpenseItem.DirectType").Include("Department").Where(d => d.BudgetYrID == budgetYrID).OrderBy(o => o.Department.Name).AsQueryable(); ;
                    //}
                    //else
                    //{
                    //    return db.DirectBudgets.Include("BudgetYear").Include("DirectExpenseItem.DirectType").Include("Department").Where(j => j.DepartmentID == deptId && j.BudgetYrID == budgetYrID).OrderBy(o => o.Department.Name).ToList();
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool AddCapexExpense(CapexBudget astBud)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    db.CapexBudgets.Add(astBud);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Utility.WriteError("Error Msg: " + ex.Message);
                throw ex;
            }
        }
        public static bool UpdateCapexExpense(CapexBudget astBud)
        {
            try
            {
                bool rst = false;
                using (var db = new BudgetCaptureDBEntities())
                {
                    db.CapexBudgets.Attach(astBud);
                    db.Entry(astBud).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    rst = true;
                }
                return rst;
            }
            catch (Exception ex)
            {
                // Utility.WriteError("Error Msg: " + ex.Message);
                throw ex;
            }
        }
        public static bool DeleteCapexExpense(CapexBudget astBud)
        {
            try
            {
                bool rst = false;
                using (var db = new BudgetCaptureDBEntities())
                {
                    db.Entry(astBud).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                    rst = true;
                }
                return rst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static CapexBudget GetCapexBudget(int Id)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    return db.CapexBudgets.Include("CapexExpenseItem").Where(k => k.ID == Id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static IEnumerable<CapexBudget> GetCapexBudgetList(int budgetYrID, int deptId = 0,string batchId="")
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    var result=db.CapexBudgets.Include("BudgetYear").Include("CapexExpenseItem.CapexType").Include("Department").Where(d => d.BudgetYrID == budgetYrID).OrderBy(o => o.Department.Name).AsQueryable();
                    if(deptId!=0)
                    {
                        result=result.Where(r=>r.DepartmentID==deptId).AsQueryable();
                    }
                    if (!string.IsNullOrEmpty(batchId))
                    {
                        result = result.Where(r => r.ATCBatchID == batchId).AsQueryable();
                    }
                    return result.ToList();
                    //if (deptId == 0)
                    //{
                    //    return db.CapexBudgets.Include("BudgetYear").Include("CapexExpenseItem.CapexType").Include("Department").Where(d => d.BudgetYrID == budgetYrID).OrderBy(o => o.Department.Name).ToList();
                    //}
                    //else
                    //{
                    //    return db.CapexBudgets.Include("BudgetYear").Include("CapexExpenseItem.CapexType").Include("Department").Where(j => j.DepartmentID == deptId && j.BudgetYrID == budgetYrID).OrderBy(o => o.Department.Name).ToList();
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public static bool AddATCHeader(ATCRequestHeader astBud)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    db.ATCRequestHeaders.Add(astBud);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Utility.WriteError("Error Msg: " + ex.Message);
                throw ex;
            }
        }
        public static bool UpdateATCHeader(ATCRequestHeader astBud)
        {
            try
            {
                bool rst = false;
                using (var db = new BudgetCaptureDBEntities())
                {
                    db.ATCRequestHeaders.Attach(astBud);
                    db.Entry(astBud).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    rst = true;
                }
                return rst;
            }
            catch (Exception ex)
            {
                // Utility.WriteError("Error Msg: " + ex.Message);
                throw ex;
            }
        }
         
         
        public static IEnumerable<ATCRequestHeader> GetATCRequestList(int budgetYrID, int deptId = 0)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    if (deptId == 0)
                    {
                        return db.ATCRequestHeaders.Include("BudgetMenuItem").Include("BudgetYear").Include("Department").Where(d => d.BudgetYrID == budgetYrID).OrderByDescending(o => o.RequestDate).ToList();
                    }
                    else
                    {
                        return db.ATCRequestHeaders.Include("BudgetMenuItem").Include("BudgetYear").Include("Department").Where(j => j.DepartmentID == deptId && j.BudgetYrID == budgetYrID).OrderByDescending(o => o.RequestDate).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static ATCRequestHeader GetATCHeader(int Id)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    return db.ATCRequestHeaders.Include("Department").Include("BudgetMenuItem").Where(k => k.ID == Id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string GenerateBatchID(int budgetYrID, int deptId, int budgetType)
        {
            using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
            {
                var q = from p in db.ATCRequestHeaders.OrderByDescending(p=>p.RequestDate) where p.BudgetYrID == budgetYrID && p.DepartmentID == deptId && p.BudgetTypeId == budgetType select p;
                if (q != null && q.Count() > 0)
                {
                    ATCRequestHeader rq=q.FirstOrDefault();
                    string no = rq.BatchID.Split('-' )[1];
                    int val = int.Parse(no);
                    return (val+1).ToString();
                }
                else
                {
                    return "1";
                }
            }
        }



        public static bool AddIndirectExpenseATC(IndirectBudgetATC astBud)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    db.IndirectBudgetATCs.Add(astBud);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Utility.WriteError("Error Msg: " + ex.Message);
                throw ex;
            }
        }
        public static bool UpdateIndirectExpenseATC(IndirectBudgetATC astBud)
        {
            try
            {
                bool rst = false;
                using (var db = new BudgetCaptureDBEntities())
                {
                    db.IndirectBudgetATCs.Attach(astBud);
                    db.Entry(astBud).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    rst = true;
                }
                return rst;
            }
            catch (Exception ex)
            {
                // Utility.WriteError("Error Msg: " + ex.Message);
                throw ex;
            }
        }
       
        public static IndirectBudgetATC GetIndirectBudgetATC(int Id)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    return db.IndirectBudgetATCs.Where(k => k.ID == Id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static IEnumerable<IndirectBudgetATC> GetIndirectBudgetATCList(string batchId = "")
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    var result = db.IndirectBudgetATCs.Include("Department").OrderBy(o => o.BudgetItem).AsQueryable();
                     
                    if (!string.IsNullOrEmpty(batchId))
                    {
                        result = result.Where(r => r.ATCBatchId == batchId).AsQueryable();
                    }
                    return result.ToList();
                    //if (deptId == 0)
                    //{
                    //    return db.IndirectBudgets.Include("BudgetYear").Include("IndirectBudgetItem.IndirectType").Include("Department").Where(d => d.BudgetYrID == budgetYrID).OrderBy(o => o.Department.Name).ToList();
                    //}
                    //else
                    //{
                    //    return db.IndirectBudgets.Include("BudgetYear").Include("IndirectBudgetItem.IndirectType").Include("Department").Where(j => j.DepartmentID == deptId && j.BudgetYrID == budgetYrID).OrderBy(o => o.Department.Name).ToList();
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool AddDirectExpenseATC(DirectBudgetATC astBud)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    db.DirectBudgetATCs.Add(astBud);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Utility.WriteError("Error Msg: " + ex.Message);
                throw ex;
            }
        }
        public static bool UpdateDirectExpenseATC(DirectBudgetATC astBud)
        {
            try
            {
                bool rst = false;
                using (var db = new BudgetCaptureDBEntities())
                {
                    db.DirectBudgetATCs.Attach(astBud);
                    db.Entry(astBud).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    rst = true;
                }
                return rst;
            }
            catch (Exception ex)
            {
                // Utility.WriteError("Error Msg: " + ex.Message);
                throw ex;
            }
        }

        public static DirectBudgetATC GetDirectBudgetATC(int Id)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    return db.DirectBudgetATCs.Where(k => k.ID == Id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static IEnumerable<DirectBudgetATC> GetDirectBudgetATCList(string batchId = "")
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    var result = db.DirectBudgetATCs.Include("Department").OrderBy(o => o.BudgetItem).AsQueryable();

                    if (!string.IsNullOrEmpty(batchId))
                    {
                        result = result.Where(r => r.ATCBatchId == batchId).AsQueryable();
                    }
                    return result.ToList();
                    //if (deptId == 0)
                    //{
                    //    return db.IndirectBudgets.Include("BudgetYear").Include("IndirectBudgetItem.IndirectType").Include("Department").Where(d => d.BudgetYrID == budgetYrID).OrderBy(o => o.Department.Name).ToList();
                    //}
                    //else
                    //{
                    //    return db.IndirectBudgets.Include("BudgetYear").Include("IndirectBudgetItem.IndirectType").Include("Department").Where(j => j.DepartmentID == deptId && j.BudgetYrID == budgetYrID).OrderBy(o => o.Department.Name).ToList();
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool AddAssetBudgetATC(AssetBudgetATC astBud)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    db.AssetBudgetATCs.Add(astBud);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Utility.WriteError("Error Msg: " + ex.Message);
                throw ex;
            }
        }
        public static bool UpdateAssetBudgetATC(AssetBudgetATC astBud)
        {
            try
            {
                bool rst = false;
                using (var db = new BudgetCaptureDBEntities())
                {
                    db.AssetBudgetATCs.Attach(astBud);
                    db.Entry(astBud).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    rst = true;
                }
                return rst;
            }
            catch (Exception ex)
            {
                // Utility.WriteError("Error Msg: " + ex.Message);
                throw ex;
            }
        }

        public static AssetBudgetATC GetAssetBudgetATC(int Id)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    return db.AssetBudgetATCs.Where(k => k.ID == Id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static IEnumerable<AssetBudgetATC> GetAssetBudgetATCList(string batchId = "")
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    var result = db.AssetBudgetATCs.Include("Department").OrderBy(o => o.BudgetItem).AsQueryable();

                    if (!string.IsNullOrEmpty(batchId))
                    {
                        result = result.Where(r => r.ATCBatchId == batchId).AsQueryable();
                    }
                    return result.ToList();
                    //if (deptId == 0)
                    //{
                    //    return db.IndirectBudgets.Include("BudgetYear").Include("IndirectBudgetItem.IndirectType").Include("Department").Where(d => d.BudgetYrID == budgetYrID).OrderBy(o => o.Department.Name).ToList();
                    //}
                    //else
                    //{
                    //    return db.IndirectBudgets.Include("BudgetYear").Include("IndirectBudgetItem.IndirectType").Include("Department").Where(j => j.DepartmentID == deptId && j.BudgetYrID == budgetYrID).OrderBy(o => o.Department.Name).ToList();
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool AddCapexBudgetATC(CapexBudgetATC astBud)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    db.CapexBudgetATCs.Add(astBud);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Utility.WriteError("Error Msg: " + ex.Message);
                throw ex;
            }
        }
        public static bool UpdateCapexBudgetATC(CapexBudgetATC astBud)
        {
            try
            {
                bool rst = false;
                using (var db = new BudgetCaptureDBEntities())
                {
                    db.CapexBudgetATCs.Attach(astBud);
                    db.Entry(astBud).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    rst = true;
                }
                return rst;
            }
            catch (Exception ex)
            {
                // Utility.WriteError("Error Msg: " + ex.Message);
                throw ex;
            }
        }

        public static CapexBudgetATC GetCapexBudgetATC(int Id)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    return db.CapexBudgetATCs.Where(k => k.ID == Id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static IEnumerable<CapexBudgetATC> GetCapexBudgetATCList(string batchId = "")
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    var result = db.CapexBudgetATCs.Include("Department").OrderBy(o => o.BudgetItem).AsQueryable();

                    if (!string.IsNullOrEmpty(batchId))
                    {
                        result = result.Where(r => r.ATCBatchId == batchId).AsQueryable();
                    }
                    return result.ToList();
                    //if (deptId == 0)
                    //{
                    //    return db.IndirectBudgets.Include("BudgetYear").Include("IndirectBudgetItem.IndirectType").Include("Department").Where(d => d.BudgetYrID == budgetYrID).OrderBy(o => o.Department.Name).ToList();
                    //}
                    //else
                    //{
                    //    return db.IndirectBudgets.Include("BudgetYear").Include("IndirectBudgetItem.IndirectType").Include("Department").Where(j => j.DepartmentID == deptId && j.BudgetYrID == budgetYrID).OrderBy(o => o.Department.Name).ToList();
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool AddExistingBudgetATC(ExistingStaffBudgetATC astBud)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    db.ExistingStaffBudgetATCs.Add(astBud);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Utility.WriteError("Error Msg: " + ex.Message);
                throw ex;
            }
        }
        public static bool UpdateExistingStaffBudgetATC(ExistingStaffBudgetATC astBud)
        {
            try
            {
                bool rst = false;
                using (var db = new BudgetCaptureDBEntities())
                {
                    db.ExistingStaffBudgetATCs.Attach(astBud);
                    db.Entry(astBud).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    rst = true;
                }
                return rst;
            }
            catch (Exception ex)
            {
                // Utility.WriteError("Error Msg: " + ex.Message);
                throw ex;
            }
        }

        public static ExistingStaffBudgetATC GetExistingStaffBudgetATC(int Id)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    return db.ExistingStaffBudgetATCs.Where(k => k.ID == Id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static IEnumerable<ExistingStaffBudgetATC> GetExistingStaffBudgetATCList(string batchId = "")
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    var result = db.ExistingStaffBudgetATCs.Include("Department").OrderBy(o => o.BudgetItem).AsQueryable();

                    if (!string.IsNullOrEmpty(batchId))
                    {
                        result = result.Where(r => r.ATCBatchId == batchId).AsQueryable();
                    }
                    return result.ToList();
                    //if (deptId == 0)
                    //{
                    //    return db.IndirectBudgets.Include("BudgetYear").Include("IndirectBudgetItem.IndirectType").Include("Department").Where(d => d.BudgetYrID == budgetYrID).OrderBy(o => o.Department.Name).ToList();
                    //}
                    //else
                    //{
                    //    return db.IndirectBudgets.Include("BudgetYear").Include("IndirectBudgetItem.IndirectType").Include("Department").Where(j => j.DepartmentID == deptId && j.BudgetYrID == budgetYrID).OrderBy(o => o.Department.Name).ToList();
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}