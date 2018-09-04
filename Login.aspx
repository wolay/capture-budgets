<%@ Page Language="C#" Title="Login Page" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="BudgetCapture.Login" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=9"/>
    <title><%: Page.Title %> - BudgetCapture</title>
    <link href="css/style.css" rel="stylesheet" /> 
    <link href="favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="css/bootstrap.css" rel="stylesheet" />
    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/ModelPopup.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-1.8.2.js"></script>
    <meta name="viewport" content="width=device-width" />
</head>
<body>
   <form id="form1" runat="server">
    <div class="container">
     <div class="navbar navbar-inverse navbar-fixed-top nav-header-bg-login">
      <div class="navbar-inner-updated">
        <div class="container-fluid container-fluid-fixedwidth">
          <button type="button" class="btn btn-navbar" data-toggle="collapse" data-target=".nav-collapse">
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
          </button>
          <a class="brand appName" href="#">
              <img src="~/img/ngclogo.png" runat="server" width="80" height="69" />  ** Budget Capture **</a>
          <div class="nav-collapse collapse"
        
          </div><!--/.nav-collapse -->

        </div>
      </div>
    </div>
    <div class="row">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="span4 offset6 well">
			<legend>Please Sign In</legend>
          	<div class="alert alert-error" runat="server" id="error" visible="false">
            <button type="button" class="close" data-dismiss="alert">
                &times;</button>
        </div>
     <div class=" alert alert-success" runat="server" id="success" visible="false">
            <button type="button" class="close" data-dismiss="alert">
                &times;</button>
           
      </div>
			<div>Username:<span class="require">*</span>  <asp:RequiredFieldValidator ID="Reqlg" runat="server" Font-Size="9px" ErrorMessage="Required" ForeColor="Maroon" Display="Dynamic" ControlToValidate="username" ></asp:RequiredFieldValidator></div>
            <input type="text" id="username" class="span4" name="username" placeholder="Username" runat="server">	 
          
            <div>Password:<span class="require">*</span><asp:RequiredFieldValidator ID="Reqpwd1" runat="server" Font-Size="9px" ForeColor="Maroon" ErrorMessage="Required" Display="Dynamic" ControlToValidate="password" ></asp:RequiredFieldValidator></div>
            <input type="password" id="password" class="span4" name="password" runat="server" placeholder="Password">
   		    
			
            <%--<label class="checkbox">
            	<input type="checkbox" name="remember" value="1"> Remember Me
            </label>--%>
			<asp:Button runat="server" ID="btnSubmit" class="btn btn-info btn-block" 
                Text="Sign in" onclick="btnSubmit_Click"/>
            <div class="alert alert-msg">Kindly sign-in with your Window's Credentials</div> 
            <div runat="server" id="popDv" visible="true"></div>
             <asp:ModalPopupExtender ID="mpeAppr" runat="server" PopupControlID="pnlPopupAppr" TargetControlID="popDv"
                         CancelControlID="btnCloseAppr" BackgroundCssClass="modalBackground">
                    </asp:ModalPopupExtender>
                 <asp:Panel ID="pnlPopupAppr" runat="server" CssClass="modalPopup" Style="display: none">
                        <div class="header">
                          Welcome to Budget Capture:: Kindly select Budget Year and Proceed
                        </div>
                        <div class="body">
                            <div class="form-horizontal">
                              <div class=" alert alert-error" runat="server" id="modalErr" visible="false">
                               <button type="button" class="close" data-dismiss="alert"> &times;</button></div>
                                <div class="control-group">
                                    <label class="control-label" for="txtFeature">
                                        Select Budget Year:</label>
                                    <div class="controls">
                                        <asp:DropDownList ID="ddlBudgetYr" runat="server" AppendDataBoundItems="true">
                                          <asp:ListItem Value="" Selected="True">...Select Year...</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                               
                                <div class="control-group">
                                    <label class="control-label" for="btnSubmit">
                                    </label>
                                    <div class="controls">
                                        <asp:Button ID="btnAppr" runat="server" Text="Proceed" CausesValidation="false" CssClass="btn btn-info btn-block"
                                            OnClick="btnAppr_Click" />
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
    </form>
</body>
</html>
