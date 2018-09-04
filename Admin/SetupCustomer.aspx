<%@ Page Title="" Language="C#" MasterPageFile="~/SiteGSS.Master" AutoEventWireup="true" CodeBehind="SetupCustomer.aspx.cs" Inherits="BudgetCapture.Admin.SetupCustomer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row-fluid">
     <div class=" span12 pageheader-text">Customer Setup Page</div>
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
  <div class="inner-header">Setup Customer Details</div>
    <div class="form-horizontal">
    <div class="nav-header" runat="server" id="dvMsg">Add/Update customer Details :</div>
        <div class="control-group" runat="server" id="dvID" visible="false">
            <label class="control-label" for="txtStaffID"> ID:<span>*</span>:</label>
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
        <label class="control-label" for="txtStaffID">Company Name<span>*</span>:</label>
        <div class="controls">
            <asp:TextBox ID="txtAsset1" runat="server" CssClass=""></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2"  runat="server" ForeColor=Maroon ControlToValidate="txtAsset1" Display="Dynamic" Text="Required!"></asp:RequiredFieldValidator>
        </div>
        </div>
        <div class="control-group">
        <label class="control-label" for="txtStaffID">Company Address<span>*</span>:</label>
        <div class="controls">
            <asp:TextBox ID="txtaddr" runat="server" TextMode="MultiLine" Rows="4" CssClass=""></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFiel"  runat="server" ForeColor=Maroon ControlToValidate="txtaddr" Display="Dynamic" Text="Required!"></asp:RequiredFieldValidator>
        </div>
        </div>
        <div class="control-group">
        <label class="control-label" for="txtStaffID">Contact Person<span>*</span>:</label>
        <div class="controls">
            <asp:TextBox ID="txtperson" runat="server" CssClass=""></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3"  runat="server" ForeColor=Maroon ControlToValidate="txtperson" Display="Dynamic" Text="Required!"></asp:RequiredFieldValidator>
        </div>
        </div>
        <div class="control-group">
        <label class="control-label" for="txtStaffID">Contact PhoneNo<span>*</span>:</label>
        <div class="controls">
            <asp:TextBox ID="txtPhone" runat="server" CssClass=""></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4"  runat="server" ForeColor=Maroon ControlToValidate="txtPhone" Display="Dynamic" Text="Required!"></asp:RequiredFieldValidator>
        </div>
        </div>
        <div class="control-group">
        <label class="control-label" for="txtStaffID">Contact Email<span>*</span>:</label>
        <div class="controls">
            <asp:TextBox ID="txtEmail" runat="server" CssClass=""></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator5"  runat="server" ForeColor=Maroon ControlToValidate="txtEmail" Display="Dynamic" Text="Required!"></asp:RequiredFieldValidator>
           <asp:RegularExpressionValidator ID="RegEmail" runat="server" ForeColor="Maroon" ErrorMessage="Invalid Email" ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="cat"></asp:RegularExpressionValidator>
        </div>
        </div>
         <div class="control-group">
        <label class="control-label" for="txtStaffID">Pipeline Section<span>*</span>:</label>
        <div class="controls">
          <asp:DropDownList ID="ddlSection" runat="server" AppendDataBoundItems="true">
             <asp:ListItem Value="">...Select Pipeline Section...</asp:ListItem>
            </asp:DropDownList>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator6" InitialValue=""  runat="server" ForeColor=Maroon ControlToValidate="ddlSection" Display="Dynamic" Text="Required!"></asp:RequiredFieldValidator>  
        </div>
        </div>
         <div class="control-group">
        <label class="control-label" for="txtStaffID">Payment Term<span>*</span>:</label>
        <div class="controls">
          <asp:DropDownList ID="ddlTerm" runat="server" AppendDataBoundItems="true">
             <asp:ListItem Value="">...Select Payment Term...</asp:ListItem>
            </asp:DropDownList>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator7" InitialValue=""  runat="server" ForeColor=Maroon ControlToValidate="ddlTerm" Display="Dynamic" Text="Required!"></asp:RequiredFieldValidator>  
        </div>
        </div>
        <div class="control-group">
        <label class="control-label" for="txtStaffID">Unit Price(=N=)  <span>*</span>:</label>
        <div class="controls">
            <asp:TextBox ID="txtPrice" runat="server" CssClass="" onkeyup = "javascript:this.value=Comma(this.value);"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator8"  runat="server" ForeColor=Maroon ControlToValidate="txtPrice" Display="Dynamic" Text="Required!"></asp:RequiredFieldValidator>
        </div>
        </div>
         <div class="control-group">
        <label class="control-label" for="txtStaffID">Minimum Order Volume  <span>*</span>:</label>
        <div class="controls">
            <asp:TextBox ID="txtMinOrder" runat="server" CssClass="" onkeyup = "javascript:this.value=Comma(this.value);"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator9"  runat="server" ForeColor=Maroon ControlToValidate="txtMinOrder" Display="Dynamic" Text="Required!"></asp:RequiredFieldValidator>
        </div>
        </div>
        <div class="control-group">
        <label class="control-label" for="txtStaffID">VAT<span>*</span>:</label>
        <div class="controls">
          <asp:DropDownList ID="ddlVat" runat="server" AppendDataBoundItems="true">
             <asp:ListItem Value="">...Select VAT Flag...</asp:ListItem>
              <asp:ListItem Value="1">Applicable</asp:ListItem>
              <asp:ListItem Value="2">Not Applicable</asp:ListItem>
            </asp:DropDownList>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator10" InitialValue=""  runat="server" ForeColor=Maroon ControlToValidate="ddlTerm" Display="Dynamic" Text="Required!"></asp:RequiredFieldValidator>  
        </div>
        </div>
         <div class="control-group">
        <label class="control-label" for="txtStaffID">Capital Recovery?<span>*</span>:</label>
        <div class="controls">
            <asp:CheckBox ID="chkCap" runat="server" Checked="true" />
            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3"  runat="server" ForeColor=Maroon ControlToValidate="chkActive" Display="Dynamic" Text="Required!"></asp:RequiredFieldValidator>  --%>
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
 <div class="inner-header">Administrative action
 </div>
    <asp:HiddenField ID="hid" runat="server" />
    
</div>
</div>
</div>
</div>
</asp:Content>
