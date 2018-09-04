using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BudgetCapture.DAL;


namespace BudgetCapture.BLL
{
    public class CustomerMap : Customer
    {
    }
    public class LookUpBLL
    {
        public static bool AddBudgetYear(BudgetYear byr)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    db.BudgetYears.Add(byr);
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
        public static bool UpdateBudgetYear(BudgetYear byr)
        {
            try
            {
                bool rst = false;
                using (var db = new BudgetCaptureDBEntities())
                {
                    db.BudgetYears.Attach(byr);
                    db.Entry(byr).State = System.Data.Entity.EntityState.Modified;
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
        public static IEnumerable<BudgetYear> GetBudgetYearList()
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    return db.BudgetYears.OrderByDescending(o => o.Year).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static BudgetYear GetBudgetYear(int Id)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    return db.BudgetYears.Find(Id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void DeactivateAllBudgetYear()
        {
            try
            {
                using (var db = new BudgetCaptureDBEntities())
                {
                    var q = db.BudgetYears;
                    if (q != null)
                    {
                        foreach(BudgetYear bg in q)
                        {
                            bg.IsActive = false;
                        }
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool AddGrade(Grade grd)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    db.Grades.Add(grd);
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
        public static bool UpdateGrade(Grade grd)
        {
            try
            {
                bool rst = false;
                using (var db = new BudgetCaptureDBEntities())
                {
                    db.Grades.Attach(grd);
                    db.Entry(grd).State = System.Data.Entity.EntityState.Modified;
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
        public static IEnumerable<Grade> GetGradeList()
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    return db.Grades.OrderByDescending(o =>o.CostPerYear).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static IEnumerable<Grade> GetGradeLookup()
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    return db.Grades.Where(g=>g.isActive==true).OrderByDescending(o => o.CostPerYear).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static Grade GetGrade(int Id)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    return db.Grades.Find(Id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<MonthMap> GetMonthList()
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    var q = from p in db.MonthOfYears
                            select new MonthMap
                            {
                                ID=p.ID,
                                //MonthName=p.MonthName+"("+p.MonthNumber.ToString()+")",
                                MonthName=p.MonthName
                            };
                    if (q != null)
                    {
                        return q.ToList();
                    }else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool AddExpenseType(ExpenseType exp)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    db.ExpenseTypes.Add(exp);
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
        public static bool UpdateExpenseType(ExpenseType exp)
        {
            try
            {
                bool rst = false;
                using (var db = new BudgetCaptureDBEntities())
                {
                    db.ExpenseTypes.Attach(exp);
                    db.Entry(exp).State = System.Data.Entity.EntityState.Modified;
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
        public static IEnumerable<ExpenseType> GetExpenseTypeList(int deptId=0)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    if (deptId == 0)
                    {
                        return db.ExpenseTypes.Where(e=>e.DepartmentID==0).OrderBy(o => o.Name).ToList();
                    }
                    else
                    {
                        return db.ExpenseTypes.OrderBy(o => o.Name).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static ExpenseType GetExpenseType(int Id)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    return db.ExpenseTypes.Find(Id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static bool AddAssetType(AssetType exp)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    db.AssetTypes.Add(exp);
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
        public static bool UpdateAssetType(AssetType exp)
        {
            try
            {
                bool rst = false;
                using (var db = new BudgetCaptureDBEntities())
                {
                    db.AssetTypes.Attach(exp);
                    db.Entry(exp).State = System.Data.Entity.EntityState.Modified;
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
        public static IEnumerable<AssetType> GetAssetTypeList()
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    return db.AssetTypes.OrderBy(o => o.Name).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static AssetType GetAssetType(int Id)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    return db.AssetTypes.Find(Id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool AddAsset(Asset ast)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    db.Assets.Add(ast);
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
        public static bool UpdateAsset(Asset ast)
        {
            try
            {
                bool rst = false;
                using (var db = new BudgetCaptureDBEntities())
                {
                    db.Assets.Attach(ast);
                    db.Entry(ast).State = System.Data.Entity.EntityState.Modified;
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
        

        public static IEnumerable<Asset> GetAssetList()
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    return db.Assets.Include("AssetType").Where(p=>p.DelFlg=="N").OrderByDescending(o => o.AssetName).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static IEnumerable<Asset> GetAssetLookup(int AssetTypeID)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    return db.Assets.Include("AssetType").Where(g => g.AssetTypeID == AssetTypeID && g.DelFlg=="N").OrderBy(o => o.AssetName).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static Asset GetAsset(int Id)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    return db.Assets.Find(Id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool AddCustomerType(CustomerType exp)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    db.CustomerTypes.Add(exp);
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
        public static bool UpdateCustomerType(CustomerType exp)
        {
            try
            {
                bool rst = false;
                using (var db = new BudgetCaptureDBEntities())
                {
                    db.CustomerTypes.Attach(exp);
                    db.Entry(exp).State = System.Data.Entity.EntityState.Modified;
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
        public static IEnumerable<CustomerType> GetCustomerTypeList(bool filter=true)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    if(filter)
                       return db.CustomerTypes.Where(c=>c.isActive==filter).OrderBy(o => o.Name).ToList();
                    else
                      return db.CustomerTypes.OrderBy(o => o.Name).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static CustomerType GetCustomerType(int Id)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    return db.CustomerTypes.Find(Id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        

        public static bool AddCustomer(Customer exp)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    db.Customers.Add(exp);
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
        public static bool UpdateCustomer(Customer exp)
        {
            try
            {
                bool rst = false;
                using (var db = new BudgetCaptureDBEntities())
                {
                    db.Customers.Attach(exp);
                    db.Entry(exp).State = System.Data.Entity.EntityState.Modified;
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
        public static IEnumerable<Customer> GetCustomerList()
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    return db.Customers.Include("CustomerType").OrderBy(o => o.FullName).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static IEnumerable<Customer> GetCustomerLookUpList()
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    var q = from p in db.Customers
                           // where p.ObligorTypeID == typ
                            select new CustomerMap
                            {
                                ID=p.ID,
                                FullName=p.FullName+"["+p.Code+"]"
                            };
                   
                    return q.OrderBy(p=>p.FullName).ToList();
                   // return db.Obligors.Include("ObligorType").OrderBy(o => o.FullName).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static Customer GetCustomer(int Id)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    return db.Customers.Find(Id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        
        public static bool AddSalaryElement(SalaryElement exp)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    db.SalaryElements.Add(exp);
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
        public static bool UpdateSalaryElement(SalaryElement exp)
        {
            try
            {
                bool rst = false;
                using (var db = new BudgetCaptureDBEntities())
                {
                    db.SalaryElements.Attach(exp);
                    db.Entry(exp).State = System.Data.Entity.EntityState.Modified;
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
        public static IEnumerable<SalaryElement> GetSalaryElementList(bool filter=false)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    if (filter)
                    {
                        return db.SalaryElements.Include("SalBenCategory").Where(n=>n.isActive==true).OrderBy(o => o.CategoryId).ToList();
                    }else
                    {
                        return db.SalaryElements.Include("SalBenCategory").OrderBy(o => o.CategoryId).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static SalaryElement GetSalaryElement(int Id)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    return db.SalaryElements.Find(Id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool AddSalBenCategory(SalBenCategory exp)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    db.SalBenCategories.Add(exp);
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
        public static bool UpdateSalBenCategory(SalBenCategory exp)
        {
            try
            {
                bool rst = false;
                using (var db = new BudgetCaptureDBEntities())
                {
                    db.SalBenCategories.Attach(exp);
                    db.Entry(exp).State = System.Data.Entity.EntityState.Modified;
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
        public static IEnumerable<SalBenCategory> GetSalBenCategoryList(bool filter = false)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    if (filter)
                    {
                        return db.SalBenCategories.Where(n => n.isActive == true).OrderBy(o => o.ID).ToList();
                    }
                    else
                    {
                        return db.SalBenCategories.OrderBy(o => o.ID).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static SalBenCategory GetSalBenCategory(int Id)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    return db.SalBenCategories.Find(Id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool AddGradeSalElement(GradeSalaryElement exp)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    db.GradeSalaryElements.Add(exp);
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
        public static bool UpdateGradeSalElement(GradeSalaryElement exp)
        {
            try
            {
                bool rst = false;
                using (var db = new BudgetCaptureDBEntities())
                {
                    db.GradeSalaryElements.Attach(exp);
                    db.Entry(exp).State = System.Data.Entity.EntityState.Modified;
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
        public static IEnumerable<GradeSalElement> GetGradeSalElementList(int gd=0,int elem=0)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    var gg = from r in db.GradeSalaryElements
                                     join g in db.SalaryElements.Include("SalBenCategory") on r.SalElementID equals g.ID
                                     join k in db.Grades on r.GradeId equals k.ID
                                     where r.isActive==true
                                     select new GradeSalElement
                                     {
                                       GradeID=r.GradeId,
                                       GradeName=k.Name,
                                       ElementCode=g.Code,
                                       ElementName=g.Elements,
                                       SalaryElementID=r.SalElementID,
                                       Amount=r.Amount.HasValue?r.Amount.Value:0,
                                       isActive=r.isActive.Value,
                                       Category=g.SalBenCategory.Name
                                         // return db.GradeSalaryElements.ToList();
                                    };
                    if(gd!=0){
                        gg = gg.Where(l => l.GradeID==gd);
                    }
                    if (elem != 0){
                        gg = gg.Where(l => l.SalaryElementID == elem);
                    }
                    return gg.OrderBy(j=>j.GradeID).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static GradeSalaryElement GetGradeSalElement(int grdId,int SalEleId)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    return db.GradeSalaryElements.Where(p => p.GradeId == grdId && p.SalElementID==SalEleId ).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DepartmentBudgetMenu GetMenuForDept(int deptId, int menuId)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    var dm = db.DepartmentBudgetMenus.Where(p => p.MenuItemID == menuId && p.DepartmentID == deptId) ;
                    if (dm != null)
                    {
                        return dm.FirstOrDefault();
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static IEnumerable<DeptBudgetItem> GetDeptMenuItem(int deptId = 0)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    var gg = from r in db.DepartmentBudgetMenus
                             join g in db.BudgetMenuItems on r.MenuItemID equals g.ID
                            // where r.DepartmentID==deptId
                             select new DeptBudgetItem
                             {
                                 DeptId = r.DepartmentID,
                                 MenuItemId = r.MenuItemID,
                                 BudgetItem = g.BudgetItem,
                                 Url = g.Url,
                                 Code=g.Code
                                 // return db.GradeSalaryElements.ToList();
                             };
                    if (deptId != 0)
                    {
                        gg = gg.Where(k => k.DeptId == deptId);
                    }
                    return gg.OrderBy(j => j.MenuItemId).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool AddMeunItem(DepartmentBudgetMenu dpmeun)
        {
            try{

            using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    db.DepartmentBudgetMenus.Add(dpmeun);
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
        public static bool DeleteDeptMenuItems(DepartmentBudgetMenu exp)
        {
            try
            {
                bool rst = false;
                using (var db = new BudgetCaptureDBEntities())
                {
                   // db.DepartmentBudgetMenus.Remove(exp);
                    db.Entry(exp).State = System.Data.Entity.EntityState.Deleted;
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
        public static IEnumerable<BudgetMenuItem> GetBudgetMenuList()
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                     
                    {
                        return db.BudgetMenuItems.OrderBy(o => o.ID).ToList();
                    }
                   
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public static bool AddIndirectType(IndirectType exp)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    db.IndirectTypes.Add(exp);
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
        public static bool UpdateIndirectType(IndirectType exp)
        {
            try
            {
                bool rst = false;
                using (var db = new BudgetCaptureDBEntities())
                {
                    db.IndirectTypes.Attach(exp);
                    db.Entry(exp).State = System.Data.Entity.EntityState.Modified;
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
        public static IEnumerable<IndirectType> GetIndirectTypeList(bool filter=false )
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    if (filter)
                    {
                        return db.IndirectTypes.Where(p => p.isActive == true).OrderBy(o => o.Name).ToList();
                    }
                    else
                    {
                        return db.IndirectTypes.OrderBy(o => o.Name).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static IndirectType GetIndirectType(int Id)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    return db.IndirectTypes.Find(Id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool AddIndirectItem(IndirectBudgetItem exp)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    db.IndirectBudgetItems.Add(exp);
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
        public static bool UpdateIndirectItem(IndirectBudgetItem exp)
        {
            try
            {
                bool rst = false;
                using (var db = new BudgetCaptureDBEntities())
                {
                    db.IndirectBudgetItems.Attach(exp);
                    db.Entry(exp).State = System.Data.Entity.EntityState.Modified;
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
        public static IEnumerable<IndirectBudgetItem> GetIndirectItemList(string filter="N")
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                     
                    return db.IndirectBudgetItems.Include("IndirectType").Where(p=>p.DelFlg==filter).OrderBy(o => o.IndirectTypeID).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static IEnumerable<IndirectBudgetItem> GetIndirectItemListLookUp(int type)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {

                    return db.IndirectBudgetItems.Include("IndirectType").Where(p => p.DelFlg == "N" && p.IndirectTypeID==type).OrderBy(o => o.Name).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static IndirectBudgetItem GetIndirectItem(int Id)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    return db.IndirectBudgetItems.Find(Id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool AddDirectType(DirectType exp)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    db.DirectTypes.Add(exp);
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
        public static bool UpdateDirectType(DirectType exp)
        {
            try
            {
                bool rst = false;
                using (var db = new BudgetCaptureDBEntities())
                {
                    db.DirectTypes.Attach(exp);
                    db.Entry(exp).State = System.Data.Entity.EntityState.Modified;
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
        public static IEnumerable<DirectType> GetDirectTypeList(bool filter = false)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    if (filter)
                    {
                        return db.DirectTypes.Where(p => p.isActive == true).OrderBy(o => o.Name).ToList();
                    }
                    else
                    {
                        return db.DirectTypes.OrderBy(o => o.Name).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DirectType GetDirectType(int Id)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    return db.DirectTypes.Find(Id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool AddDirectItem(DirectExpenseItem exp)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    db.DirectExpenseItems.Add(exp);
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
        public static bool UpdateDirectItem(DirectExpenseItem exp)
        {
            try
            {
                bool rst = false;
                using (var db = new BudgetCaptureDBEntities())
                {
                    db.DirectExpenseItems.Attach(exp);
                    db.Entry(exp).State = System.Data.Entity.EntityState.Modified;
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
        public static IEnumerable<DirectExpenseItem> GetDirectItemList(string filter = "N")
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {

                    return db.DirectExpenseItems.Include("DirectType").Where(p => p.DelFlg == filter).OrderBy(o => o.DirectTypeID).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static IEnumerable<DirectExpenseItem> GetDirectItemListLookUp(int type)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {

                    return db.DirectExpenseItems.Include("DirectType").Where(p => p.DelFlg == "N" && p.DirectTypeID == type).OrderBy(o => o.Name).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DirectExpenseItem GetDirectItem(int Id)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    return db.DirectExpenseItems.Find(Id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool AddCapexType(CapexType exp)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    db.CapexTypes.Add(exp);
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
        public static bool UpdateCapexType(CapexType exp)
        {
            try
            {
                bool rst = false;
                using (var db = new BudgetCaptureDBEntities())
                {
                    db.CapexTypes.Attach(exp);
                    db.Entry(exp).State = System.Data.Entity.EntityState.Modified;
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
        public static IEnumerable<CapexType> GetCapexTypeList(int budgetYr,bool filter = false)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    if (filter)
                    {
                        return db.CapexTypes.Where(p => p.isActive == true && p.BudgetYearID==budgetYr).OrderBy(o => o.Name).ToList();
                    }
                    else
                    {
                        return db.CapexTypes.Where(p=>p.BudgetYearID==budgetYr).OrderBy(o => o.Name).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static CapexType GetCapexType(int Id)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    return db.CapexTypes.Find(Id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool AddCapexItem(CapexExpenseItem exp)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    db.CapexExpenseItems.Add(exp);
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
        public static bool UpdateCapexItem(CapexExpenseItem exp)
        {
            try
            {
                bool rst = false;
                using (var db = new BudgetCaptureDBEntities())
                {
                    db.CapexExpenseItems.Attach(exp);
                    db.Entry(exp).State = System.Data.Entity.EntityState.Modified;
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
        public static IEnumerable<CapexExpenseItem> GetCapexItemList(int budYr,string filter = "N")
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {

                    return db.CapexExpenseItems.Include("CapexType").Where(p => p.DelFlg == filter && p.CapexType.BudgetYearID==budYr).OrderBy(o => o.CapexTypeID).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static IEnumerable<CapexExpenseItem> GetCapexItemListLookUp(int type, int budYr)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {

                    return db.CapexExpenseItems.Include("CapexType").Where(p => p.DelFlg == "N" && p.CapexTypeID == type && p.CapexType.BudgetYearID==budYr).OrderBy(o => o.Name).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static CapexExpenseItem GetCapexItem(int Id)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    return db.CapexExpenseItems.Find(Id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool AddApprovaltrack(TrackApproval byr)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    db.TrackApprovals.Add(byr);
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
        public static bool UpdateApprovalStatus(TrackApproval exp)
        {
            try
            {
                bool rst = false;
                using (var db = new BudgetCaptureDBEntities())
                {
                    db.TrackApprovals.Attach(exp);
                    db.Entry(exp).State = System.Data.Entity.EntityState.Modified;
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
        public static IEnumerable<TrackApproval> GetTrackApproval(int deptId,int budyr)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {

                    return db.TrackApprovals.Where(p=>p.DepartmentID==deptId && p.BudgetYr==budyr).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public static bool AddPipelineSection(GSS_PipelineSection exp)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    db.GSS_PipelineSection.Add(exp);
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
        public static bool UpdatePipelineSection(GSS_PipelineSection exp)
        {
            try
            {
                bool rst = false;
                using (var db = new BudgetCaptureDBEntities())
                {
                    db.GSS_PipelineSection.Attach(exp);
                    db.Entry(exp).State = System.Data.Entity.EntityState.Modified;
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
        public static IEnumerable<GSS_PipelineSection> GetPipelineSectionList(bool filter = true)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    if (filter)
                        return db.GSS_PipelineSection.Where(c => c.isActive == filter).OrderBy(o => o.Name).ToList();
                    else
                        return db.GSS_PipelineSection.OrderBy(o => o.Name).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static GSS_PipelineSection GetpipelineSection(int Id)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    return db.GSS_PipelineSection.Find(Id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool AddGssCustomer(GSS_Customer exp)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    db.GSS_Customer.Add(exp);
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
        public static bool UpdateGssCustomer(GSS_Customer exp)
        {
            try
            {
                bool rst = false;
                using (var db = new BudgetCaptureDBEntities())
                {
                    db.GSS_Customer.Attach(exp);
                    db.Entry(exp).State = System.Data.Entity.EntityState.Modified;
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
        public static IEnumerable<GSS_Customer> GetGssCustomerLookUp()
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    return db.GSS_Customer.Where(c => c.isActive == true).OrderBy(o => o.CompanyName).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static IEnumerable<GSS_Customer> GetGssCustomerList(bool filter = true)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    if (filter)
                        return db.GSS_Customer.Include("CustomerType").Include("GSS_PipelineSection").Where(c => c.isActive == filter).OrderBy(o => o.CompanyName).ToList();
                    else
                        return db.GSS_Customer.Include("CustomerType").Include("GSS_PipelineSection").OrderBy(o => o.CompanyName).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static IEnumerable<GSS_Customer> GetGssCustomerListFilter(int Id=0,string name="",int type=0,int section=0)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    var result=db.GSS_Customer.Include("CustomerType").Include("GSS_PipelineSection").OrderBy(o => o.CompanyName).AsQueryable();
                    if (Id != 0)
                        result = result.Where(c => c.ID == Id);
                    if (!string.IsNullOrEmpty(name))
                        result = result.Where(c => c.CompanyName.Contains(name));
                    if (type != 0)
                        result = result.Where(c => c.CustomerTypeID == type);
                    if (section != 0)
                        result = result.Where(c => c.PipelineSectionID == section);
                    return result.ToList();

             //return db.GSS_Customer.Include("CustomerType").Include("GSS_PipelineSection").OrderBy(o => o.CompanyName).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static GSS_Customer GetGssCustomer(int Id)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    return db.GSS_Customer.Include("CustomerType").Include("GSS_PipelineSection").Where(c => c.ID == Id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<GSS_Customer> GetGssCustomerById(int Id)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    return db.GSS_Customer.Include("CustomerType").Include("GSS_PipelineSection").Where(c => c.ID == Id).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<GSS_Customer> SearchCustomer(string pretext)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                   var lstn =db.GSS_Customer.Where(c => c.CompanyName.Contains(pretext)).ToList();
                  int cnt= lstn.Count();
                  return lstn;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}