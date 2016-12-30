<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="RulePermissions.aspx.cs" Inherits="Safenetpro.RulePermissions" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="description" content="" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="apple-touch-icon" href="apple-touch-icon.png" />

    <link rel="stylesheet" href="../css/bootstrap.min.css" />

    <link rel="stylesheet" href="../css/bootstrap-theme.min.css" />
    <link rel="stylesheet" href="../css/main.css" />

    <script src="../js/vendor/modernizr-2.8.3-respond-1.4.2.min.js"></script>
    <script src="../js/vendor/jquery-1.11.2.min.js"></script>
    <script src="../Typescript/CombineTypescript.js"></script>
    <script>
        $(document).ready(function () {
            RulePermissions.FormFunctions();
            var setHeight = $(this).outerHeight() - 220;
            if (setHeight <= 0)
                setHeight = "450px";

            $("#TabContainer1_body").height(setHeight);
        });

        function removeAllDisplayedActions() {
            RulePermissions.FormFunctions();
            RulePermissions.removeAllDisplayActions();

            var setHeight = $(this).outerHeight() - 220;
            if (setHeight <= 0)
                setHeight = "450px";

            $("#TabContainer1_body").height(setHeight);
        }

        function displayActionChanged(displayId) {
            RulePermissions.addDisplayActions(displayId, $("#TabContainer1_ctl00_" + displayId).val());
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:UpdatePanel ID="upPanelMain" runat="server">
            <ContentTemplate>
                <nav class="navbar navbg" role="navigation">
                    <div class="container">
                        <div class="navbar-header">
                            <a class="navbar-brand" href="#">
                                <img src="../img/logoBlack.png" alt="" /></a>
                        </div>
                        <div class="pull-right margin-top10">
                            <label id="lblWelcomeUser">Welcome Abe</label>
                            <asp:DropDownList ID="ddlCustomers" OnSelectedIndexChanged="ddlCustomers_SelectedIndexChanged" AutoPostBack="true" runat="server" CssClass="form-control"></asp:DropDownList>
                            <asp:DropDownList ID="ddlMasterGroups" AutoPostBack="true" OnSelectedIndexChanged="ddlMasterGroups_SelectedIndexChanged" CssClass="form-control" runat="server"></asp:DropDownList>

                        </div>
                    </div>
                </nav>
                <div>
                    <table style="width: 100%">
                        <tr>
                            <td>
                                <asp:ScriptManager ID="ScriptManager1" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <cc1:TabContainer ID="TabContainer1" runat="server" AutoPostBack="true" OnActiveTabChanged="TabContainer1_ActiveTabChanged"
                                    CssClass="Tab" ActiveTabIndex="0">
                                </cc1:TabContainer>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                                <input type="button" id="btnSave" value="Save" class="inputbtnsave" />
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0">
            <ProgressTemplate>
                <div class="overlay" />
                <div class="overlayContent">
                    &nbsp;<img src="../img/ajax-loader.gif" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </form>
</body>
</html>
