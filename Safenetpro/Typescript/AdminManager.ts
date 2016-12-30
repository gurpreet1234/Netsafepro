class userProfileObject {
    FirstName: string;
    LastName: string;
    EmailAddress: string;
    Phone: string;
    Zip: string;
    UserId: string;
}

class AdminManagement {

    constructor() { }

    public static FormFunctions() {
        $("#btnSearch").click(function () {
            var fromDate = $("#txtCustomerFrom").val();
            var toDate = $("#txtCustomerTo").val();

            var filterType = $("#selField").val();
            var filterText = $("#txtCustomer").val();

            var filterQuery = "";
            if (filterText != "") {
                if (filterType == 'CustomerName')
                    filterQuery = "!CustomerName~'" + filterText + "'";
                else if (filterType == 'Email')
                    filterQuery = "!Email~'" + filterText + "'";
                else
                    filterQuery = "!UniqueId~'" + filterText + "'";
            }

            if (fromDate != "" && toDate != "") {
                if (filterQuery != '')
                    filterQuery = filterQuery + "^" + "(CreatedDate GE '" + fromDate + "'^CreatedDate LE '" + toDate + "')";
                else
                    filterQuery = "!CreatedDate GE '" + fromDate + "'^CreatedDate LE '" + toDate + "'";
            }

            if (filterQuery != '')
                AdminManagement.ListAllCustomers(filterQuery);
            else
                AdminManagement.ListAllCustomers();
        });

        $("#setupCancelLicenseUpdate").unbind('click');
        $("#setupCancelLicenseUpdate").click(function () {
            $("#hiderLicense").fadeOut("slow");
            $('#licenseDate').fadeOut("slow");
        });

        $("#setupCancel0").unbind('click');
        $("#setupCancel0").click(function () {
            $("#hiderProducts").fadeOut("slow");
            $('#paymentDetails').fadeOut("slow");
        });

        $("#customerCancel").click(function () {
            $("#addCustomer").fadeOut("slow");
            $("#hiderProducts").fadeOut("fast");
        });

        $("#customerUpdateCancel").click(function () {
            $("#updateCustomer").fadeOut("slow");
            $("#hiderProducts").fadeOut("fast");
        });

        AdminManagement.ListAllCustomers();
    }

    public static ListFilterCustomers(filterKey, filterQuery = "") {
        $("#addbuttonproduct").hide();
        $("#headertext").text("Customers");
        var gridList = "<table id='tblCustomers' cellspacing='0' cellpadding='0' class='grid'>" +
            "<thead class='grid_header'><tr>" +
            "<th class='grid_header_first_column'>First Name</th><th class='grid_header_first_column'>Last Name</th><th class='grid_header_column'>Email</th><th class='grid_header_column'>Phone</th>" +
            "<th class='grid_header_column'>Zip</th><th class='grid_header_column'>Actions</th><th class='grid_header_column'>View</th></tr></thead><tbody>";
        var response = JSON.parse(localStorage.getItem("UserProfile_Poco"));

        $(response).each(function () {

            var filterKeyValue = "";
            if (filterKey == 1)
                filterKeyValue = this.FirstName + " " + this.LastName;
            if (filterKey == 2)
                filterKeyValue = this.EmailAddress;
            if (filterKey == 3)
                filterKeyValue = this.Phone;

            if (filterKeyValue.toLowerCase().indexOf(filterQuery.toLowerCase()) >= 0 || filterQuery == "") {
                $('#OneCustomer').hide();
                $('#tabDetails').hide();
                var customerName = this.FirstName + " " + this.LastName;
                gridList = gridList + "<tr><td class='first_bottom_border'>" + this.FirstName + "</td>" +
                "<td class='first_bottom_border'>" + this.LastName + "</td>" +
                "<td class='bottom_border'>" + this.EmailAddress + "</td>" +
                "<td class='bottom_border'>" + this.Phone + "</td>" +
                "<td class='bottom_border'>" + this.Zip + "</td>" +
                "<td class='bottom_border'><a href='#' onclick='OpenCustomerProducts(" + this.UserId + ",\"" + customerName + "\")'>Products</a>" +
                " | <a href='#' onclick='PayCustomerProducts(" + this.UserId + ")'>Pay</a>" +
                " | <a href='#' onclick='EditCustomer(" + this.UserId + ")'>Edit</a>" +
                " | <a href='#' onclick='DeleteCustomer(" + this.UserId + ")'>Delete</a></td>" +
                "<td class='bottom_border'><a href='#' onclick='CustomerInformation(" + this.UserId + ",\"" + this.EmailAddress + "\",\"" + this.Phone + "\",\"" + this.Zip + "\",\"" + this.FirstName + "\",\"" + this.LastName + "\",\"" + this.Address + "\",\"" + this.City + "\",\"" + this.State + "\")'>View</a>" +
                " </td>" +
                "</tr>";
            }
        });
        gridList = gridList + "</tbody></table>";
        $("#dvCustomers").html(gridList).triggerHandler("contentChanged");
        $('#loadingmessage').hide();
    }

    public static ListAllCustomers(filterQuery = "") {
        $("#addbuttonproduct").hide();
        $('#loadingmessage').show();

        $("#searchCustomer").html("Search:<select id='searchValue' class='form-control'>" +
            "<option value='1'>Name</option>" +
            "<option value='2'>Email</option>" +
            "<option value='3'>Phone</option>" +
            "</select>" +
            "<input type='text' id='txtSearch' placeholder='Enter search text' class='form-control' onkeyup='searchCustomers(this)'/>");

        $("#headertext").text("Customers");

        var gridList = "<table id='tblCustomers' cellspacing='0' cellpadding='0' class='grid'>" +
            "<thead class='grid_header'><tr>" +
            "<th class='grid_header_first_column'>First Name</th><th class='grid_header_first_column'>Last Name</th><th class='grid_header_column'>Email</th><th class='grid_header_column'>Phone</th>" +
            "<th class='grid_header_column'>Zip</th><th class='grid_header_column'>Actions</th><th class='grid_header_column'>View</th></tr></thead><tbody>";

        var url = "";
        if (filterQuery != "")
            url = encodeURI("Customers/" + filterQuery + "/filter");
        else
            url = "AdminCustomersList";
        var successData = function (response) {
            var userProfile = [];
            $(response).find('UserProfile_Poco').each(function () {
                var userProfileObject = <userProfileObject>{};
                $('#OneCustomer').hide();
                $('#tabDetails').hide();
                var customerName = $(this).find('FirstName').text() + " " + $(this).find('LastName').text();
                gridList = gridList + "<tr><td class='first_bottom_border'>" + $(this).find('FirstName').text() + "</td>" +
                "<td class='first_bottom_border'>" + $(this).find('LastName').text() + "</td>" +
                "<td class='bottom_border'>" + $(this).find('EmailAddress').text() + "</td>" +
                "<td class='bottom_border'>" + $(this).find('Phone').text() + "</td>" +
                "<td class='bottom_border'>" + $(this).find('Zip').text() + "</td>" +
                "<td class='bottom_border'><a href='#' onclick='OpenCustomerProducts(" + $(this).find('UserId').text() + ",\"" + customerName + "\")'>Products</a>" +
                " | <a href='#' onclick='PayCustomerProducts(" + $(this).find('UserId').text() + ")'>Pay</a>" +
                " | <a href='#' onclick='EditCustomer(" + $(this).find('UserId').text() + ")'>Edit</a>" +
                " | <a href='#' onclick='DeleteCustomer(" + $(this).find('UserId').text() + ")'>Delete</a></td>" +
                "<td class='bottom_border'><a href='#' onclick='CustomerInformation(" + $(this).find('UserId').text() + ",\"" + $(this).find('EmailAddress').text() + "\",\"" + $(this).find('Phone').text() + "\",\"" + $(this).find('Zip').text() + "\",\"" + $(this).find('FirstName').text() + "\",\"" + $(this).find('LastName').text() + "\",\"" + $(this).find('Address').text() + "\",\"" + $(this).find('City').text() + "\",\"" + $(this).find('State').text() + "\")'>View</a>" +
                " </td>" +
                "</tr>";
                userProfileObject.FirstName = $(this).find('FirstName').text();
                userProfileObject.LastName = $(this).find('LastName').text();
                userProfileObject.EmailAddress = $(this).find('EmailAddress').text();
                userProfileObject.Phone = $(this).find('Phone').text();
                userProfileObject.Zip = $(this).find('Zip').text();
                userProfileObject.UserId = $(this).find('UserId').text();
                userProfile.push(userProfileObject);
            });
            localStorage.setItem("UserProfile_Poco", JSON.stringify(userProfile));
            gridList = gridList + "</tbody></table>";
            $("#dvCustomers").html(gridList).triggerHandler("contentChanged");
            $('#loadingmessage').hide();
        };
        var errorData = function (response) {
            $('#loadingmessage').hide();
        };
        Utils.GetREST(url, successData, errorData);
    }


    public static customerAdd() {
        $("#addbuttonproduct").hide();
        if ($("#CustomerFirstName").val() && $("#CustomerLastName").val() && $("#CustomerEmail").val() && $("#CustomerPhone").val() && $("#CustomerZip").val() && $("#CustomerPassword").val()) {
            var firstname = $("#CustomerFirstName").val(),
                lastname = $("#CustomerLastName").val(),
                email = $("#CustomerEmail").val(),
                phone = $("#CustomerPhone").val(),
                zip = $("#CustomerZip").val(),
                password = $("#CustomerPassword").val()

        var data = {
                CellPhone: phone,
                FirstName: firstname,
                LastName: lastname,
                EmailAddress: email,
                Zip: zip,
                Password: password,
                Active: true
            }

        var svcUrl;
            svcUrl = "Addcustomers";
            var successData = function (response, textStatus, xhr) {

                $("#hiderProducts").fadeOut("slow");
                $("#addCustomer").fadeOut("slow");
                AdminManagement.ListAllCustomers();
            };
            var errorData = function (response) {
                $("#loadingmessage").hide();
                $("#hiderProducts").fadeOut("slow");
                $("#addCustomer").fadeOut("slow");
                AdminManagement.ListAllCustomers();
                alert("The record has been added successfully.");
            };
            Utils.JsonREST(svcUrl, data, successData, errorData);
        } else {
            alert("All fields are mandatory.");
        }
    }

    public static EditCustomer(customerId) {
        $("#addbuttonproduct").hide();
        var svcUrl;
        svcUrl = "getCustomerById/" + customerId;
        var successData = function (response) {
            $("#hdnCustomerId").val($(response).find('UserId').text());
            $("#txtUpdateFirstName").val($(response).find('FirstName').text());
            $("#txtUpdateLastName").val($(response).find('LastName').text());
            $("#txtUpdateEmail").val($(response).find('EmailAddress').text());
            $("#txtUpdatePhone").val($(response).find('CellPhone').text());
            $("#txtUpdateZip").val($(response).find('Zip').text());
            $("#txtUpdatePassword").val('');
            //$("#txtUpdatePassword").val($(response).find('Password').text());
            $('#loadingmessage').hide();

        };
        var errorData = function (response) {
            $('#loadingmessage').hide();
        };
        Utils.GetREST(svcUrl, successData, errorData);
    }

    public static UpdateCustomer() {
        $("#addbuttonproduct").hide();
        if ($("#txtUpdateFirstName").val() && $("#txtUpdateLastName").val() && $("#txtUpdateEmail").val() && $("#txtUpdatePhone").val() && $("#txtUpdateZip").val()) {
            $("#loadingmessage").show();
            var firstname = $("#txtUpdateFirstName").val(),
                lastname = $("#txtUpdateLastName").val(),
                email = $("#txtUpdateEmail").val(),
                phone = $("#txtUpdatePhone").val(),
                zip = $("#txtUpdateZip").val(),
                password = $("#txtUpdatePassword").val()

        var data = {
                CellPhone: phone,
                FirstName: firstname,
                LastName: lastname,
                EmailAddress: email,
                Password: password,
                Zip: zip,
                Active: 1
            }
            var svcUrl;
            svcUrl = "updateCustomer/" + $("#hdnCustomerId").val();
            var successData = function (response, textStatus, xhr) {
                alert("The record has been updated successfully.");
                $("#hiderProducts").fadeOut("slow");
                $('#updateCustomer').fadeOut("slow");
                $('#loadingmessage').hide();
                AdminManagement.ListAllCustomers();
            };
            var errorData = function (response) {
                $("#loadingmessage").hide();
                $("#hiderProducts").fadeOut("slow");
                $('#updateCustomer').fadeOut("slow");
                $('#loadingmessage').hide();
                AdminManagement.ListAllCustomers();
                alert("Something happened wrong, Please try again later.");
            };
            Utils.JsonREST(svcUrl, data, successData, errorData);
        } else {
            alert("All fields are mandatory.");
        }
    }

    public static EditHistory(customerId) {
        $("#addbuttonproduct").hide();
        var svcUrl;
        svcUrl = "getCustomerhistoryById/" + customerId;
        var successData = function (response) {
            $("#id").val($(response).find('Id').text());
            $("#Username").val($(response).find('UserName').text());
            $("#YearlyPrice").val($(response).find('YearlyPrice').text());
            $("#MonthlyPrice").val($(response).find('MonthlyPrice').text());
            $("#PaymentPeriod").val($(response).find('PaymentPeriod').text());
            $("#LastPaymentDate").val($(response).find('LastPaymentDate').text());
            $('#loadingmessage').hide();

        };
        var errorData = function (response) {
            $('#loadingmessage').hide();
        };
        Utils.GetREST(svcUrl, successData, errorData);
    }

    public static UpdateHistory() {
        $("#addbuttonproduct").hide();
        if ($("#Username").val() && $("#YearlyPrice").val() && $("#MonthlyPrice").val() && $("#PaymentPeriod").val() && $("#LastPaymentDate").val()) {
            $("#loadingmessage").show();
            var UserName = $("#Username").val(),
                Id = $("#id").val(),
                YearlyPrice = $("#YearlyPrice").val(),
                MonthlyPrice = $("#MonthlyPrice").val(),
                PaymentPeriod = $("#PaymentPeriod").val(),
                LastPaymentDate = $("#LastPaymentDate").val()

            var data = {
                UserName: UserName,
                YearlyPrice: YearlyPrice,
                MonthlyPrice: MonthlyPrice,
                PaymentPeriod: PaymentPeriod,
                LastPaymentDate: LastPaymentDate
            }
            var svcUrl;
            svcUrl = "updateHistory/" + $("#id").val();
            var successData = function (response, textStatus, xhr) {
                $('#historypopup').fadeOut("slow");
                $("#hiderProducts").fadeOut("slow");
                $('#loadingmessage').hide();
                //AdminManagement.ListAllCustomers();
                alert("The record has been updated successfully.");
            };
            var errorData = function (response) {
                $("#loadingmessage").hide();
                $('#historypopup').fadeOut("slow");
                $("#hiderProducts").fadeOut("slow");
                $('#loadingmessage').hide();
                AdminManagement.ListAllCustomers();
                alert("Something happened wrong, Please try again later.");
            };
            Utils.JsonREST(svcUrl, data, successData, errorData);
        } else {
            alert("All fields are mandatory.");
        }
    }


    public static ListCustomerProducts(customerId, customerName) {
        var paidstatus;
        $("#addbutton").hide();
        $("#addbuttonproduct").show();
        $('#loadingmessage').show();
        $("#headertext").text("Customer Products");

        //historyGrid
        var historyList = "<table id='tblCustomers' cellspacing='0' cellpadding='0' class='grid'>" +
            "<tr style='font-weight:bold;'><td style='width:70px;'>Customer: </td><td>" + customerName + "</td></tr></table>";

        historyList = historyList + "<table id='tblCustomerProducts' cellspacing='0' cellpadding='00' class='grid'>" +
        "<thead class='grid_header'><tr>" +
        "<th class='grid_header_first_column' style='width: 10 %;'>Primary UserName</th><th class='grid_header_column' style='width:10%;'>License Start Date</th><th class='grid_header_column' style='width: 10 %;'>License End Date</th><th class='grid_header_column' style='width: 10 %;'>YearlyPrice</th>" +
        "<th class='grid_header_column' style='width:10%;'>MonthlyPrice</th>" +
        "<th class='grid_header_column' style='width: 10 %;'>PaymentPeriod</th>" +
        "<th class='grid_header_column' style='width: 10 %;'>Last Payment Date</th>" +
        "<th class='grid_header_column' style='width: 10 %;'>Paid</th>" +
        "<th class='grid_header_column' style='width: 10 %;'>Status</th>" +
        "<th class='grid_header_column' style='width: 10%;'>Created Date</th>" +
        "<th class='grid_header_column' style='width: 10%;'>Edit</th></tr></thead><tbody>";


        //ProductGrid
        var gridList = "<table id='tblCustomers' cellspacing='0' cellpadding='0' class='grid'>" +
            "<tr style='font-weight:bold;'><td style='width:70px;'>Customer: </td><td>" + customerName + "</td></tr></table>";

        gridList = gridList + "<table id='tblCustomerProducts' cellspacing='0' cellpadding='0' class='grid'>" +
        "<thead class='grid_header'><tr>" +
        "<th class='grid_header_first_column'>Primary UserName</th><th class='grid_header_column' style='width:10%;'>Location</th><th class='grid_header_column'>Operating System</th><th class='grid_header_column'>Type</th>" +
        "<th class='grid_header_column'>Status</th>" + "<th class='grid_header_column'>Created Date</th>" +
        "<th class='grid_header_column'>License Start Date</th>" +
        "<th class='grid_header_column'>License End Date</th>" +
        "<th class='grid_header_column'>Actions</th></tr></thead><tbody>";

        var url = "Customer/" + customerId + "/AdminCustomersList";

        var successData = function (response) {

            //For Product
            $(response).find('ProductToUser_Price_POCO').each(function () {
                gridList = gridList + "<tr>";

                var settings = $(this).find('Settings').text();
                if (settings == "1" || settings == "2" || settings == "3") {
                    gridList = gridList + "<td class='first_bottom_border'>" + $(this).find('PrimaryUserName').text() + "</td>" +
                    "<td class='bottom_border' style='width:10%;'>" + $(this).find('Location').text() + "</td>" +
                    "<td class='bottom_border'>" + $(this).find('OperatingSystem').text() + "</td>";

                    if (settings == "1")
                        gridList = gridList + "<td class='bottom_border'>Moderate License</td>";
                    if (settings == "2")
                        gridList = gridList + "<td class='bottom_border'>Business License</td>";
                    if (settings == "3")
                        gridList = gridList + "<td class='bottom_border'>Personal License</td>";
                }
                else {
                    gridList = gridList + "<td class='first_bottom_border'>" + $(this).find('PrimaryUser').text() + "</td>" +
                    "<td class='bottom_border'>" + $(this).find('Usage').text() + "</td>" +
                    "<td class='bottom_border'>" + $(this).find('PhoneOS').text() + "</td>" +
                    "<td class='bottom_border'>Device</td>";
                }

                //if ($(this).find('Paid').text() == "true")
                //    gridList = gridList + "<td class='bottom_border' style='width:10%;'>Yes</td>";
                //else
                //    gridList = gridList + "<td class='bottom_border' style='width:10%;'>No</td>";

                if ($(this).find('Paid').text() == "true")
                    gridList = gridList + "<td class='bottom_border' style='width:10%;'><span class='label label-success'>Yes</span></td>";
                else
                    gridList = gridList + "<td class='bottom_border' style='width:10%;'><span class='label label-danger'>No</span></td>";



                gridList = gridList + "<td class='bottom_border'>" + AdminManagement.dateformate($(this).find('CreatedDate').text()) + "</td>";
                gridList = gridList + "<td class='bottom_border'>" + $(this).find('StartDate').text() + "</td>";
                var endDate;
                if ($(this).find('StartDate').text() != null && $(this).find('StartDate').text() != "" && $(this).find('StartDate').text() != undefined) {
                    endDate = new Date($(this).find('StartDate').text());

                    var paymentPeriod = $(this).find('PaymentPeriod').text();
                    if (paymentPeriod != null && paymentPeriod != "" && paymentPeriod != undefined) {
                        endDate.setMonth(endDate.getMonth() + parseInt(paymentPeriod));
                        endDate = AdminManagement.getFormattedDate(endDate);
                    }

                } else {
                    endDate = "";
                }
                gridList = gridList + "<td class='bottom_border'>" + endDate + "</td>";
                gridList = gridList + "<td class='bottom_border'><a href='#' onclick='UpdateCustomerLicense(" + $(this).find('Id').text() + ",\"" + customerName + "\")'>Update</a></td>";
                gridList = gridList + "</tr>";
            });
            gridList = gridList + "</tbody></table>";


            //For History
            $(response).find('ProductToUser_Price_POCO').each(function () {
                if ($(this).find('Paid').text() == "true") {
                    historyList = historyList + "<tr>";
                    var endDate;
                    if ($(this).find('StartDate').text() != null && $(this).find('StartDate').text() != "" && $(this).find('StartDate').text() != undefined) {
                        endDate = new Date($(this).find('StartDate').text());

                        var paymentPeriod = $(this).find('PaymentPeriod').text();
                        if (paymentPeriod != null && paymentPeriod != "" && paymentPeriod != undefined) {
                            endDate.setMonth(endDate.getMonth() + parseInt(paymentPeriod));
                            endDate = AdminManagement.getFormattedDate(endDate);
                        }

                    } else {
                        endDate = "";
                    }

                    var settings = $(this).find('Settings').text();
                    if (settings == "1" || settings == "2" || settings == "3") {
                        historyList = historyList + "<td class='first_bottom_border' style='width: 10%;'>" + $(this).find('PrimaryUserName').text() + "</td>" +
                        "<td class='bottom_border' style='width: 10%;'>" + $(this).find('StartDate').text() + "</td>" +
                        "<td class='bottom_border' style='width: 10%;'>" + endDate + "</td>";
                    }
                    else {
                        historyList = historyList + "<td class='first_bottom_border' style='width: 10%;'>" + $(this).find('PrimaryUser').text() + "</td>" +
                        "<td class='bottom_border' style='width: 10%;'>" + $(this).find('StartDate').text() + "</td>" +
                        "<td class='bottom_border' style='width: 10%;'>" + endDate + "</td>";
                    }
                    historyList = historyList + "<td class='bottom_border' style='width: 10%;'>" + $(this).find('YearlyPrice').text() + "</td>";
                    historyList = historyList + "<td class='bottom_border' style='width: 10%;'>" + $(this).find('MonthlyPrice').text() + "</td>";

                    historyList = historyList + "<td class='bottom_border' style='width: 10%;'>" + $(this).find('PaymentPeriod').text() + "</td>";
                    historyList = historyList + "<td class='bottom_border' style='width: 10%;'>" + $(this).find('LastPaymentDate').text() + "</td>";
                    //console.log($(this).find('Paid').text());
                    if ($(this).find('Paid').text() == "true")
                        historyList = historyList + "<td class='bottom_border' style='width:10%;'>Yes</td>";
                    else
                        historyList = historyList + "<td class='bottom_border' style='width:10%;'>No</td>";

                    if ($(this).find('Paid').text() == "true")
                        historyList = historyList + "<td class='bottom_border' style='width:10%;'><span class='label label-success'>Yes</span></td>";
                    else
                        historyList = historyList + "<td class='bottom_border' style='width:10%;'><span class='label label-danger'>No</span></td>";

                    historyList = historyList + "<td class='bottom_border' style='width: 10%;'>" + $(this).find('CreatedDate').text() + "</td>";

                    historyList = historyList + "<td class='bottom_border'><a href='#'onclick= 'EditHistory(" + $(this).find('Id').text() + ")'>Edit</a></td>";


                    historyList = historyList + "</tr>"
                }
            });


            $("#historyList").html(historyList);
            $("#dvCustomers").html(gridList);
            $("#Customersproduct").html(gridList);
            $('#loadingmessage').hide();
        };
        var errorData = function (response) {
            $('#loadingmessage').hide();
        };
        Utils.GetREST(url, successData, errorData);
    }


    public static ListProducts() {

        $('#loadingmessage').show();
        var url = "Products";
        var successData = function (response) {
            $(response).find('Product').each(function (index, value) {
                $("#AllProductList").append($("<option  />").val($(this).find('Id').text()).text($(this).find('ProductName').text()));
            });
            $('#loadingmessage').hide();
        };
        var errorData = function (response) {
            $('#loadingmessage').hide();
        };
        Utils.GetREST(url, successData, errorData);
    }


    public static SingleCustomerProductAdd(getvalues) {
        if (!getvalues) {
            getvalues = [];
        }
        if ($("#PrimaryuserName").val() && $("#operatingSystem").val() && $("#settings").val()) {
            var primaryusername = $("#PrimaryuserName").val(), productusername = $("#productusername").val(), userId = $("#productuserid").val(), productid = $("#AllProductList").val(), location = $("#location").val(), operatingSystem = $("#operatingSystem").val(), settings = $("#settings").val(), Usage = $("#Usage").val()

               var data = {
                PrimaryUserName: primaryusername,
                ProductId: location == "0" ? 2 : 1,
                Location: location == "0" ? "" : location,
                OperatingSystem: operatingSystem,
                Settings: parseInt(settings),
                PhoneOS: "",
                Manufacturer: "",
                PrimaryUser: "",
                Usage: Usage == "0" ? "" : Usage,
                uDevicePoco: getvalues
            }

             $("#loadingmessage").show();
            var svcUrl = "Product/" + userId + "/SingleCustomerProductAdd";
            var successData = function (response, textStatus, xhr) {
                $("#hiderProducts").fadeOut("slow");
                $("#addoneCustomerproduct").fadeOut("slow");
                $("#PrimaryuserName").val("");
                $("#AllProductList").val("");
                $("#Usage").prop('selectedIndex', 0);
                $('#location').prop('selectedIndex', 0);
                $('#operatingSystem').prop('selectedIndex', 0);
                $('#settings').prop('selectedIndex', 0);
                $("#loadingmessage").hide();
                getvalues = [];
                AdminManagement.ListCustomerProducts(userId, productusername);

            };
            var errorData = function (response) {
                $("#loadingmessage").hide();
                $("#hiderProducts").fadeOut("slow");
                $("#addoneCustomerproduct").fadeOut("slow");
                $("#PrimaryuserName").val("");
                $("#AllProductList").val("");
                $("#Usage").prop('selectedIndex', 0);
                $('#location').prop('selectedIndex', 0);
                $('#operatingSystem').prop('selectedIndex', 0);
                $('#settings').prop('selectedIndex', 0);
                getvalues = [];
                AdminManagement.ListCustomerProducts(userId, productusername);
                alert("The record has been added successfully.");
            };
            Utils.JsonREST(svcUrl, data, successData, errorData);
        } else {
            alert("All fields are mandatory.");
        }
    }


    public static dateformate(date) {
        var d = new Date(date),
            month = d.getMonth() + 1,
            day = d.getDate(),
            year = d.getFullYear();
        var finaldate = month + "-" + day + "-" + year;
        return finaldate;

    }

    public static getFormattedDate(date) {
        var year = date.getFullYear();
        var month = (1 + date.getMonth()).toString();
        month = month.length > 1 ? month : '0' + month;
        var day = date.getDate().toString();
        day = day.length > 1 ? day : '0' + day;

        var seconds = date.getSeconds();
        var minutes = date.getMinutes();
        var hour = date.getHours();

        return year + '-' + month + '-' + day + 'T' + hour + ':' + minutes + ':' + seconds + '.' + 0;
    }

    public static getCustomerProductPrice(customerId) {
        $('#loadingmessage').show();
        var url = "UserProductsAmount/" + customerId;
        var successData = function (response) {

            $(response).find('productPrice').each(function () {
                $("#ContentPlaceHolder1_txtActualAmount").val($(this).find('Monthly').text());
                $("#ContentPlaceHolder1_txtAmount").val($(this).find('Monthly').text());
            });
            $('#loadingmessage').hide();
        };
        var errorData = function (response) {
            $('#loadingmessage').hide();
        };
        Utils.GetREST(url, successData, errorData);
    }

    public static DeleteCustomerProducts(customerId) {
        debugger;
        $("#processingDiv").show();
        var svcUrl;
        svcUrl = "DeleteCustomer/" + customerId;
        var successData = function (response) {
            $("#processingDiv").hide();
            alert("Customer has been deleted successfully.");
            AdminManagement.ListAllCustomers();
        };
        var errorData = function (response) {
            $("#processingDiv").hide();
            alert("The record couldn't been deleted.<br />Error message: Internal server error.");
        };
        Utils.GetREST(svcUrl, successData, errorData);
    }
}