<%@ Page Title="" Language="C#" MasterPageFile="~/SiteGSS.Master" AutoEventWireup="true" CodeBehind="GssHome.aspx.cs" Inherits="BudgetCapture.GssHome" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <div class="row-fluid">
     <div class=" span12 pageheader-text">Manage Certified Sales Data Page</div>
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
  <div class="inner-header">Search Criterias</div>
    <div class="form-horizontal row-fluid">
        <div class="page-body span6">
    <%--<div class="nav-header" runat="server" id="dvMsg">Add New Pipeline Section :</div>--%>
        <div class="control-group" runat="server" id="dvID">
            <label class="control-label" for="txtStaffID">From Date:<span>*</span>:</label>
            <div class="controls">
               <asp:TextBox ID="txtFilterDateFrom" runat="server" CssClass="" Width="120px"></asp:TextBox>
           <asp:Image ID="Image1" runat="server" ImageUrl="~/img/cal.gif" />
           <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupPosition="Right" Format="dd/MM/yyyy" TargetControlID="txtFilterDateFrom" PopupButtonID="Image1"></asp:CalendarExtender> 
            </div>
        </div>
         
        <div class="control-group">
        <label class="control-label" for="txtStaffID">To Date:</label>
        <div class="controls">
            <asp:TextBox ID="txtFilterDateTo" runat="server" CssClass="" Width="120px"></asp:TextBox>
           <asp:Image ID="Image2" runat="server" ImageUrl="~/img/cal.gif" />
           <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupPosition="Right" Format="dd/MM/yyyy" TargetControlID="txtFilterDateTo" PopupButtonID="Image2"></asp:CalendarExtender> 
             </div>
        </div>
       
            
        </div>
        <div class="page-body span6">
        
      <div class="control-group">
        <label class="control-label" for="txtStaffID">Select Customer:</label>
        <div class="controls">
             <asp:DropDownList ID="ddlCustFilter"  Width="140px" runat="server" AppendDataBoundItems="true" Font-Size="11px">
             <asp:ListItem Value="" Selected="True">...Filter By Customer...</asp:ListItem>
            </asp:DropDownList>
        </div>
        </div>
        <div class="control-group">
        <label class="control-label" for="btnSubmit"></label>
        <div class="controls">
          <asp:Button ID="Button1" runat="server" Text="Search" 
                CssClass="btn btn-submit" onclick="btnSubmit_Click" />
            &nbsp;&nbsp;
            <asp:Button ID="btnAll" runat="server" Text="Clear" 
                CssClass="btn btn-submit" OnClick="btnAll_Click"/>
         </div>
      </div>
  </div>
            </div>
        
</div>
<div class="row-fluid">
   <div class="page-body-control span12">
 <div class="inner-header">Certified Sales Volume Data
 </div>
       
    <asp:HiddenField ID="hid" runat="server" />
     <asp:GridView ID="gvCommon" runat="server" GridLines="None"  Font-Size="11px" EmptyDataText="No record found" 
        AutoGenerateColumns="false" CssClass="table table-striped" 
           AllowPaging="true" PageIndex="0" PageSize="50" 
           onpageindexchanging="gvCommon_PageIndexChanging" OnRowCommand="gvDept_RowCommand">                                                                                                        
      <Columns>
        <asp:TemplateField HeaderText="Customer Name">
            <ItemTemplate>
                <asp:Label ID="lbRecID" runat="server" Text='<%#Eval("CustomerId") %>' Visible="false"></asp:Label>
                 <asp:Label ID="lbCompany" runat="server" Text='<%#Eval("CustomerName")%>'></asp:Label>
             </ItemTemplate>
          </asp:TemplateField>
        
        <asp:BoundField HeaderText="Sales Date" DataField="SalesDate" DataFormatString="{0:dd-MMM-yyyy}"/>
        <asp:BoundField HeaderText=" Total Captured Volume" DataField="TotalVolumeCaptured" DataFormatString="{0:N}"/>
        <asp:BoundField HeaderText=" Total Certified Volume" DataField="TotalVolumeCertified" DataFormatString="{0:N}"/>
         
           <asp:TemplateField HeaderText="Details">
        <ItemTemplate>
             <asp:ImageButton ID="imgBtnEdt" CommandName="edt" CommandArgument='<%#Eval("CustomerId") %>' CausesValidation="false" ToolTip="View Item" runat="server"  ImageUrl="~/img/view_icon.png" />
         </ItemTemplate>
         </asp:TemplateField>
           
      </Columns>
       <SelectedRowStyle BackColor="#E0D9BD" />
    </asp:GridView>
</div>
</div>
</div>
</div>
</asp:Content>
