<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageSalaryCategory.aspx.cs" Inherits="BudgetCapture.Admin.ManageSalaryCategory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row-fluid">
     <div class=" span12 pageheader-text">Manage Salary Category Page</div>
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
  <div class="inner-header">Setup Categoty</div>
    <div class="form-horizontal">
    <div class="nav-header" runat="server" id="dvMsg">Add New Category :</div>
        <div class="control-group" runat="server" id="dvID" visible="false">
            <label class="control-label" for="txtStaffID">ID:<span>*</span>:</label>
            <div class="controls">
                <asp:TextBox ID="txtID" runat="server" ReadOnly="true" CssClass=""></asp:TextBox>
            </div>
        </div>
        <div class="control-group">
        <label class="control-label" for="txtStaffID">Category<span>*</span>:</label>
        <div class="controls">
            <asp:TextBox ID="txtDept" runat="server" CssClass=""></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator15"  runat="server" ForeColor=Maroon ControlToValidate="txtDept" Display="Dynamic" Text="Required!"></asp:RequiredFieldValidator>  
        </div>
        </div>
        <div class="control-group">
        <label class="control-label" for="txtStaffID">IsActive<span>*</span>:</label>
        <div class="controls">
            <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3"  runat="server" ForeColor=Maroon ControlToValidate="chkActive" Display="Dynamic" Text="Required!"></asp:RequiredFieldValidator>  --%>
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
 <div class="inner-header">List of Category</div>
    <asp:HiddenField ID="hid" runat="server" />
    <asp:GridView ID="gvDir" runat="server" GridLines="None" Font-Size="11px" 
        AutoGenerateColumns="false" CssClass="table table-striped" DataKeyNames="ID" 
           AllowPaging="true" PageIndex="0" PageSize="15" EmptyDataText="No Record Found"
          onselectedindexchanged="gvDir_SelectedIndexChanged" 
           onpageindexchanging="gvDir_PageIndexChanging">
      <Columns>
        <asp:BoundField HeaderText="#" DataField="ID" />
        <asp:BoundField HeaderText="Name" DataField="Name" />
       <%-- <asp:BoundField HeaderText="Cost Per Month(=N=)" DataField="CostPerMonth" DataFormatString="{0:N}" />
        <asp:BoundField HeaderText="Cost Per Year(=N=)" DataField="CostPerYear" DataFormatString="{0:N}" />--%>
        <asp:TemplateField HeaderText="isActive">
           <ItemTemplate>
            <asp:CheckBox ID="cbCheckBox" runat="server" Enabled="false" checked='<%# Convert.ToBoolean(Eval("isActive")) %>'/>
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
