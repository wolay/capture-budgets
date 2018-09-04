using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BudgetCapture.DAL; 
namespace BudgetCapture.BLL
{
    public class CertifiedSalesData
    {
        public DateTime SalesDate { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName{get; set;}
        public Decimal TotalVolumeCaptured { get; set; }
        public Decimal TotalVolumeCertified { get; set; }
    }
    public class GasSalesBLL
    {
        public static bool AddSalesData(GSS_SalesTbl byr)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    db.GSS_SalesTbl.Add(byr);
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
        public static bool UpdateSalesData(GSS_SalesTbl byr)
        {
            try
            {
                bool rst = false;
                using (var db = new BudgetCaptureDBEntities())
                {
                    db.GSS_SalesTbl.Attach(byr);
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
        public static IEnumerable<GSS_SalesTbl> GetMySalesList(int userId,int status,DateTime filter, int cust=0)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    if (status == (int)Utility.SalesStatus.Pending_Capture_Approval)
                    {
                        var result= db.GSS_SalesTbl.Include("GSS_Customer").Include("AppUser").Where(p => p.CapturedBy == userId && p.Status==status).OrderByDescending(o => o.DateCaptured).AsQueryable();
                        if (cust != 0)
                        {
                            result = result.Where(c => c.CustomerID == cust).AsQueryable();
                        }
                        if (DateTime.Now.AddDays(31) >= filter)
                        {
                            result = result.Where(c => c.DateCaptured>= filter&& c.DateCaptured<=filter).AsQueryable();
                        }
                        return result.ToList();
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
        public static IEnumerable<CertifiedSalesData> GetAggregateSalesList(DateTime from, DateTime to, int cust = 0)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    var result = db.GSS_SalesTbl.Include("GSS_Customer").Include("AppUser").Include("AppUser1").Include("AppUser2").Where(p => p.Status == (int)Utility.SalesStatus.Certified_Volume_Approved)
                         .GroupBy(o => new
                         {
                             CustomerId = o.CustomerID,
                             SalesDate=o.DateCertified,
                             CustomerName=o.GSS_Customer.CompanyName
                         })
                         .Select(g => new CertifiedSalesData
                         {
                             SalesDate = g.Key.SalesDate.Value,
                             CustomerId=g.Key.CustomerId.Value,
                             CustomerName = g.Key.CustomerName,
                             TotalVolumeCaptured = g.Sum(x=>x.CapturedVolumeSale.Value),
                             TotalVolumeCertified=g.Sum(x=>x.CertifiedVolumeSale.Value)
                         })
                        .OrderByDescending(o => o.SalesDate).AsQueryable();
                    if (cust != 0)
                    {
                        result = result.Where(c => c.CustomerId == cust).AsQueryable();
                    }

                    result = result.Where(c => c.SalesDate >= from && c.SalesDate <= to).AsQueryable();
                   
                 return result.ToList();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static IEnumerable<GSS_SalesTbl> GetGasSalesList(int status, DateTime filter, int cust = 0)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                        var result = db.GSS_SalesTbl.Include("GSS_Customer").Include("AppUser").Include("AppUser1").Include("AppUser2").Where(p=>p.Status == status).OrderByDescending(o => o.DateCaptured).AsQueryable();
                        if (cust != 0)
                        {
                            result = result.Where(c => c.CustomerID == cust).AsQueryable();
                        }
                        if (DateTime.Now.AddDays(31) >= filter)
                        {
                            result = result.Where(c => c.DateCaptured >= filter && c.DateCaptured <= filter).AsQueryable();
                        }
                        return result.ToList();
                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static GSS_SalesTbl GetGasSale(int Id)
        {
            try
            {
                using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
                {
                    return db.GSS_SalesTbl.Find(Id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool DeleteGasSales(GSS_SalesTbl staff)
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
    }
}