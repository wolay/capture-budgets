<%@ Page Title="" Language="C#" MasterPageFile="~/SiteGSS.Master" AutoEventWireup="true" CodeBehind="ApproveCertifiedVolume.aspx.cs" Inherits="BudgetCapture.GSS.ApproveCertifiedVolume" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row-fluid">
     <div class=" span12 pageheader-text">Unapproved Certified Sales Data</div>
     <div class="alert alert-error" runat="server" id="error" visible="false">
      <button type="button" class="close" data-dismiss="alert">&times;</button>
     </div>
    <div class="alert alert-success" runat="server" id="success" visible="false">
      <button type="button" class="close" data-dismiss="alert">&times;</button>
  </div>
  </div>
     <div class="row-fluid">
<div class="page-body span12">
   
<div class="row-fluid">
   <div class="page-body-control span12">
 <div class="inner-header">Unapproved Captured Sales Data
     <div class="form-horizontal" style="float:right;margin-top:-27px">
            <div class="controls-group">
            <div class="controls">
             <asp:DropDownList ID="ddlCustFilter"  Width="140px" runat="server" AppendDataBoundItems="true" Font-Size="11px">
             <asp:ListItem Value="" Selected="True">...Filter By Customer...</asp:ListItem>
            </asp:DropDownList>
            <asp:TextBox ID="txtFilterDate" runat="server" CssClass="" Width="120px"></asp:TextBox>
           <asp:Image ID="Image1" runat="server" ImageUrl="~/img/cal.gif" />
           <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupPosition="Right" Format="dd/MM/yyyy" TargetControlID="txtFilterDate" PopupButtonID="Image1"></asp:CalendarExtender> 
                <asp:Button ID="btnSearch" runat="server" Text="Go" CausesValidation="false" CssClass="btn" ValidationGroup="" 
                    onclick="btnSearch_Click"/>  <asp:Button ID="btnClr" runat="server" 
                    Text="Clr" CssClass="btn" onclick="btnClr_Click" CausesValidation="false" ValidationGroup="" 
                    /></div>
              </div>
              
          </div>
 </div>
    <asp:HiddenField ID="hid" runat="server" />
    <asp:GridView ID="gvCommon" runat="server" GridLines="None"  Font-Size="11px" EmptyDataText="No record found" 
        AutoGenerateColumns="false" CssClass="table table-striped" DataKeyNames="ID" 
           AllowPaging="true" PageIndex="0" PageSize="50" 
           onpageindexchanging="gvCommon_PageIndexChanging" OnRowDataBound="gvCommon_RowDataBound">                                                                                                        
      <Columns>
        <asp:BoundField HeaderText="#" DataField="ID" Visible=false />
        
        <asp:TemplateField HeaderText="Customer Name">
            <ItemTemplate>
                <asp:Label ID="lbRecID" runat="server" Text='<%#Eval("ID") %>' Visible="false"></asp:Label>
                 <asp:Label ID="lbCompany" runat="server" Text='<%#Eval("GSS_Customer.CompanyName")%>'></asp:Label>
             </ItemTemplate>
          </asp:TemplateField>
        <asp:BoundField HeaderText="Captured Volume" DataField="CapturedVolumeSale" DataFormatString="{0:N}"/>
        <asp:BoundField HeaderText="Date Captured" DataField="DateCaptured" DataFormatString="{0:dd-MMM-yyyy}"/>
        <asp:BoundField HeaderText="Certified Volume" DataField="CertifiedVolumeSale" DataFormatString="{0:N}"/>
        <asp:BoundField HeaderText="Date Certified" DataField="DateCertified" DataFormatString="{0:dd-MMM-yyyy}"/>
          <asp:TemplateField HeaderText="Certified By">
            <ItemTemplate>
                 <asp:Label ID="lbfName" runat="server" Text='<%#Eval("AppUser2.FullName")%>'></asp:Label>
             </ItemTemplate>
          </asp:TemplateField>
           <asp:TemplateField HeaderText="Status">
              <ItemTemplate>
                 <asp:Label ID="lbStatus" runat="server" Visible="false" Text='<%#Eval("Status") %>'></asp:Label>
                  <asp:Label ID="lbSt" runat="server" ForeColor=Maroon Text='<%#GetStatus(Eval("Status")) %>'></asp:Label>
              </ItemTemplate>
             </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <asp:CheckBox ID="chkHeader" Text="All" runat="server" visible="false"/>
            </HeaderTemplate>
            <ItemTemplate>
                <asp:CheckBox ID="chkRow" runat="server" Visible="false"/>
            </ItemTemplate>
        </asp:TemplateField>
      </Columns>
       <SelectedRowStyle BackColor="#E0D9BD" />
    </asp:GridView>
       <div class="row-fluid" runat="server" id="dvAppr" visible="false">
<div class="page-body-controlbtn">
 <h3>Approval Action</h3>
    <asp:Button ID="btnApprv" CausesValidation="false" runat="server" Text="Approve Selected Record(s)" ToolTip="Check all rows to be approved and click to approve"
     CssClass="btn btn-submit"   onclick="btnApprv_Click" />
     <%-- <asp:Button ID="btnAppr" runat="server" Text="" visible="true" ToolTip="Check all rows to be approved and click to approve"
                CssClass="btn btn-submit" onclick="btnAppr1_Click" />--%>      
   <asp:Button ID="btnReject" runat="server" CausesValidation="false" Text="Reject Selected Record(s)" visible="true" ToolTip="Check all rows to be rejectd and click to reject"
                CssClass="btn btn-submit btn-reject" OnClick="btnReject_Click"/>         
 </div>
 
</div>
</div>
</div>
</div>
</div>
</asp:Content>
