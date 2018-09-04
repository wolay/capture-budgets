using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Mail;
using System.Configuration; 
using System.IO;
using System.Linq;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data.OracleClient;
using BudgetCapture.DAL;
using System.DirectoryServices;
using System.Threading;
namespace BudgetCapture.BLL
{
    public class RequestStatus
    {
        public string DataValue { get; set; }
        public string DataField { get; set; }
    }
    public class ATCRequestStatus
    {
        public string DataValue { get; set; }
        public string DataField { get; set; }
    }
    public class LoanType
    {
        public string DataValue { get; set; }
        public string DataField { get; set; }
    }
    public class PaymentTerm
    {
        public string DataValue { get; set; }
        public string DataField { get; set; }
    }
    public class MonthMap : MonthOfYear
    {
    }
    public class GradeSalElement
    {
        public int GradeID { get; set; }
        public string GradeName { get; set; }
        public int SalaryElementID { get; set; }
        public string ElementCode { get; set; }
        public string ElementName { get; set; }
        public decimal Amount { get; set; }
        public bool isActive { get; set; }
        public string Category { get; set; }
    }
    public class ManningCas
    {
        public decimal ManningTotal { get; set; }
        public decimal CasTotal { get; set; }
    }
    public class SalElementResult
    {
        public decimal Amount { get; set; }
        public int CatId { get; set; }
    }
    public class DeptBudgetItem
    {
        public int DeptId { get; set; }
        public int MenuItemId { get; set; }
        public string BudgetItem { get; set; }
        public string Url { get; set; }
        public string Code { get; set; }
    }
   public class Utility
    {
       private static string DeptApprRoleName = ConfigurationManager.AppSettings["DeptApprverRole"].ToString();
       private static string assetCode = ConfigurationManager.AppSettings["AssetCode"].ToString();
       private static string creditCode = ConfigurationManager.AppSettings["CreditCode"].ToString();
       private static string treasuryCode = ConfigurationManager.AppSettings["treasuryCode"].ToString();

       private static string lDAPConnectionString = ConfigurationManager.AppSettings["LDAP_URL"].ToString();
       private static string AdUserName = ConfigurationManager.AppSettings["ADUser"].ToString();
       private static string AdPwd = ConfigurationManager.AppSettings["ADpwd"].ToString();
       
       public enum BudgetItemStatus
       {
           Pending_HOD_Approval = 1,
           HOD_Approved = 2,
           Rejected=3,
           Returned_For_Correction=4,
           Pending_ED_Approval=5,
           Pending_MD_Approval=6,
           Pending_ReAlignment=7,
           Pending_MD_Final_Approval=8,
           MD_Approved=9

       }
       public enum ATCStatus
       {
           Pending_PBManager_Approval=1,
           Pending_MD_Approval=2,
           MD_Approved=3,
           Declined=4
       }
       public enum PaymentTermFlag
       {
           PaymentBeforeDelivery = 1,
           PaymentAfterDelivery = 2,
           PartPaymentBeforeDelivery= 3,
           PartPaymentAfterDelivery = 4
       }
       public enum VatFlag
       {
           Applicable=1,
           Not_Applicable=2
       }
       public enum RecoveryCategory
       {
           Disbursement = 1,
           Recovery = 2
       }
       public enum SalesStatus
       {
           Pending_Capture_Approval=1,
           Captured_Volume_Approved=2,
           Pending_Certified_Approval=3,
           Certified_Volume_Approved=4
       }
       public static string GetSalesStatus(object o)
       {
           try
           {
               string priority = "";
               if (o != null)
               {
                   if (o.ToString() == "1")
                       priority = "Pending Capture Approval";
                   if (o.ToString() == "2")
                       priority = "Approve Captured Volume";
                   if (o.ToString() == "3")
                       priority = "Pending Certified Approval";
                   if (o.ToString() == "4")
                       priority = "Approved Certified Volume";
               }
               return priority;
           }
           catch
           {
               return "";
           }
       }
       public static int GetOutstandingMonth(int monthNo)
       {
           switch (monthNo)
           {
               case 1:
                   return 12;
               case 2:
                   return 11;
               case 3:
                   return 10;
               case 4:
                   return 9;
               case 5:
                   return 8;
               case 6:
                   return 7;
               case 7:
                   return 6;
               case 8:
                   return 5;
               case 9:
                   return 4;
               case 10:
                   return 3;
               case 11:
                   return 2;
               case 12:
                   return 1;
               default:
                   return 1;
           }
       }
       public static List<LoanType> GetLoanTypeList()
       {
           List<LoanType> range = new List<LoanType>
               {
                   new LoanType{DataField="Medium Loan Lagos",DataValue="Medium Loan Lagos"},
                   new LoanType{DataField="Medium Loan Abuja",DataValue="Medium Loan Abuja"},
                   new LoanType{DataField="Small Loan 1 Abuja",DataValue="Small Loan 1 Abuja"},
                   new LoanType{DataField="Small Loan 2 Abuja",DataValue="Small Loan 2 Abuja"},
                   new LoanType{DataField="Small Loan 3 Abuja",DataValue="Small Loan 3 Abuja"},
                   new LoanType{DataField="Small Loan 1 Lagos",DataValue="Small Loan 1 Lagos"},
                   new LoanType{DataField="Small Loan 2 Lagos",DataValue="Small Loan 2 Lagos"},
                   new LoanType{DataField="Strategic 1",DataValue="Strategic 1"},
                   new LoanType{DataField="Strategic 2",DataValue="Strategic 2"},
                   new LoanType{DataField="Large Loan",DataValue="Large Loan"},
                   new LoanType{DataField="Public Sector",DataValue="Public Sector"}

               };
           return range;
       }
       public static string GetPaymentTerm(object o)
       {
           try
           {
               string priority = "";
               if (o != null)
               {
                   if (o.ToString() == "1")
                       priority = "Payment Before Delivery";
                   if (o.ToString() == "2")
                       priority = "Payment After Delivery";
                   if (o.ToString() == "3")
                       priority = "Part Payment Before Delivery";
                   if (o.ToString() == "4")
                       priority = "Part Payment After Delivery";
               }
               return priority;
           }
           catch
           {
               return "";
           }
       }
       public static string GetPriority(object o)
       {
           try
           {
               string priority = "";
               if (o != null)
               {
                   if (o.ToString() == "1")
                       priority = "Emergency";
                   if (o.ToString() == "2")
                       priority = "High";
                   if (o.ToString() == "3")
                       priority = "Normal";
                   if (o.ToString() == "4")
                       priority = "Low";
               }
               return priority;
           }
           catch
           {
               return "";
           }
       }
       public static string GetATCStatus(object o)
       {
           try
           {
               string status = "";
               if (o != null)
               {
                   if (o.ToString() == "1")
                       status = "Pending PBManager Approval";
                   if (o.ToString() == "2")
                       status = "Pending MD Approval";
                   if (o.ToString() == "3")
                       status = "MD_Approved";
                   if (o.ToString() == "4")
                       status = "Declined";
               }
               return status;
           }
           catch
           {
               return "";
           }
       }
       public static string GetBudgetStatus(object o)
       {
           try
           {
               string status = "";
               if (o != null)
               {
                   if (o.ToString() == "1")
                       status = "Pending HOD Approval";
                   if (o.ToString() == "2")
                       status = "HOD Approved";
                   if (o.ToString() == "3")
                       status = "Rejected";
                   if (o.ToString() == "4")
                       status = "Returned For Correction";
                   if (o.ToString() == "5")
                       status = "Pending ED Approval";
                   if (o.ToString() == "6")
                       status = "Pending MD Approved";
                   if (o.ToString() == "7")
                       status = "Pending ReAlignment";
                   if (o.ToString() == "8")
                       status = "Pending MD Final Approval";
                   if (o.ToString() == "9")
                       status = "MD Approved";
                   
               }
               return status;
           }
           catch
           {
               return "";
           }
       }
       public static List<ATCRequestStatus> ATCRequestList()
       {
           List<ATCRequestStatus> range = new List<ATCRequestStatus>
               {
                   new ATCRequestStatus{DataField="Pending PBManager Approval",DataValue="1"},
                   new ATCRequestStatus{DataField="Pending MD Approval",DataValue="2"},
                   new ATCRequestStatus{DataField="MD Approved",DataValue="3"},
                   new ATCRequestStatus{DataField="Declined",DataValue="4"},
                   

               };
           return range;
       }
       public static List<PaymentTerm> PaymentTermList()
       {
           List<PaymentTerm> range = new List<PaymentTerm>
               {
                   new PaymentTerm{DataField="Payment Before Delivery",DataValue="1"},
                   new PaymentTerm{DataField="Payment After Delivery",DataValue="2"},
                   new PaymentTerm{DataField="Part Payment Before Delivery",DataValue="3"},
                   new PaymentTerm{DataField="Part Payment After Delivery",DataValue="4"},
                   

               };
           return range;
       }
       public static List<RequestStatus> StatusList()
       {
           List<RequestStatus> range = new List<RequestStatus>
               {
                   new RequestStatus{DataField="Pending HOD Approval",DataValue="1"},
                   new RequestStatus{DataField="HOD Approved",DataValue="2"},
                   new RequestStatus{DataField="Rejected",DataValue="3"},
                   new RequestStatus{DataField="Returned For Correction",DataValue="4"},
                   new RequestStatus{DataField="Pending ED Approval",DataValue="5"},
                   new RequestStatus{DataField="Pending MD Approved",DataValue="6"},
                   new RequestStatus{DataField="Pending ReAlignment",DataValue="7"},
                   new RequestStatus{DataField="Pending MD Final Approval",DataValue="8"},
                   new RequestStatus{DataField="MD Approved",DataValue="9"}
               };
           return range;
       }

       public static void BindPipelineSection(DropDownList ddlist)
       {
           ddlist.DataTextField = "Name";
           ddlist.DataValueField = "ID";
           ddlist.DataSource = LookUpBLL.GetPipelineSectionList();
           ddlist.DataBind();
       }
       public static void BindDept(DropDownList ddlist,int dirID=0)
        {
            ddlist.DataTextField = "Name";
            ddlist.DataValueField = "ID";
            ddlist.DataSource = DepartmentBLL.GetDeptList(dirID);
            ddlist.DataBind();
        }

       public static void BindExpenseCategory(DropDownList ddlist,int dept=0 )
       {
           ddlist.DataTextField = "Name";
           ddlist.DataValueField = "ID";
           if (dept == 0)
           {
               ddlist.DataSource = LookUpBLL.GetExpenseTypeList(dept);
               ddlist.DataBind();
           }
           else
           {
               ddlist.DataSource = LookUpBLL.GetExpenseTypeList(dept);
               ddlist.DataBind();
           }
       }
       //public static void BindReinvestmentType(DropDownList ddlist)
       //{
       //    ddlist.DataTextField = "Name";
       //    ddlist.DataValueField = "ID";
       //    ddlist.DataSource = LookUpBLL.GetReinvestList();
       //    ddlist.DataBind();
       //}
       public static void BindAssetType(DropDownList ddlist)
       {
           ddlist.DataTextField = "Name";
           ddlist.DataValueField = "ID";
           ddlist.DataSource = LookUpBLL.GetAssetTypeList();
           ddlist.DataBind();
       }
       public static void BindIndirectType(DropDownList ddlist)
       {
           ddlist.DataTextField = "Name";
           ddlist.DataValueField = "ID";
           ddlist.DataSource = LookUpBLL.GetIndirectTypeList(true);
           ddlist.DataBind();
       }
       public static void BindDirectType(DropDownList ddlist)
       {
           ddlist.DataTextField = "Name";
           ddlist.DataValueField = "ID";
           ddlist.DataSource = LookUpBLL.GetDirectTypeList(true);
           ddlist.DataBind();
       }
       
       public static void BindCapexType(DropDownList ddlist,int budyr)
       {
           ddlist.DataTextField = "Name";
           ddlist.DataValueField = "ID";
           ddlist.DataSource = LookUpBLL.GetCapexTypeList(budyr,true);
           ddlist.DataBind();
       }
       public static void BindSalBenCategory(DropDownList ddlist)
       {
           ddlist.DataTextField = "Name";
           ddlist.DataValueField = "ID";
           ddlist.DataSource = LookUpBLL.GetSalBenCategoryList(true);
           ddlist.DataBind();
       }
       public static void BindSalaryElement(DropDownList ddlist)
       {
           ddlist.DataTextField = "Elements";
           ddlist.DataValueField = "ID";
           ddlist.DataSource = LookUpBLL.GetSalaryElementList(true);
           ddlist.DataBind();
       }
       public static void BindATCStatus(DropDownList ddlist)
       {
           ddlist.DataValueField = "DataValue";
           ddlist.DataTextField = "DataField";
           ddlist.DataSource = Utility.ATCRequestList();
           ddlist.DataBind();
       }
       public static void BindBudgetStatus(DropDownList ddlist)
       {
           ddlist.DataValueField = "DataValue";
           ddlist.DataTextField = "DataField";
           ddlist.DataSource = Utility.StatusList();
           ddlist.DataBind();
       }
       public static void BindGssCustomer(DropDownList ddlist)
       {
           ddlist.DataTextField = "CompanyName";
           ddlist.DataValueField = "ID";
           ddlist.DataSource = LookUpBLL.GetGssCustomerLookUp();
           ddlist.DataBind();
       }
       public static void BindPaymentTerm(DropDownList ddlist)
       {
           ddlist.DataValueField = "DataValue";
           ddlist.DataTextField = "DataField";
           ddlist.DataSource = Utility.PaymentTermList();
           ddlist.DataBind();

       }
       public static void BindCustomerType(DropDownList ddlist)
       {
           ddlist.DataTextField = "Name";
           ddlist.DataValueField = "ID";
           ddlist.DataSource = LookUpBLL.GetCustomerTypeList();
           ddlist.DataBind();
       }
       public static void BindLoanType(DropDownList ddlist)
       {
           ddlist.DataValueField = "DataValue";
           ddlist.DataTextField = "DataField";
           ddlist.DataSource = Utility.GetLoanTypeList();
           ddlist.DataBind();

       }
       public static void BindDirectorate(DropDownList ddlist)
       {
           ddlist.DataTextField = "Name";
           ddlist.DataValueField = "ID";
           ddlist.DataSource = DepartmentBLL.GetDirectorateList();
           ddlist.DataBind();
       }
       public static void BindBudgetYear(DropDownList ddlist)
       {
           ddlist.DataTextField = "Year";
           ddlist.DataValueField = "ID";
           ddlist.DataSource = LookUpBLL.GetBudgetYearList();
           ddlist.DataBind();
       }
       public static void BindGrade(DropDownList ddlist)
       {
           ddlist.DataTextField = "Name";
           ddlist.DataValueField = "ID";
           ddlist.DataSource = LookUpBLL.GetGradeList();
           ddlist.DataBind();
       }
       public static void BindMonth(DropDownList ddlist)
       {
           ddlist.DataTextField = "MonthName";
           ddlist.DataValueField = "ID";
           ddlist.DataSource = LookUpBLL.GetMonthList();
           ddlist.DataBind();
       }
       public static ManningCas ComputeManningCas(Grade gd)
       {
           using(var db=new BudgetCaptureDBEntities())
           {
               var rst= from s in db.GradeSalaryElements
                        join el in db.SalaryElements on
                        s.SalElementID equals el.ID
                       where s.GradeId== gd.ID && s.isActive==true
                      select new SalElementResult
                      {
                          Amount=s.Amount.Value,
                          CatId=el.CategoryId.Value
                      };
               rst.ToList();
               if (rst.Count() > 0)
               {
                   decimal manningTot = 0; decimal casTot = 0; ManningCas mCas = new ManningCas();
                   int manningVal = int.Parse(ConfigurationManager.AppSettings["ManningDBValue"].ToString());
                   int casVal = int.Parse(ConfigurationManager.AppSettings["CasDBValue"].ToString());
                   foreach (SalElementResult gs in rst)
                   {
                       if (gs.CatId == manningVal)
                           manningTot += gs.Amount;
                       if (gs.CatId == casVal)
                           casTot += gs.Amount;
                   }
                   mCas.CasTotal = casTot; mCas.ManningTotal = manningTot;
                   return mCas;
               }
               else
               {
                   return null;
               }

           }
           
       }
       public static string ConnectionStr()//all remember to set this to point to live Finacle
       {
           //string connection = ConfigurationManager.ConnectionStrings["finLiveConnectionString"].ConnectionString;
           string connection = ConfigurationManager.ConnectionStrings["OraConnection"].ConnectionString;
           return connection;
       }

       public static string FinnOneConnectionStr()//all remember to set this to point to live Finacle
       {
           //string connection = ConfigurationManager.ConnectionStrings["finLiveConnectionString"].ConnectionString;
           string connection = ConfigurationManager.ConnectionStrings["FinnOneConnection"].ConnectionString;
           return connection;
       }
       public static Customer GetCustomer(string CustCode)
       {
           try
           {
               return LookUpBLL.GetCustomerList().Where(c => c.Code == CustCode.ToUpper()).FirstOrDefault();
           }
           catch {
               return null;
           }

       }
       //public static Obligor GetObligor(string AgreementNo)
       //{
       //    try
       //    {
       //        string connection = FinnOneConnectionStr();
       //        using (OracleConnection con = new OracleConnection(connection))
       //        {
       //            con.Open();
       //            string sql = "select b.customername,a.agreementid,a.agreementno, amtfin,lad_onboarding_no_c,a.status,a.schemeid from lea_agreement_dtl a,nbfc_customer_m b " +
       //                         "where a.lesseeid= b.customerid and a.agreementno='" + AgreementNo.Trim() + "'";
       //            OracleCommand cmd = new OracleCommand(sql);
       //            cmd.Connection = con;
       //            OracleDataReader reader = cmd.ExecuteReader();
       //            //List<User> UserList = new List<User>();
       //            Obligor oblig = new Obligor();
       //            if (reader.HasRows)
       //            {
       //                while (reader.Read())
       //                {
       //                    oblig.FullName = reader["customername"].ToString();
       //                    oblig.AgreementID = reader["agreementno"].ToString();
       //                    // urs.DepartmentName = reader["Department"].ToString();
       //                    //urs.Directorate = reader["directorate"].ToString();
       //                    //UserList.Add(urs);
       //                }
       //            }
       //            else
       //            {
       //                oblig = null;
       //            }
       //            return oblig;
       //        }
       //    }
       //    catch (Exception ex)
       //    {
       //        throw ex;
       //    }
       //}
       //public static void BindDisbursementList(DropDownList ddlist)
       //{
       //    ddlist.DataTextField = "AssetName";
       //    ddlist.DataValueField = "ID";
       //    ddlist.DataSource = LookUpBLL.GetDisBursmentList();
       //    ddlist.DataBind();
       //}

       public static AppUser GetUser(string staffID, string old)
       {
           try
           {
               string connection = ConnectionStr();
               using (OracleConnection con = new OracleConnection(connection))
               {
                   con.Open();
                   string sql = "select staff_no, last_name, first_name, full_name,email_address, unit Department, directorate "+
                                "from amcon_master_all where staff_no='"+staffID.Trim()+"'";
                   OracleCommand cmd = new OracleCommand(sql);
                   cmd.Connection = con;
                   OracleDataReader reader = cmd.ExecuteReader();
                   //List<User> UserList = new List<User>();
                   AppUser urs = new AppUser();
                   if (reader.HasRows)
                   {
                       while (reader.Read())
                       {

                           urs.FullName = reader["last_name"].ToString() + " " + reader["first_name"].ToString();
                           urs.Email = reader["email_address"].ToString();
                          // urs.DepartmentName = reader["Department"].ToString();
                           //urs.Directorate = reader["directorate"].ToString();
                           //UserList.Add(urs);
                       }
                   }
                   else
                   {
                       urs = null;
                   }
                   return urs;
               }

           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public static AppUser GetUser(string loginName)
       {
           try
           {
               AppUser urs = null;
               //  urs = null;
               SearchResult theSearchResult = GetUserDirectoryEntryDetails(loginName);

               if (theSearchResult != null)
               {
                   urs = new AppUser();
                   ResultPropertyCollection properties = theSearchResult.Properties;
                   string fname = ""; string lname = ""; string email = ""; string dept = "";
                   if (properties.Contains("givenName"))
                   {
                       fname = theSearchResult.Properties["givenName"][0].ToString();
                   }
                   if (properties.Contains("sn"))
                   {
                       lname = theSearchResult.Properties["sn"][0].ToString();
                   }

                   if (properties.Contains("mail"))
                   {
                       email = theSearchResult.Properties["mail"][0].ToString();
                   }
                   if (properties.Contains("department"))
                   {
                      // urs.DepartmentName = theSearchResult.Properties["department"][0].ToString();
                   }
                   urs.FullName = fname + " " + lname;
                   urs.Email = email;
                  // urs.Directorate = "";
               }

               return urs;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public static SearchResult GetUserDirectoryEntryDetails(string loginUserName)
       {
           string str = loginUserName;
           try
           {
               DirectoryEntry entry = new DirectoryEntry(lDAPConnectionString, AdUserName, AdPwd)
               {
                   AuthenticationType = AuthenticationTypes.Secure
               };
               SearchResult result = new DirectorySearcher { SearchRoot = entry, Filter = "(&(objectClass=user)(SAMAccountName=" + loginUserName + "))", SearchScope = SearchScope.Subtree }.FindOne();
               if (result != null)
               {
                   return result;
               }
               return null;
           }
           catch (Exception exception)
           {
               WriteError("Error: " + exception.Message);
               return null;
           }
       }
       public static void ComputeBudgetSummaryExistingStff(int budyrID, int deptID)
       {
           try
           {
               DBAccess db = new DBAccess();
               db.AddParameter("budgetYrId", budyrID);
               db.AddParameter("deptId", deptID);
               db.ExecuteNonQuery("bc_ComputeBudgetSummary");
               return;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public static void ComputeBudgetSummaryNewHire(int budyrID, int deptID)
       {
           try
           {
               DBAccess db = new DBAccess();
               db.AddParameter("budgetYrId", budyrID);
               db.AddParameter("deptId", deptID);
               db.ExecuteNonQuery("bc_ComputeBudgetSummary_NewHire");
               return;
           }catch(Exception ex)
           {
               throw ex;
           }
       }
       public static void ComputeBudgetSummaryExpenseProjection(int budyrID, int deptID)
       {
           try
           {
               DBAccess db = new DBAccess();
               db.AddParameter("budgetYrId", budyrID);
               db.AddParameter("deptId", deptID);
               db.ExecuteNonQuery("bc_ComputeBudgetSummary_ExpenseProjection");
               return;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public static void ComputeBudgetSummaryDirectProjection(int budyrID, int deptID)
       {
           try
           {
               DBAccess db = new DBAccess();
               db.AddParameter("budgetYrId", budyrID);
               db.AddParameter("deptId", deptID);
               db.ExecuteNonQuery("bc_ComputeBudgetSummary_DirectProjection");
               return;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public static void ComputeBudgetSummaryCapexProjection(int budyrID, int deptID)
       {
           try
           {
               DBAccess db = new DBAccess();
               db.AddParameter("budgetYrId", budyrID);
               db.AddParameter("deptId", deptID);
               db.ExecuteNonQuery("bc_ComputeBudgetSummary_CapexProjection");
               return;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public static void ComputeBudgetSummaryIndirectProjection(int budyrID, int deptID)
       {
           try
           {
               DBAccess db = new DBAccess();
               db.AddParameter("budgetYrId", budyrID);
               db.AddParameter("deptId", deptID);
               db.ExecuteNonQuery("bc_ComputeBudgetSummary_IndirectProjection");
               return;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public static void ComputeBudgetSummaryFixedAsset(int budyrID, int deptID)
       {
           try
           {
               DBAccess db = new DBAccess();
               db.AddParameter("budgetYrId", budyrID);
               db.AddParameter("deptId", deptID);
               db.ExecuteNonQuery("bc_ComputeBudgetSummary_FixAsset");
               return;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public static void ComputeBudgetSummaryRevenue(int budyrID, int deptID)
       {
           try
           {
               DBAccess db = new DBAccess();
               db.AddParameter("budgetYrId", budyrID);
               db.AddParameter("deptId", deptID);
               db.ExecuteNonQuery("bc_ComputeBudgetSummary_Revenue");
               return;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public static void ComputeBudgetSummaryRestructure(int budyrID, int deptID)
       {
           try
           {
               DBAccess db = new DBAccess();
               db.AddParameter("budgetYrId", budyrID);
               db.AddParameter("deptId", deptID);
               db.ExecuteNonQuery("bc_ComputeBudgetSummary_Restructure");
               return;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public static void ComputeBudgetSummaryReinvestment(int budyrID, int deptID)
       {
           try
           {
               DBAccess db = new DBAccess();
               db.AddParameter("budgetYrId", budyrID);
               db.AddParameter("deptId", deptID);
               db.ExecuteNonQuery("bc_ComputeBudgetSummary_Reinvestment");
               return;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public static void ComputeBudgetSummaryIncome(int budyrID, int deptID)
       {
           try
           {
               DBAccess db = new DBAccess();
               db.AddParameter("budgetYrId", budyrID);
               db.AddParameter("deptId", deptID);
               db.ExecuteNonQuery("bc_ComputeBudgetSummary_Imcome");
               return;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public static void ComputeBudgetSummaryDisbursement(int budyrID, int deptID)
       {
           try
           {
               DBAccess db = new DBAccess();
               db.AddParameter("budgetYrId", budyrID);
               db.AddParameter("deptId", deptID);
               db.ExecuteNonQuery("bc_ComputeBudgetSummary_Disbursement");
               return;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public static void UpdateBudgetMovable(int status, int deptID, string approver, int who)
       {
           try
           {
               DBAccess db = new DBAccess();
               db.AddParameter("status", status);
               db.AddParameter("deptId", deptID);
               db.AddParameter("approver", approver);
               db.AddParameter("dateppr", DateTime.Now);
               db.AddParameter("who", who);
               db.ExecuteNonQuery("bc_Update_Movable");
               return;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public static void UpdateBudgetCapex(int status, int deptID, string approver, int who)
       {
           try
           {
               DBAccess db = new DBAccess();
               db.AddParameter("status", status);
               db.AddParameter("deptId", deptID);
               db.AddParameter("approver", approver);
               db.AddParameter("dateppr", DateTime.Now);
               db.AddParameter("who", who);
               db.ExecuteNonQuery("bc_Update_Capex");
               return;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public static void UpdateBudgetDirect(int status, int deptID, string approver, int who)
       {
           try
           {
               DBAccess db = new DBAccess();
               db.AddParameter("status", status);
               db.AddParameter("deptId", deptID);
               db.AddParameter("approver", approver);
               db.AddParameter("dateppr", DateTime.Now);
               db.AddParameter("who", who);
               db.ExecuteNonQuery("bc_Update_Direct");
               return;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public static void UpdateBudgetIndirect(int status, int deptID, string approver, int who)
       {
           try
           {
               DBAccess db = new DBAccess();
               db.AddParameter("status", status);
               db.AddParameter("deptId", deptID);
               db.AddParameter("approver", approver);
               db.AddParameter("dateppr", DateTime.Now);
               db.AddParameter("who", who);
               db.ExecuteNonQuery("bc_Update_Indirect");
               return;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public static void UpdateBudgetExistingSf(int status, int deptID, string approver, int who)
       {
           try
           {
               DBAccess db = new DBAccess();
               db.AddParameter("status", status);
               db.AddParameter("deptId", deptID);
               db.AddParameter("approver", approver);
               db.AddParameter("dateppr", DateTime.Now);
               db.AddParameter("who", who);
               db.ExecuteNonQuery("bc_Update_ExistingSf");
               return;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public static void UpdateBudgetRevenue(int status, int deptID, string approver, int who)
       {
           try
           {
               DBAccess db = new DBAccess();
               db.AddParameter("status", status);
               db.AddParameter("deptId", deptID);
               db.AddParameter("approver", approver);
               db.AddParameter("dateppr", DateTime.Now);
               db.AddParameter("who", who);
               db.ExecuteNonQuery("bc_Update_Revenue");
               return;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public static string GetUserMenuView(string deptCode)
       {
           try
           {
               string[] asstCodeList = assetCode.Split(new char[] { ',' });
               string[] creditCodeList = creditCode.Split(new char[] { ',' });
               string[] treasuryCodeList = treasuryCode.Split(new char[] { ',' });
               if (asstCodeList.Contains(deptCode))
               {
                   return "1";//asset
               }
               if (creditCodeList.Contains(deptCode))
               {
                   return "2";//credit
               }
               if (treasuryCodeList.Contains(deptCode))
               {
                   return "3";//treasury
               }
               return "0";
           }catch(Exception ex)
           {
               throw ex;
           }
       }
   
       public static string GetUsersEmailAdd(string RoleName,int DeptID=0)
       {
           try
           {
               string to = "";
               using (BudgetCaptureDBEntities db = new BudgetCaptureDBEntities())
               {
                   var urs = db.AppUsers.Where(u => u.UserRole == RoleName).ToList();
                   //select u;
                   if (DeptID != 0)
                   {
                       urs = urs.Where(p => p.DepartmentID == DeptID).ToList();
                   }
                   if (urs.Count() > 0)
                   {
                       foreach (AppUser us in urs)
                       {
                           to += us.Email +",";
                       }
                       to = to.TrimEnd(new char[] { ',' });
                   }
               }
               return to;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       
       public static decimal GetTripDuration(int stHr, int stMin, int rtHr, int rtMin)
       {
           try
           {
               decimal rtTotalSecs = 0; decimal stTotalSecs = 0; decimal totalDurationInSecs = 0;
               rtTotalSecs = (rtHr * 60 * 60) + (rtMin * 60);
               rtTotalSecs = rtTotalSecs / 86400;
               stTotalSecs = (stHr * 60 * 60) + (stMin * 60);
               stTotalSecs = stTotalSecs / 86400;
               totalDurationInSecs = rtTotalSecs - stTotalSecs;
               return totalDurationInSecs * 24;//convert to hours
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       
       public static Bitmap CreateThumbnail(string lcFilename, int lnWidth, int lnHeight)
       {
           System.Drawing.Bitmap bmpOut = null;
           try
           {

               Bitmap loBMP = new Bitmap(lcFilename);
               ImageFormat loFormat = loBMP.RawFormat;
               decimal lnRatio;
               int lnNewWidth = 0;
               int lnNewHeight = 0;

               //*** If the image is smaller than a thumbnail just return it

               if (loBMP.Width < lnWidth && loBMP.Height < lnHeight)

                   return loBMP;

               if (loBMP.Width > loBMP.Height)
               {

                   lnRatio = (decimal)lnWidth / loBMP.Width;
                   lnNewWidth = lnWidth;
                   decimal lnTemp = loBMP.Height * lnRatio;
                   lnNewHeight = (int)lnTemp;

               }
               else
               {
                   lnRatio = (decimal)lnHeight / loBMP.Height;
                   lnNewHeight = lnHeight;
                   decimal lnTemp = loBMP.Width * lnRatio;
                   lnNewWidth = (int)lnTemp;

               }

               // System.Drawing.Image imgOut =
               //      loBMP.GetThumbnailImage(lnNewWidth,lnNewHeight,
               //                              null,IntPtr.Zero);
               // *** This code creates cleaner (though bigger) thumbnails and properly
               // *** and handles GIF files better by generating a white background for
               // *** transparent images (as opposed to black)

               bmpOut = new Bitmap(lnNewWidth, lnNewHeight);
               Graphics g = Graphics.FromImage(bmpOut);
               g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
               g.FillRectangle(Brushes.White, 0, 0, lnNewWidth, lnNewHeight);
               g.DrawImage(loBMP, 0, 0, lnNewWidth, lnNewHeight);
               loBMP.Dispose();

           }

           catch
           {
               return null;
           }
           return bmpOut;

       }

       public static string GetUserIPAdress(HttpContext context)
       {
           string UserIPAddress = "";
           UserIPAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
           if (string.IsNullOrEmpty(UserIPAddress))
           {
               UserIPAddress = context.Request.ServerVariables["REMOTE_ADDR"];
           }

           return UserIPAddress;
       }
       public static string SendMail(string toList, string from, string ccList, string subject, string body)
       {
           MailMessage message = new MailMessage();
           message.Headers.Add("content-type", "text/html;");
           SmtpClient smtpClient = new SmtpClient();
           string msg = string.Empty;
           try
           {
               MailAddress fromAddress = new MailAddress(from);
               message.From = fromAddress;
              // ccList = "temitope.fatayo@amcon.com.ng";
               //toList = "temitope.fatayo@amcon.com.ng";
               message.To.Add(toList);
               if (ccList != null && ccList != string.Empty)
                   message.CC.Add(ccList);
               message.Subject = subject;
               message.IsBodyHtml = true;
               message.Body = body;
               smtpClient.Host = ConfigurationManager.AppSettings["smtpServer"].ToString();
               // smtpClient.Host = "lac-la1-s024.leadway.com.ng";
               smtpClient.Port = 25;
               smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
               smtpClient.UseDefaultCredentials = false;
               string exUser = ConfigurationManager.AppSettings["exUser"].ToString(); ;
               string exPwd = ConfigurationManager.AppSettings["exPwd"].ToString(); ;
               smtpClient.Credentials = new System.Net.NetworkCredential(exUser, exPwd);
               try
               {
                   Thread email = new Thread(delegate()
                   {
                       smtpClient.Send(message); //uncomment onn go live
                   });
                   email.IsBackground = true;
                   email.Start();
               }
               catch(SmtpException ex)
               {
                   WriteError(ex.Message);
               }
             
              // smtpClient.Send(message);
               msg = "Successful";
           }
           catch (SmtpException ex)
           {
               msg = ex.Message;
               WriteError(ex.Message);
               return msg;
           }
           return msg;
       }
       public static void WriteError(string errorMessage)
       {
           try
           {
               string path = "~/ErrorLog/" + DateTime.Today.ToString("dd-MMM-yy") + ".txt";
               if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(path)))
               {
                   File.Create(System.Web.HttpContext.Current.Server.MapPath(path)).Close();
               }
               using (StreamWriter w = File.AppendText(System.Web.HttpContext.Current.Server.MapPath(path)))
               {
                   w.WriteLine("\r\nLog Entry : ");
                   w.WriteLine("{0}", DateTime.Now.ToString(CultureInfo.InvariantCulture));
                   string err = "Error in: " + System.Web.HttpContext.Current.Request.Url.ToString() +
                                 ". Error Message:" + errorMessage;
                   w.WriteLine(err);
                   w.WriteLine("__________________________");
                   w.Flush();
                   w.Close();
               }
           }
           catch (Exception ex)
           {
               WriteError(ex.Message);
           }

       }

       public static void ExporttoExcel(DataTable table, GridView GridView_Result, string RptName)
       {
           HttpContext.Current.Response.Clear();
           HttpContext.Current.Response.ClearContent();
           HttpContext.Current.Response.ClearHeaders();
           HttpContext.Current.Response.Buffer = true;
           HttpContext.Current.Response.ContentType = "application/ms-excel";
           HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
           HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + RptName + ".xls");

           HttpContext.Current.Response.Charset = "utf-8";
           HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
           //sets font
           HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
           HttpContext.Current.Response.Write("<BR><BR><BR>");
           //sets the table border, cell spacing, border color, font of the text, background, foreground, font height
           HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
             "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
             "style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");
           //am getting my grid's column headers

           //int columnscount = GridView_Result.Columns.Count;
           int columnscount = table.Columns.Count;

           for (int j = 0; j < columnscount; j++)
           {      //write in new column
               HttpContext.Current.Response.Write("<Td>");
               //Get column headers  and make it as bold in excel columns
               HttpContext.Current.Response.Write("<B>");
               //HttpContext.Current.Response.Write(GridView_Result.Columns[j].HeaderText.ToString());
               HttpContext.Current.Response.Write(table.Columns[j].ColumnName.ToString());
               HttpContext.Current.Response.Write("</B>");
               HttpContext.Current.Response.Write("</Td>");
           }
           HttpContext.Current.Response.Write("</TR>");
           foreach (DataRow row in table.Rows)
           {//write in new row
               HttpContext.Current.Response.Write("<TR>");
               for (int i = 0; i < table.Columns.Count; i++)
               {
                   HttpContext.Current.Response.Write("<Td>");
                   HttpContext.Current.Response.Write(row[i].ToString());
                   HttpContext.Current.Response.Write("</Td>");
               }

               HttpContext.Current.Response.Write("</TR>");
           }
           HttpContext.Current.Response.Write("</Table>");
           HttpContext.Current.Response.Write("</font>");
           HttpContext.Current.Response.Flush();
           HttpContext.Current.Response.End();
       }


   }
 
 }
    


