<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ExpenseProjectionPage.aspx.cs" Inherits="BudgetCapture.ExpenseProjectionPage" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row-fluid">
     <div class=" span12 pageheader-text">Expense Projection Page</div>
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
    <span>List Top 20 specific services to  be paid for under professional, consultancy , legal e.t.c. with amounts against the month concerned.</span>
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
        <div class="control-group">
        <label class="control-label" for="txtStaffID">Expense Type <span>*</span>:</label>
        <div class="controls">
              <asp:DropDownList ID="ddlCat" runat="server" AppendDataBoundItems="true" Font-Size="12px">
             <asp:ListItem Value="">...Select Expense Type...</asp:ListItem>
            </asp:DropDownList>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator3" InitialValue=""  runat="server" ForeColor=Maroon ControlToValidate="ddlCat" Display="Dynamic" Text="Required!"></asp:RequiredFieldValidator>  
        </div>
        </div>
         <div class="control-group">
        <label class="control-label" for="txtStaffID">Expense Details<span>*</span>:</label>
        <div class="controls">
            <asp:TextBox ID="txtDetails" runat="server" CssClass="" TextMode="MultiLine" Rows="4"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2"  runat="server" ForeColor=Maroon ControlToValidate="txtDetails" Display="Dynamic" Text="Required!"></asp:RequiredFieldValidator>
        </div>
        </div>
       <div class="control-group">
        <label class="control-label" for="txtStaffID">Select Month <span>*</span>:</label>
        <div class="controls">
              <asp:DropDownList ID="ddlMonth" runat="server" AppendDataBoundItems="true" Font-Size="12px">
             <asp:ListItem Value="">...Select Expense Month...</asp:ListItem>
            </asp:DropDownList>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" InitialValue=""  runat="server" ForeColor=Maroon ControlToValidate="ddlMonth" Display="Dynamic" Text="Required!"></asp:RequiredFieldValidator>  
        </div>
        </div>
        <div class="control-group">
        <label class="control-label" for="txtStaffID">Amount(=N=)<span>*</span>:</label>
        <div class="controls">
            <asp:TextBox ID="txtTot" runat="server" CssClass="" onkeyup = "javascript:this.value=Comma(this.value);"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator15"  runat="server" ForeColor=Maroon ControlToValidate="txtTot" Display="Dynamic" Text="Required!"></asp:RequiredFieldValidator>
            <br /><small>All figures should be in multiple of thousands(N'000)</small>
        </div>
        </div>
        
       <div class="control-group">
        <label class="control-label" for="btnSubmit"></label>
        <div class="controls">
          <asp:Button ID="btnSubmit" runat="server" Text="Add" 
                CssClass="btn btn-submit" onclick="btnSubmit_Click" />
         </div>
        </div>
   </fieldset>
    </div>
  
<div class="row-fluid">
   <div class="page-body-control span12">
 <div class="inner-header">List of Expense Projection
    <div class="form-horizontal" style="float:right;margin-top:-5px">
            <div class="controls-group">
            <div class="controls">
              <asp:DropDownList ID="ddlDeptFilter" visible="false"  Width="140px" runat="server" AppendDataBoundItems="true" Font-Size="11px">
             <asp:ListItem Value="" Selected="True">...Filter By Department...</asp:ListItem>
            </asp:DropDownList>
                     <asp:DropDownList ID="ddlExpTyp"  Width="140px"  runat="server" AppendDataBoundItems="true" Font-Size="12px">
             <asp:ListItem Value="" Selected="True">...Filter By ExpenseType...</asp:ListItem>
            </asp:DropDownList>
                <asp:Button ID="btnSearch" runat="server" Text="Go" CausesValidation="false" CssClass="btn" ValidationGroup="" 
                    onclick="btnSearch_Click"/>  <asp:Button ID="btnClr" runat="server" 
                    Text="Clr" CssClass="btn" onclick="btnClr_Click" CausesValidation="false" ValidationGroup="" 
                    /></div>
              </div>
              
          </div>
 </div>
    <asp:HiddenField ID="hid" runat="server" />
    <asp:GridView ID="gvDept" runat="server" GridLines="None"  Font-Size="11px" EmptyDataText="No record found" ShowFooter="true"
        AutoGenerateColumns="false" CssClass="table table-striped" DataKeyNames="ID" 
           AllowPaging="true" PageIndex="0" PageSize="100" OnRowCommand="gvDept_RowCommand"
           onpageindexchanging="gvDept_PageIndexChanging" 
           onrowdatabound="gvDept_RowDataBound">
      <Columns>
        <asp:BoundField HeaderText="#" DataField="ID" Visible=false />
        <asp:TemplateField HeaderText="Expense Type">
             <ItemTemplate>
              <asp:Label ID="lbRecID" runat="server" Text='<%#Eval("ID") %>' Visible="false"></asp:Label>
              <asp:Label ID="lbID" runat="server" Text='<%#Eval("ExpenseType.ID") %>' Visible="false"></asp:Label>
                 <asp:Label ID="lbloc1" runat="server" Text='<%#Eval("ExpenseType.Name") %>'></asp:Label>
             </ItemTemplate>
             <FooterTemplate>
               <asp:Label ID="lbExp" runat="server" Text="Total Expense(N'000)"></asp:Label>
             </FooterTemplate>
           </asp:TemplateField>
         <asp:BoundField HeaderText="Expense Details" DataField="Details" />
       <%-- <asp:BoundField HeaderText="Amount(N'000)" DataField="Amount" DataFormatString="{0:N}"/>--%>
         <asp:TemplateField HeaderText="Amount(N'000)">
            <ItemTemplate>
                 <asp:Label ID="lbAmt" runat="server" Text='<%#Eval("Amount","{0:N}") %>'></asp:Label>
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
        <asp:TemplateField HeaderText="Department">
             <ItemTemplate>
                 <asp:Label ID="lblDept" runat="server" Text='<%#Eval("Department.Name") %>'></asp:Label>
             </ItemTemplate>
           </asp:TemplateField>
            <asp:TemplateField HeaderText="Budget Year">
             <ItemTemplate>
                 <asp:Label ID="lblyear" runat="server" Text='<%#Eval("BudgetYear.Year") %>'></asp:Label>
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
                <asp:CheckBox ID="chkHeader"   TextAlign="Right" runat="server" visible="false"/>
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
       <FooterStyle BackColor="#E0D9BD" Font-Bold="true" ForeColor="Maroon" Font-Size="13px" />
       <SelectedRowStyle BackColor="#E0D9BD" />
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
     CssClass="btn btn-submit" />
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
</asp:Content>
