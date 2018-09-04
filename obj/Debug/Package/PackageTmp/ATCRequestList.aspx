<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ATCRequestList.aspx.cs" Inherits="BudgetCapture.ATCRequestList" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <div class="row-fluid">
     <div class="pageheader-text">Manager ATC requests</div>
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
     <span>ATC Request View. All figures should be in thousands (N'million) </span>
    <li>Budget Year: 
      <asp:Label ID="lbyr" runat="server" Text="" Font-Bold="true" Font-Size="12px"></asp:Label> </li>
      <li>Department: 
      <asp:Label ID="lbDept" runat="server" Text="" Font-Bold="true" Font-Size="12px"></asp:Label> </li>
  </div>
    <asp:HiddenField ID="HiddenField1" runat="server" />
   <div class="inner-header">
    <div class="form-horizontal" runat="server" id="dvFilter" visible="false">
    <div class="control-group">
        <label class="control-label" for="txtStaffID">Filter By Department <span>*</span>:</label>
        <div class="controls">
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
           AllowPaging="true" PageIndex="0" PageSize="100" OnPageIndexChanging="gvDept_PageIndexChanging" OnRowCommand="gvDept_RowCommand"
             OnRowDataBound="gvDept_RowDataBound">
      <Columns>
        <asp:BoundField HeaderText="#" DataField="ID" Visible="false" />
          <asp:TemplateField HeaderText="Batch ID">
             <ItemTemplate>
                 <asp:Label ID="lbProp" runat="server"  Text='<%#Eval("BatchID") %>'></asp:Label>
             </ItemTemplate>
           </asp:TemplateField>
        <asp:TemplateField HeaderText="Budget Type">
             <ItemTemplate>
              <asp:Label ID="lbRecID" runat="server" Text='<%#Eval("ID") %>' Visible="false"></asp:Label>
                 <asp:Label ID="lbloc1" runat="server" Text='<%#Eval("BudgetMenuItem.BudgetItem") %>' ></asp:Label>
             </ItemTemplate>
           </asp:TemplateField>
          <asp:TemplateField HeaderText="Department">
             <ItemTemplate>
                 <asp:Label ID="lbDeptId" runat="server" Text='<%#Eval("DepartmentID") %>' Visible="false"></asp:Label>
              <asp:Label ID="lbDept" runat="server" Text='<%#Eval("Department.Name") %>' Visible="true"></asp:Label>
             </ItemTemplate>
              <FooterTemplate>
                  <asp:Label ID="lbToto" runat="server" Text="Total(N'million)"></asp:Label>
              </FooterTemplate>
           </asp:TemplateField>
        <asp:TemplateField HeaderText="Requested By">
             <ItemTemplate>
              <asp:Label ID="lbReq" runat="server" Text='<%#Eval("RequestBy") %>' Visible="true"></asp:Label>
             </ItemTemplate>
           </asp:TemplateField>
        <asp:TemplateField HeaderText="Total Amount(N'million)">
             <ItemTemplate>
                <asp:Label ID="lbamtpApp" runat="server" CssClass="label label-warning" Text='<%#Eval("Amount","{0:N}") %>'></asp:Label>
             </ItemTemplate>
            <FooterTemplate>
                <asp:Label ID="lbSubTotalFt" runat="server" Text=""></asp:Label>
            </FooterTemplate>
           </asp:TemplateField>
           <asp:TemplateField HeaderText="Request Date">
             <ItemTemplate>
                 <asp:Label ID="lblAppr" runat="server" Text='<%#Eval("RequestDate","{0:dd/MM/yyyy hh:mm:ss}") %>'></asp:Label>
             </ItemTemplate>
           </asp:TemplateField>
           <asp:TemplateField HeaderText="Status">
            <ItemTemplate>
                 <asp:Label ID="lbpApp" runat="server" ForeColor="Maroon" Text='<%#GetStatus(Eval("Status")) %>'></asp:Label>
             </ItemTemplate>
          </asp:TemplateField>
          <asp:TemplateField>
              
            <ItemTemplate>
                <asp:LinkButton ID="imgBtnEdit" CommandName="edt" Text="View Details" CommandArgument='<%#Eval("ID") %>' CausesValidation="false" ToolTip="Edit Item" Visible="true" runat="server" />
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
      <asp:Button ID="btnMD" CausesValidation="false" runat="server" Text="Approve Department ATC" ToolTip="" Visible="false"
     CssClass="btn btn-submit" OnClick="btnMD_Click" />  
     &nbsp;&nbsp;
    <asp:Button ID="btnReject" CausesValidation="false" runat="server" Text="Return Department ATC" ToolTip=""
     CssClass="btn btn-submit btn-reject" OnClick="btnReject_Click" /><%--(<span style="color:maroon">This action is for notification purpose only</span>)--%>
    
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
                                             />
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
