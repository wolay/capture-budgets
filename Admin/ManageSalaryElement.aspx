<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageSalaryElement.aspx.cs" Inherits="BudgetCapture.Admin.ManageSalaryElement" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row-fluid">
     <div class=" span12 pageheader-text">Manage Salary Element  Page</div>
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
  <div class="inner-header">Setup Salary Element</div>
    <div class="form-horizontal">
    <div class="nav-header" runat="server" id="dvMsg">Add New Salary Element :</div>
        <div class="control-group" runat="server" id="dvID" visible="false">
            <label class="control-label" for="txtStaffID">Element ID:<span>*</span>:</label>
            <div class="controls">
                <asp:TextBox ID="txtID" runat="server" ReadOnly="true" CssClass="" ></asp:TextBox>
            </div>
        </div>
        <div class="control-group">
        <label class="control-label" for="txtStaffID">Category:<span>*</span>:</label>
        <div class="controls">
            <asp:DropDownList ID="ddlCat" runat="server" AppendDataBoundItems="true">
             <asp:ListItem Value="">...Select Category...</asp:ListItem>
            </asp:DropDownList>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" InitialValue=""  runat="server" ForeColor=Maroon ControlToValidate="ddlCat" Display="Dynamic" Text="Required!"></asp:RequiredFieldValidator>  
        </div>
        </div>
        <div class="control-group">
        <label class="control-label" for="txtStaffID">Code<span>*</span>:</label>
        <div class="controls">
            <asp:TextBox ID="txtDept" runat="server" CssClass=""></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator15"  runat="server" ForeColor=Maroon ControlToValidate="txtDept" Display="Dynamic" Text="Required!"></asp:RequiredFieldValidator>  
        </div>
        </div>
        
        <div class="control-group">
        <label class="control-label" for="txtStaffID">Name<span>*</span>:</label>
        <div class="controls">
            <asp:TextBox ID="txtName" runat="server" CssClass=""></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2"  runat="server" ForeColor=Maroon ControlToValidate="txtName" Display="Dynamic" Text="Required!"></asp:RequiredFieldValidator>
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
 <div class="inner-header">List of Salary Element
   <%--<div class="form-horizontal" style="float:right;margin-top:-5px">
            <div class="controls-group">
            <div class="controls">
                     <asp:DropDownList ID="ddlAstTyp" runat="server" AppendDataBoundItems="true" Font-Size="12px">
             <asp:ListItem Value="" Selected="True">...Filter By AssetType...</asp:ListItem>
            </asp:DropDownList>
                <asp:Button ID="btnSearch" runat="server" Text="Go" CausesValidation="false" CssClass="btn" ValidationGroup="" 
                    onclick="btnSearch_Click"/>  <asp:Button ID="btnClr" runat="server" 
                    Text="Clr" CssClass="btn" onclick="btnClr_Click" CausesValidation="false" ValidationGroup="" 
                    /></div>
              </div>
              
          </div>--%>
 </div>
    <asp:HiddenField ID="hid" runat="server" />
    <asp:GridView ID="gvDir" runat="server" GridLines="None"  Font-Size="11px" 
        AutoGenerateColumns="false" CssClass="table table-striped" DataKeyNames="ID" 
           AllowPaging="true" PageIndex="0" PageSize="100"
          onselectedindexchanged="gvDir_SelectedIndexChanged" 
           onpageindexchanging="gvDir_PageIndexChanging" >
      <Columns>
        <asp:BoundField HeaderText="#" DataField="ID" Visible=false />
        <asp:TemplateField HeaderText="Category">
             <ItemTemplate>
             <asp:Label ID="lbID" runat="server" Text='<%#Eval("ID") %>' Visible="false"></asp:Label>
                  <asp:Label ID="lbDir" runat="server" Text='<%#Eval("SalBenCategory.ID") %>' Visible="false"></asp:Label>
                 <asp:Label ID="lbloc1" runat="server" Text='<%#Eval("SalBenCategory.Name") %>'></asp:Label>
             </ItemTemplate>
           </asp:TemplateField>
         <asp:BoundField HeaderText="Code" DataField="Code" />
        <asp:BoundField HeaderText="Name" DataField="Elements" />
       
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
