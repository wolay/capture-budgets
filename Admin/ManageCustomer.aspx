<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageCustomer.aspx.cs" Inherits="BudgetCapture.Admin.ManageCustomer" %>
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
  <div class="row-fluid">
<div class="page-body span12">
  <div class="page-body-wrapper">
  <div class="inner-header">Setup Customer</div>
    <div class="form-horizontal">
    <div class="nav-header" runat="server" id="dvMsg">Add New Customer :</div>
        <div class="control-group" runat="server" id="dvID" visible="false">
            <label class="control-label" for="txtStaffID">ID:<span>*</span>:</label>
            <div class="controls">
                <asp:TextBox ID="txtID" runat="server" ReadOnly="true" CssClass=""></asp:TextBox>
            </div>
        </div>
            <div class="control-group">
        <label class="control-label" for="txtStaffID">Customer Type:<span>*</span>:</label>
        <div class="controls">
            <asp:DropDownList ID="ddlDir" runat="server" AppendDataBoundItems="true">
             <asp:ListItem Value="">...Select CustomerType...</asp:ListItem>
            </asp:DropDownList>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" InitialValue=""  runat="server" ForeColor=Maroon ControlToValidate="ddlDir" Display="Dynamic" Text="Required!"></asp:RequiredFieldValidator>  
        </div>
        </div>
        <div class="control-group">
        <label class="control-label" for="txtStaffID">Code<span>*</span>:</label>
        <div class="controls">
            <asp:TextBox ID="txtAgr" runat="server" CssClass=""></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3"  runat="server" ForeColor=Maroon ControlToValidate="txtAgr" Display="Dynamic" Text="Required!"></asp:RequiredFieldValidator>
        </div>
        </div>
        <div class="control-group">
        <label class="control-label" for="txtStaffID">Full Name<span>*</span>:</label>
        <div class="controls">
            <asp:TextBox ID="txtOblg" runat="server" CssClass=""></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2"  runat="server" ForeColor=Maroon ControlToValidate="txtOblg" Display="Dynamic" Text="Required!"></asp:RequiredFieldValidator>
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
 <div class="inner-header">List of Customer
   <div class="form-horizontal" style="float:right;margin-top:-5px">
            <div class="controls-group">
            <div class="controls">
                     <asp:DropDownList ID="ddlOblTyp" runat="server" AppendDataBoundItems="true" Font-Size="12px">
             <asp:ListItem Value="" Selected="True">...Filter By Customer Type...</asp:ListItem>
            </asp:DropDownList>
                <asp:Button ID="btnSearch" runat="server" Text="Go" CausesValidation="false" CssClass="btn" ValidationGroup="" 
                    onclick="btnSearch_Click"/>  <asp:Button ID="btnClr" runat="server" 
                    Text="Clr" CssClass="btn" onclick="btnClr_Click" CausesValidation="false" ValidationGroup="" 
                    /></div>
              </div>
              
          </div>
 </div>
    <asp:HiddenField ID="hid" runat="server" />
    <asp:GridView ID="gvCommon" runat="server" GridLines="None"  Font-Size="11px" 
        AutoGenerateColumns="false" CssClass="table table-striped" DataKeyNames="ID" 
           AllowPaging="true" PageIndex="0" PageSize="100"
          onselectedindexchanged="gvCommon_SelectedIndexChanged" 
           onpageindexchanging="gvCommon_PageIndexChanging">
      <Columns>
        <asp:BoundField HeaderText="#" DataField="ID" Visible=false />
        <asp:TemplateField HeaderText="Customer Type">
             <ItemTemplate>
             <asp:Label ID="lbID" runat="server" Text='<%#Eval("ID") %>' Visible="false"></asp:Label>
                  <asp:Label ID="lbDir" runat="server" Text='<%#Eval("CustomerType.ID") %>' Visible="false"></asp:Label>
                 <asp:Label ID="lbloc1" runat="server" Text='<%#Eval("CustomerType.Name") %>'></asp:Label>
             </ItemTemplate>
           </asp:TemplateField>
         <asp:BoundField HeaderText="Customer Code" DataField="Code"  /> 
        <asp:BoundField HeaderText="Customer Name" DataField="FullName" />
       
        <asp:CommandField ButtonType="Image" HeaderText="Edit" ShowSelectButton="true" SelectImageUrl="../img/edit_icon.png"   />
       <%-- <asp:ButtonField ButtonType="Image" CommandName="del" HeaderText="Delete" ImageUrl="../img/delete-icon.png" />--%>
      </Columns>
       <SelectedRowStyle BackColor="#E0D9BD" />
    </asp:GridView>
</div>
</div>
</div>
</div>
</asp:Content>
