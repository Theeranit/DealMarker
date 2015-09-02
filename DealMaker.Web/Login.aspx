<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="KK.DealMaker.Web.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <title>Kiatnakin - Deal Maker System v1.3</title>
    <link href="Styles/style.css" rel="stylesheet" type="text/css" />
    <link href="Styles/cupertino/jquery-ui-1.10.0.custom.css" rel="stylesheet" type="text/css" />

    <script src="Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery.html5-placeholder.js" type="text/javascript"></script>
    
    <script src="Scripts/jquery.validate.js" type="text/javascript"></script>
    <script src="Scripts/jquery.blockUI.js" type="text/javascript"></script>
    <script src="Scripts/jquery.common.js" type="text/javascript"></script>
    <script src="Scripts/login.js" type="text/javascript"></script>
    <!-- Optimize for mobile devices -->
	<meta name="viewport" content="width=device-width, initial-scale=1.0"/>  
</head>
<body>
    <!-- TOP BAR -->
	<div id="top-bar">
		
		<div class="page-full-width">
		
			<a href="#" class="round button dark ic-left-arrow image-left ">Return to website</a>

		</div> <!-- end full-width -->	
	
	</div> <!-- end top-bar -->
	
	
	
	<!-- HEADER -->
	<div id="header">
		
		<div class="page-full-width cf">
	
			<div id="login-intro" class="fl">
			
				<h1>Login to Deal Maker System</h1>
				<h5>Enter your credentials below</h5>
			
			</div> <!-- login-intro -->
			
			<!-- Change this image to your own company's logo -->
			<!-- The logo will automatically be resized to 39px height. -->
			<a href="#" id="company-branding" class="fr"><img src="images/kklogo_new.jpg" alt="Kiatnakin" /></a>
			
		</div> <!-- end full-width -->	

	</div> <!-- end header -->
	
	
	
	<!-- MAIN CONTENT -->
	<div id="content">
	
		<form id="loginform" runat="server">
		
			<fieldset>

				<p>
					<label for="login-username">username</label>
					<%--<input type="text" id="loginusername" class="round full-width-input required" autofocus />--%>
                    <asp:TextBox ID="UserName" runat="server" CssClass="round full-width-input required"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" 
                            CssClass="failureNotification" ErrorMessage="User Name is required." ToolTip="User Name is required." 
                            ValidationGroup="LoginUserValidationGroup">User Name is required.</asp:RequiredFieldValidator>
				</p>

				<p>
					<label for="login-password">password</label>
					<%--<input type="password" id="loginpassword" class="round full-width-input required" />--%>
                    <asp:TextBox ID="Password" runat="server" CssClass="round full-width-input required" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" 
                            CssClass="failureNotification" ErrorMessage="Password is required." ToolTip="Password is required." 
                            ValidationGroup="LoginUserValidationGroup">Password is required.</asp:RequiredFieldValidator>
				</p>
				
				<%--<p>I've <a href="#">forgotten my password</a>.</p>--%>
				
                <%--<input type="submit" id="login-button" value="LOG IN" class="button round blue image-right ic-right-arrow" />--%>
                <asp:Button ID="LoginButton" runat="server" Text="Log In" 
                    CssClass="button round blue image-right ic-right-arrow" 
                    onclick="LoginButton_Click" />
			</fieldset>

			<br/>
            <asp:Label ID="lblInfo" runat="server"></asp:Label>
            <asp:Label ID="lblErrorMessage" runat="server"></asp:Label>
            <!--<div id="error-label" class="error-box round" style="display:none;">
                <asp:Label ID="lblErrorMessgae" runat="server"></asp:Label>
            </div>-->
            <div id="warning-label" class="warning-box round" style="display:none;"></div>
		</form>
		
	</div> <!-- end content -->
	
	
	
	<!-- FOOTER -->
	<div id="footer">

		<p>&copy; Copyright 2013 <a href="http://www.kiatnakin.co.th">Kiatnakin Bank.,co.ltd</a>. All rights reserved.</p>
		<p>Develop by iShaMan</p>
	</div> <!-- end footer -->
</body>
</html>
