<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Setup.aspx.cs" Inherits="Safenetpro.Setup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="js/vendor/jquery-1.11.2.min.js"></script>
    <script src="js/setup.js"></script>
    <script src="Typescript/CombineTypescript.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            Setup.FormFunctions();
        });
        function continueSetup() {
            //if (confirm("Unsaved changes will be disacrd!") == true) {
            Setup.saveComputerOrDevices();
            //}            
        }
        function checkUserNameAvailability(txtValue, c) {
            Setup.checkPrimaryNameAvailablity(txtValue, sessionStorage.getItem("userId"), c);

            //$("#setupYes" + c).attr("disabled", true);
            //$("#setupNo" + c).attr("disabled", true);

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="upPanelMain" runat="server">
        <ContentTemplate>
            <div id="processingDiv" style="display: none;">
                <img src="img/ajax-loader.gif" class="ajax-loader">
            </div>
            <div id="dvDynamicSetup" runat="server">
            </div>
            <div class="container margin-top30 formContainer" id="setup" runat="server">
                <h1>Set up a Computer/Device</h1>
                <p>Please select if you would like to set up a computer/laptop or another type of device:</p>
                <!-- Example row of columns -->
                <div class="row">

                    <div class="col-sm-3">
                        <a href="#" runat="server" id="setupComputer" onserverclick="setupComputer_ServerClick">
                            <div class="prodBox">
                                <p>
                                    <img src="img/desktop.png" alt="">
                                </p>
                                <p>Set up Computer</p>
                            </div>
                        </a>
                    </div>

                    <div class="col-sm-3">
                        <a href="#" runat="server" id="setupDevice" onserverclick="setupDevice_ServerClick">
                            <div class="prodBox">
                                <p>
                                    <img src="img/iPhone.png" alt="">
                                </p>
                                <p>Set up Device</p>
                            </div>
                        </a>
                    </div>

                    <div class="col-sm-3"></div>
                    <div class="col-sm-3"></div>

                </div>

            </div>

            <div id="dvWelcome" runat="server">
                <div class="container">
                    <div class="action text-right">
                        <button type="button" runat="server" onclick="continueSetup()" id="Button1" class="btn btn-primary">Continue</button>
                    </div>

                </div>
            </div>

            <%--<div class="action text-right" style="width: 88%; margin-left: 8%;">
                <button type="button" runat="server" onclick="continueSetup()" id="continueRegistration" class="btn btn-primary">Continue</button>
            </div>--%>
            <button type="submit" style="display: none;" onserverclick="setupYes_ServerClick" id="yesTemp" runat="server"></button>
            <button type="submit" style="display: none;" onserverclick="setupNo_ServerClick" id="noTemp" runat="server"></button>
        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>
