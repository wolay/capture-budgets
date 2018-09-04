<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageBudgetYr.aspx.cs" Inherits="BudgetCapture.Admin.ManageBudgetYr" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="row-fluid">
     <div class=" span12 pageheader-text">Manage Budget Year Page</div>
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
  <div class="inner-header">Setup Budget Year</div>
    <div class="form-horizontal">
    <div class="nav-header" runat="server" id="dvMsg">Add New Budget Year :</div>
        <div class="control-group" runat="server" id="dvID" visible="false">
            <label class="control-label" for="txtStaffID">ID:<span>*</span>:</label>
            <div class="controls">
                <asp:TextBox ID="txtID" runat="server" ReadOnly="true" CssClass=""></asp:TextBox>
            </div>
        </div>
        <div class="control-group">
        <label class="control-label" for="txtStaffID">Year<span>*</span>:</label>
        <div class="controls">
            <asp:TextBox ID="txtDept" runat="server" CssClass=""></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator15"  runat="server" ForeColor=Maroon ControlToValidate="txtDept" Display="Dynamic" Text="Required!"></asp:RequiredFieldValidator>  
        </div>
        </div>
         <div class="control-group">
               <label class="control-label" for="">Start Date:<span>*</span>:</label>
                <div class="controls">
                     <asp:TextBox ID="txtDate" runat="server"  CssClass=""></asp:TextBox>&nbsp;
                      <asp:Image ID="Image1" runat="server" ImageUrl="~/img/cal.gif" /><br />
                      <small>format(dd/MM/yyyy)</small> 
                 <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupPosition="Right" Format="dd/MM/yyyy" TargetControlID="txtDate" PopupButtonID="Image1"></asp:CalendarExtender> 
                 <asp:RequiredFieldValidator ID="RequiredFieldValidator4"  runat="server" ForeColor=Maroon ControlToValidate="txtDate" Display="Dynamic" Text="Required!!"></asp:RequiredFieldValidator>  
                 <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="" ControlToValidate="txtDate" Display="Dynamic" Text="Enter valid date(dd/MM/yyyy)" ValidationExpression="((((0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/])?)?(19|20)\d\d(\s+)?)+"   ></asp:RegularExpressionValidator>
                </div>
           </div>
            <div class="control-group">
               <label class="control-label" for="">End Date:<span>*</span>:</label>
                <div class="controls">
                     <asp:TextBox ID="txtEDate" runat="server"  CssClass=""></asp:TextBox>&nbsp;
                      <asp:Image ID="Image2" runat="server" ImageUrl="~/img/cal.gif" /><br />
                      <small>format(dd/MM/yyyy)</small> 
                 <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupPosition="Right" Format="dd/MM/yyyy" TargetControlID="txtEDate" PopupButtonID="Image2"></asp:CalendarExtender> 
                 <asp:RequiredFieldValidator ID="RequiredFieldValidator1"  runat="server" ForeColor=Maroon ControlToValidate="txtEDate" Display="Dynamic" Text="Required!!"></asp:RequiredFieldValidator>  
                 <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="" ControlToValidate="txtEDate" Display="Dynamic" Text="Enter valid date(dd/MM/yyyy)" ValidationExpression="((((0[1-9]|[12][0-9]|3[01])[/](0[1-9]|1[012])[/])?)?(19|20)\d\d(\s+)?)+"   ></asp:RegularExpressionValidator>
                </div>
           </div>
        <div class="control-group">
        <label class="control-label" for="txtStaffID">isActive<span>*</span>:</label>
        <div class="controls">
            <asp:CheckBox ID="chkActive" runat="server" />
             <small>Only 1 budget year can be active per time</small>  
        </div>
        </div>
       <div class="control-group">
        <label class="control-label" for="btnSubmit"></label>
        <div class="controls">
          <asp:Button ID="btnSubmit" runat="server" Text="Submit" 
                CssClass="btn btn-submit" onclick="btnSubmit_Click" />
         </div>
        </div>
  </div>
</div>
<div class="row-fluid">
   <div class="page-body-control span12">
 <div class="inner-header">List of Budget Year</div>
    <asp:HiddenField ID="hid" runat="server" />
    <asp:GridView ID="gvDir" runat="server" GridLines="None" Font-Size="11px" 
        AutoGenerateColumns="false" CssClass="table table-striped" DataKeyNames="ID" 
           AllowPaging="true" PageIndex="0" PageSize="15" EmptyDataText="No Record Found"
          onselectedindexchanged="gvDir_SelectedIndexChanged" 
           onpageindexchanging="gvDir_PageIndexChanging">
      <Columns>
        <asp:TemplateField HeaderText="ID" Visible="false">
             <ItemTemplate>
             <asp:Label ID="lbID" runat="server" Text='<%#Eval("ID") %>' Visible="false"></asp:Label>
             </ItemTemplate>
           </asp:TemplateField>
        <asp:BoundField HeaderText="Year" DataField="Year" />
       <asp:BoundField HeaderText="Start Date" DataField="StartDate" DataFormatString="{0:dd-MM-yyyy}" />
       <asp:BoundField HeaderText="End Date" DataField="EndDate" DataFormatString="{0:dd-MM-yyyy}" />
       <asp:TemplateField HeaderText="isActive">
           <ItemTemplate>
            <asp:CheckBox ID="cbCheckBox" runat="server" Enabled="false" checked='<%# Convert.ToBoolean(Eval("IsActive")) %>'/>
           </ItemTemplate>
        </asp:TemplateField>
        <asp:CommandField ButtonType="Image" HeaderText="Edit" ShowSelectButton="true" SelectImageUrl="../img/edit_icon.png"   />
      </Columns>
       <SelectedRowStyle BackColor="#E0D9BD" />
    </asp:GridView>
</div>
</div>
</div>
</div>
</asp:Content>
