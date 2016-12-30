function setupEdit(type, c) {
    $("#setupYes" + c).hide();
    $("#setupNo" + c).hide();
    $("#setupEdit" + c).hide();
    $("#setupUpdate" + c).show();
    $("#setupDelete" + c).show();

    if (type == 1) {
        $("#primaryusername" + c).removeAttr("disabled");
        $("#operatingSystem" + c).removeAttr("disabled");
        $("#location" + c).removeAttr("disabled");
        $("#settings" + c).removeAttr("disabled");

        var productURL = Setup.getURLsWithRow(1, c);
        for (var u = 0; u < productURL.length; u++) {
            $("#URL_device" + c + "_" + u).removeAttr("disabled");
            $("#addURL" + c + "_" + u).removeAttr("disabled");
            $("#deleteURL" + c + "_" + u).removeAttr("disabled");
        }
    }
    if (type == 2) {
        $("#operatingSystem_Device" + c).removeAttr("disabled");
        $("#manufacturer_device" + c).removeAttr("disabled");
        $("#primaryusername_device" + c).removeAttr("disabled");
        $("#usage_device" + c).removeAttr("disabled");
    }
}
function setupUpdate(type, c) {
    var validForm = clearBackground(type, c);
    if (validForm == true) {
        if (type == 1) {
            var productURL = Setup.getURLsWithRow(1, c);
            Setup.updateComputerorDevice($("#operatingSystem" + c).val(), $("#location" + c).val(), $("#primaryusername" + c).val(), $("#settings" + c).val(), "", "", "", "", 1, c);
            $("#dvDevice_" + c + " input[type='text']").each(function () {
                Setup.updateURLDevice(1, $(this).val(), c);
            });
        }
        if (type == 2) {
            Setup.updateComputerorDevice("", "", "", "", $("#operatingSystem_Device" + c).val(), $("#manufacturer_device" + c).val(), $("#primaryusername_device" + c).val(), $("#usage_device" + c).val(), 2, c);
        }
        clearpreviousbuttons(Setup.setupComputerDeviceList.length);
    }
}
function setupDelete(type, c) {
    if (type == 1) {
        $("#computer" + c).detach();
        Setup.deleteComputerorDevice(1, c);
    }
    if (type == 2) {
        $("#device" + c).detach();
        Setup.deleteComputerorDevice(2, c);
    }
}

function setupYes(type, c) {
    var validForm = clearBackground(type, c);
    if (validForm == true) {
        if (!$("#ContentPlaceHolder1_setup").is(':visible')) {
            if (type == 1) {
                Setup.addComputerorDevice($("#operatingSystem" + c).val(), $("#location" + c).val(), $("#primaryusername" + c).val(), $("#settings" + c).val(), "", "", "", "", 1, c);
                $("#dvDevice_" + c + " input[type='text']").each(function () {
                    Setup.addURLDevice($(this).val());
                });
            }
            if (type == 2) {
                Setup.addComputerorDevice("", "", "", $("#settings_device" + c).val(), $("#operatingSystem_Device" + c).val(), $("#manufacturer_device" + c).val(), $("#primaryusername_device" + c).val(), $("#usage_device" + c).val(), 4, c);
            }
        }
        $("#ContentPlaceHolder1_setup").show();
        $('html,body').animate({
            scrollTop: $("#ContentPlaceHolder1_setup").offset().top
        }, 'slow');
        clearpreviousbuttons(c + 1);
    }
}
function setupNo(type, c) {
    var validForm = clearBackground(type, c);
    if (validForm == true) {
        var formValidate = true;
        var focusField = "";
        if (type == 1) {
            Setup.addComputerorDevice($("#operatingSystem" + c).val(), $("#location" + c).val(), $("#primaryusername" + c).val(), $("#settings" + c).val(), "", "", "", "", 1, c);
            $("#dvDevice_" + c + " input[type='text']").each(function () {
                Setup.addURLDevice($(this).val());
            });
        }
        if (type == 2) {
            Setup.addComputerorDevice("", "", "", $("#settings_device" + c).val(), $("#operatingSystem_Device" + c).val(), $("#manufacturer_device" + c).val(), $("#primaryusername_device" + c).val(), $("#usage_device" + c).val(), 4, c);
        }
        debugger;
        $("#processingDiv").show();
        Setup.saveComputerOrDevices();
    }
}
function clearpreviousbuttons(value) {
    for (var i = 0; i < value; i++) {
        $("#setupYes" + i).hide();
        $("#setupNo" + i).hide();
        $("#setupUpdate" + i).hide();
        $("#setupEdit" + i).show();
        $("#setupDelete" + i).show();

        $("#lblQuestion" + i).hide();

        var productDetails = Setup.getComputerDeviceDetails(i);
        if (productDetails != null) {
            if (productDetails.productType == 1) {
                $("#lblComputer" + i).text(productDetails.PrimaryUserName);

                $("#primaryusername" + i).val(productDetails.PrimaryUserName);
                $("#operatingSystem" + i).val(productDetails.OperatingSystem);
                $("#location" + i).val(productDetails.Location);
                $("#settings" + i).val(productDetails.Settings);

                $("#primaryusername" + i).attr("disabled", "disabled");
                $("#operatingSystem" + i).attr("disabled", "disabled");
                $("#location" + i).attr("disabled", "disabled");
                $("#settings" + i).attr("disabled", "disabled");

                var productURL = Setup.getURLs(productDetails.Id);
                for (var u = 0; u < productURL.length; u++) {
                    if (u > 0)
                        addNewURL(i, u - 1);
                    $("#URL_device" + i + "_" + u).val(productURL[u].URL);

                    $("#URL_device" + i + "_" + u).attr("disabled", "disabled");
                    $("#addURL" + i + "_" + u).attr("disabled", "disabled");
                    $("#deleteURL" + i + "_" + u).attr("disabled", "disabled");
                }
            }
            else if (productDetails.productType == 2) {
                $("#lblDevice" + i).text(productDetails.PrimaryUser);

                $("#operatingSystem_Device" + i).val(productDetails.PhoneOS);
                $("#manufacturer_device" + i).val(productDetails.Manufacturer);
                $("#primaryusername_device" + i).val(productDetails.PrimaryUser);
                $("#usage_device" + i).val(productDetails.Usage);

                $("#operatingSystem_Device" + i).attr("disabled", "disabled");
                $("#manufacturer_device" + i).attr("disabled", "disabled");
                $("#primaryusername_device" + i).attr("disabled", "disabled");
                $("#usage_device" + i).attr("disabled", "disabled");
            }
        }
        else {
            $("#computer" + i).detach();
            $("#device" + i).detach();
        }
    }
}
function addNewURL(c, val) {

    $("#addURL" + c + "_" + (val)).hide();
    $("#deleteURL" + c + "_" + (val)).hide();

    var str = " <div class=\"field\" id=\"dvURL" + c + "_" + (val + 1) + "\">"
                + " <input style='width:87%; float:left; margin-bottom:5px;' type=\"text\" id=\"URL_device" + c + "_" + (val + 1) + "\" class=\"form-control\">"
                + " <button onclick='addNewURL(" + c + "," + (val + 1) + ")' style='padding:4px; float:left; margin-left:5px;' type=\"button\" id=\"addURL" + c + "_" + (val + 1) + "\" class=\"btn btn-primary\">+</button>"
                + " <button onclick='deleteURL(" + c + "," + (val + 1) + ",\"dvURL" + c + "_" + (val + 1) + "\")' style='padding:4px; float:left; margin-left:5px;' type=\"button\" id=\"deleteURL" + c + "_" + (val + 1) + "\" class=\"btn btn-primary\">-</button>"
                + " </div>"
    $("#dvDevice_" + c).append(str);
}
function deleteURL(c, textId, val) {
    $("#addURL" + c + "_" + (textId - 1)).show();
    $("#deleteURL" + c + "_" + (textId - 1)).show();
    if (textId > 0)
        $("#" + val).detach();
    else
        $("#URL_device" + c + "_" + textId).val('');
}

function clearBackground(type, c) {
    var formValidate = true;
    var focusField = "";
    $("#primaryusername" + c).css('border-color', '');
    $("#primaryusername" + c).css('background', '');
    $("#operatingSystem" + c).css('border-color', '');
    $("#operatingSystem" + c).css('background', '');
    $("#location" + c).css('border-color', '');
    $("#location" + c).css('background', '');
    $("#settings" + c).css('border-color', '');
    $("#settings" + c).css('background', '');

    $("#primaryusername_device" + c).css('border-color', '');
    $("#primaryusername_device" + c).css('background', '');
    $("#operatingSystem_Device" + c).css('border-color', '');
    $("#operatingSystem_Device" + c).css('background', '');
    $("#usage_device" + c).css('border-color', '');
    $("#usage_device" + c).css('background', '');

    if (type == 1) {

        if ($("#settings" + c).val() == "0") {
            formValidate = false;
            focusField = "settings" + c;
            $("#" + focusField).css('border-color', '#c0392b');
            $("#" + focusField).css('background', '#FCDACD');
        }
        if ($("#location" + c).val() == "0") {
            formValidate = false;
            focusField = "location" + c;
            $("#" + focusField).css('border-color', '#c0392b');
            $("#" + focusField).css('background', '#FCDACD');
        }
        
        if ($("#primaryusername" + c).val() == "") {
            formValidate = false;
            focusField = "primaryusername" + c;
            $("#" + focusField).css('border-color', '#c0392b');
            $("#" + focusField).css('background', '#FCDACD');
        }
        if ($("#operatingSystem" + c).val() == "0") {
            formValidate = false;
            focusField = "operatingSystem" + c;
            $("#" + focusField).css('border-color', '#c0392b');
            $("#" + focusField).css('background', '#FCDACD');
        }
    }
    if (type == 2) {
        if ($("#usage_device" + c).val() == "0") {
            formValidate = false;
            focusField = "usage_device" + c;
            $("#" + focusField).css('border-color', '#c0392b');
            $("#" + focusField).css('background', '#FCDACD');
        }
        if ($("#primaryusername_device" + c).val() == "") {
            formValidate = false;
            focusField = "primaryusername_device" + c;
            $("#" + focusField).css('border-color', '#c0392b');
            $("#" + focusField).css('background', '#FCDACD');
        }
        if ($("#operatingSystem_Device" + c).val() == "0") {
            formValidate = false;
            focusField = "operatingSystem_Device" + c;
            $("#" + focusField).css('border-color', '#c0392b');
            $("#" + focusField).css('background', '#FCDACD');
        }
        
    }

    if (formValidate == false) {
        $('html,body').animate({
            scrollTop: $("#" + focusField).offset().top - 200
        }, 'slow');

        $("#" + focusField).focus();
        return false;
    }
    else
        return true;
}