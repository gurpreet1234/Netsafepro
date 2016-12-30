var DisplayActionListValues = (function () {
    function DisplayActionListValues() {
    }
    return DisplayActionListValues;
})();
var RulePermissions = (function () {
    function RulePermissions() {
    }
    RulePermissions.FormFunctions = function () {
        $("#btnSave").unbind('click');
        $("#btnSave").click(function () {
            RulePermissions.saveCustomerGroups();
        });
    };

    RulePermissions.removeAllDisplayActions = function () {
        RulePermissions.displayActionList = [];
    };

    RulePermissions.addDisplayActions = function (displayId, ActionId) {
        for (var i = RulePermissions.displayActionList.length - 1; i >= 0; i--) {
            if (RulePermissions.displayActionList[i].Id === displayId)
                RulePermissions.displayActionList.splice(i, 1);
        }

        RulePermissions.displayActionList.push({ Id: displayId, ActionId: ActionId });
    };

    RulePermissions.saveCustomerGroups = function () {
        var GroupData = "<ArrayOfDisplayNameUserActions xmlns=\"http://schemas.datacontract.org/2004/07/SafenetproAPI.Controllers\" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\">";

        for (var i = RulePermissions.displayActionList.length - 1; i >= 0; i--) {
            GroupData = GroupData + "<DisplayNameUserActions>" + "<ActionID>" + RulePermissions.displayActionList[i].ActionId + "</ActionID>" + "<ID>" + RulePermissions.displayActionList[i].Id + "</ID>" + "</DisplayNameUserActions>";
        }

        GroupData = GroupData + "</ArrayOfDisplayNameUserActions>";

        var svcUrl;

        svcUrl = "Customer/" + $("#ddlCustomers option:selected").val() + "/RuleTypes/1/DisplayNameActions"; //RuleTypeId
        var successData = function (response, textStatus, xhr) {
            RulePermissions.removeAllDisplayActions();
        };
        var errorData = function (response) {
            RulePermissions.removeAllDisplayActions();
        };
        RulePermissions.PostREST(svcUrl, GroupData, successData, errorData);
    };

    RulePermissions.PostREST = function (url, data, fsuccess, ferror) {
        $.ajax({
            type: "POST",
            url: this.getBaseUrl() + url,
            dataType: "xml",
            contentType: "application/xml;charset=UTF-8",
            data: data,
            success: fsuccess,
            error: ferror
        });
    };

    RulePermissions.getBaseUrl = function () {
        if (window.location.toString().indexOf("safenetpro") >= 0)
            return "http://safenetpro.com/api/";
        else if (window.location.toString().indexOf("localhost") >= 0)
            return "http://localhost:2796/api/";
    };
    RulePermissions.displayActionList = [];
    return RulePermissions;
})();
//# sourceMappingURL=RulePermissions.js.map
