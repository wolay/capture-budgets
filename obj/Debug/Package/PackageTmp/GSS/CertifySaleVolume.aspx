<%@ Page Title="" Language="C#" MasterPageFile="~/SiteGSS.Master" AutoEventWireup="true" CodeBehind="CertifySaleVolume.aspx.cs" Inherits="BudgetCapture.GSS.CertifySaleVolume" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row-fluid">
     <div class=" span12 pageheader-text">Certify Gas Sales Volume</div>
     <div class="alert alert-error" runat="server" id="error" visible="false">
      <button type="button" class="close" data-dismiss="alert">&times;</button>
     </div>
    <div class="alert alert-success" runat="server" id="success" visible="false">
      <button type="button" class="close" data-dismiss="alert">&times;</button>
  </div>
  </div>
     <div class="row-fluid">
<div class="page-body span12">
  <div class="page-body-wrapper" id="dvAdd" runat="server" visible="false">
  <div class="inner-header">Input Certified Sales Volume</div>
    <div class="form-horizontal">
    <div class="nav-header" runat="server" id="dvMsg">Update Sales Volume detail :</div>
        <div class="control-group" runat="server" id="dvID" visible="false">
            <label class="control-label" for="txtStaffID"> ID:<span>*</span>:</label>
            <div class="controls">
                <asp:TextBox ID="txtID" runat="server" ReadOnly="true" CssClass=""></asp:TextBox>
            </div>
        </div>
        <div class="control-group">
        <label class="control-label" for="txtStaffID">Select Customer<span>*</span>:</label>
        <div class="controls">
             <asp:DropDownList ID="ddlDir" runat="server" AppendDataBoundItems="true">
             <asp:ListItem Value="">...Select Customer...</asp:ListItem>
            </asp:DropDownList>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator2" InitialValue=""  runat="server" ForeColor=Maroon ControlToValidate="ddlDir" Display="Dynamic" Text="Required!"></asp:RequiredFieldValidator>  
          
             </div> 
        </div>
          <div class="control-group">
        <label class="control-label" for="txtStaffID">Captured Volume<span>*</span>:</label>
        <div class="controls">
            <asp:TextBox ID="txtCap" Enabled="false" runat="server" CssClass="" onkeyup = "javascript:this.value=Comma(this.value);"></asp:TextBox>
        </div>
        </div>
        <div class="control-group">
        <label class="control-label" for="txtStaffID">Certified Volume<span>*</span>:</label>
        <div class="controls">
            <asp:TextBox ID="txtVol" runat="server" CssClass="" onkeyup = "javascript:this.value=Comma(this.value);"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1"  runat="server" ForeColor=Maroon ControlToValidate="txtVol" Display="Dynamic" Text="Required!"></asp:RequiredFieldValidator>
        </div>
        </div>
        <div class="control-group">
        <label class="control-label" for="txtStaffID">Certified Date<span>*</span>:</label>
        <div class="controls">
            <asp:TextBox ID="txtDate" runat="server" CssClass=""></asp:TextBox>
            <asp:RequiredFieldValidator ID="Requir" runat="server" ForeColor="Maroon" ErrorMessage="Date is required" ControlToValidate="txtDate" Display="Dynamic" Text="*"></asp:RequiredFieldValidator>
           <asp:Image ID="Image2" runat="server" ImageUrl="~/img/cal.gif" />
           <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupPosition="Right" Format="dd/MM/yyyy" TargetControlID="txtDate" PopupButtonID="Image2"></asp:CalendarExtender> 
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
 <div class="inner-header">List of Captured Sales Data
     <div class="form-horizontal" style="float:right;margin-top:-5px">
            <div class="controls-group">
            <div class="controls">
             <asp:DropDownList ID="ddlCustFilter"  Width="140px" runat="server" AppendDataBoundItems="true" Font-Size="11px">
             <asp:ListItem Value="" Selected="True">...Filter By Customer...</asp:ListItem>
            </asp:DropDownList>
            <asp:TextBox ID="txtFilterDate" runat="server" CssClass="" Width="120px"></asp:TextBox>
           <asp:Image ID="Image1" runat="server" ImageUrl="~/img/cal.gif" />
           <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupPosition="Right" Format="dd/MM/yyyy" TargetControlID="txtFilterDate" PopupButtonID="Image1"></asp:CalendarExtender> 
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
           AllowPaging="true" PageIndex="0" PageSize="50"  
            OnRowDataBound="gvCommon_RowDataBound" 
           onpageindexchanging="gvCommon_PageIndexChanging" 
           onrowcommand="gvCommon_RowCommand" OnRowEditing="gvCommon_RowEditing1">                                                                                                        
      <Columns>
        <asp:BoundField HeaderText="#" DataField="ID" Visible=false />
        
        <asp:TemplateField HeaderText="Customer Name">
            <ItemTemplate>
                <asp:Label ID="lbRecID" runat="server" Text='<%#Eval("ID") %>' Visible="false"></asp:Label>
                 <asp:Label ID="lbCompany" runat="server" Text='<%#Eval("GSS_Customer.CompanyName")%>'></asp:Label>
             </ItemTemplate>
          </asp:TemplateField>
        <asp:BoundField HeaderText="Volume Captured" DataField="CapturedVolumeSale" DataFormatString="{0:N}"/>
        <asp:BoundField HeaderText="Date Captured" DataField="DateCaptured" DataFormatString="{0:dd-MMM-yyyy}"/>
           <asp:TemplateField HeaderText="Status">
              <ItemTemplate>
                 <asp:Label ID="lbStatus" runat="server" Visible="false" Text='<%#Eval("Status") %>'></asp:Label>
                  <asp:Label ID="lbSt" runat="server" ForeColor=Maroon Text='<%#GetStatus(Eval("Status")) %>'></asp:Label>
              </ItemTemplate>
             </asp:TemplateField>
        <%--<asp:CommandField ButtonType="Image" HeaderText="Edit" Visible="false" ShowSelectButton="true" SelectImageUrl="../img/edit_icon.png"   />--%>
         <asp:TemplateField HeaderText="Edit">
        <ItemTemplate>
             <asp:ImageButton ID="imgBtnEdt" CommandName="edt" Visible="false" CommandArgument='<%#Eval("ID") %>' CausesValidation="false" ToolTip="Edit Item" runat="server"  ImageUrl="~/img/edit_icon.png" />
         </ItemTemplate>
         </asp:TemplateField>
      </Columns>
       <SelectedRowStyle BackColor="#E0D9BD" />
    </asp:GridView>
</div>
</div>
</div>
</div>
</asp:Content>
