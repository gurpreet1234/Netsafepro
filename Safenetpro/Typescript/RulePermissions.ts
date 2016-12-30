
class DisplayActionListValues {
    public Id: number;
    public ActionId: number;
}
class RulePermissions {

    constructor() { }

    public static displayActionList: DisplayActionListValues[] = [];

    public static FormFunctions() {
        $("#btnSave").unbind('click');
        $("#btnSave").click(function () {
            RulePermissions.saveCustomerGroups();
        });
    }

    public static removeAllDisplayActions() {
        RulePermissions.displayActionList = [];
    }

    public static addDisplayActions(displayId, ActionId) {
        for (var i = RulePermissions.displayActionList.length - 1; i >= 0; i--) {
            if (RulePermissions.displayActionList[i].Id === displayId) RulePermissions.displayActionList.splice(i, 1);
        }

        RulePermissions.displayActionList.push({ Id: displayId, ActionId: ActionId });
    }

    public static saveCustomerGroups() {

        var GroupData = "<ArrayOfDisplayNameUserActions xmlns=\"http://schemas.datacontract.org/2004/07/SafenetproAPI.Controllers\" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\">";

        for (var i = RulePermissions.displayActionList.length - 1; i >= 0; i--) {
            GroupData = GroupData + "<DisplayNameUserActions>" +
            "<ActionID>" + RulePermissions.displayActionList[i].ActionId + "</ActionID>" +
            "<ID>" + RulePermissions.displayActionList[i].Id + "</ID>" +
            "</DisplayNameUserActions>";
        }

        GroupData = GroupData + "</ArrayOfDisplayNameUserActions>";

        var svcUrl;

        svcUrl = "Customer/" + $("#ddlCustomers option:selected").val() + "/RuleTypes/1/DisplayNameActions";  //RuleTypeId
        var successData = function (response, textStatus, xhr) {
            RulePermissions.removeAllDisplayActions();
            alert("Actions saved successfully.");
        };
        var errorData = function (response) {
            RulePermissions.removeAllDisplayActions();
            alert("Something happened wrong, Please try again.");
        };
        Utils.PostREST(svcUrl, GroupData, successData, errorData);
    }
}