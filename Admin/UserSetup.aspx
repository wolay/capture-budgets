<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserSetup.aspx.cs" Inherits="BudgetCapture.Admin.UserSetup" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row-fluid">
 <span class="span12 pageheader-text">Back Office User SetUp</span>
     <div class="alert alert-error" runat="server" id="error" visible="false">
      <button type="button" class="close" data-dismiss="alert">&times;</button>
    </div>
    <div class="alert alert-success" runat="server" id="success" visible="false">
      <button type="button" class="close" data-dismiss="alert">&times;</button>
    </div>
</div>
<div class="row-fluid">
 <div class="page-body span10">
<div class="form-horizontal form-container">
<div class="nav-header">User Basic Info :</div>
  <div class="control-group">
    <label class="control-label" for="txtStaffID">Staff ID:<span class="required">*</span></label>
    <div class="controls">
        <asp:TextBox ID="txtStaffID" runat="server" ></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="cat" runat="server" ForeColor=Maroon ControlToValidate="txtStaffID" Display="Dynamic" Text="Required!!!"></asp:RequiredFieldValidator>  
        <asp:Button ID="btnLoad" runat="server" Text="Load User" 
            CssClass="btn btn-submit" onclick="btnLoad_Click" />
    </div>
  </div>
  <div runat="server" visible="false" id="dvDetail">
  <div class="control-group">
    <label class="control-label" for="txtfName">Full Name:<span class="required">*</span></label>
    <div class="controls">
        <asp:TextBox ID="txtfName" runat="server" CssClass="" Enabled="false" ></asp:TextBox>
         <asp:RequiredFieldValidator ID="RequiredFieldValidator15" ValidationGroup="cat" runat="server" ForeColor=Maroon ControlToValidate="txtfName" Display="Dynamic" Text="Required!!!"></asp:RequiredFieldValidator>  
    </div>
  </div>
  <div class="control-group">
   <label class="control-label" for="txtEmail">Email: </label>
    <div class="controls">
      <asp:TextBox ID="txtEmail" runat="server" CssClass="" Enabled="true"   ></asp:TextBox>
       <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator14" ValidationGroup="cat" runat="server" ForeColor=Maroon ControlToValidate="txtEmail" Display="Dynamic" Text="Required!!!"></asp:RequiredFieldValidator>  
       <asp:RegularExpressionValidator ID="RegEmail" runat="server" ForeColor=Maroon  ErrorMessage="Invalid Email" ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="cat"></asp:RegularExpressionValidator>--%>
    </div>
  </div>
   <div class="control-group">
    <label class="control-label" for="txtfName">Directorate: </label>
    <div class="controls">
         <asp:DropDownList Id="ddlDir" runat="server" CssClass="" 
           AppendDataBoundItems="true" Enabled="true">
     <asp:ListItem Value="" Selected="True">..Select User Directorate..</asp:ListItem>
     </asp:DropDownList>
     <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="cat" InitialValue="" runat="server" ForeColor=Maroon ControlToValidate="ddlDir" Display="Dynamic" Text="Required!!!"></asp:RequiredFieldValidator>  
    </div>
  </div>
   <div class="control-group">
   <label class="control-label" for="txtPwd">User Department:<span class="required">*</span></label>
   <div class="controls">
    <asp:DropDownList Id="ddlDept" runat="server" CssClass="" 
           AppendDataBoundItems="true" Enabled="true">
     <asp:ListItem Value="" Selected="True">..Select User Department..</asp:ListItem>
     </asp:DropDownList>
     <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="cat" InitialValue="" runat="server" ForeColor=Maroon ControlToValidate="ddlDept" Display="Dynamic" Text="Required!!!"></asp:RequiredFieldValidator>  
    </div>
  </div>
  <div class="control-group">
   <label class="control-label" for="txtPwd">Select User Role:<span class="required">*</span></label>
   <div class="controls">
    <asp:DropDownList Id="ddlRole" runat="server" CssClass="" AutoPostBack="true">
     <asp:ListItem Value="" Selected="True">..Select User Role..</asp:ListItem>
     <asp:ListItem Value="Administrator">Administrator</asp:ListItem>
        <asp:ListItem Value="GssAdmin">GSS Admin</asp:ListItem>
      <asp:ListItem Value="BudgetInitiator">Budget Initiator</asp:ListItem>
      <asp:ListItem Value="BudgetApprover">Budget Approver</asp:ListItem>

      <asp:ListItem Value="PB-Officer">P&B Officer</asp:ListItem>
      <asp:ListItem Value="PB-Manager">P&B Manager</asp:ListItem>
     
      <asp:ListItem Value="MD">MD</asp:ListItem>
      <asp:ListItem Value="ED">ED</asp:ListItem>
      <asp:ListItem Value="Commercial-Officer">Commercial-Officer</asp:ListItem>
      <asp:ListItem Value="Commercial-Supervisor">Commercial-Supervisor</asp:ListItem>
      <asp:ListItem Value="WOD-Officer">WOD-Officer</asp:ListItem>
      <asp:ListItem Value="WOD-Supervisor">WOD-Supervisor</asp:ListItem>
    </asp:DropDownList>
     <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ValidationGroup="cat" InitialValue="" runat="server" ForeColor=Maroon ControlToValidate="ddlRole" Display="Dynamic" Text="Required!!!"></asp:RequiredFieldValidator>  
    </div>
  </div>
    
   <div class="control-group">
    <label class="control-label" for="txtPwd"></label>
    <div class="controls">
        <asp:Button ID="btnSubmit" runat="server" Text="Add User" ValidationGroup="cat"
            CssClass="btn btn-submit" onclick="btnSubmit_Click" />
     
    </div>
  </div>
  </div>
</div>
</div>

</div>
<div class="row-fluid">
  <div class="page-body span12">
      <div class="page-body-control">
          <div class="inner-header search-header">List of Users
          <div class="form-horizontal" style="float:right;margin-top:-5px">
            <div class="controls-group">
            <div class="controls">
                  <input type="text" runat="server" id="txtSea" placeholder="Search" />
                <asp:Button ID="btnSearch" runat="server" Text="Go" CssClass="btn" 
                    onclick="btnSearch_Click"/>  <asp:Button ID="btnClr" runat="server" 
                    Text="Clr" CssClass="btn" onclick="btnClr_Click" 
                    /></div>
              </div>
              
          </div>
          </div>
         <asp:GridView ID="gvUsers" runat="server" CssClass="table table-striped" 
            DataKeyNames="ID" HorizontalAlign="Center"
        GridLines="None" AutoGenerateColumns=false Font-Size="11px"
            onselectedindexchanged="gvUsers_SelectedIndexChanged">
          <Columns>
           <asp:BoundField DataField="StaffID" HeaderText="Staff ID" />
           <asp:BoundField DataField="FullName" HeaderText="FullName" />
            <asp:TemplateField HeaderText="Department">
             <ItemTemplate>
               <asp:Label ID="lbloc" runat="server" Text='<%#Eval("Department.Name") %>'></asp:Label>
             </ItemTemplate>
           </asp:TemplateField>
           <asp:TemplateField HeaderText="Directorate">
             <ItemTemplate>
                 <asp:Label ID="lbloc1" runat="server" Text='<%#Eval("Department.Directorate.Name") %>'></asp:Label>
             </ItemTemplate>
           </asp:TemplateField>
           <%--<asp:BoundField DataField="isHOD" HeaderText="IsHOD" />--%>
           <asp:BoundField DataField="UserRole" HeaderText="User Role" />
           <asp:BoundField DataField="DateAdded" HeaderText="Date Created" DataFormatString="{0:dd-MM-yyyy hh:mm:ss tt}" />
          
           <asp:CommandField ButtonType="Image" HeaderText="View Detail" ShowSelectButton="true" SelectImageUrl="../img/view_icon.png"   />
          </Columns>
        </asp:GridView>
      </div>
  </div>
</div>
</asp:Content>
