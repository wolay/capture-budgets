<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportHome.aspx.cs" Inherits="BudgetCapture.Reports.ReportHome" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="row-fluid">
        <span class="span12 label label-info pageheader-text">Report Home</span>
    </div>
<div class="page-body">
  <div class="page-body-wrapper">
   <div class="inner-header">REPORTS</div>
    <div class="row-fluid">
     <div class="lookup span5">
      <ul>
       <%-- <li> <a href="NewHireRpt.aspx" class="btn ">New Hire Report </a></li>--%>
      <li> <a href="DirectProRpt.aspx" class="btn">Direct Budget Reports</a>	 </li>
       <li> <a href="ExistingStaffRpt.aspx" class="btn">Salary & Benefit Report </a> </li>
       <li > <a href="MovableRpt.aspx" class="btn">Movable Report</a>  </li>
        <%-- <li > <a href="#" class="btn">	Setup Tracker Company</a>  </li>	
         <li > <a href="#" class="btn">	Setup DriverEmployee</a>  </li>--%>
      </ul>
    </div>
     <div class="lookup span5">
      <ul>
            <li > <a id="A1" href="IndirectProRpt.aspx" class="btn" runat="server">	Indirect Budget Report</a>  </li>
           <li> <a href="CapexProRpt.aspx" class="btn"> Capex Report </a> </li>
      <li> <a href="RevenueProRpt.aspx" class="btn ">Revenue Projection Report</a></li>
           
      </ul>
    </div>
    </div>
    </div>

    <div class="page-body-wrapper">
   <div class="inner-header">BUDGET ATC REPORTS</div>
    <div class="row-fluid">
     <div class="lookup span5">
      <ul>
        <%--<li> <a href="NewHireRpt_ED.aspx" class="btn ">New Hire ED Report </a></li>--%>
      <li> <a href="CapexATCProRpt.aspx" class="btn">Capex ATC Reports</a>	 </li>
      <li > <a href="MovableProATCRpt.aspx" class="btn">Movable ATC Report</a>  </li>
            <li> <a href="DirectProATCRpt.aspx" class="btn">Direct ATC Report </a> </li>	
       <%--<li> <a href="ExistingStaffRpt.aspx" class="btn">Existing Staff Report </a> </li>
       <li > <a href="CapexRpt.aspx" class="btn">Capex Report</a>  </li>--%>
        <%-- <li > <a href="#" class="btn">	Setup Tracker Company</a>  </li>	
         <li > <a href="#" class="btn">	Setup DriverEmployee</a>  </li>--%>
      </ul>
    </div>
     <div class="lookup span5">
      <ul>
      <li> <a href="IndirectProATCRpt.aspx" class="btn ">Indirect ATC Report</a></li>
          <li> <a href="ExistingStaffATCRpt.aspx" class="btn">Salary & Benefit ATC Report</a>	 </li>
       
     <%--   <li > <a id="A2"  href="ExStaffRpt_ED.aspx" class="btn" runat="server">	Salary & benefit ED Report</a>  </li>--%>
        
        <%-- <li > <a href="DriversEmployerSetup.aspx" class="btn">	Setup DriversEmployer</a>  </li>--%>
      </ul>
    </div>
    </div>
    </div>
   <div class="page-body-wrapper">
   <div class="inner-header">EXECUTIVES REPORTS</div>
    <div class="row-fluid">
     <div class="lookup span5">
      <ul>
        <%--<li> <a href="NewHireRpt_ED.aspx" class="btn ">New Hire ED Report </a></li>--%>
      <li> <a href="DirectPro_ED.aspx" class="btn">Direct Budget ED Reports</a>	 </li>
      <li > <a href="MovableRpt_ED.aspx" class="btn">Movable Budget ED Report</a>  </li>
           
       <%--<li> <a href="ExistingStaffRpt.aspx" class="btn">Existing Staff Report </a> </li>
       <li > <a href="CapexRpt.aspx" class="btn">Capex Report</a>  </li>--%>
        <%-- <li > <a href="#" class="btn">	Setup Tracker Company</a>  </li>	
         <li > <a href="#" class="btn">	Setup DriverEmployee</a>  </li>--%>
      </ul>
    </div>
     <div class="lookup span5">
      <ul>
      
        <li> <a href="ATCRequestProRpt.aspx" class="btn"> ATC Request Report </a> </li>	
        <li > <a  href="ExStaffRpt_ED.aspx" class="btn" runat="server">	Salary & benefit ED Report</a>  </li>
        
        <%-- <li > <a href="DriversEmployerSetup.aspx" class="btn">	Setup DriversEmployer</a>  </li>--%>
      </ul>
    </div>
    </div>
    </div>
</div>
</asp:Content>
