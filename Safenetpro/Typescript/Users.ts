class Users {
    constructor() { }

    public static FormFunctions() {
    }
    public static checkUserNameAvailablityForgotpassword(email) {
        $("#processingDiv").show();
        var CustomerData = "<Customer_Poco xmlns=\"http://schemas.datacontract.org/2004/07/SafenetproAPI.Controllers\" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\">" +
            "<EmailAddress>" + email + "</EmailAddress>" +
            "</Customer_Poco>";

        var data = "Email:" + email;
        var svcUrl;
        svcUrl = "ForgotPassword";
        var successData = function (response) {
            $("#processingDiv").hide();
            $("#lblEmailNotExists").hide();

            $("#emailSection").hide();
            $("#emailSent").show();
            $("#sendButton").hide();
        };
        var errorData = function (response) {
            if (response.status == "404") {
                $("#processingDiv").hide();
                $("#lblEmailNotExists").show();

                $("#emailSection").show();
                $("#emailSent").hide();
                $("#sendButton").show();
                $("#ContentPlaceHolder1_txtForgotEmailAddress").focus();
            }
            else {
                $("#processingDiv").hide();
                $("#lblEmailNotExists").hide();

                $("#emailSection").hide();
                $("#emailSent").show();
                $("#sendButton").hide();
            }
        };
        Utils.PostREST(svcUrl, CustomerData, successData, errorData);
    }

    public static checkUserNameAvailablity(userName, userId) {
        $("#processingDiv").show();
        if (userId == "null")
            userId = 0;
        var svcUrl;
        svcUrl = "User/" + userName + "/" + userId;

        var successData = function (response) {
            debugger;
            $("#processingDiv").hide();
            $("#lblAlreadyExists").hide();
        };
        var errorData = function (response) {
            debugger;
            if (response.status == "404") {
                $("#processingDiv").hide();
                $("#lblAlreadyExists").hide();
            }
            else {
                $("#processingDiv").hide();
                $("#lblAlreadyExists").show();

                $('html,body').animate({
                    scrollTop: $("#ContentPlaceHolder1_txtUserName").offset().top - 180
                }, 'slow');
                $("#ContentPlaceHolder1_txtUserName").focus();
                $("#ContentPlaceHolder1_txtNewUserName").focus();
                $("#continueRegistration").removeAttr("disabled");
            }
        };
        Utils.GetREST(svcUrl, successData, errorData);
    }

    public static checkUserNameAvailable(userName, userId) {
        $("#processingDiv").show();
        if (userId == "null")
            userId = 0;
        var svcUrl;
        svcUrl = "User/" + userName + "/" + userId;
        var successData = function (response) {
            $("#processingDiv").hide();
            $("#lblAlreadyExists").hide();
        };
        var errorData = function (response) {
            if (response.status == "404") {
                //$("#processingDiv").hide();
                $("#lblAlreadyExists").hide();
                $("#ContentPlaceHolder1_btnhiddenServerClick").click();
            }
            else {
                $("#processingDiv").hide();
                $("#lblAlreadyExists").show();

                $('html,body').animate({
                    scrollTop: $("#ContentPlaceHolder1_txtUserName").offset().top - 180
                }, 'slow');
                $("#ContentPlaceHolder1_txtUserName").focus();
                $("#continueRegistration").removeAttr("disabled");
            }
        };
        Utils.GetREST(svcUrl, successData, errorData);
    }
}