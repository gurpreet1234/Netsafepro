<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="MyLicenses.aspx.cs" Inherits="Safenetpro.MyLicenses" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="js/vendor/jquery-1.11.2.min.js"></script>
    <script src="js/setup.js"></script>
    <script src="Typescript/CombineTypescript.js"></script>
    <script type="text/javascript">

        $(document).ready(function () {
            formFunctions();
        });
        function formFunctions() {
            ProductLicenses.FormFunctions();
        }
        function deleteAlert(productItemId, productType) {
            var delRecord = confirm("Are you sure you want to delete?");
            if (delRecord) {
                ProductLicenses.deleteComputerOrDevicesChange(productItemId);
            }
        }

        function editProduct(productItemId, productType) {
            ProductLicenses.editComputerOrDevices(productItemId);

            $("#hiderProducts").fadeIn("slow");
            if (productType != 4)
                $('#computerUpdate').fadeIn("slow");
            else
                $('#deviceUpdate').fadeIn("slow");
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="upPanelMain" runat="server">
        <ContentTemplate>
            <div id="processingDiv" style="display: none;">
                <img src="img/ajax-loader.gif" class="ajax-loader">
            </div>
            <div class="container margin-top30 formContainer">
                <div class="pull-right" style="clear: both; margin-top: 10px;">
                </div>
                <h1>Your Licenses</h1>

                <div id="dvMain" runat="server">
                </div>
                <div style="float: right;">
                    <h5>TOTALS</h5>
                </div>
                <div style="clear: both; float: right; margin-top: 10px;">
                    <span class="yearly">Yearly Subtotal:<label style="margin-left: 35px;" id="lblYearlyPayment" runat="server">$0</label></span>
                </div>
                <div style="clear: both; float: right;">
                    <span class="monthly">Monthly Total:<label style="margin-right: 11px; margin-left: 25px;" id="lblMonthlyPayment" runat="server">$0</label></span>
                </div>

                <div style="clear: both; margin-top: 30px; border: 1px solid #d7d7d7;"></div>
            </div>
            <button style="display: none;" id="btnRefreshData" type="submit" value="Submit" runat="server" onserverclick="btnRefreshData_ServerClick"></button>


            <div id="hiderProducts" style="display: none;"></div>
            <div class="container margin-top30 formContainer" id="computerUpdate" style="display: none; background-color: #e3e3e3; border-radius: 10px;">
                <h3>
                    <label id="lblComputer0">SET UP A COMPUTER</label><img class='setupImage' src="img/desktop.png" alt=''></h3>
                <div class="row">
                    <div class="col-md-5" style="width: 100%;">
                        <div class="field">
                            <label>Operating System</label>
                            <select class="form-control" id="operatingSystem0">
                                <option value='0'>Select</option>
                                <option value='Windows 7 32'>Windows 7 32</option>
                                <option value='Windows 7 64'>Windows 7 64</option>
                                <option value='Windows 8/10'>Windows 8/10</option>
                                <option value='Windows 8/10 32bit'>Windows 8/10 32bit</option>
                            </select>
                        </div>
                        <div class="field">
                            <label>Primary UserName</label>
                            <input type="text" id="primaryusername0" class="form-control">
                        </div>
                        <div class="field">
                            <label>Location</label>
                            <select class="form-control" id="location0">
                                <option value='0'>Select</option>
                                <option value='Home'>Home</option>
                                <option value='Office'>Office</option>
                            </select>
                        </div>
                       <div class="field">
                            <label>Filter Settings</label>
                            <select class="form-control" id="settings0" runat="server">
                            </select>
                        </div>
                        <div class="field" id="dvDevice_0">
                            <label>URL</label>
                            <div class="field" id="dvURL0_0">
                                <input style='width: 87%; float: left; margin-bottom: 5px;' type="text" id="URL_device0_0" class="form-control">
                                <button onclick='addNewURL(0,0)' style='padding: 4px; float: left; margin-left: 5px;' type="button" id="addURL0_0" class="btn btn-primary">+</button>
                                <button onclick='deleteURL(0,0,"dvURL0_0")' style='padding: 4px; float: left; margin-left: 5px;' type="button" id="deleteURL0_0" class="btn btn-primary">-</button>
                            </div>
                        </div>
                        <div class="field" style="clear: both;">
                            <button type="button" id="setupUpdateLicense0" class="btn btn-darkBlue"><i class="customIcon icon-yes"></i>Update</button>
                            <button type="button" id="setupCancel0" class="btn btn-darkBlue"><i class="customIcon icon-yes"></i>Cancel</button>
                        </div>
                    </div>
                </div>
            </div>



            <div class="container margin-top30 formContainer" id="deviceUpdate" style="display: none; background-color: #e3e3e3; border-radius: 10px;">
                <h3>
                    <label id="lblDevice0">SET UP A DEVICE</label><img class='setupImage' src="img/iPhone.png" alt=''></h3>
                <input type="hidden" id="hdnProductId" />
                <input type="hidden" id="hdnId" />
                <div class="row">
                    <div class="col-md-5" style="width: 100%;">
                        <div class="field">
                            <label style='float: left;'>Operating System</label>
                            <select class="form-control" id="operatingSystem_Device0">
                                <option value='0'>Select</option>
                                <option value='IOS'>IOS</option>
                                <option value='Android'>Android</option>
                            </select>
                        </div>
                        <div class="field" style="display: none;">
                            <label>Manufacturer</label>
                            <select class="form-control" id="manufacturer_device0">
                                <option value='0'>Select</option>
                            </select>
                        </div>
                        <div class="field">
                            <label>Primary UserName</label>
                            <input type="text" id="primaryusername_device0" class="form-control">
                        </div>
                        <div class="field">
                            <label>Usage</label>
                            <select class="form-control" id="usage_device0">
                                <option value='0'>Select</option>
                                <option value='Personal'>Personal</option>
                                <option value='Business'>Business</option>
                            </select>
                        </div>
                        <div class="field">
                            <label>Filter Settings</label>
                            <select class="form-control" id="settings_device0" runat="server">
                            </select>
                        </div>
                        <div class="field" style="clear: both;">
                            <button type="button" id="setupDeviceUpdateLicense0" class="btn btn-darkBlue"><i class="customIcon icon-yes"></i>Update</button>
                            <button type="button" id="setupDeviceCancel0" class="btn btn-darkBlue"><i class="customIcon icon-yes"></i>Cancel</button>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
