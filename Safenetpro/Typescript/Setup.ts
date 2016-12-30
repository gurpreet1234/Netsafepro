

class SetupComputer {
    public Id: number;
    public OperatingSystem: string;
    public Location: string;
    public PrimaryUserName: string;
    public Settings: string;
    public PhoneOS: string;
    public Manufacturer: string;
    public PrimaryUser: string;
    public Usage: string;
    public productType: number;
    public rowId: number;
}
class SetupURL {
    public Id: number;
    public URL: string;
    public productToUser: number;
}

class Setup {
    constructor() { }

    public static setupComputerDeviceList: SetupComputer[] = [];
    public static setupDeviceURL: SetupURL[] = [];
    public static FormFunctions() {
        Setup.setupComputerDeviceList = [];
        Setup.setupDeviceURL = [];
    }

    public static addComputerorDevice(operatingSystem, location, primaryUserName, settings, phoneOS, manufacturer, primaryUser, usage, productType, rowId) {
        var Id = 0;
        if (Setup.setupComputerDeviceList.length > 0)
            Id = Setup.setupComputerDeviceList[Setup.setupComputerDeviceList.length - 1].Id + 1;

        Setup.setupComputerDeviceList.push({
            Id: Id, OperatingSystem: operatingSystem, Location: location, PrimaryUserName: primaryUserName,
            Settings: settings, PhoneOS: phoneOS, Manufacturer: manufacturer, PrimaryUser: primaryUser, Usage: usage, productType: productType,
            rowId: rowId
        });
    }

    public static addURLDevice(URL) {
        var Id = 0;
        if (Setup.setupDeviceURL.length > 0)
            Id = Setup.setupDeviceURL[Setup.setupDeviceURL.length - 1].Id + 1;


        var prodId = 0;
        if (Setup.setupComputerDeviceList.length > 0)
            prodId = Setup.setupComputerDeviceList[Setup.setupComputerDeviceList.length - 1].Id;

        Setup.setupDeviceURL.push({
            Id: Id, URL: URL, productToUser: prodId
        });
    }

    public static updateURLDevice(productType, URL, rowId) {
        var Id = 0;
        if (Setup.setupDeviceURL.length > 0)
            Id = Setup.setupDeviceURL[Setup.setupDeviceURL.length - 1].Id;


        var cArray: SetupComputer[] = Setup.setupComputerDeviceList.filter(function (obj) {
            return (obj.productType == productType && obj.rowId === rowId);
        });

        Setup.setupDeviceURL.push({
            Id: Id, URL: URL, productToUser: cArray[0].Id
        });
    }

    public static updateComputerorDevice(operatingSystem, location, primaryUserName, settings, phoneOS, manufacturer, primaryUser, usage, productType, rowId) {
        var cArray: SetupComputer[] = Setup.setupComputerDeviceList.filter(function (obj) {
            return (obj.productType == productType && obj.rowId === rowId);
        });

        cArray[0].OperatingSystem = operatingSystem;
        cArray[0].Location = location;
        cArray[0].PrimaryUserName = primaryUserName;
        cArray[0].Settings = settings;
        cArray[0].PhoneOS = phoneOS;
        cArray[0].Manufacturer = manufacturer;
        cArray[0].PrimaryUser = primaryUser;
        cArray[0].Usage = usage;

        Setup.setupDeviceURL = Setup.setupDeviceURL.filter(function (obj) {
            return (obj.productToUser != cArray[0].Id);
        });
    }

    public static deleteComputerorDevice(productType, row) {
        var cArray: SetupComputer[] = Setup.setupComputerDeviceList.filter(function (obj) {
            return (obj.productType == productType && obj.rowId === row);
        });

        Setup.setupDeviceURL = Setup.setupDeviceURL.filter(function (obj) {
            return (obj.productToUser != cArray[0].Id);
        });

        Setup.setupComputerDeviceList = Setup.setupComputerDeviceList.filter(function (obj) {
            return (obj.rowId != row);
        });

        for (var r = 0; r < Setup.setupComputerDeviceList.length; r++) {
            Setup.setupComputerDeviceList[r].rowId = r;
        }
    }

    public static getComputerDeviceDetails(row) {
        //return Setup.setupComputerDeviceList[row];
        var cArray: SetupComputer[] = Setup.setupComputerDeviceList.filter(function (obj) {
            return (obj.rowId === row);
        });

        return cArray[0];
    }

    public static getURLs(row) {
        var fArray: SetupURL[] = Setup.setupDeviceURL.filter(function (obj) {
            return (obj.productToUser === row);
        });

        return fArray;
    }

    public static getURLsWithRow(productType, row) {
        var cArray: SetupComputer[] = Setup.setupComputerDeviceList.filter(function (obj) {
            return (obj.productType == productType && obj.rowId === row);
        });

        var fArray: SetupURL[] = Setup.setupDeviceURL.filter(function (obj) {
            return (obj.productToUser === cArray[0].Id);
        });

        return fArray;
    }

    public static saveComputerOrDevices() {
        $("#processingDiv").show();
        var userId = sessionStorage.getItem("userId");

        var GroupData = "<ArrayOfProductToUser_POCO xmlns=\"http://schemas.datacontract.org/2004/07/SafenetproAPI.Controllers\" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\">";

        for (var i = 0; i < Setup.setupComputerDeviceList.length; i++) {
            //var _settings = Setup.setupComputerDeviceList[i].Settings;
            //if (_settings == "" || _settings == null)
            //    _settings = "4";

            GroupData = GroupData + "<ProductToUser_POCO>" +
            "<ID>" + Setup.setupComputerDeviceList[i].Id + "</ID>" +
            "<Location>" + Setup.setupComputerDeviceList[i].Location + "</Location>" +
            "<Manufacturer>" + Setup.setupComputerDeviceList[i].Manufacturer + "</Manufacturer>" +
            "<OperatingSystem>" + Setup.setupComputerDeviceList[i].OperatingSystem + "</OperatingSystem>" +
            "<PhoneOS>" + Setup.setupComputerDeviceList[i].PhoneOS + "</PhoneOS>" +
            "<PrimaryUser>" + Setup.setupComputerDeviceList[i].PrimaryUser + "</PrimaryUser>" +
            "<PrimaryUserName>" + Setup.setupComputerDeviceList[i].PrimaryUserName + "</PrimaryUserName>" +
            "<ProductId>" + Setup.setupComputerDeviceList[i].productType + "</ProductId>" +
            "<Settings>" + Setup.setupComputerDeviceList[i].Settings + "</Settings>" +
            "<Usage>" + Setup.setupComputerDeviceList[i].Usage + "</Usage>";

            if (Setup.setupComputerDeviceList[i].productType == 1) {
                var productURL = Setup.getURLs(Setup.setupComputerDeviceList[i].Id)
                GroupData = GroupData + "<uDevicePoco>";

                for (var j = productURL.length - 1; j >= 0; j--) {
                    GroupData = GroupData + "<URLtoDevice_POCO>" +
                    "<ID>" + productURL[j].Id + "</ID>" +
                    "<ProductToUser>" + productURL[j].productToUser + "</ProductToUser>" +
                    "<URL>" + productURL[j].URL + "</URL>" +
                    "</URLtoDevice_POCO>";
                }
                GroupData = GroupData + "</uDevicePoco>";
            }
            GroupData = GroupData + "</ProductToUser_POCO>";
        }

        GroupData = GroupData + "</ArrayOfProductToUser_POCO>";
        debugger;
        var svcUrl;
        svcUrl = "Users/" + userId + "/ProductToUsers";
        var successData = function (response, textStatus, xhr) {
            Setup.removeAllProducts();
            $("#processingDiv").hide();
            window.location.href = "ProductLicenses.aspx";
        };
        var errorData = function (response) {
            Setup.removeAllProducts();
            $("#processingDiv").hide();
            alert("Something happened wrong, Please try again later.");
        };
        Utils.PostREST(svcUrl, GroupData, successData, errorData);
    }

    public static removeAllProducts() {
        Setup.setupComputerDeviceList = [];
        Setup.setupDeviceURL = [];
    }

    public static checkPrimaryNameAvailablity(primaryuserName, userId, c) {
        $("#processingDiv").show();
        if (userId == "null")
            userId = 0;
        var svcUrl;
        svcUrl = "ProductToUser/" + primaryuserName + "/" + userId;

        var successData = function (response) {
            debugger;
            $("#processingDiv").hide();
            $("#lblAlreadyExists").hide();
            $("#setupYes" + c).removeAttr("disabled");
            $("#setupNo" + c).removeAttr("disabled");
        };
        var errorData = function (response) {
            debugger;
            if (response.status == "404") {
                $("#processingDiv").hide();
                $("#lblAlreadyExists").hide();
                $("#setupYes" + c).removeAttr("disabled");
                $("#setupNo" + c).removeAttr("disabled");
            }
            else {
                $("#processingDiv").hide();
                $("#lblAlreadyExists").show();

                $("#setupYes" + c).attr("disabled", true);
                $("#setupNo" + c).attr("disabled", true);
            }
        };
        Utils.GetREST(svcUrl, successData, errorData);
    }
}