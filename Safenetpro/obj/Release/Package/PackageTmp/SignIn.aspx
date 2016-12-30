<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="SignIn.aspx.cs" Inherits="Safenetpro.SignIn" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="Typescript/CombineTypescript.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($(document).height() > $(window).height()) {
                $('footer').css('position', 'relative')
            } else {
                $('footer').css('position', 'absolute')
            }
            $("#showforgotpasswordpopup").click(function () {
                $("#emailSection").show();
                $("#emailSent").hide();
                $("#sendButton").show();
                $("#txtForgotEmailAddress").val('');

                $("#hiderpassword").fadeIn("slow");
                $('#popup_boxpassword').fadeIn("slow");
            });
            $("#buttonClosepassword").click(function () {
                $("#hiderpassword").fadeOut("slow");
                $('#popup_boxpassword').fadeOut("slow");
            });

            $("footer").css("bottom", "0");
            $("footer").css("position", "absolute");
            $("footer").css("width", "100%");

        });

        function loginerror() {
            $(".alert-box").slideDown();
            $(".alert-box").delay(5000).slideUp();
            $("#processingDiv").hide();
        }
        function loginsuccess(userID) {
            sessionStorage.setItem("userId", userID);
            window.location.href = "Setup.aspx";
        }
        function checkNewUserValidators() {
            var validForm = true;
            var firstField = "";
            var validationFields = [];
            validationFields.push("ContentPlaceHolder1_txtNewUserName");
            validationFields.push("ContentPlaceHolder1_txtNewUserPwd");

            $(".red").hide();
            for (var v = 0; v < validationFields.length; v++) {
                $("#" + validationFields[v]).css('border-color', '');
                $("#" + validationFields[v]).css('background', '');
                if ($("#" + validationFields[v]).val() == "") {
                    $("#" + validationFields[v]).css('border-color', '#c0392b');
                    $("#" + validationFields[v]).css('background', '#FCDACD');
                    if (firstField == "")
                        firstField = validationFields[v];
                    validForm = false;
                }
            }
            if (validForm == true) {
                $("#processingDiv").show();
                $("#ContentPlaceHolder1_btnNewuser").click();
            }
            else {
                $('html,body').animate({
                    scrollTop: $("#" + firstField).offset().top - 180
                }, 'slow');
                $("#" + firstField).focus();
            }
        }
        function checkValidators() {
            var validForm = true;
            var firstField = "";
            var validationFields = [];
            validationFields.push("ContentPlaceHolder1_txtUserName");
            validationFields.push("ContentPlaceHolder1_txtPassword");

            $(".red").hide();
            for (var v = 0; v < validationFields.length; v++) {
                $("#" + validationFields[v]).css('border-color', '');
                $("#" + validationFields[v]).css('background', '');
                if ($("#" + validationFields[v]).val() == "") {
                    $("#" + validationFields[v]).css('border-color', '#c0392b');
                    $("#" + validationFields[v]).css('background', '#FCDACD');
                    if (firstField == "")
                        firstField = validationFields[v];
                    validForm = false;
                }
            }

            if (validForm == true) {
                $("#processingDiv").show();
                $("#ContentPlaceHolder1_btnContinueClick").click();
            }
            else {
                $('html,body').animate({
                    scrollTop: $("#" + firstField).offset().top - 180
                }, 'slow');
                $("#" + firstField).focus();
            }
        }

        function logout() {
            sessionStorage.setItem("userId", null);
            sessionStorage.setItem("fullName", null);
            sessionStorage.removeItem("userId");
            sessionStorage.removeItem("fullName");
        }

        function checkuserExists() {
            var emailAddress = $("#ContentPlaceHolder1_txtForgotEmailAddress").val();
            Users.checkUserNameAvailablityForgotpassword(emailAddress);
        }

        function newUser() {
            $("#divLoginuser").hide();
            $("#divNewUser").show();
        }
        function loginUser() {
            $("#divLoginuser").show();
            $("#divNewUser").hide();
        }
        function checkUserAvailability() {
            var userName = $("#ContentPlaceHolder1_txtNewUserName").val();
            Users.checkUserNameAvailablity(userName, sessionStorage.getItem("userId"));
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="upPanelMain" runat="server">
        <ContentTemplate>

            <div id="hiderpassword" style="display: none;"></div>
            <div class="container margin-top30 formContainer" id="popup_boxpassword" style="display: none; background-color: #e3e3e3; border-radius: 10px; padding: 0px;">
                <div class="popupHeader">
                    <ul class="nav navbar-nav" id="ulProducts">
                        <li style="width: 87%;"><a href="#">Don't know your password?</a></li>
                        <li style="width: 11%;"><a id="buttonClosepassword" style="cursor: pointer; float: right;">X</a></li>
                    </ul>
                </div>
                <div id="productsList" style="padding: 10px;">
                    <div class="field" id="emailSection">
                        <label>Email: </label>
                        Enter your email address and we'll send you a link to set your password.
                            <input type="email" id="txtForgotEmailAddress" placeholder="Email Address" runat="server" class="form-control" />
                        <label id="lblEmailNotExists" style="display: none; color: #c0392b;">Email not exist in system.</label>
                    </div>

                    <div class="field" id="emailSent" style="display: none;">
                        <label>Request received: </label>
                        <br />
                        An email has been sent with a link to reset your password.
                    </div>
                </div>
                <div class="action text-right" id="sendButton">
                    <button type="button" id="btn" runat="server" onclick="checkuserExists()" class="btn btn-primary">Send</button>
                </div>
            </div>

            <div id="processingDiv" style="display: none;">
                <img src="img/ajax-loader.gif" class="ajax-loader">
            </div>
            <div class="alert-box error" style="display: none;"><span>error: </span>Username or password is incorrect.</div>
            <div class="container margin-top30 formContainer" id="divLoginuser" style="margin-top: 6%; margin-left: 16%;">
                <div class="row">
                    <div class="col-md-5">
                        <div class="module-nav">
                            <ul class="zsg-tabs zsg-tabs_lg">
                                <li class="zsg-tab_active"><a>Sign in</a></li>
                                <li>
                                    <a onclick="newUser()">New account</a>
                                </li>
                            </ul>
                        </div>
                        <div class="field">
                            <label>User Name: </label>
                            <input type="text" autocomplete="off" id="txtUserName" runat="server" class="form-control" />
                        </div>
                        <div class="field">
                            <label>Password</label>
                            <input type="password" autocomplete="off" id="txtPassword" runat="server" class="form-control" />
                            <label id="lblForgotPassword" style="color: #c0392b;"><a id="showforgotpasswordpopup" href="#">Don't know your password?</a></label>
                        </div>
                        <div class="action text-right">
                            <button type="submit" style="display: none;" id="btnContinueClick" onserverclick="btnContinueClick_ServerClick" runat="server"></button>
                            <button type="button" id="continue" runat="server" onclick="checkValidators()" class="btn btn-primary">Sign in</button>
                            <%--<button type="button" id="register" onclick="document.location.href='SignUp.aspx';" runat="server" class="btn btn-primary">Submit</button>--%>
                        </div>
                    </div>
                </div>
            </div>




            <div class="container margin-top30 formContainer" id="divNewUser" style="display: none; margin-top: 6%; margin-left: 16%;">
                <div class="row">
                    <div class="col-md-5">
                        <div class="module-nav">
                            <ul class="zsg-tabs zsg-tabs_lg">
                                <li><a onclick="loginUser()">Sign in</a></li>
                                <li class="zsg-tab_active">
                                    <a>New account</a>
                                </li>
                            </ul>
                        </div>
                        <div class="field">
                            <label>User Name: </label>
                            <input type="text" onblur="checkUserAvailability()" id="txtNewUserName" runat="server" class="form-control" />
                            <label id="lblAlreadyExists" style="display: none; color: #c0392b;">Someone already has that username. Try another?</label>
                        </div>
                        <div class="field">
                            <label>Create Password</label>
                            <input type="password" id="txtNewUserPwd" runat="server" class="form-control" />
                        </div>
                        <div class="action text-right">
                            <button type="button" style="display: none;" id="btnNewuser" onserverclick="btnNewuser_ServerClick" runat="server"></button>
                            <button type="button" id="btnRegisterNewUser" runat="server" onclick="checkNewUserValidators()" class="btn btn-primary">Submit</button>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
