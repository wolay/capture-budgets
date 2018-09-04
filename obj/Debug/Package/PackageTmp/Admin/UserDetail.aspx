<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserDetail.aspx.cs" Inherits="BudgetCapture.Admin.UserDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row-fluid">
     <div class=" span12 pageheader-text">Back Office Users Detail Page</div>
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
  <div class="inner-header">Uses Detail</div>
   <asp:HiddenField ID="hid" runat="server" />
       <asp:GridView ID="gvheader" runat="server" Width="100%" ShowHeader="false" EnableTheming="false" EmptyDataText="No Record found" GridLines="None" 
        AutoGenerateColumns="False"> 
        <Columns>
             <asp:TemplateField>
              <ItemTemplate>
               <div style="padding-bottom: 10px; font-size:12px" class="boxheader">
              <%-- <h3>User Detail</h3>--%>
               <asp:Label ID="lbInit" runat="server" Text='<%# Eval("StaffID")%>' Visible="false" />
               <table border="0" class="table-detail">
                <tr>
                <td class="detail-title">Staff ID:</td><td class="nav-header"><b><%#Eval("StaffID")%></b></td>
                <td class="detail-title">Full Name:</td><td class="nav-header"><%# Eval("FullName")%></td>
                </tr>
                 <tr>
                <td class="detail-title">Department:</td> <td class="nav-header"><%# Eval("Department.Name")%>  <asp:Label ID="lbDept" runat="server" Text=' <%# Eval("DepartmentID")%>' Visible="false"></asp:Label></td>
                <td class="detail-title">Email:</td><td class="nav-header"><%#Eval("Email")%></td
                </tr>
                <tr>
                <td class="detail-title">Added By:</td><td class="nav-header"><%# Eval("AddedBy")%></td>
                 <td class="detail-title">Date Added:</td><td class="nav-header"><%#Eval("DateAdded","{0:dd-MM-yyyy hh:mm:ss tt}")%></td>
                </tr>
                <tr>
                  <td class="detail-title"> User Status:</td><td class="nav-header">
                   <asp:Label ID="lbEbl" runat="server" Text=' <%# Eval("isActive")%>' Visible="false"></asp:Label>
                  <asp:Label ID="lbEnable" runat="server" Text=' <%# (Eval("isActive") != null&&Boolean.Parse(Eval("isActive").ToString())) ? "Active" : "Inactive"%>' ForeColor="Maroon"></asp:Label></td>
                  <td class="detail-title">User Role:</td><td class="nav-header"style="color:Maroon"><asp:Label ID="lbrole" runat="server" Text= '<%# Eval("UserRole")%>'></asp:Label></td>
                </tr>
                <tr>
                 <td class="detail-title"> Directorate:</td><td class="nav-header">
                   <asp:Label ID="lbLoc1" runat="server" Text=' <%# Eval("Department.Directorate.Name")%>'></asp:Label></td>
                   <td></td>
                </tr>
               </table>
               </div>
                 </ItemTemplate>
             </asp:TemplateField>
        </Columns>
    </asp:GridView>

  
    </div>
    
    <div class="page-body-control">
        <div class="form-horizontal">
       <div class="nav-header">Update User :</div>
       <div class="control-group">
            <label class="control-label" for="txtPwd">Select User Role: </label>
                <div class="controls">
                <asp:DropDownList Id="ddlRole" runat="server" CssClass="" ValidationGroup="cat">
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
    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator5" ValidationGroup="cat"  InitialValue="" runat="server" ForeColor=Maroon ControlToValidate="ddlRole" Display="Dynamic" Text="Required!!!"></asp:RequiredFieldValidator>--%>
    </div>
  </div>
   <div class="control-group">
   <label class="control-label" for="txtPwd">User Department:<span class="required"></span></label>
   <div class="controls">
    <asp:DropDownList Id="ddlDept" runat="server" CssClass="" 
           AppendDataBoundItems="true" Enabled="true">
     <asp:ListItem Value="" Selected="True">..Select User Department..</asp:ListItem>
     </asp:DropDownList>
     <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="cat" InitialValue="" runat="server" ForeColor=Maroon ControlToValidate="ddlDept" Display="Dynamic" Text="Required!!!"></asp:RequiredFieldValidator>  --%>
    </div>
  </div>
   
  <div class="control-group">
   <label class="control-label" for="txtPwd"> </label>
   <div class="controls">
        <asp:Button ID="btnUpdate" runat="server" Text="Update User" ValidationGroup="cat"
            CssClass="btn btn-submit" onclick="btnUpdate_Click" />
             <asp:Button ID="btnSubmit" runat="server" Text="Enable/Disable" CausesValidation="false"
            CssClass="btn btn-submit" onclick="btnSubmit_Click"/>
    </div>
  </div>
     </div>
     
    </div>

 </div>
 </div>
</asp:Content>
