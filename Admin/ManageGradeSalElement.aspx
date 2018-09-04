<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageGradeSalElement.aspx.cs" Inherits="BudgetCapture.Admin.ManageGradeSalElement1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <div class="row-fluid">
     <div class=" span12 pageheader-text">Manage Salary Grade Element  Page</div>
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
  <div class="inner-header">Setup Grade Salary Element</div>
    <div class="form-horizontal">
    <div class="nav-header" runat="server" id="dvMsg">Add New Grade Salary Element :</div>
        <div class="control-group" runat="server" id="dvID" visible="false">
            <label class="control-label" for="txtStaffID"> ID:<span>*</span>:</label>
            <div class="controls">
                <asp:TextBox ID="txtID" runat="server" ReadOnly="true" CssClass="" ></asp:TextBox>
            </div>
        </div>
        <div class="control-group">
        <label class="control-label" for="txtStaffID">Grade:<span>*</span>:</label>
        <div class="controls">
            <asp:DropDownList ID="ddlGrd" runat="server" AppendDataBoundItems="true">
             <asp:ListItem Value="">...Select Grade...</asp:ListItem>
            </asp:DropDownList>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" InitialValue=""  runat="server" ForeColor=Maroon ControlToValidate="ddlGrd" Display="Dynamic" Text="Required!"></asp:RequiredFieldValidator>  
        </div>
        </div>
         <div class="control-group">
        <label class="control-label" for="txtStaffID">Salary Item:<span>*</span>:</label>
        <div class="controls">
            <asp:DropDownList ID="ddlEle" runat="server" AppendDataBoundItems="true">
             <asp:ListItem Value="">...Select Salary Item...</asp:ListItem>
            </asp:DropDownList>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator3" InitialValue=""  runat="server" ForeColor=Maroon ControlToValidate="ddlGrd" Display="Dynamic" Text="Required!"></asp:RequiredFieldValidator>  
        </div>
        </div>
        <div class="control-group">
        <label class="control-label" for="txtStaffID">Amount(=N='million)<span>*</span>:</label>
        <div class="controls">
            <asp:TextBox ID="txtAmt" runat="server" CssClass=""></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator15"  runat="server" ForeColor=Maroon ControlToValidate="txtAmt" Display="Dynamic" Text="Required!"></asp:RequiredFieldValidator>  
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
 <div class="inner-header">List of Salary Grade Element
   <div class="form-horizontal" style="float:right;margin-top:-28px">
            <div class="controls-group">
            <div class="controls">
                     <asp:DropDownList ID="ddlGradeFilter" runat="server" AppendDataBoundItems="true" Font-Size="12px">
             <asp:ListItem Value="" Selected="True">...Filter By Grade...</asp:ListItem>
            </asp:DropDownList>
                 <asp:DropDownList ID="ddlElemFilter" runat="server" AppendDataBoundItems="true" Font-Size="12px">
             <asp:ListItem Value="" Selected="True">...Filter By Salary Item...</asp:ListItem>
            </asp:DropDownList>
                <asp:Button ID="btnSearch" runat="server" Text="Go" CausesValidation="false" CssClass="btn" ValidationGroup="" 
                    onclick="btnSearch_Click"/>  <asp:Button ID="btnClr" runat="server" 
                    Text="Clr" CssClass="btn" onclick="btnClr_Click" CausesValidation="false" ValidationGroup="" 
                    /></div>
              </div>
              
          </div>
 </div>
    <asp:HiddenField ID="hid" runat="server" /> <asp:HiddenField ID="hidEle" runat="server" />
       <asp:HiddenField ID="hidGrd" runat="server" />
    <asp:GridView ID="gvDir" runat="server" GridLines="None"  Font-Size="11px" 
        AutoGenerateColumns="false" CssClass="table table-striped" DataKeyNames="SalaryElementID,GradeID" 
           AllowPaging="true" PageIndex="0" PageSize="100"
          onselectedindexchanged="gvDir_SelectedIndexChanged" 
           onpageindexchanging="gvDir_PageIndexChanging" >
      <Columns>
        <asp:BoundField HeaderText="#" DataField="ID" Visible=false />
           <asp:BoundField HeaderText="Category" DataField="Category" />
        <asp:TemplateField HeaderText="Grade">
             <ItemTemplate>
                 <asp:Label ID="lbloc1" runat="server" Text='<%#Eval("GradeName") %>'></asp:Label>
             </ItemTemplate>
           </asp:TemplateField>
         <asp:BoundField HeaderText="Salary Item" DataField="ElementName" />
        <asp:BoundField HeaderText="Amount(=N='million)" DataField="Amount" DataFormatString="{0:N}" />
       
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
