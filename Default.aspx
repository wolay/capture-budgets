<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="BudgetCapture._Default" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="row-fluid">
     <div class="pageheader-text">Budget Capture Home Page</div>
     <div class="alert alert-error" runat="server" id="error" visible="false">  
      <button type="button" class="close" data-dismiss="alert">&times;</button>
     </div>
    <div class="span11 alert alert-success" runat="server" id="success" visible="false">
      <button type="button" class="close" data-dismiss="alert">&times;</button>
  </div>
  </div>

    <div class="row-fluid">
   <div class="page-body-control span12">
        <div class="alert alert-info">
            <span>Budget Capture Summary View. All figures should be in thousands (N'million) </span>
    <li>Budget Year: 
      <asp:Label ID="lbyr" runat="server" Text="" Font-Bold="true" Font-Size="12px"></asp:Label> </li>
      <li>Department: 
      <asp:Label ID="lbDept" runat="server" Text="" Font-Bold="true" Font-Size="12px"></asp:Label> </li>
  </div>
 <div class="alert alert-block la">Budget Metric View
 
 </div>
    <asp:HiddenField ID="HiddenField1" runat="server" />
   <div class="inner-header">
    <div class="form-horizontal" runat="server" id="dvFilter" visible="false">
    <div class="control-group">
        <label class="control-label" for="txtStaffID">Filter By Department <span>*</span>:</label>
        <div class="controls">
            <asp:HiddenField ID="hidEDPending" runat="server" /> <asp:HiddenField ID="hidMDPending" runat="server" />
              <asp:DropDownList ID="ddlDept" runat="server" AppendDataBoundItems="true" 
                  Font-Size="11px">
             <asp:ListItem Value="" Selected="True">...Select Department...</asp:ListItem>
            </asp:DropDownList>
               <asp:Button ID="btnSearch" runat="server" Text="Go" CausesValidation="false" CssClass="btn" ValidationGroup="" 
                    onclick="btnSearch_Click"/>  <asp:Button ID="btnClr" runat="server" 
                    Text="Clr" CssClass="btn" onclick="btnClr_Click" CausesValidation="false" ValidationGroup="" 
                    />
        </div>
        </div>
 </div>
  </div>
    <asp:GridView ID="gvDept" runat="server" GridLines="None"  Font-Size="12px" ShowFooter="true"
        AutoGenerateColumns="false" CssClass="table table-striped table-condensed" DataKeyNames="ID" 
           AllowPaging="true" PageIndex="0" PageSize="50" OnPageIndexChanging="gvDept_PageIndexChanging" OnRowDataBound="gvDept_RowDataBound" OnRowCommand="gvDept_RowCommand"  
            >
      <Columns>
        <asp:BoundField HeaderText="#" DataField="ID" Visible="false" />
        <asp:TemplateField HeaderText="Budget Type">
             <ItemTemplate>
              <asp:Label ID="lbRecID" runat="server" Text='<%#Eval("ID") %>' Visible="false"></asp:Label>
                 <asp:Label ID="lbloc1" runat="server" Text='<%#Eval("BudgetElement") %>' ></asp:Label>
             </ItemTemplate>
           
           </asp:TemplateField>
          <asp:TemplateField>
             <ItemTemplate>
              <asp:Label ID="lbDept" runat="server" Text='<%#Eval("Department.Name") %>' Visible="false"></asp:Label>
             </ItemTemplate>
              <FooterTemplate>
                   <asp:Label ID="lbToto" runat="server" Text="Total(N'million)"></asp:Label>
              </FooterTemplate>
           </asp:TemplateField>
        <asp:TemplateField HeaderText="No of Records">
             <ItemTemplate>
                 <asp:Label ID="lbProp" runat="server" CssClass="label" Text='<%#Eval("TotalNoCapture") %>'></asp:Label>
             </ItemTemplate>
           </asp:TemplateField>
        <asp:TemplateField HeaderText="No of Records Pending Approval">
            <ItemTemplate>
                 <asp:Label ID="lbpApp" runat="server" CssClass="label" Text='<%#Eval("TotalPending") %>'></asp:Label>
             </ItemTemplate>
          </asp:TemplateField>
           <asp:TemplateField HeaderText="No of Records Approved">
             <ItemTemplate>
                 <asp:Label ID="lblAppr" runat="server" CssClass="label" Text='<%#Eval("TotalApproved") %>'></asp:Label>
             </ItemTemplate>
           </asp:TemplateField>
        <asp:TemplateField HeaderText="TotalAmount For PendingRecords (N'million)">
             <ItemTemplate>
                <asp:Label ID="lbamtpApp" runat="server" CssClass="label label-warning" Text='<%#Eval("TotalAmtPending","{0:N}") %>'></asp:Label>
             </ItemTemplate>
           </asp:TemplateField>
            <asp:TemplateField HeaderText="TotalAmount For ApprovedRecords (N'million)">
             <ItemTemplate>
                 <asp:Label ID="lblyear" runat="server" CssClass="label label-success" Text='<%#Eval("TotalAmtApproved","{0:N}") %>'></asp:Label>
             </ItemTemplate>
                <FooterTemplate>
                     <asp:Label ID="lbSubTotalFt" runat="server" Text=""></asp:Label>
                </FooterTemplate>
           </asp:TemplateField>

          <asp:TemplateField>
        <ItemTemplate>
            <asp:LinkButton ID="imgBtnEdit" CommandName="edt" Text="View Details" CommandArgument='<%#Eval("BudgetElement") %>' CausesValidation="false" ToolTip="Edit Item" Visible="false" runat="server" />
         </ItemTemplate>
         </asp:TemplateField>
      </Columns>
        <FooterStyle BackColor="#E0D9BD" Font-Bold="true" ForeColor="Maroon" Font-Size="13px" />
       <SelectedRowStyle BackColor="#E0D9BD" />
    </asp:GridView>
<div class="row-fluid page-body" runat="server" id="dvExco" visible="false">
<div class="page-body-controlbtn">
 <h3>Exco Action</h3>
     <h5>
         <asp:Label ID="lbmsg" runat="server" Text="" ForeColor="Gray"></asp:Label></h5>
    <asp:Button ID="btnCorrect" CausesValidation="false" runat="server" Text="Approve Department Budget" ToolTip="" Visible="false"
     CssClass="btn btn-submit" OnClick="btnCorrect_Click" />         

      <asp:Button ID="btnMD" CausesValidation="false" runat="server" Text="Approve Department Budget" ToolTip="" Visible="false"
     CssClass="btn btn-submit" OnClick="btnMD_Click" />  

     <asp:Button ID="btnMDFinal" CausesValidation="false" runat="server" Text="Final Budget Approval" ToolTip="" Visible="false"
     CssClass="btn btn-submit" OnClick="btnMDFinal_Click" /> 
     &nbsp;&nbsp;
    <asp:Button ID="btnReject" CausesValidation="false" runat="server" Text="Return Department Budget" ToolTip=""
     CssClass="btn btn-submit btn-reject" /><%--(<span style="color:maroon">This action is for notification purpose only</span>)--%>
    
    <p style="color:maroon">Note: By clicking approve button all the budget type in the selected department will be approved.</p>
    
      <asp:ModalPopupExtender ID="mpeAppr" runat="server" PopupControlID="pnlPopupAppr" TargetControlID="btnReject"
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

<div class="row-fluid page-body" runat="server" id="dvDept" visible="false">
    <div class="page-body-controlbtn">
    <h3>HOD Action</h3>
     <h5>
         <asp:Label ID="lbdeptmsg" runat="server" Text="" ForeColor="Gray"></asp:Label></h5>
    <asp:Button ID="btnForward" CausesValidation="false" runat="server" Text="Forward Budget To ED For Approval" ToolTip=""  
     CssClass="btn btn-submit" OnClick="btnForward_Click" /> 
     <p style="color:maroon">Note: By clicking approve button all the budget type in the selected department will be approved.</p>
        </div>
</div>

<div class="row-fluid page-body" runat="server" id="dvPB" visible="false">
    <div class="page-body-controlbtn">
    <h3>Budget&Planning Action</h3>
     <h5>
         <asp:Label ID="lbPBmsg" runat="server" Text="" ForeColor="Gray"></asp:Label></h5>
    <asp:Button ID="btnPBForward" CausesValidation="false" runat="server" Text="Forward Budget To MD For Final Approval" ToolTip=""  
     CssClass="btn btn-submit" OnClick="btnPBForward_Click" /> 
     <p style="color:maroon">Note: By clicking approve button all the budget type in the selected department will be approved.</p>
        </div>
</div>
</div>
 
</div>


</asp:Content>
