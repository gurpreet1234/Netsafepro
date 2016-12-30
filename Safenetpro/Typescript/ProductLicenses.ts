class ProductLicenses {
    constructor() { }

    public static FormFunctions() {
        $("#setupUpdate0").unbind('click');
        $("#setupUpdate0").click(function () {
            ProductLicenses.updateComputerOrDevices(1);
        });

        $("#setupDeviceUpdate0").unbind('click');
        $("#setupDeviceUpdate0").click(function () {
            ProductLicenses.updateComputerOrDevices(2);
        });

        $("#setupUpdateLicense0").unbind('click');
        $("#setupUpdateLicense0").click(function () {
            ProductLicenses.updateComputerOrDevicesChange(1);
        });

        $("#setupDeviceUpdateLicense0").unbind('click');
        $("#setupDeviceUpdateLicense0").click(function () {
            ProductLicenses.updateComputerOrDevicesChange(2);
        });

        $("#setupCancel0").unbind('click');
        $("#setupCancel0").click(function () {
            $("#hiderProducts").fadeOut("slow");
            $('#computerUpdate').fadeOut("slow");
            $("#ContentPlaceHolder1_btnRefreshData").click();
        });

        $("#setupDeviceCancel0").unbind('click');
        $("#setupDeviceCancel0").click(function () {
            $("#hiderProducts").fadeOut("slow");
            $('#deviceUpdate').fadeOut("slow");
            $("#ContentPlaceHolder1_btnRefreshData").click();
        });
    }

    public static deleteComputerOrDevices(productItemId) {
        $("#processingDiv").show();
        var svcUrl;
        svcUrl = "ProductToUser/" + productItemId;
        var successData = function (response) {
            $("#ContentPlaceHolder1_btnRefreshData").click();
            $("#processingDiv").hide();
            alert("The record has been deleted successfully.");
        };
        var errorData = function (response) {
            $("#processingDiv").hide();
            alert("The record couldn't been deleted.<br />Error message: Internal server error.");
        };
        Utils.DeleteREST(svcUrl, successData, errorData);
    }

    public static editComputerOrDevices(productItemId) {
        $("#processingDiv").show();
        var svcUrl;
        svcUrl = "GetProductToUser/" + productItemId;
        var successData = function (response) {
            $(response).find('ProductToUser_POCO').each(function () {
                $("#hdnProductId").val($(this).find('ProductId').text());
                $("#hdnId").val($(this).find('ID')[0].textContent);
                if (parseInt($(this).find('ProductId').text()) != 4) {
                    $("#lblComputer0").text($(this).find('PrimaryUserName').text());
                    $("#primaryusername0").val($(this).find('PrimaryUserName').text());
                    $("#operatingSystem0").val($(this).find('OperatingSystem').text());
                    $("#location0").val($(this).find('Location').text());
                    $("#ContentPlaceHolder1_settings0").val($(this).find('Settings').text());

                    for (var u = 0; u < $(this).find('URLtoDevice_POCO').length; u++) {
                        if (u > 0)
                            ProductLicenses.addNewURL(0, u - 1);

                        $("#URL_device0" + "_" + u).val($(this).find('URL')[u].textContent);
                    }
                }
                else {
                    $("#lblDevice0").text($(this).find('PrimaryUser').text());
                    $("#operatingSystem_Device0").val($(this).find('PhoneOS').text());
                    $("#manufacturer_device0").val($(this).find('Manufacturer').text());
                    $("#primaryusername_device0").val($(this).find('PrimaryUser').text());
                    $("#ContentPlaceHolder1_settings_device0").val($(this).find('Settings').text());

                    $("#usage_device0").val($(this).find('Usage').text());
                }
                $("#processingDiv").hide();
            });
        };
        var errorData = function (response) {
            alert("Something happened wrong, Please try again later.");
        };
        Utils.GetREST(svcUrl, successData, errorData);
    }

    public static addNewURL(c, val) {
        $("#addURL" + c + "_" + (val)).hide();
        $("#deleteURL" + c + "_" + (val)).hide();

        var str = " <div class=\"field\" id=\"dvURL" + c + "_" + (val + 1) + "\">"
            + " <input style='width:87%; float:left; margin-bottom:5px;' type=\"text\" id=\"URL_device" + c + "_" + (val + 1) + "\" class=\"form-control\">"
            + " <button onclick='addNewURL(" + c + "," + (val + 1) + ")' style='padding:4px; float:left; margin-left:5px;' type=\"button\" id=\"addURL" + c + "_" + (val + 1) + "\" class=\"btn btn-primary\">+</button>"
            + " <button onclick='deleteURL(" + c + "," + (val + 1) + ",\"dvURL" + c + "_" + (val + 1) + "\")' style='padding:4px; float:left; margin-left:5px;' type=\"button\" id=\"deleteURL" + c + "_" + (val + 1) + "\" class=\"btn btn-primary\">-</button>"
            + " </div>"
        $("#dvDevice_" + c).append(str);
    }

    public static updateComputerOrDevices(productType) {
        $("#processingDiv").show();
        var GroupData = "<ProductToUser_POCO xmlns=\"http://schemas.datacontract.org/2004/07/SafenetproAPI.Controllers\" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\">" +
            "<ID>" + $("#hdnId").val() + "</ID>" +
            "<Location>" + $("#location0").val() + "</Location>" +
            "<Manufacturer>" + $("#manufacturer_device0").val() + "</Manufacturer>" +
            "<OperatingSystem>" + $("#operatingSystem0").val() + "</OperatingSystem>" +
            "<PhoneOS>" + $("#operatingSystem_Device0").val() + "</PhoneOS>" +
            "<PrimaryUser>" + $("#primaryusername_device0").val() + "</PrimaryUser>" +
            "<PrimaryUserName>" + $("#primaryusername0").val() + "</PrimaryUserName>" +
            "<ProductId>" + $("#hdnProductId").val() + "</ProductId>";
        if (productType == 1)
            GroupData = GroupData + "<Settings>" + $("#ContentPlaceHolder1_settings0").val() + "</Settings>";
        else
            GroupData = GroupData + "<Settings>" + $("#ContentPlaceHolder1_settings_device0").val() + "</Settings>";

        GroupData = GroupData + "<Settings>" + $("#ContentPlaceHolder1_settings0").val() + "</Settings>" +
        "<Usage>" + $("#usage_device0").val() + "</Usage>";

        if (productType == 1) {
            GroupData = GroupData + "<uDevicePoco>";
            var count = 0;
            $("#dvDevice_0" + " input[type='text']").each(function () {
                GroupData = GroupData + "<URLtoDevice_POCO>" +
                "<ID>" + count + "</ID>" +
                "<ProductToUser>" + $("#hdnId").val() + "</ProductToUser>" +
                "<URL>" + $(this).val() + "</URL>" +
                "</URLtoDevice_POCO>";
                count = count + 1;
            });
            GroupData = GroupData + "</uDevicePoco>";
        }
        GroupData = GroupData + "</ProductToUser_POCO>";

        var svcUrl;
        svcUrl = "ProductToUser/" + $("#hdnId").val();
        var successData = function (response, textStatus, xhr) {
            $("#ContentPlaceHolder1_btnRefreshData").click();
            $("#processingDiv").hide();
            alert("The record has been updated successfully.");
        };
        var errorData = function (response) {
            $("#processingDiv").hide();
            alert("Something happened wrong, Please try again later.");
        };
        Utils.PutREST(svcUrl, GroupData, successData, errorData);
    }

    public static updateComputerOrDevicesChange(productType) {
        $("#processingDiv").show();
        var GroupData = "<ProductToUser_POCO xmlns=\"http://schemas.datacontract.org/2004/07/SafenetproAPI.Controllers\" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\">" +
            "<ID>" + $("#hdnId").val() + "</ID>" +
            "<Location>" + $("#location0").val() + "</Location>" +
            "<Manufacturer>" + $("#manufacturer_device0").val() + "</Manufacturer>" +
            "<OperatingSystem>" + $("#operatingSystem0").val() + "</OperatingSystem>" +
            "<PhoneOS>" + $("#operatingSystem_Device0").val() + "</PhoneOS>" +
            "<PrimaryUser>" + $("#primaryusername_device0").val() + "</PrimaryUser>" +
            "<PrimaryUserName>" + $("#primaryusername0").val() + "</PrimaryUserName>" +
            "<ProductId>" + $("#hdnProductId").val() + "</ProductId>";

        if (productType == 1)
            GroupData = GroupData + "<Settings>" + $("#ContentPlaceHolder1_settings0").val() + "</Settings>";
        else
            GroupData = GroupData + "<Settings>" + $("#ContentPlaceHolder1_settings_device0").val() + "</Settings>";

        GroupData = GroupData + "<Usage>" + $("#usage_device0").val() + "</Usage>";

        if (productType == 1) {
            GroupData = GroupData + "<uDevicePoco>";
            var count = 0;
            $("#dvDevice_0" + " input[type='text']").each(function () {
                GroupData = GroupData + "<URLtoDevice_POCO>" +
                "<ID>" + count + "</ID>" +
                "<ProductToUser>" + $("#hdnId").val() + "</ProductToUser>" +
                "<URL>" + $(this).val() + "</URL>" +
                "</URLtoDevice_POCO>";
                count = count + 1;
            });
            GroupData = GroupData + "</uDevicePoco>";
        }
        GroupData = GroupData + "</ProductToUser_POCO>";

        var userId = sessionStorage.getItem("userId");
        var svcUrl;
        svcUrl = "ProductToUserChange/" + $("#hdnId").val() + "/" + userId;
        var successData = function (response, textStatus, xhr) {
            $("#ContentPlaceHolder1_btnRefreshData").click();
            $("#processingDiv").hide();
            alert("The record has been updated successfully.");
        };
        var errorData = function (response) {
            $("#processingDiv").hide();
            alert("Something happened wrong, Please try again later.");
        };
        Utils.PutREST(svcUrl, GroupData, successData, errorData);
    }

    public static deleteComputerOrDevicesChange(productItemId) {
        $("#processingDiv").show();
        var userId = sessionStorage.getItem("userId");
        var svcUrl;
        svcUrl = "ProductToUserChange/" + productItemId + "/" + userId;
        var successData = function (response) {
            $("#ContentPlaceHolder1_btnRefreshData").click();
            $("#processingDiv").hide();
            alert("The record has been deleted successfully.");
        };
        var errorData = function (response) {
            $("#processingDiv").hide();
            alert("The record couldn't been deleted.<br />Error message: Internal server error.");
        };
        Utils.DeleteREST(svcUrl, successData, errorData);
    }
}