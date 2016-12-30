<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="Safenetpro.ResetPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="Typescript/CombineTypescript.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            
        });
        function checkNewUserValidators() {
            var validForm = true;
            var firstField = "";
            var validationFields = [];
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
                $("#ContentPlaceHolder1_btnResetPassword").click();
            }
            else {
                $('html,body').animate({
                    scrollTop: $("#" + firstField).offset().top - 180
                }, 'slow');
                $("#" + firstField).focus();
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="upPanelMain" runat="server">
        <ContentTemplate>
            <div class="alert-box error" style="display: none;" id="errorMessage" runat="server"><span>error: </span>Link expired.</div>
            <div class="container margin-top30 formContainer" id="divNewUser" style="margin-top: 6%; margin-left: 16%;">
                <div class="row">
                    <div class="col-md-5">
                        <div class="module-nav">
                            <ul class="zsg-tabs zsg-tabs_lg">
                                <li class="zsg-tab_active"><a>Reset Password</a></li>
                            </ul>
                        </div>
                        <div class="field">
                            <label>New Password</label>
                            <input type="password" id="txtNewUserPwd" runat="server" class="form-control" />
                        </div>
                        <div class="action text-right">
                            <button type="button" style="display: none;" id="btnResetPassword" onserverclick="btnResetPassword_ServerClick" runat="server"></button>
                            <button type="button" id="btnRegisterNewUser" runat="server" onclick="checkNewUserValidators()" class="btn btn-primary">Submit</button>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
