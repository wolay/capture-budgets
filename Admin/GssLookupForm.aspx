<%@ Page Title="" Language="C#" MasterPageFile="~/SiteGSS.Master" AutoEventWireup="true" CodeBehind="GssLookupForm.aspx.cs" Inherits="BudgetCapture.Admin.GssLookupForm" %>
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
        <li> <a href="ManageCustomerType.aspx" class="btn ">Manage Customer Section </a></li>
        <li> <a href="ManagePipelineSection.aspx" class="btn">Manage Pipeline Section</a>	 </li>
         
      </ul>
    </div>
     <div class="lookup span5">
      <ul>
       <%-- <li> <a href="ManageIndirectType.aspx" class="btn ">Manage Indirect ExpenseType</a>	 </li>
        <li > <a href="ManageIndirectItem.aspx" class="btn">Manage Indirect ExpenseItems</a>  </li>	--%>
        
       
      </ul>
    </div>
    </div>
    </div>
</div>
</asp:Content>
