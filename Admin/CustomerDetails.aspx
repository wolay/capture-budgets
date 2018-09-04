<%@ Page Title="" Language="C#" MasterPageFile="~/SiteGSS.Master" AutoEventWireup="true" CodeBehind="CustomerDetails.aspx.cs" Inherits="BudgetCapture.Admin.CustomerDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row-fluid">
     <div class=" span12 pageheader-text">Customer Details Page</div>
     <div class="alert alert-error" runat="server" id="error" visible="false">
      <button type="button" class="close" data-dismiss="alert">&times;</button>
     </div>
    <div class="alert alert-success" runat="server" id="success" visible="false">
      <button type="button" class="close" data-dismiss="alert">&times;</button>
  </div>
  </div>
    <div class="row-fluid">
 <div class="page-body span12">
  <div class="page-body-wrapper">
  <div class="inner-header">Customer Detail</div>
   <asp:HiddenField ID="hid" runat="server" />
       <asp:GridView ID="gvheader" runat="server" Width="100%" ShowHeader="false" EnableTheming="false" EmptyDataText="No Record found" GridLines="None" 
        AutoGenerateColumns="False"> 
        <Columns>
             <asp:TemplateField>
              <ItemTemplate>
               <div style="padding-bottom: 10px; font-size:12px" class="boxheader">
              <%-- <h3>User Detail</h3>--%>
               <asp:Label ID="lbInit" runat="server" Text='<%# Eval("ID")%>' Visible="false" />
               <table border="0" class="table-detail">
                <tr>
                <td class="detail-title">Company ID:</td><td class="nav-header"><b><%#Eval("ID")%></b></td>
                <td class="detail-title">Company Name:</td><td class="nav-header"><%# Eval("CompanyName")%></td>
                </tr>
                 <tr>
                <td class="detail-title">Customer Type:</td> <td class="nav-header"><%# Eval("CustomerType.Name")%> </td>
                <td class="detail-title">Address:</td><td class="nav-header"><%#Eval("CompanyAddress")%></td
                </tr>
                <tr>
                <td class="detail-title">Contact Person:</td> <td class="nav-header"><%# Eval("ContactPerson")%> </td>
                <td class="detail-title">Telehone No:</td><td class="nav-header"><%#Eval("MobileNo")%></td
                </tr>
                <tr>
                <td class="detail-title">Email:</td> <td class="nav-header"><%# Eval("Email")%> </td>
                <td class="detail-title">Pipeline Section:</td><td class="nav-header"><%#Eval("GSS_PipelineSection.Name")%></td
                </tr>
                <tr>
                <td class="detail-title">Unit Price(N):</td> <td class="nav-header"><%# Eval("UnitPrice","{0:N}")%> </td>
                <td class="detail-title">Minimum Order Volume:</td><td class="nav-header"><%#Eval("MinimumOrderVolume","{0:N}")%></td
                </tr>
                <tr>
                <td class="detail-title">Payment Term:</td> <td class="nav-header"><%# GetPaymentTerm(Eval("PaymentTermFlg"))%> </td>
                <td class="detail-title">Pipeline Section:</td><td class="nav-header"><%#Eval("GSS_PipelineSection.Name")%></td
                </tr>
                 <tr>
                <td class="detail-title">Capital Recovery:</td> <td class="nav-header"><asp:Label ID="Label1" runat="server" Text='<%# (Eval("CapitalRecovery") != null&&Boolean.Parse(Eval("CapitalRecovery").ToString())) ? "YES" : "NO"%>' ForeColor="Maroon"></asp:Label> </td>
                <td class="detail-title">Is Active:</td><td class="nav-header"><asp:Label ID="lblatc" runat="server" Text='<%# (Eval("isActive") != null&&Boolean.Parse(Eval("isActive").ToString())) ? "Active" : "Inactive"%>' ForeColor="Maroon"></asp:Label></td
                </tr>
                <tr>
                <td class="detail-title">Added By:</td><td class="nav-header"><%# Eval("AddedBy")%></td>
                 <td class="detail-title">Date Added:</td><td class="nav-header"><%#Eval("DateAdded","{0:dd-MM-yyyy hh:mm:ss tt}")%></td>
                </tr>
                 <tr>
                <td class="detail-title">Last UpdatedBy:</td><td class="nav-header"><%# Eval("LastUpdatedBy")%></td>
                 <td class="detail-title">Last Updated Date:</td><td class="nav-header"><%#Eval("LastUpdatedDate","{0:dd-MM-yyyy hh:mm:ss tt}")%></td>
                </tr>
                 
               </table>
               </div>
                 </ItemTemplate>
             </asp:TemplateField>
        </Columns>
    </asp:GridView>
    </div>
    
    <div class="page-body-control" runat="server" id="dvAdmin" visible="false">
        <div class="form-horizontal">
       <div class="nav-header">Update Details :</div>
         <asp:Button ID="btnSubmit" runat="server" Text="Update Customer Details" CausesValidation="false"
            CssClass="btn btn-submit" onclick="btnSubmit_Click"/>
     </div>
     
    </div>

 </div>
 </div>
</asp:Content>
