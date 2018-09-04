<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LookupForms.aspx.cs" Inherits="BudgetCapture.Admin.LookupForms" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div class="row-fluid">
        <span class="span12 pageheader-text">Look-Up Forms</span>
    </div>
    <div class="page-body">
  <div class="page-body-wrapper">
   <div class="inner-header">Lookup Forms</div>
    <div class="row-fluid">
     <div class="lookup span5">
      <ul>
        <li> <a href="ManageDirectorate.aspx" class="btn ">Manage Directorate </a></li>
        <li> <a href="ManageBudgetYr.aspx" class="btn">Manage Budget Year</a>	 </li>
        <li> <a href="ManageGrades.aspx" class="btn"> Manage Grades </a> </li>
        <li > <a href="ManageCapexType.aspx" class="btn">Manage Capex Type</a>  </li>
        <li > <a href="ManageCapexItem.aspx" class="btn">Manage Capex ExpenseItem</a>  </li>
         <li > <a href="ManageAssetType.aspx" class="btn">Manage Asset Type</a>  </li>	
         <li > <a href="ManageSalaryCategory.aspx" class="btn">	Manage Salary Category</a>  </li>
         <li > <a href="ManageGradeSalElement.aspx" class="btn"> Manage Grade Salary Items</a>  </li>
      </ul>
    </div>
     <div class="lookup span5">
      <ul>
        <li> <a href="ManageIndirectType.aspx" class="btn ">Manage Indirect ExpenseType</a>	 </li>
        <li > <a href="ManageIndirectItem.aspx" class="btn">Manage Indirect ExpenseItems</a>  </li>	
        <li> <a href="ManageCustomer.aspx" class="btn">Manage Customer </a> </li>
         <li> <a href="ManageCustomerType.aspx" class="btn">Manage CustomerType </a> </li>
         <li > <a href="ManageDirectType.aspx" class="btn">Manage Direct ExpenseType</a>  </li>	
       <li > <a href="ManageDirectExpenseItem.aspx" class="btn">Manage DirectExpenseItems</a>  </li>
        <li > <a href="ManageAsset.aspx" class="btn">	Manage Asset</a>  </li>
        <li > <a href="ManageSalaryElement.aspx" class="btn">	Manage Salary Elements</a>  </li>
       
      </ul>
    </div>
    </div>
    </div>
</div>
</asp:Content>
