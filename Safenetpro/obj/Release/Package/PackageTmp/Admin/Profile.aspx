<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/admin.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="Safenetpro.Admin.Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function checkValidators() {
            $("#continueRegistration").attr("disabled", "disabled");
            var validForm = true;
            var firstField = "";
            var validationFields = [];
            validationFields.push("ContentPlaceHolder1_txtlName");
            validationFields.push("ContentPlaceHolder1_txtfname");
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
                $("#ContentPlaceHolder1_btnhiddenServerClick").click();
            }
            else {
                $('html,body').animate({
                    scrollTop: $("#" + firstField).offset().top - 180
                }, 'slow');
                $("#" + firstField).focus();
                $("#continueRegistration").removeAttr("disabled");
            }
        }
        function signupsuccess(userID) {
            sessionStorage.setItem("adminUserId", userID);
            window.location.href = "Customers.aspx";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="processingDiv" style="display: none;">
        <img src="../img/ajax-loader.gif" class="ajax-loader">
    </div>
    <div class="container margin-top30 formContainer" style="margin: 0px; width: 100%;">
        <h1>Profile</h1>

        <!-- Example row of columns -->
        <div class="row">
            <div class="col-md-5">
                <h2>Personal Information</h2>
                <div class="field">
                    <label>Last Name</label>
                    <input type="text" id="txtlName" runat="server" class="form-control" />
                </div>
                <div class="field">
                    <label>First Name</label>
                    <input type="text" id="txtfname" runat="server" class="form-control" />
                </div>
            </div>

            <div class="col-md-5">
                <h2>Account Information</h2>
                <div class="field">
                    <label>User Name: </label>
                    <input type="text" onblur="checkUserAvailability()" id="txtUserName" runat="server" class="form-control" />
                    <label id="lblAlreadyExists" style="display: none; color: #c0392b;">User name is already exist.</label>
                </div>
                <div class="field">
                    <label>Password</label>
                    <input type="password" id="txtPassword" runat="server" class="form-control" />
                </div>
            </div>
        </div>

        <div class="action text-right">
            <button type="submit" style="display: none;" onserverclick="btnhiddenServerClick_ServerClick" id="btnhiddenServerClick" runat="server"></button>
            <button type="button" onclick="checkValidators()" id="continueRegistration" class="btn btn-primary">Update Profile</button>
        </div>
    </div>
</asp:Content>
