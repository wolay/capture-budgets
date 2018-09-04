<%@ Page Title="" Language="C#" MasterPageFile="~/SiteGSS.Master" AutoEventWireup="true" CodeBehind="ManageGssCustomer.aspx.cs" Inherits="BudgetCapture.Admin.ManageGssCustomer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row-fluid">
     <div class=" span12 pageheader-text">Manage Customer Page</div>
     <div class="alert alert-error" runat="server" id="error" visible="false">
      <button type="button" class="close" data-dismiss="alert">&times;</button>
     </div>
    <div class="alert alert-success" runat="server" id="success" visible="false">
      <button type="button" class="close" data-dismiss="alert">&times;</button>
  </div>
  </div>
      <%--<h4 class="input-heading">Search Criterias :</h4>--%>
    <div class="row-fluid">
<div class="page-body span12">
  <div class="page-body-wrapper">
  <div class="inner-header">Search Criterias</div>
    <div class="form-horizontal row-fluid">
        <div class="page-body span6">
    <%--<div class="nav-header" runat="server" id="dvMsg">Add New Pipeline Section :</div>--%>
        <div class="control-group" runat="server" id="dvID">
            <label class="control-label" for="txtStaffID">Customer ID:<span>*</span>:</label>
            <div class="controls">
                <asp:TextBox ID="txtStaffID" runat="server"  CssClass=""></asp:TextBox>
            </div>
        </div>
         
        <div class="control-group">
        <label class="control-label" for="txtStaffID">Customer Name:</label>
        <div class="controls">
            <asp:TextBox ID="txtfName" runat="server" CssClass=""></asp:TextBox>
             </div>
        </div>
       
             <div class="control-group">
        <label class="control-label" for="btnSubmit"></label>
        <div class="controls">
          <asp:Button ID="Button1" runat="server" Text="Search" 
                CssClass="btn btn-submit" onclick="btnSubmit_Click" />
            &nbsp;&nbsp;
            <asp:Button ID="btnAll" runat="server" Text="View All" 
                CssClass="btn btn-submit" OnClick="btnAll_Click"/>
         </div>
        </div>
            </div>
        <div class="page-body span6">
        
      <div class="control-group">
        <label class="control-label" for="txtStaffID">Customer Type:</label>
        <div class="controls">
            <asp:DropDownList ID="ddlType" runat="server"   AppendDataBoundItems="true" Width="200px">
           <asp:ListItem Value="" Selected="True">..Filter Customer Type..</asp:ListItem>
             </asp:DropDownList>
              
        </div>
        </div>
       <div class="control-group">
        <label class="control-label" for="txtStaffID">Pipeline Section:</label>
        <div class="controls">
            <asp:DropDownList ID="ddlSection" runat="server" AppendDataBoundItems="true" Width="200px">
             <asp:ListItem Value="" Selected="True">..Filter Pipeline Section..</asp:ListItem>
           </asp:DropDownList>
              
        </div>
        </div>
      </div>
  </div>
</div>
<div class="row-fluid">
   <div class="page-body-control span12">
 <div class="inner-header">List of Customers
 </div>
       <asp:HyperLink ID="lnkCreate" runat="server" NavigateUrl="~/Admin/SetupCustomer.aspx">Create New Customer</asp:HyperLink>
    <asp:HiddenField ID="hid" runat="server" />
    <asp:GridView ID="gvDept" runat="server" GridLines="None"  Font-Size="11px" EmptyDataText="No record found" 
        AutoGenerateColumns="false" CssClass="table table-striped" DataKeyNames="ID" ShowFooter="true"
           AllowPaging="true" PageIndex="0" PageSize="80" OnRowCommand="gvDept_RowCommand"
           onpageindexchanging="gvDept_PageIndexChanging"  
           onrowdatabound="gvDept_RowDataBound">
      <Columns>
        <asp:BoundField HeaderText="#" DataField="ID" Visible="false" />
        <asp:TemplateField HeaderText="Customer Type">
             <ItemTemplate>
              <asp:Label ID="lbRecID" runat="server" Text='<%#Eval("ID") %>' Visible="false"></asp:Label>
              <asp:Label ID="lbloc1" runat="server" Text='<%#Eval("CustomerType.Name") %>'></asp:Label>
             </ItemTemplate>
           </asp:TemplateField>
            <asp:TemplateField HeaderText="Customer Name">
            <ItemTemplate>
                 <asp:Label ID="lbCompany" runat="server" Text='<%#Eval("CompanyName")%>'></asp:Label>
             </ItemTemplate>
          </asp:TemplateField>
          <asp:TemplateField HeaderText="Contact Person">
            <ItemTemplate>
                 <asp:Label ID="lbAsst" runat="server" Text='<%#Eval("ContactPerson") %>'></asp:Label>
             </ItemTemplate>
          </asp:TemplateField>
           
           <asp:BoundField HeaderText="Contact PhoneNo" DataField="MobileNo"/>  

           <asp:TemplateField HeaderText="Unit Price(N)">
            <ItemTemplate>
                 <asp:Label ID="lbAmt" runat="server" Text='<%#Eval("UnitPrice","{0:N}") %>'></asp:Label>
             </ItemTemplate>
               
          </asp:TemplateField>
          <asp:TemplateField HeaderText="Payment Term">
             <ItemTemplate>
                 <asp:Label ID="lblJust" runat="server" Text='<%#GetPaymentTerm(Eval("PaymentTermFlg")) %>'></asp:Label>
             </ItemTemplate>
           </asp:TemplateField>
          <asp:TemplateField HeaderText="Pipeline Section">
             <ItemTemplate>
                  <asp:Label ID="lbatcflg" runat="server" Text='<%#Eval("GSS_PipelineSection.Name") %>'></asp:Label>
             </ItemTemplate>
           </asp:TemplateField>
        
           <asp:TemplateField HeaderText="Status">
              <ItemTemplate>
                 <asp:Label ID="lblatc" runat="server" Text='<%# (Eval("isActive") != null&&Boolean.Parse(Eval("isActive").ToString())) ? "Active" : "Inactive"%>' ForeColor="Maroon"></asp:Label>
              </ItemTemplate>
             </asp:TemplateField>
         <%--<asp:TemplateField>
            <HeaderTemplate>
                <asp:CheckBox ID="chkHeader" Text="All" runat="server" visible="false"/>
            </HeaderTemplate>
            <ItemTemplate>
                <asp:CheckBox ID="chkRow" runat="server" Visible="false"/>
                  <asp:Label ID="lbDeptID" runat="server" Text='<%#Eval("Department.ID") %>' Visible="false"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>--%>
        <asp:TemplateField>
        <ItemTemplate>
            <asp:ImageButton ID="imgBtnEdit" CommandName="edt" CommandArgument='<%#Eval("ID") %>' CausesValidation="false" ToolTip="Edit Item" Visible="false" runat="server" ImageUrl="~/img/edit_icon.png" />
         </ItemTemplate>
         </asp:TemplateField>
          <asp:TemplateField>
        <ItemTemplate>
             <asp:ImageButton ID="imgBtnDel" CommandName="View" CommandArgument='<%#Eval("ID") %>' CausesValidation="false" ToolTip="View Item" runat="server" Visible="true" ImageUrl="~/img/view_icon.png" />
         </ItemTemplate>
         </asp:TemplateField>
      </Columns>
       <SelectedRowStyle BackColor="#E0D9BD" />
      <FooterStyle BackColor="#E0D9BD" Font-Bold="true" ForeColor="Maroon" Font-Size="13px" />
    </asp:GridView>
</div>
</div>
</div>
</div>
 

</asp:Content>
