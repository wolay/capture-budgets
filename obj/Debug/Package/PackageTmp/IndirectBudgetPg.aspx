<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="IndirectBudgetPg.aspx.cs" Inherits="BudgetCapture.IndirectBudgetPg" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row-fluid">
     <div class=" span12 pageheader-text">Indirect Expense Projection Page</div>
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
    <span> </span>
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
        <label class="control-label" for="txtStaffID">Indirect
              Type <span>*</span>:</label>
        <div class="controls">
              <asp:DropDownList ID="ddlCat" runat="server" AppendDataBoundItems="true" 
                  Font-Size="11px" AutoPostBack="true" 
                  onselectedindexchanged="ddlCat_SelectedIndexChanged">
             <asp:ListItem Value="" Selected="True">...Select Indirect Type...</asp:ListItem>
            </asp:DropDownList>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator3" InitialValue=""  runat="server" ForeColor=Maroon ControlToValidate="ddlCat" Display="Dynamic" Text="Required!"></asp:RequiredFieldValidator>  
        </div>
        </div>
        <div class="control-group">
        <label class="control-label" for="txtStaffID">Indirect ExpenseItem <span>*</span>:</label>
        <div class="controls">
              <asp:DropDownList ID="ddlAsset" runat="server" AppendDataBoundItems="true" Font-Size="11px">
             <asp:ListItem Value="" Selected="True">...Select Expense...</asp:ListItem>
            </asp:DropDownList>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator4" InitialValue=""  runat="server" ForeColor=Maroon ControlToValidate="ddlAsset" Display="Dynamic" Text="Required!"></asp:RequiredFieldValidator>  
        </div>
        </div>
       <div class="control-group">
        <label class="control-label" for="txtStaffID">Amount(=N='million)<span></span>:</label>
        <div class="controls">
            <asp:TextBox ID="txtAmt" runat="server" CssClass=""></asp:TextBox>    
        </div>
        </div>
       <%--<div class="control-group">
        <label class="control-label" for="txtStaffID">Select Month <span>*</span>:</label>
        <div class="controls">
              <asp:DropDownList ID="ddlMonth" runat="server" AppendDataBoundItems="true" Font-Size="12px">
             <asp:ListItem Value="">...Select Expense Month...</asp:ListItem>
            </asp:DropDownList>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" InitialValue=""  runat="server" ForeColor=Maroon ControlToValidate="ddlMonth" Display="Dynamic" Text="Required!"></asp:RequiredFieldValidator>  
        </div>
        </div>--%>
         
        <div class="control-group">
        <label class="control-label" for="txtStaffID">Justification<span>(Optional)</span>:</label>
        <div class="controls">
            <asp:TextBox ID="txtJust" runat="server" CssClass="" TextMode="MultiLine" Rows="4"></asp:TextBox>    
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
 <div class="inner-header">List of Projected Indirect Expense
    <div class="form-horizontal" style="float:right;margin-top:-5px" runat="server" id="dvFilterHeader">
            <div class="controls-group">
            <div class="controls">
             <asp:DropDownList ID="ddlDeptFilter" visible="false"  Width="140px" runat="server" AppendDataBoundItems="true" Font-Size="11px">
             <asp:ListItem Value="" Selected="True">...Filter By Department...</asp:ListItem>
            </asp:DropDownList>
                     <asp:DropDownList ID="ddlExpTyp"  Width="140px" runat="server" AppendDataBoundItems="true" Font-Size="11px">
             <asp:ListItem Value="" Selected="True">...Filter By IndirectType...</asp:ListItem>
            </asp:DropDownList>
                <asp:Button ID="btnSearch" runat="server" Text="Go" CausesValidation="false" CssClass="btn" ValidationGroup="" 
                    onclick="btnSearch_Click"/>  <asp:Button ID="btnClr" runat="server" 
                    Text="Clr" CssClass="btn" onclick="btnClr_Click" CausesValidation="false" ValidationGroup="" 
                    /></div>
              </div>
              
          </div>
 </div>
    <asp:HiddenField ID="hid" runat="server" />
    <asp:GridView ID="gvDept" runat="server" GridLines="None"  Font-Size="11px" EmptyDataText="No record found" 
        AutoGenerateColumns="false" CssClass="table table-striped" DataKeyNames="ID" ShowFooter="true"
           AllowPaging="true" PageIndex="0" PageSize="80" OnRowCommand="gvDept_RowCommand"
           onpageindexchanging="gvDept_PageIndexChanging" 
           onrowdatabound="gvDept_RowDataBound">
      <Columns>
        <asp:BoundField HeaderText="#" DataField="ID" Visible=false />
        <asp:TemplateField HeaderText="Department">
             <ItemTemplate>
              <asp:Label ID="lbRecID" runat="server" Text='<%#Eval("ID") %>' Visible="false"></asp:Label>
              <asp:Label ID="lbID" runat="server" Text='<%#Eval("IndirectBudgetItem.IndirectTypeID") %>' Visible="false"></asp:Label>
                 <asp:Label ID="lbloc1" runat="server" Text='<%#Eval("Department.Name") %>'></asp:Label>
             </ItemTemplate>
             <FooterTemplate>
                <asp:Label ID="lbTot" runat="server" Text="GrandTotal(N'million)"></asp:Label>
             </FooterTemplate>
           </asp:TemplateField>
            <%--<asp:TemplateField HeaderText="IndirectType">
            <ItemTemplate>
                 <asp:Label ID="lbAsstTyp" runat="server" Text='<%#Eval("IndirectBudgetItem.IndirectType.Name") %>'></asp:Label>
             </ItemTemplate>
          </asp:TemplateField>--%>
          <asp:TemplateField HeaderText="Indirect Item">
            <ItemTemplate>
                 <asp:Label ID="lbAsst" runat="server" Text='<%#Eval("IndirectBudgetItem.Name") %>'></asp:Label>
             </ItemTemplate>
          </asp:TemplateField>
           
          <%-- <asp:BoundField HeaderText="Quantity" DataField="Quantity"/>  --%>
           
           <asp:TemplateField HeaderText="Amount(N'million)">
            <ItemTemplate>
                 <asp:Label ID="lbAmt" runat="server" Text='<%#Eval("Amount","{0:N}") %>'></asp:Label>
                 <asp:Label ID="lbATCAmt" runat="server" Text='<%#Eval("ATCAmount","{0:N}") %>' Visible="false"></asp:Label>
             </ItemTemplate>
               <FooterTemplate>
                <asp:Label ID="lbSubTotalFt" runat="server" Text=""></asp:Label>
             </FooterTemplate>
          </asp:TemplateField>
          <asp:TemplateField HeaderText="Justification">
             <ItemTemplate>
                 <asp:Label ID="lblJust" runat="server" Text='<%#Eval("Justification") %>'></asp:Label>
                 <asp:Label ID="lbusr" runat="server" Text='<%#Eval("AddeBy") %>' Visible="false"></asp:Label>
             </ItemTemplate>
           </asp:TemplateField>
           <asp:TemplateField HeaderText="ATC Approved" Visible="false">
             <ItemTemplate>
                  <asp:Label ID="lbatcflg" runat="server" Visible="false" Text='<%#Eval("ATCStatus") %>'></asp:Label>
                 <asp:Label ID="lblatc" runat="server" Text='<%# (Eval("ATCApproved") != null&&Boolean.Parse(Eval("ATCApproved").ToString())) ? "Yes" : "No"%>' ForeColor="Maroon"></asp:Label>
             </ItemTemplate>
           </asp:TemplateField>
          <%-- <asp:BoundField HeaderText="SubTotal(N'000)" DataField="TotalCost" DataFormatString="{0:N}"/>--%>
           <%--<asp:TemplateField HeaderText="SubTotal(N'million)">
            <ItemTemplate>
                 <asp:Label ID="lbSubTotal" runat="server" Text='<%#Eval("TotalCost","{0:N}") %>'></asp:Label>
             </ItemTemplate>
             <FooterTemplate>
                <asp:Label ID="lbSubTotalFt" runat="server" Text=""></asp:Label>
             </FooterTemplate>
          </asp:TemplateField>--%>
          <%-- <asp:TemplateField HeaderText="Month">
             <ItemTemplate>
                 <asp:Label ID="lblMonth" runat="server" Text='<%#Eval("MonthOfYear.MonthName") %>'></asp:Label>
             </ItemTemplate>
           </asp:TemplateField>
            --%>
           <asp:TemplateField HeaderText="Status">
              <ItemTemplate>
                 <asp:Label ID="lbStatus" runat="server" Visible="false" Text='<%#Eval("Status") %>'></asp:Label>
                  <asp:Label ID="lbSt" runat="server" ForeColor=Maroon Text='<%#GetStatus(Eval("Status")) %>'></asp:Label>
              </ItemTemplate>
             </asp:TemplateField>
          
          <asp:TemplateField>
              <HeaderTemplate>
                <asp:Label ID="AtcAmtHeader" Text=" Input ATC Amount ('million)" runat="server" visible="false"/>
            </HeaderTemplate>
              <ItemTemplate>
                  <asp:TextBox ID="txtAtcAmount"  onkeypress="return isNumberKey(event);" runat="server" Width="40px" Visible="false"></asp:TextBox>
              </ItemTemplate>
          </asp:TemplateField>
          <asp:TemplateField>
              <HeaderTemplate>
                <asp:Label ID="lbOutAmtHeader" Text="ATC Outstanding Amount ('million)" runat="server" visible="false"/>
            </HeaderTemplate>
              <ItemTemplate>
                  <asp:Label ID="lbOutAtcAmt" runat="server" Width="40px" Text='<%#GetOutStandingATC(Eval("Amount"),Eval("ATCAmount")) %>' Visible="false"></asp:Label>
              </ItemTemplate>
          </asp:TemplateField>
         <asp:TemplateField>
            <HeaderTemplate>
                <asp:CheckBox ID="chkHeader" Text="All" runat="server" visible="false"/>
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
        
    <asp:GridView ID="gvATC" runat="server" GridLines="None"  Font-Size="11px" EmptyDataText="No record found" Visible="false"
        AutoGenerateColumns="false" CssClass="table table-striped" DataKeyNames="ID" ShowFooter="true"
           AllowPaging="true" PageIndex="0" PageSize="80"  onrowdatabound="gvATC_RowDataBound">
      <Columns>
        <asp:BoundField HeaderText="#" DataField="ID" Visible=false />
        <asp:TemplateField HeaderText="Department">
             <ItemTemplate>
                  <asp:Label ID="lbID" runat="server" Text='<%#Eval("ID") %>' Visible="false"></asp:Label>
              <asp:Label ID="lbRecID" runat="server" Text='<%#Eval("BudgetItemId") %>' Visible="false"></asp:Label>
                 <asp:Label ID="lbloc1" runat="server" Text='<%#Eval("Department.Name") %>'></asp:Label>
             </ItemTemplate>
             <FooterTemplate>
                <asp:Label ID="lbTot" runat="server" Text="GrandTotal(N'million)"></asp:Label>
             </FooterTemplate>
           </asp:TemplateField>
            <asp:BoundField HeaderText="ATC Batch ID" DataField="ATCBatchId"/>  
           <asp:BoundField HeaderText="Budget Item" DataField="BudgetItem"/>  
           <asp:TemplateField HeaderText="ATC Amount(N'million)">
            <ItemTemplate>
                 <asp:Label ID="lbATCAmt" runat="server" Text='<%#Eval("ATCAmount","{0:N}") %>'></asp:Label>
             </ItemTemplate>
               <FooterTemplate>
                <asp:Label ID="lbSubTotalFt" runat="server" Text=""></asp:Label>
             </FooterTemplate>
          </asp:TemplateField>
          <asp:BoundField HeaderText="Request Date" DataField="DateAdded" DataFormatString="{0:dd-MMM-yyyy}"/>  
           <asp:TemplateField HeaderText="Status">
              <ItemTemplate>
                 <asp:Label ID="lbStatus" runat="server" Visible="false" Text='<%#Eval("Status") %>'></asp:Label>
                  <asp:Label ID="lbSt" runat="server" ForeColor=Maroon Text='<%#GetATCStatus(Eval("Status")) %>'></asp:Label>
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
                CssClass="btn btn-submit btn-reject"/>  
      <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" PopupControlID="pnlPopupRej" TargetControlID="btnReject"
                         CancelControlID="btnCloseRej" BackgroundCssClass="modalBackground">
                    </asp:ModalPopupExtender>
                 <asp:Panel ID="pnlPopupRej" runat="server" CssClass="modalPopup" Style="display: none">
                        <div class="header">
                          Add Comments/Reason for the this action
                        </div>
                        <div class="body">
                            <div class="form-horizontal">
                              <div class=" alert alert-error" runat="server" id="Div1" visible="false">
                               <button type="button" class="close" data-dismiss="alert"> &times;</button></div>
                                <div class="control-group">
                                    <label class="control-label" for="txtFeature">
                                       Add Comment:</label>
                                    <div class="controls">
                                         <asp:TextBox ID="txtRejComment" runat="server" TextMode="MultiLine" Rows="4"  CssClass=""></asp:TextBox>
                                    </div>
                                </div>
                               
                                <div class="control-group">
                                    <label class="control-label" for="btnSubmit">
                                    </label>
                                    <div class="controls">
                                        <asp:Button ID="btnRej" runat="server" Text="Reject" CausesValidation="false" CssClass="btn btn-info btn-block"
                                            OnClick="btnReject_Click" />
                                        <asp:Button ID="btnCloseRej" runat="server" Text="Close" CausesValidation="false" CssClass="btn  btn-submit btn-block" />
                                    </div>
                                </div>
                            </div>
                          </div>
                        <div class="footer" align="right">
                        </div>
                    </asp:Panel>             
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
          
 <div class="row-fluid" runat="server" id="dvATC" visible="false">
<div class="page-body-controlbtn">
 <h3>ATC Request Action</h3>
    <asp:Button ID="btnATC" CausesValidation="false" runat="server" Text="Request ATC Selected Record(s)" ToolTip=""
     CssClass="btn btn-submit"   onclick="btnATC_Click" />
          
 </div>
 
</div>

<div class="row-fluid" runat="server" id="dvATCApproval" visible="false">
<div class="page-body-controlbtn">
 <h3>ATC Approval Action</h3>
    <asp:Button ID="btnATCForward" CausesValidation="false" runat="server" Text="Foward ATC Request for MD Approval" ToolTip="" Visible="false"
     CssClass="btn btn-submit"   onclick="btnATCForward_Click" />
    <asp:Button ID="btnATCMDApproval" CausesValidation="false" runat="server" Text="Approve ATC Request" ToolTip="" Visible="false"
     CssClass="btn btn-submit"   onclick="btnATCMDApproval_Click" />  
    <asp:Button ID="btnATCReject" CausesValidation="false" runat="server" Text="Reject ATC Request" ToolTip="" Visible="false"
     CssClass="btn btn-submit btn-reject" /> 
    <asp:ModalPopupExtender ID="mpeATC" runat="server" PopupControlID="pnlPopupATC" TargetControlID="btnATCReject"
                         CancelControlID="btnATClose" BackgroundCssClass="modalBackground">
                    </asp:ModalPopupExtender>
                 <asp:Panel ID="pnlPopupATC" runat="server" CssClass="modalPopup" Style="display: none">
                        <div class="header">
                          Add Comments/Reason for the this action
                        </div>
                        <div class="body">
                            <div class="form-horizontal">
                              <div class=" alert alert-error" runat="server" id="lbatcmsg" visible="false">
                               <button type="button" class="close" data-dismiss="alert"> &times;</button></div>
                                <div class="control-group">
                                    <label class="control-label" for="txtFeature">
                                       Add Comment:</label>
                                    <div class="controls">
                                         <asp:TextBox ID="txtATCcmt" runat="server" TextMode="MultiLine" Rows="4"  CssClass=""></asp:TextBox>
                                    </div>
                                </div>
                               
                                <div class="control-group">
                                    <label class="control-label" for="btnSubmit">
                                    </label>
                                    <div class="controls">
                                        <asp:Button ID="btnATCReturn" runat="server" Text="Decline" CausesValidation="false" CssClass="btn btn-info btn-block"
                                            OnClick="btnATCReject_Click" />
                                        <asp:Button ID="btnATClose" runat="server" Text="Close" CausesValidation="false" CssClass="btn  btn-submit btn-block" />
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
