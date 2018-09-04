using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BudgetCapture.DAL;
using System.Configuration;

namespace BudgetCapture.BLL
{
    public class UserBLL
    {
        public static bool AddUser(AppUser usr)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    db.AppUsers.Add(usr);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteError("Error Msg: " + ex.Message);
                throw ex;
            }
        }
        public static bool UpdateUser(AppUser usr)
        {
            try
            {
                bool retVal = false;
                using (var db = new BudgetCaptureDBEntities())
                {
                    usr.Department = null;
                    db.Entry(usr).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    retVal = true;
                }
                return retVal;
            }
            catch (Exception ex)
            {
                Utility.WriteError("Error Msg: " + ex.InnerException);
                throw ex;
            }
        }
       public static AppUser GetUserByUserName(string username)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    var user=from usr in db.AppUsers.Include("Department") where usr.StaffID==username
                           select usr;
                    return user.FirstOrDefault<AppUser>();
                }
                 
            }
            catch (Exception ex)
            {
                Utility.WriteError("Error Msg: " + ex.Message);
                throw ex;
            }
        }
       public static List<AppUser> GetUserByName(string username)
       {
           try
           {
               using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
               {
                   var user = from usr in db.AppUsers.Include("Department.Directorate")
                              where usr.FullName.Contains(username)
                              select usr;
                   return user.ToList();
               }

           }
           catch (Exception ex)
           {
               Utility.WriteError("Error Msg: " + ex.Message);
               throw ex;
           }
       }
       public static AppUser GetByID(int Id)
       {
           try
           {
               using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
               {
                   return db.AppUsers.Find(Id);
               }
           }
           catch (Exception ex)
           {
               Utility.WriteError("Error Msg: " + ex.Message);
               throw ex;
           }
       }
        public static List<AppUser> GetUserByID(int Id)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    var user = from usr in db.AppUsers.Include("Department").Include("Department.Directorate")
                               where usr.ID == Id
                               select usr;
                    return user.ToList<AppUser>();
                }
                // return null;
                
            }
            catch (Exception ex)
            {
                Utility.WriteError("Error Msg: " + ex.Message);
                throw ex;
            }
        }
        public static List<AppUser> GetUsersList()
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    var usrList = from p in db.AppUsers.Include("Department").Include("Department.Directorate") select p;
                    return usrList.ToList<AppUser>();
                }
            }
            catch(Exception ex)
            {
                Utility.WriteError("Error Msg: " + ex.Message);
                throw ex;
            }
        }
        public static bool UpdateUserById(int Id,bool status)
        {
            try
            {
                bool result = false;
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    var usr =(from p in db.AppUsers 
                              where p.ID==Id 
                              select p).FirstOrDefault();
                    if (status)
                    {
                        usr.isActive= false;
                    }
                    else
                    {
                        usr.isActive = true;
                    }
                    db.SaveChanges();
                    result = true;
                }
                return result;
            }
            catch (Exception ex)
            {
                Utility.WriteError("Error Msg: " + ex.Message);
                throw ex;
            }
        }

        public static string GetApproverEmailByDept(int deptId)
        {
            try
            {
                string DeptApprRole = ConfigurationManager.AppSettings["DeptApprverRole"].ToString();
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    var Hod = from p in db.AppUsers where p.DepartmentID == deptId && p.UserRole == DeptApprRole select p;
                    if (Hod != null && Hod.Count() > 0)
                    {
                        return Hod.FirstOrDefault().Email;
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}