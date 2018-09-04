<%@ Page Title="ManageDepartment" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageDept.aspx.cs" Inherits="BudgetCapture.Admin.ManageDept" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row-fluid">
     <div class=" span12 pageheader-text">Manage Department Page</div>
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
  <div class="inner-header">Setup Department</div>
    <div class="form-horizontal">
    <div class="nav-header" runat="server" id="dvMsg">Add New Department :</div>
        <div class="control-group" runat="server" id="dvID" visible="false">
            <label class="control-label" for="txtStaffID">Department ID:<span>*</span>:</label>
            <div class="controls">
                <asp:TextBox ID="txtID" runat="server" ReadOnly="true" CssClass=""></asp:TextBox>
            </div>
        </div>
        <%--<div class="control-group">
        <label class="control-label" for="txtStaffID">Department Code<span>*</span>:</label>
        <div class="controls">
            <asp:TextBox ID="txtCode" runat="server" CssClass=""></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2"  runat="server" ForeColor=Maroon ControlToValidate="txtCode" Display="Dynamic" Text="Required!"></asp:RequiredFieldValidator>
        </div>
        </div>--%>
        <div class="control-group">
        <label class="control-label" for="txtStaffID">Department Name<span>*</span>:</label>
        <div class="controls">
            <asp:TextBox ID="txtDept" runat="server" CssClass=""></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator15"  runat="server" ForeColor=Maroon ControlToValidate="txtDept" Display="Dynamic" Text="Required!"></asp:RequiredFieldValidator>
        </div>
        </div>
        <div class="control-group">
        <label class="control-label" for="txtStaffID">Directorate:<span>*</span>:</label>
        <div class="controls">
            <asp:DropDownList ID="ddlDir" runat="server" AppendDataBoundItems="true">
             <asp:ListItem Value="">...Select Directorate...</asp:ListItem>
            </asp:DropDownList>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" InitialValue=""  runat="server" ForeColor=Maroon ControlToValidate="ddlDir" Display="Dynamic" Text="Required!"></asp:RequiredFieldValidator>  
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
   <div class="page-body-control span9">
 <div class="inner-header">List of Department</div>
    <asp:HiddenField ID="hid" runat="server" /><asp:HiddenField ID="hidbudget" runat="server" />
    <asp:GridView ID="gvDept" runat="server" GridLines="None"  Font-Size="11px" 
        AutoGenerateColumns="false" CssClass="table table-striped" DataKeyNames="ID" 
           AllowPaging="true" PageIndex="0" PageSize="50"
          onselectedindexchanged="gvDept_SelectedIndexChanged" 
           onpageindexchanging="gvDept_PageIndexChanging"  onrowcommand="gvDept_RowCommand">
      <Columns>
        <asp:BoundField HeaderText="#" DataField="ID" Visible=false />
        <asp:BoundField HeaderText="Code" DataField="Code" Visible="false"/>
        <asp:BoundField HeaderText="Department Name" DataField="Name" />
        
        <asp:TemplateField HeaderText="Directorate">
             <ItemTemplate>
             <asp:Label ID="lbID" runat="server" Text='<%#Eval("ID") %>' Visible="false"></asp:Label>
                  <asp:Label ID="lbDir" runat="server" Text='<%#Eval("DirectorateID") %>' Visible="false"></asp:Label>
                 <asp:Label ID="lbloc1" runat="server" Text='<%#Eval("Directorate.Name") %>'></asp:Label>
             </ItemTemplate>
           </asp:TemplateField>
        <asp:CommandField ButtonType="Image" HeaderText="Edit" ShowSelectButton="true" SelectImageUrl="../img/edit_icon.png"   />
         <asp:ButtonField ButtonType="Image"  CommandName="del" HeaderText="BudgetType" ImageUrl="../img/view_icon.png" />
      </Columns>
       <SelectedRowStyle BackColor="#E0D9BD" />
    </asp:GridView>
</div>
    <div  class="page-body-control span3" runat="server" id="bType" visible="false">
        <div class="inner-header">Budget Type</div>
        Department: <asp:Label ID="lbDept" runat="server" Text="" ForeColor="Maroon" Font-Size="10px"></asp:Label>
        <asp:CheckBoxList ID="chkBudgetType" AutoPostBack="false" runat="server" CssClass="budgetItem" Font-Size="10px" OnSelectedIndexChanged="chkBudgetType_SelectedIndexChanged" >
            
        </asp:CheckBoxList>
        <asp:LinkButton ID="lnkBtn" runat="server" Visible="false" OnClick="lnkBtn_Click" CausesValidation="false"
            >Update</asp:LinkButton>
    </div>
</div>
</div>
</div>
</asp:Content>
