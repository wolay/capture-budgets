using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BudgetCapture.DAL;
namespace BudgetCapture.BLL
{
    public class DepartmentMap : Department { }
    public class DepartmentBLL
    {
        public static bool AddDepartment(Department dept)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    db.Departments.Add(dept);
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
        public static bool Update(Department dept)
        {
            try
            {
                bool rst = false;
                using (var db = new BudgetCaptureDBEntities())
                {
                    db.Departments.Attach(dept);
                    db.Entry(dept).State = System.Data.Entity.EntityState.Modified;
                    //db.ObjectStateManager.ChangeObjectState(auction, System.Data.EntityState.Modified);
                    db.SaveChanges();
                    rst = true;
                }
                return rst;
            }
            catch (Exception ex)
            {
                Utility.WriteError("Error Msg: " + ex.InnerException);
                throw ex;
            }
        }
        public static List<Department> GetDeptList(int dirID=0)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    var d = (from p in db.Departments.Include("Directorate").OrderBy(k=>k.Name)
                             select p);
                             //select new Department { 
                             //    Name=p.Name.ToUpper(),
                             //    ID=p.ID
                             // } );
                    if (dirID != 0)
                    {
                        d = d.Where(p => p.DirectorateID == dirID);
                    }
                    return d.ToList();   
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static Department GetDepartment(int Id)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    return db.Departments.Find(Id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool AddDirectorate(Directorate dir)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    db.Directorates.Add(dir);
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
        public static bool UpdateDir(Directorate dir)
        {
            try
            {
                bool rst = false;
                using (var db = new BudgetCaptureDBEntities())
                {
                    db.Directorates.Attach(dir);
                    db.Entry(dir).State = System.Data.Entity.EntityState.Modified;
                    //db.ObjectStateManager.ChangeObjectState(auction, System.Data.EntityState.Modified);
                    db.SaveChanges();
                    rst = true;
                }
                return rst;
            }
            catch (Exception ex)
            {
                Utility.WriteError("Error Msg: " + ex.InnerException);
                throw ex;
            }
        }
        public static IEnumerable<Directorate> GetDirectorateList()
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    return db.Directorates.OrderBy(o => o.Name).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static Directorate GetDirectorate(int Id)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    return db.Directorates.Find(Id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}