<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/admin.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Safenetpro.Admin.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function loginerror() {
            $(".alert-box").slideDown();
            $(".alert-box").delay(5000).slideUp();
            $("#processingDiv").hide();
        }
        function loginsuccess(userID) {
            sessionStorage.setItem("adminUserId", userID);
            window.location.href = "Customers.aspx";
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
            sessionStorage.setItem("adminUserId", null);
            sessionStorage.setItem("adminUserName", null);
            sessionStorage.setItem("adminFullName", null);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="upPanelMain" runat="server">
        <ContentTemplate>
            <div id="processingDiv" style="display: none;">
                <img src="../img/ajax-loader.gif" class="ajax-loader">
            </div>
            <div class="alert-box error" style="display: none;"><span>error: </span>Username or password is incorrect.</div>
            <div class="container margin-top30 formContainer">
                <div class="row">
                    <div class="col-md-5">
                        <h2>Account Information</h2>
                        <div class="field">
                            <label>User Name: </label>
                            <input type="text" id="txtUserName" runat="server" class="form-control" />
                        </div>
                        <div class="field">
                            <label>Password</label>
                            <input type="password" id="txtPassword" runat="server" class="form-control" />
                        </div>
                    </div>
                </div>

                <div class="action text-right">
                    <button type="submit" style="display: none;" id="btnContinueClick" onserverclick="btnContinueClick_ServerClick" runat="server"></button>
                    <button type="button" id="continue" runat="server" onclick="checkValidators()" class="btn btn-primary">Login</button>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
