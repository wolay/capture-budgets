<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RevenueProjectionPage.aspx.cs" Inherits="BudgetCapture.RecoveryProjectionPage" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <asp:UpdatePanel ID="UpdatePanel1" runat="server">
         <ContentTemplate>
    <div class="row-fluid">
     <div class=" span12 pageheader-text">Revenue Projection Page</div>
     <div class="alert alert-error" runat="server" id="error" visible="false">
      <button type="button" class="close" data-dismiss="alert">&times;</button>
     </div>
    <div class="alert alert-success" runat="server" id="success" visible="false">
      <button type="button" class="close" data-dismiss="alert">&times;</button>
  </div>
  </div>
 <div class="row-fluid">

<div class="page-body span12">
  
 <div class="form-horizontal" runat="server" id="dvAdd" visible=false>
<div class="alert alert-info">
    <span>Enter your projected Revenue.All figures should be in thousands (N'million) </span>
    <li>Budget Year: 
      <asp:Label ID="lbyr" runat="server" Text="" Font-Bold="true" Font-Size="12px"></asp:Label> </li>
      <li>Department: 
      <asp:Label ID="lbDept" runat="server" Text="" Font-Bold="true" Font-Size="12px"></asp:Label> </li>
  </div>
 
  <fieldset>
          <div id="legend">
            </div>
            <legend class="legend">

          <asp:LinkButton ID="lnkAddNew" CausesValidation="false" runat="server" onclick="lnkAddNew_Click">Add New Item</asp:LinkButton></legend>
         
         <div class="control-group" runat="server" id="dvID" visible="false">
            <label class="control-label" for="txtStaffID">ID:<span>*</span>:</label>
            <div class="controls">
                <asp:TextBox ID="txtID" runat="server" ReadOnly="true"  CssClass=""></asp:TextBox>
            </div>
        </div>
    <%--   <div class="control-group">
        <label class="control-label" for="txtStaffID">Category<span>*</span>:</label>
        <div class="controls">
              <asp:DropDownList ID="ddlCat" runat="server" AppendDataBoundItems="true" 
                  Font-Size="11px">
             <asp:ListItem Value="" Selected="True">...Select Category...</asp:ListItem>
              <asp:ListItem Value="1" >RECOVERY</asp:ListItem>
               <asp:ListItem Value="2">DISBURSEMENT</asp:ListItem>
            </asp:DropDownList>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator2" InitialValue=""  runat="server" ForeColor=Maroon ControlToValidate="ddlCat" Display="Dynamic" Text="Required!"></asp:RequiredFieldValidator>  
        </div>
        </div>--%>
        <div class="control-group">
        <label class="control-label" for="txtStaffID">Type <span>*</span>:</label>
        <div class="controls">
              <asp:DropDownList ID="ddlType" runat="server" AppendDataBoundItems="true" 
                  Font-Size="11px" AutoPostBack="true" 
                  onselectedindexchanged="ddlType_SelectedIndexChanged">
             <asp:ListItem Value="" Selected="True">...Select Type...</asp:ListItem>
            </asp:DropDownList>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator3" InitialValue=""  runat="server" ForeColor=Maroon ControlToValidate="ddlType" Display="Dynamic" Text="Required!"></asp:RequiredFieldValidator>  
        </div>

        </div>
       <%-- <div class="control-group">
        <label class="control-label" for="txtStaffID">Loan Type <span>*</span>:</label>
        <div class="controls">
              <asp:DropDownList ID="ddlLoanType" runat="server" AppendDataBoundItems="true" 
                  Font-Size="11px" AutoPostBack="false" 
                   >
             <asp:ListItem Value="" Selected="True">...Select Loan Type...</asp:ListItem>
            </asp:DropDownList>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator2" InitialValue=""  runat="server" ForeColor=Maroon ControlToValidate="ddlLoanType" Display="Dynamic" Text="Required!"></asp:RequiredFieldValidator>  
        </div>

        </div>--%>
         <div class="control-group">
        <label class="control-label" for="txtStaffID">Select Input Method <span>*</span>:</label>
        <div class="controls">
            <asp:RadioButtonList ID="rboList" runat="server" AutoPostBack="true" 
                onselectedindexchanged="rboList_SelectedIndexChanged" 
                RepeatDirection="Horizontal" Width="400px">
             <asp:ListItem Value="1">Single upload</asp:ListItem>
             <asp:ListItem Value="2">Batch Upload</asp:ListItem>
            </asp:RadioButtonList>
        </div>
        </div>
      <div id="dvSingle" runat="server" visible="false">
        <div class="control-group">
        <label class="control-label" for="txtStaffID">Enter Customer Code: <span>*</span>:</label>
        <div class="controls">
        
             <asp:TextBox ID="txtobligor" runat="server" CssClass=""></asp:TextBox>
              <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ForeColor=Maroon ControlToValidate="txtobligor" Display="Dynamic" Text="Required!"></asp:RequiredFieldValidator> 
             <asp:Button ID="btnLoad" runat="server" CausesValidation="false" Text="Load"  CssClass="btn btn-submit" onclick="btnLoad_Click" /><br />
             <asp:Label ID="lbName" runat="server" Text="" Font-Bold="true" Font-Size="12px"></asp:Label>
             
             <%-- <asp:DropDownList ID="ddlObligor" runat="server" AppendDataBoundItems="true" Font-Size="11px">
             <asp:ListItem Value="" Selected="True">...Select Obligor...</asp:ListItem>
            </asp:DropDownList>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator4" InitialValue=""  runat="server" ForeColor=Maroon ControlToValidate="ddlObligor" Display="Dynamic" Text="Required!"></asp:RequiredFieldValidator>  --%>
        
        </div>
        </div>
       <div class="control-group">
        <label class="control-label" for="txtStaffID">Select Month <span>*</span>:</label>
        <div class="controls">
              <asp:DropDownList ID="ddlMonth" runat="server" AppendDataBoundItems="true" Font-Size="12px">
             <asp:ListItem Value="">...Select Month...</asp:ListItem>
            </asp:DropDownList>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" InitialValue=""  runat="server" ForeColor=Maroon ControlToValidate="ddlMonth" Display="Dynamic" Text="Required!"></asp:RequiredFieldValidator>  
        </div>
        </div>
        <div class="control-group">
        <label class="control-label" for="txtStaffID">Amount<span>(N'million)*</span>:</label>
        <div class="controls">
            <asp:TextBox ID="txtTot" runat="server" CssClass="" onkeyup = "javascript:this.value=Comma(this.value);"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator15"  runat="server" ForeColor=Maroon ControlToValidate="txtTot" Display="Dynamic" Text="Required!"></asp:RequiredFieldValidator>
              <br /><small>All figures should be in multiple of million(N'million)</small>
        </div>
        </div>
        
       <div class="control-group">
        <label class="control-label" for="btnSubmit"></label>
        <div class="controls">
          <asp:Button ID="btnSubmit" runat="server" Text="Add" Enabled="false"
                CssClass="btn btn-submit" onclick="btnSubmit_Click" />
         </div>
        </div>
     </div>
     <div id="dvBatch" runat="server" visible="false">
         <div class="control-group">
        <label class="control-label" for="txtStaffID">Select File To Upload: <span>*</span>:</label>
        <div class="controls">
             <asp:FileUpload ID="xlsUpload" runat="server" Font-Size="Small" Width="400px" 
                     CssClass="txtbox" /><br />
                     <small>File Must be either .xlsx or.xls and it must be in the specified format</small>
                     <br />
            <asp:Button ID="btnUpload" runat="server" Text="Upload Record"  CssClass="btn btn-submit" 
                 onclick="btnUpload_Click" />
                 
        </div>
        </div>
     </div>
   </fieldset>
    
    </div>
  
<div class="row-fluid">
   <div class="page-body-control span12">
   <div class="form-horizontal" runat="server" id="dvFilter" visible="false">
    <div class="control-group">
        <label class="control-label" for="txtStaffID">Filter By Department <span>*</span>:</label>
        <div class="controls">
              <asp:DropDownList ID="ddlDept" runat="server" AppendDataBoundItems="true" AutoPostBack="true" 
                  Font-Size="11px" onselectedindexchanged="ddlDept_SelectedIndexChanged">
             <asp:ListItem Value="" Selected="True">...All Department...</asp:ListItem>
            </asp:DropDownList>
          </div>
       </div>
     </div>
 <div class="inner-header">
    <div class="form-horizontal" style="float:right;margin-top:-5px">
            <div class="controls-group">
            <div class="controls">
            <%-- <asp:DropDownList ID="ddlCatFilter" runat="server" AppendDataBoundItems="true" Font-Size="11px" Width="140px">
             <asp:ListItem Value="" Selected="True">...Filter By CustomerType...</asp:ListItem>
             <%-- <asp:ListItem Value="1" >Recovery</asp:ListItem>
               <asp:ListItem Value="2">Disbursement</asp:ListItem>
            </asp:DropDownList>--%>
                     <asp:DropDownList ID="ddlAsstTyp" runat="server" AppendDataBoundItems="true" Font-Size="11px" Width="140px">
             <asp:ListItem Value="" Selected="True">...Filter By CustomerType...</asp:ListItem>
            </asp:DropDownList>
                <asp:Button ID="btnSearch" runat="server" Text="Go" CausesValidation="false" CssClass="btn" ValidationGroup="" 
                    onclick="btnSearch_Click"/>  <asp:Button ID="btnClr" runat="server" 
                    Text="Clr" CssClass="btn" onclick="btnClr_Click" CausesValidation="false" ValidationGroup="" 
                    /></div>
              </div>
              
          </div>
 </div>
    <asp:HiddenField ID="hid" runat="server" />
    <asp:GridView ID="gvDept" runat="server" GridLines="None"  Font-Size="11px" 
        AutoGenerateColumns="false" CssClass="table table-striped" DataKeyNames="ID" ShowFooter="true"
           AllowPaging="true" PageIndex="0" PageSize="100" OnRowCommand="gvDept_RowCommand"
           onpageindexchanging="gvDept_PageIndexChanging" 
           onrowdatabound="gvDept_RowDataBound">
      <Columns>
        <asp:BoundField HeaderText="#" DataField="ID" Visible=false />
        <asp:TemplateField HeaderText="Department">
         <ItemTemplate>
            <asp:Label ID="lbldept" runat="server" Text='<%#Eval("Department.Name") %>'></asp:Label>
         </ItemTemplate>
        </asp:TemplateField>
        <%--<asp:TemplateField HeaderText="Loan Type">
         <ItemTemplate>
            <asp:Label ID="lblloan" runat="server" Text='<%#Eval("LoanType") %>'></asp:Label>
         </ItemTemplate>
        </asp:TemplateField>--%>
        <asp:TemplateField HeaderText="Customer Type">
             <ItemTemplate>
              <asp:Label ID="lbRecID" runat="server" Text='<%#Eval("ID") %>' Visible="false"></asp:Label>
               <asp:Label ID="lbCat" runat="server" Text='<%#Eval("CustomerType.ID") %>' Visible="false"></asp:Label>
             <%-- <asp:Label ID="lbID" runat="server" Text='<%#Eval("Obligor.ObligorType.ID") %>' Visible="false"></asp:Label>--%>
                 <asp:Label ID="lbloc1" runat="server" Text='<%#Eval("CustomerType.Name") %>'></asp:Label>
             </ItemTemplate>
             <FooterTemplate>
                <asp:Label ID="lbTot" runat="server" Text="Net Revenue(N'million)"></asp:Label>
             </FooterTemplate>
           </asp:TemplateField>
          <asp:TemplateField HeaderText="Customer">
            <ItemTemplate>
                 <asp:Label ID="lbOblig" runat="server" Text='<%#Eval("CustomerName") %>'></asp:Label>
             </ItemTemplate>
          </asp:TemplateField>
          <asp:TemplateField HeaderText="Code">
            <ItemTemplate>
                 <asp:Label ID="lbAgree" runat="server" Text='<%#Eval("CustomerCode") %>'></asp:Label>
             </ItemTemplate>
          </asp:TemplateField>
          <%-- <asp:BoundField HeaderText="Quantity" DataField="Quantity"/>  --%>
           <asp:TemplateField HeaderText="Amount(N'000)">
            <ItemTemplate>
                 <asp:Label ID="lbAmt" runat="server" Text='<%#Eval("Amount","{0:N}") %>'></asp:Label>
                 <asp:Label ID="lbusr" runat="server" Text='<%#Eval("AddedBy") %>' Visible="false"></asp:Label>
             </ItemTemplate>
             <FooterTemplate>
                <asp:Label ID="lbSumAmt" runat="server" Text=""></asp:Label>
             </FooterTemplate>
          </asp:TemplateField>
       
           <asp:TemplateField HeaderText="Month">
             <ItemTemplate>
                 <asp:Label ID="lblMonth" runat="server" Text='<%#Eval("MonthOfYear.MonthName") %>'></asp:Label>
             </ItemTemplate>
           </asp:TemplateField>
           <asp:TemplateField HeaderText="Status">
              <ItemTemplate>
                 <asp:Label ID="lbStatus" runat="server" Visible="false" Text='<%#Eval("Status") %>'></asp:Label>
                  <asp:Label ID="lbSt" runat="server" ForeColor=Maroon Text='<%#GetStatus(Eval("Status")) %>'></asp:Label>
              </ItemTemplate>
             </asp:TemplateField>
           <asp:TemplateField>
            <HeaderTemplate>
                <asp:CheckBox ID="chkHeader" runat="server" visible="false"/>
            </HeaderTemplate>
            <ItemTemplate>
                <asp:CheckBox ID="chkRow" runat="server" Visible="false"/>
                  <asp:Label ID="lbDeptID" runat="server" Text='<%#Eval("Department.ID") %>' Visible="false"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
        <ItemTemplate>
            <asp:ImageButton ID="imgBtnEdit" CommandName="edt" CommandArgument='<%#Eval("ID") %>' CausesValidation="false" ToolTip="Edit Item" Visible="false" runat="server" ImageUrl="~/img/edit_icon.png" />
         </ItemTemplate>
         </asp:TemplateField>
          <asp:TemplateField>
        <ItemTemplate>
             <asp:ImageButton ID="imgBtnDel" CommandName="del" CommandArgument='<%#Eval("ID") %>' CausesValidation="false" ToolTip="Delete Item" runat="server" Visible="false" ImageUrl="~/img/delete-icon.png" />
         </ItemTemplate>
         </asp:TemplateField>
      </Columns>
       <SelectedRowStyle BackColor="#E0D9BD" />
      <FooterStyle BackColor="#E0D9BD" Font-Bold="true" ForeColor="Maroon" Font-Size="13px" />
    </asp:GridView>
</div>
</div>

<div class="row-fluid" runat="server" id="dvAppr" visible="false">
<div class="page-body-controlbtn">
 <h3>Approval Action</h3>
    <asp:Button ID="btnApprv" CausesValidation="false" runat="server" Text="Approve Selected Record(s)" ToolTip="Check all rows to be approved and click to approve"
     CssClass="btn btn-submit"   onclick="btnApprv_Click" />
     <%-- <asp:Button ID="btnAppr" runat="server" Text="" visible="true" ToolTip="Check all rows to be approved and click to approve"
                CssClass="btn btn-submit" onclick="btnAppr1_Click" />--%>      
   <asp:Button ID="btnReject" runat="server" CausesValidation="false" Text="Reject Selected Record(s)" visible="true" ToolTip="Check all rows to be rejectd and click to reject"
                CssClass="btn btn-submit btn-reject" onclick=" btnReject_Click" />         
 </div>
 
</div>
<div class="row-fluid" runat="server" id="dvAdmin" visible="false">
<div class="page-body-controlbtn">
 <h3>Administrator Action</h3>
    <asp:Button ID="btnCorrect" CausesValidation="false" runat="server" Text="Return Selected Record(s)" ToolTip="Check all rows to be to be corrected"
     CssClass="btn btn-submit" />&nbsp;&nbsp;
    <asp:Button ID="btnForward" CausesValidation="false" runat="server" Text="Forward Approved Dept Budget to ED" ToolTip="Check all rows to be forwarded"
     CssClass="btn btn-submit" OnClick="btnForward_Click" />
       <asp:ModalPopupExtender ID="mpeAppr" runat="server" PopupControlID="pnlPopupAppr" TargetControlID="btnCorrect"
                         CancelControlID="btnCloseAppr" BackgroundCssClass="modalBackground">
                    </asp:ModalPopupExtender>
                 <asp:Panel ID="pnlPopupAppr" runat="server" CssClass="modalPopup" Style="display: none">
                        <div class="header">
                          Add Comments/Reason for the this action
                        </div>
                        <div class="body">
                            <div class="form-horizontal">
                              <div class=" alert alert-error" runat="server" id="modalErr" visible="false">
                               <button type="button" class="close" data-dismiss="alert"> &times;</button></div>
                                <div class="control-group">
                                    <label class="control-label" for="txtFeature">
                                       Add Comment:</label>
                                    <div class="controls">
                                         <asp:TextBox ID="txtcomment" runat="server" TextMode="MultiLine" Rows="4"  CssClass=""></asp:TextBox>
                                    </div>
                                </div>
                               
                                <div class="control-group">
                                    <label class="control-label" for="btnSubmit">
                                    </label>
                                    <div class="controls">
                                        <asp:Button ID="btnReturn" runat="server" Text="Return For Correction" CausesValidation="false" CssClass="btn btn-info btn-block"
                                            OnClick="btnReturn_Click" />
                                        <asp:Button ID="btnCloseAppr" runat="server" Text="Close" CausesValidation="false" CssClass="btn  btn-submit btn-block" />
                                    </div>
                                </div>
                            </div>
                          </div>
                        <div class="footer" align="right">
                        </div>
                    </asp:Panel>         
 </div>
 
</div>
</div>

</div>
<asp:UpdateProgress ID="UpProgress" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                        <ProgressTemplate>
                            <div class="throbber">
                                <img src="img/loadinfo.gif" alt="" />
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
       </ContentTemplate>
       <Triggers>
              <asp:PostBackTrigger ControlID="btnUpload" />
            </Triggers>
       </asp:UpdatePanel>
</asp:Content>
