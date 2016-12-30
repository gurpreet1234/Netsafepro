var ProductsManagement = (function () {
    function ProductsManagement() {
    }
    ProductsManagement.FormFunctions = function () {
        ProductsManagement.ListProducts();
        $("#productCancel").unbind('click');
        $("#productCancel").click(function () {
            $("#hiderProducts").fadeOut("slow");
            $('#updateProduct').fadeOut("slow");
        });

        $("#productUpdate").unbind('click');
        $("#productUpdate").click(function () {
            var prodID = $("#hdnProductId").val();
            ProductsManagement.updateProduct();
        });

        $("#productRefresh").unbind('click');
        $("#productRefresh").click(function () {
            ProductsManagement.ListProducts();
            $("#hiderProducts").fadeOut("slow");
            $('#updateProduct').fadeOut("slow");
        });
    };

    ProductsManagement.ListProducts = function () {
        $('#loadingmessage').show();
        $("#headertext").text("Products");

        var gridList = "<table id='tblProducts' cellspacing='0' cellpadding='0' class='grid'>" + "<thead class='grid_header'><tr>" + "<th class='grid_header_first_column'>Name</th><th class='grid_header_column'>Monthly Price</th>" + "<th class='grid_header_column' colspan='2'>Actions</th></tr></thead><tbody>";

        var url = "Products";

        var successData = function (response) {
            $(response).find('Product').each(function () {
                gridList = gridList + "<tr><td class='first_bottom_border'>" + $(this).find('ProductName').text() + "</td>" + "<td class='bottom_border'>$" + $(this).find('MonthlyPrice').text() + "</td>" + "<td class='bottom_border'><a href='#' onclick='OpenProducts(" + $(this).find('Id').text() + ")'>Edit</a></td>" + "</tr>";
            });
            gridList = gridList + "</tbody></table>";
            $("#dvProducts").html(gridList);
            $('#loadingmessage').hide();
        };
        var errorData = function (response) {
            $('#loadingmessage').hide();
        };
        Utils.GetREST(url, successData, errorData);
    };

    ProductsManagement.addProduct = function () {
        if ($("#ProductDescription").val() && $("#MonthlyPrice").val() && $("#ProductName").val()) {
            var description = $("#ProductDescription").val(), monthlyPrice = $("#MonthlyPrice").val(), productName = $("#ProductName").val();
            var data = {
                Description: description,
                MonthlyPrice: monthlyPrice,
                ProductName: productName
            };
            var svcUrl;
            svcUrl = "AddProduct";
            var successData = function (response, textStatus, xhr) {
                $("#productRefresh").click();
                $("#loadingmessage").hide();
                alert("The record has been added successfully.");
                $("#hiderProducts").fadeOut("slow");
                $("#addProduct").fadeOut("slow");
                ProductsManagement.ListProducts();
            };
            var errorData = function (response) {
                $("#loadingmessage").hide();
                $("#hiderProducts").fadeOut("slow");
                $("#addProduct").fadeOut("slow");
                ProductsManagement.ListProducts();
            };
            Utils.JsonREST(svcUrl, data, successData, errorData);
        } else {
            alert("All fields are mandatory.");
        }
    };

    ProductsManagement.editProduct = function (productId) {
        $("#loadingmessage").show();
        var svcUrl;
        svcUrl = "Product/" + productId;
        var successData = function (response) {
            $("#hdnProductId").val($(response).find('Id').text());
            $("#txtproductName").val($(response).find('ProductName').text());
            $("#txtproductDescription").val($(response).find('Description').text());
            $("#txtmonthlyPrice").val($(response).find('MonthlyPrice').text());
            $("#loadingmessage").hide();
        };
        var errorData = function (response) {
            alert("Something happened wrong, Please try again later.");
        };
        Utils.GetREST(svcUrl, successData, errorData);
    };

    ProductsManagement.updateProduct = function () {
        if ($("#txtproductDescription").val() && $("#txtmonthlyPrice").val() && $("#txtproductName").val()) {
            $("#loadingmessage").show();
            var description = $("#txtproductDescription").val(), monthlyPrice = $("#txtmonthlyPrice").val(), productName = $("#txtproductName").val();
            var GroupData = {
                Description: description,
                MonthlyPrice: monthlyPrice,
                ProductName: productName
            };
            var svcUrl;
            svcUrl = "updateProduct/" + $("#hdnProductId").val();
            var successData = function (response, textStatus, xhr) {
                $("#productRefresh").click();
                $("#loadingmessage").hide();
                alert("The record has been updated successfully.");
            };
            var errorData = function (response) {
                $("#loadingmessage").hide();
                alert("Something happened wrong, Please try again later.");
            };
            Utils.JsonREST(svcUrl, GroupData, successData, errorData);
        } else {
            alert("All fields are mandatory.");
        }
    };
    return ProductsManagement;
})();
var userProfileObject = (function () {
    function userProfileObject() {
    }
    return userProfileObject;
})();

var AdminManagement = (function () {
    function AdminManagement() {
    }
    AdminManagement.FormFunctions = function () {
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
    };

    AdminManagement.ListFilterCustomers = function (filterKey, filterQuery) {
        if (typeof filterQuery === "undefined") { filterQuery = ""; }
        $("#addbuttonproduct").hide();
        $("#headertext").text("Customers");
        var gridList = "<table id='tblCustomers' cellspacing='0' cellpadding='0' class='grid'>" + "<thead class='grid_header'><tr>" + "<th class='grid_header_first_column'>First Name</th><th class='grid_header_first_column'>Last Name</th><th class='grid_header_column'>Email</th><th class='grid_header_column'>Phone</th>" + "<th class='grid_header_column'>Zip</th><th class='grid_header_column'>Actions</th><th class='grid_header_column'>View</th></tr></thead><tbody>";
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
                gridList = gridList + "<tr><td class='first_bottom_border'>" + this.FirstName + "</td>" + "<td class='first_bottom_border'>" + this.LastName + "</td>" + "<td class='bottom_border'>" + this.EmailAddress + "</td>" + "<td class='bottom_border'>" + this.Phone + "</td>" + "<td class='bottom_border'>" + this.Zip + "</td>" + "<td class='bottom_border'><a href='#' onclick='OpenCustomerProducts(" + this.UserId + ",\"" + customerName + "\")'>Products</a>" + " | <a href='#' onclick='PayCustomerProducts(" + this.UserId + ")'>Pay</a>" + " | <a href='#' onclick='EditCustomer(" + this.UserId + ")'>Edit</a>" + " | <a href='#' onclick='DeleteCustomer(" + this.UserId + ")'>Delete</a></td>" + "<td class='bottom_border'><a href='#' onclick='CustomerInformation(" + this.UserId + ",\"" + this.EmailAddress + "\",\"" + this.Phone + "\",\"" + this.Zip + "\",\"" + this.FirstName + "\",\"" + this.LastName + "\",\"" + this.Address + "\",\"" + this.City + "\",\"" + this.State + "\")'>View</a>" + " </td>" + "</tr>";
            }
        });
        gridList = gridList + "</tbody></table>";
        $("#dvCustomers").html(gridList).triggerHandler("contentChanged");
        $('#loadingmessage').hide();
    };

    AdminManagement.ListAllCustomers = function (filterQuery) {
        if (typeof filterQuery === "undefined") { filterQuery = ""; }
        $("#addbuttonproduct").hide();
        $('#loadingmessage').show();

        $("#searchCustomer").html("Search:<select id='searchValue' class='form-control'>" + "<option value='1'>Name</option>" + "<option value='2'>Email</option>" + "<option value='3'>Phone</option>" + "</select>" + "<input type='text' id='txtSearch' placeholder='Enter search text' class='form-control' onkeyup='searchCustomers(this)'/>");

        $("#headertext").text("Customers");

        var gridList = "<table id='tblCustomers' cellspacing='0' cellpadding='0' class='grid'>" + "<thead class='grid_header'><tr>" + "<th class='grid_header_first_column'>First Name</th><th class='grid_header_first_column'>Last Name</th><th class='grid_header_column'>Email</th><th class='grid_header_column'>Phone</th>" + "<th class='grid_header_column'>Zip</th><th class='grid_header_column'>Actions</th><th class='grid_header_column'>View</th></tr></thead><tbody>";

        var url = "";
        if (filterQuery != "")
            url = encodeURI("Customers/" + filterQuery + "/filter");
        else
            url = "AdminCustomersList";
        var successData = function (response) {
            var userProfile = [];
            $(response).find('UserProfile_Poco').each(function () {
                var userProfileObject = {};
                $('#OneCustomer').hide();
                $('#tabDetails').hide();
                var customerName = $(this).find('FirstName').text() + " " + $(this).find('LastName').text();
                gridList = gridList + "<tr><td class='first_bottom_border'>" + $(this).find('FirstName').text() + "</td>" + "<td class='first_bottom_border'>" + $(this).find('LastName').text() + "</td>" + "<td class='bottom_border'>" + $(this).find('EmailAddress').text() + "</td>" + "<td class='bottom_border'>" + $(this).find('Phone').text() + "</td>" + "<td class='bottom_border'>" + $(this).find('Zip').text() + "</td>" + "<td class='bottom_border'><a href='#' onclick='OpenCustomerProducts(" + $(this).find('UserId').text() + ",\"" + customerName + "\")'>Products</a>" + " | <a href='#' onclick='PayCustomerProducts(" + $(this).find('UserId').text() + ")'>Pay</a>" + " | <a href='#' onclick='EditCustomer(" + $(this).find('UserId').text() + ")'>Edit</a>" + " | <a href='#' onclick='DeleteCustomer(" + $(this).find('UserId').text() + ")'>Delete</a></td>" + "<td class='bottom_border'><a href='#' onclick='CustomerInformation(" + $(this).find('UserId').text() + ",\"" + $(this).find('EmailAddress').text() + "\",\"" + $(this).find('Phone').text() + "\",\"" + $(this).find('Zip').text() + "\",\"" + $(this).find('FirstName').text() + "\",\"" + $(this).find('LastName').text() + "\",\"" + $(this).find('Address').text() + "\",\"" + $(this).find('City').text() + "\",\"" + $(this).find('State').text() + "\")'>View</a>" + " </td>" + "</tr>";
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
    };

    AdminManagement.customerAdd = function () {
        $("#addbuttonproduct").hide();
        if ($("#CustomerFirstName").val() && $("#CustomerLastName").val() && $("#CustomerEmail").val() && $("#CustomerPhone").val() && $("#CustomerZip").val() && $("#CustomerPassword").val()) {
            var firstname = $("#CustomerFirstName").val(), lastname = $("#CustomerLastName").val(), email = $("#CustomerEmail").val(), phone = $("#CustomerPhone").val(), zip = $("#CustomerZip").val(), password = $("#CustomerPassword").val();

            var data = {
                CellPhone: phone,
                FirstName: firstname,
                LastName: lastname,
                EmailAddress: email,
                Zip: zip,
                Password: password,
                Active: true
            };

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
    };

    AdminManagement.EditCustomer = function (customerId) {
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
    };

    AdminManagement.UpdateCustomer = function () {
        $("#addbuttonproduct").hide();
        if ($("#txtUpdateFirstName").val() && $("#txtUpdateLastName").val() && $("#txtUpdateEmail").val() && $("#txtUpdatePhone").val() && $("#txtUpdateZip").val()) {
            $("#loadingmessage").show();
            var firstname = $("#txtUpdateFirstName").val(), lastname = $("#txtUpdateLastName").val(), email = $("#txtUpdateEmail").val(), phone = $("#txtUpdatePhone").val(), zip = $("#txtUpdateZip").val(), password = $("#txtUpdatePassword").val();

            var data = {
                CellPhone: phone,
                FirstName: firstname,
                LastName: lastname,
                EmailAddress: email,
                Password: password,
                Zip: zip,
                Active: 1
            };
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
    };

    AdminManagement.EditHistory = function (customerId) {
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
    };

    AdminManagement.UpdateHistory = function () {
        $("#addbuttonproduct").hide();
        if ($("#Username").val() && $("#YearlyPrice").val() && $("#MonthlyPrice").val() && $("#PaymentPeriod").val() && $("#LastPaymentDate").val()) {
            $("#loadingmessage").show();
            var UserName = $("#Username").val(), Id = $("#id").val(), YearlyPrice = $("#YearlyPrice").val(), MonthlyPrice = $("#MonthlyPrice").val(), PaymentPeriod = $("#PaymentPeriod").val(), LastPaymentDate = $("#LastPaymentDate").val();

            var data = {
                UserName: UserName,
                YearlyPrice: YearlyPrice,
                MonthlyPrice: MonthlyPrice,
                PaymentPeriod: PaymentPeriod,
                LastPaymentDate: LastPaymentDate
            };
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
    };

    AdminManagement.ListCustomerProducts = function (customerId, customerName) {
        var paidstatus;
        $("#addbutton").hide();
        $("#addbuttonproduct").show();
        $('#loadingmessage').show();
        $("#headertext").text("Customer Products");

        //historyGrid
        var historyList = "<table id='tblCustomers' cellspacing='0' cellpadding='0' class='grid'>" + "<tr style='font-weight:bold;'><td style='width:70px;'>Customer: </td><td>" + customerName + "</td></tr></table>";

        historyList = historyList + "<table id='tblCustomerProducts' cellspacing='0' cellpadding='00' class='grid'>" + "<thead class='grid_header'><tr>" + "<th class='grid_header_first_column' style='width: 10 %;'>Primary UserName</th><th class='grid_header_column' style='width:10%;'>License Start Date</th><th class='grid_header_column' style='width: 10 %;'>License End Date</th><th class='grid_header_column' style='width: 10 %;'>YearlyPrice</th>" + "<th class='grid_header_column' style='width:10%;'>MonthlyPrice</th>" + "<th class='grid_header_column' style='width: 10 %;'>PaymentPeriod</th>" + "<th class='grid_header_column' style='width: 10 %;'>Last Payment Date</th>" + "<th class='grid_header_column' style='width: 10 %;'>Paid</th>" + "<th class='grid_header_column' style='width: 10 %;'>Status</th>" + "<th class='grid_header_column' style='width: 10%;'>Created Date</th>" + "<th class='grid_header_column' style='width: 10%;'>Edit</th></tr></thead><tbody>";

        //ProductGrid
        var gridList = "<table id='tblCustomers' cellspacing='0' cellpadding='0' class='grid'>" + "<tr style='font-weight:bold;'><td style='width:70px;'>Customer: </td><td>" + customerName + "</td></tr></table>";

        gridList = gridList + "<table id='tblCustomerProducts' cellspacing='0' cellpadding='0' class='grid'>" + "<thead class='grid_header'><tr>" + "<th class='grid_header_first_column'>Primary UserName</th><th class='grid_header_column' style='width:10%;'>Location</th><th class='grid_header_column'>Operating System</th><th class='grid_header_column'>Type</th>" + "<th class='grid_header_column'>Status</th>" + "<th class='grid_header_column'>Created Date</th>" + "<th class='grid_header_column'>License Start Date</th>" + "<th class='grid_header_column'>License End Date</th>" + "<th class='grid_header_column'>Actions</th></tr></thead><tbody>";

        var url = "Customer/" + customerId + "/AdminCustomersList";

        var successData = function (response) {
            //For Product
            $(response).find('ProductToUser_Price_POCO').each(function () {
                gridList = gridList + "<tr>";

                var settings = $(this).find('Settings').text();
                if (settings == "1" || settings == "2" || settings == "3") {
                    gridList = gridList + "<td class='first_bottom_border'>" + $(this).find('PrimaryUserName').text() + "</td>" + "<td class='bottom_border' style='width:10%;'>" + $(this).find('Location').text() + "</td>" + "<td class='bottom_border'>" + $(this).find('OperatingSystem').text() + "</td>";

                    if (settings == "1")
                        gridList = gridList + "<td class='bottom_border'>Moderate License</td>";
                    if (settings == "2")
                        gridList = gridList + "<td class='bottom_border'>Business License</td>";
                    if (settings == "3")
                        gridList = gridList + "<td class='bottom_border'>Personal License</td>";
                } else {
                    gridList = gridList + "<td class='first_bottom_border'>" + $(this).find('PrimaryUser').text() + "</td>" + "<td class='bottom_border'>" + $(this).find('Usage').text() + "</td>" + "<td class='bottom_border'>" + $(this).find('PhoneOS').text() + "</td>" + "<td class='bottom_border'>Device</td>";
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
                        historyList = historyList + "<td class='first_bottom_border' style='width: 10%;'>" + $(this).find('PrimaryUserName').text() + "</td>" + "<td class='bottom_border' style='width: 10%;'>" + $(this).find('StartDate').text() + "</td>" + "<td class='bottom_border' style='width: 10%;'>" + endDate + "</td>";
                    } else {
                        historyList = historyList + "<td class='first_bottom_border' style='width: 10%;'>" + $(this).find('PrimaryUser').text() + "</td>" + "<td class='bottom_border' style='width: 10%;'>" + $(this).find('StartDate').text() + "</td>" + "<td class='bottom_border' style='width: 10%;'>" + endDate + "</td>";
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

                    historyList = historyList + "</tr>";
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
    };

    AdminManagement.ListProducts = function () {
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
    };

    AdminManagement.SingleCustomerProductAdd = function (getvalues) {
        if (!getvalues) {
            getvalues = [];
        }
        if ($("#PrimaryuserName").val() && $("#operatingSystem").val() && $("#settings").val()) {
            var primaryusername = $("#PrimaryuserName").val(), productusername = $("#productusername").val(), userId = $("#productuserid").val(), productid = $("#AllProductList").val(), location = $("#location").val(), operatingSystem = $("#operatingSystem").val(), settings = $("#settings").val(), Usage = $("#Usage").val();

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
            };

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
    };

    AdminManagement.dateformate = function (date) {
        var d = new Date(date), month = d.getMonth() + 1, day = d.getDate(), year = d.getFullYear();
        var finaldate = month + "-" + day + "-" + year;
        return finaldate;
    };

    AdminManagement.getFormattedDate = function (date) {
        var year = date.getFullYear();
        var month = (1 + date.getMonth()).toString();
        month = month.length > 1 ? month : '0' + month;
        var day = date.getDate().toString();
        day = day.length > 1 ? day : '0' + day;

        var seconds = date.getSeconds();
        var minutes = date.getMinutes();
        var hour = date.getHours();

        return year + '-' + month + '-' + day + 'T' + hour + ':' + minutes + ':' + seconds + '.' + 0;
    };

    AdminManagement.getCustomerProductPrice = function (customerId) {
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
    };

    AdminManagement.DeleteCustomerProducts = function (customerId) {
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
    };
    return AdminManagement;
})();
var Users = (function () {
    function Users() {
    }
    Users.FormFunctions = function () {
    };
    Users.checkUserNameAvailablityForgotpassword = function (email) {
        $("#processingDiv").show();
        var CustomerData = "<Customer_Poco xmlns=\"http://schemas.datacontract.org/2004/07/SafenetproAPI.Controllers\" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\">" + "<EmailAddress>" + email + "</EmailAddress>" + "</Customer_Poco>";

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
            } else {
                $("#processingDiv").hide();
                $("#lblEmailNotExists").hide();

                $("#emailSection").hide();
                $("#emailSent").show();
                $("#sendButton").hide();
            }
        };
        Utils.PostREST(svcUrl, CustomerData, successData, errorData);
    };

    Users.checkUserNameAvailablity = function (userName, userId) {
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
            } else {
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
    };

    Users.checkUserNameAvailable = function (userName, userId) {
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
            } else {
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
    };
    return Users;
})();
var ProductLicenses = (function () {
    function ProductLicenses() {
    }
    ProductLicenses.FormFunctions = function () {
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
    };

    ProductLicenses.deleteComputerOrDevices = function (productItemId) {
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
    };

    ProductLicenses.editComputerOrDevices = function (productItemId) {
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
                } else {
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
    };

    ProductLicenses.addNewURL = function (c, val) {
        $("#addURL" + c + "_" + (val)).hide();
        $("#deleteURL" + c + "_" + (val)).hide();

        var str = " <div class=\"field\" id=\"dvURL" + c + "_" + (val + 1) + "\">" + " <input style='width:87%; float:left; margin-bottom:5px;' type=\"text\" id=\"URL_device" + c + "_" + (val + 1) + "\" class=\"form-control\">" + " <button onclick='addNewURL(" + c + "," + (val + 1) + ")' style='padding:4px; float:left; margin-left:5px;' type=\"button\" id=\"addURL" + c + "_" + (val + 1) + "\" class=\"btn btn-primary\">+</button>" + " <button onclick='deleteURL(" + c + "," + (val + 1) + ",\"dvURL" + c + "_" + (val + 1) + "\")' style='padding:4px; float:left; margin-left:5px;' type=\"button\" id=\"deleteURL" + c + "_" + (val + 1) + "\" class=\"btn btn-primary\">-</button>" + " </div>";
        $("#dvDevice_" + c).append(str);
    };

    ProductLicenses.updateComputerOrDevices = function (productType) {
        $("#processingDiv").show();
        var GroupData = "<ProductToUser_POCO xmlns=\"http://schemas.datacontract.org/2004/07/SafenetproAPI.Controllers\" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\">" + "<ID>" + $("#hdnId").val() + "</ID>" + "<Location>" + $("#location0").val() + "</Location>" + "<Manufacturer>" + $("#manufacturer_device0").val() + "</Manufacturer>" + "<OperatingSystem>" + $("#operatingSystem0").val() + "</OperatingSystem>" + "<PhoneOS>" + $("#operatingSystem_Device0").val() + "</PhoneOS>" + "<PrimaryUser>" + $("#primaryusername_device0").val() + "</PrimaryUser>" + "<PrimaryUserName>" + $("#primaryusername0").val() + "</PrimaryUserName>" + "<ProductId>" + $("#hdnProductId").val() + "</ProductId>";
        if (productType == 1)
            GroupData = GroupData + "<Settings>" + $("#ContentPlaceHolder1_settings0").val() + "</Settings>";
        else
            GroupData = GroupData + "<Settings>" + $("#ContentPlaceHolder1_settings_device0").val() + "</Settings>";

        GroupData = GroupData + "<Settings>" + $("#ContentPlaceHolder1_settings0").val() + "</Settings>" + "<Usage>" + $("#usage_device0").val() + "</Usage>";

        if (productType == 1) {
            GroupData = GroupData + "<uDevicePoco>";
            var count = 0;
            $("#dvDevice_0" + " input[type='text']").each(function () {
                GroupData = GroupData + "<URLtoDevice_POCO>" + "<ID>" + count + "</ID>" + "<ProductToUser>" + $("#hdnId").val() + "</ProductToUser>" + "<URL>" + $(this).val() + "</URL>" + "</URLtoDevice_POCO>";
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
    };

    ProductLicenses.updateComputerOrDevicesChange = function (productType) {
        $("#processingDiv").show();
        var GroupData = "<ProductToUser_POCO xmlns=\"http://schemas.datacontract.org/2004/07/SafenetproAPI.Controllers\" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\">" + "<ID>" + $("#hdnId").val() + "</ID>" + "<Location>" + $("#location0").val() + "</Location>" + "<Manufacturer>" + $("#manufacturer_device0").val() + "</Manufacturer>" + "<OperatingSystem>" + $("#operatingSystem0").val() + "</OperatingSystem>" + "<PhoneOS>" + $("#operatingSystem_Device0").val() + "</PhoneOS>" + "<PrimaryUser>" + $("#primaryusername_device0").val() + "</PrimaryUser>" + "<PrimaryUserName>" + $("#primaryusername0").val() + "</PrimaryUserName>" + "<ProductId>" + $("#hdnProductId").val() + "</ProductId>";

        if (productType == 1)
            GroupData = GroupData + "<Settings>" + $("#ContentPlaceHolder1_settings0").val() + "</Settings>";
        else
            GroupData = GroupData + "<Settings>" + $("#ContentPlaceHolder1_settings_device0").val() + "</Settings>";

        GroupData = GroupData + "<Usage>" + $("#usage_device0").val() + "</Usage>";

        if (productType == 1) {
            GroupData = GroupData + "<uDevicePoco>";
            var count = 0;
            $("#dvDevice_0" + " input[type='text']").each(function () {
                GroupData = GroupData + "<URLtoDevice_POCO>" + "<ID>" + count + "</ID>" + "<ProductToUser>" + $("#hdnId").val() + "</ProductToUser>" + "<URL>" + $(this).val() + "</URL>" + "</URLtoDevice_POCO>";
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
    };

    ProductLicenses.deleteComputerOrDevicesChange = function (productItemId) {
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
    };
    return ProductLicenses;
})();
var Utils = (function () {
    function Utils() {
    }
    Utils.FormFunctions = function () {
    };

    Utils.GetREST = function (url, fsuccess, ferror) {
        $.ajax({
            type: "GET",
            url: this.getBaseUrl() + url,
            dataType: "xml",
            success: fsuccess,
            cache: false,
            error: ferror
        });
    };

    Utils.PutREST = function (url, data, fsuccess, ferror) {
        $.ajax({
            type: "PUT",
            url: this.getBaseUrl() + url,
            dataType: "xml",
            contentType: "application/xml;charset=UTF-8",
            data: data,
            success: fsuccess,
            error: ferror
        });
    };

    Utils.PostREST = function (url, data, fsuccess, ferror) {
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

    Utils.DeleteREST = function (url, fsuccess, ferror) {
        $.ajax({
            type: "DELETE",
            url: this.getBaseUrl() + url,
            success: fsuccess,
            error: ferror
        });
    };

    Utils.getBaseUrl = function () {
        if (window.location.toString().indexOf("safenetpro") >= 0)
            return "http://netsafepro.safenetpro.com/api/";
        else if (window.location.toString().indexOf("buyschoolbook") >= 0)
            return "http://buyschoolbook.com/api/";
        else if (window.location.toString().indexOf("localhost") >= 0)
            return "http://localhost:50751/api/";
        else if (window.location.toString().indexOf("netsafepro") >= 0)
            return "https://www.netsafepro.com/api/";
    };
    Utils.JsonREST = function (url, data, fsuccess, ferror) {
        $.ajax({
            type: "POST",
            url: this.getBaseUrl() + url,
            dataType: "json",
            data: data,
            success: fsuccess,
            error: ferror
        });
    };

    Utils.JsonGetREST = function (url, fsuccess, ferror) {
        $.ajax({
            type: "GET",
            url: this.getBaseUrl() + url,
            dataType: "json",
            success: fsuccess,
            error: ferror
        });
    };
    return Utils;
})();
var SetupComputer = (function () {
    function SetupComputer() {
    }
    return SetupComputer;
})();
var SetupURL = (function () {
    function SetupURL() {
    }
    return SetupURL;
})();

var Setup = (function () {
    function Setup() {
    }
    Setup.FormFunctions = function () {
        Setup.setupComputerDeviceList = [];
        Setup.setupDeviceURL = [];
    };

    Setup.addComputerorDevice = function (operatingSystem, location, primaryUserName, settings, phoneOS, manufacturer, primaryUser, usage, productType, rowId) {
        var Id = 0;
        if (Setup.setupComputerDeviceList.length > 0)
            Id = Setup.setupComputerDeviceList[Setup.setupComputerDeviceList.length - 1].Id + 1;

        Setup.setupComputerDeviceList.push({
            Id: Id, OperatingSystem: operatingSystem, Location: location, PrimaryUserName: primaryUserName,
            Settings: settings, PhoneOS: phoneOS, Manufacturer: manufacturer, PrimaryUser: primaryUser, Usage: usage, productType: productType,
            rowId: rowId
        });
    };

    Setup.addURLDevice = function (URL) {
        var Id = 0;
        if (Setup.setupDeviceURL.length > 0)
            Id = Setup.setupDeviceURL[Setup.setupDeviceURL.length - 1].Id + 1;

        var prodId = 0;
        if (Setup.setupComputerDeviceList.length > 0)
            prodId = Setup.setupComputerDeviceList[Setup.setupComputerDeviceList.length - 1].Id;

        Setup.setupDeviceURL.push({
            Id: Id, URL: URL, productToUser: prodId
        });
    };

    Setup.updateURLDevice = function (productType, URL, rowId) {
        var Id = 0;
        if (Setup.setupDeviceURL.length > 0)
            Id = Setup.setupDeviceURL[Setup.setupDeviceURL.length - 1].Id;

        var cArray = Setup.setupComputerDeviceList.filter(function (obj) {
            return (obj.productType == productType && obj.rowId === rowId);
        });

        Setup.setupDeviceURL.push({
            Id: Id, URL: URL, productToUser: cArray[0].Id
        });
    };

    Setup.updateComputerorDevice = function (operatingSystem, location, primaryUserName, settings, phoneOS, manufacturer, primaryUser, usage, productType, rowId) {
        var cArray = Setup.setupComputerDeviceList.filter(function (obj) {
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
    };

    Setup.deleteComputerorDevice = function (productType, row) {
        var cArray = Setup.setupComputerDeviceList.filter(function (obj) {
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
    };

    Setup.getComputerDeviceDetails = function (row) {
        //return Setup.setupComputerDeviceList[row];
        var cArray = Setup.setupComputerDeviceList.filter(function (obj) {
            return (obj.rowId === row);
        });

        return cArray[0];
    };

    Setup.getURLs = function (row) {
        var fArray = Setup.setupDeviceURL.filter(function (obj) {
            return (obj.productToUser === row);
        });

        return fArray;
    };

    Setup.getURLsWithRow = function (productType, row) {
        var cArray = Setup.setupComputerDeviceList.filter(function (obj) {
            return (obj.productType == productType && obj.rowId === row);
        });

        var fArray = Setup.setupDeviceURL.filter(function (obj) {
            return (obj.productToUser === cArray[0].Id);
        });

        return fArray;
    };

    Setup.saveComputerOrDevices = function () {
        $("#processingDiv").show();
        var userId = sessionStorage.getItem("userId");

        var GroupData = "<ArrayOfProductToUser_POCO xmlns=\"http://schemas.datacontract.org/2004/07/SafenetproAPI.Controllers\" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\">";

        for (var i = 0; i < Setup.setupComputerDeviceList.length; i++) {
            //var _settings = Setup.setupComputerDeviceList[i].Settings;
            //if (_settings == "" || _settings == null)
            //    _settings = "4";
            GroupData = GroupData + "<ProductToUser_POCO>" + "<ID>" + Setup.setupComputerDeviceList[i].Id + "</ID>" + "<Location>" + Setup.setupComputerDeviceList[i].Location + "</Location>" + "<Manufacturer>" + Setup.setupComputerDeviceList[i].Manufacturer + "</Manufacturer>" + "<OperatingSystem>" + Setup.setupComputerDeviceList[i].OperatingSystem + "</OperatingSystem>" + "<PhoneOS>" + Setup.setupComputerDeviceList[i].PhoneOS + "</PhoneOS>" + "<PrimaryUser>" + Setup.setupComputerDeviceList[i].PrimaryUser + "</PrimaryUser>" + "<PrimaryUserName>" + Setup.setupComputerDeviceList[i].PrimaryUserName + "</PrimaryUserName>" + "<ProductId>" + Setup.setupComputerDeviceList[i].productType + "</ProductId>" + "<Settings>" + Setup.setupComputerDeviceList[i].Settings + "</Settings>" + "<Usage>" + Setup.setupComputerDeviceList[i].Usage + "</Usage>";

            if (Setup.setupComputerDeviceList[i].productType == 1) {
                var productURL = Setup.getURLs(Setup.setupComputerDeviceList[i].Id);
                GroupData = GroupData + "<uDevicePoco>";

                for (var j = productURL.length - 1; j >= 0; j--) {
                    GroupData = GroupData + "<URLtoDevice_POCO>" + "<ID>" + productURL[j].Id + "</ID>" + "<ProductToUser>" + productURL[j].productToUser + "</ProductToUser>" + "<URL>" + productURL[j].URL + "</URL>" + "</URLtoDevice_POCO>";
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
    };

    Setup.removeAllProducts = function () {
        Setup.setupComputerDeviceList = [];
        Setup.setupDeviceURL = [];
    };

    Setup.checkPrimaryNameAvailablity = function (primaryuserName, userId, c) {
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
            } else {
                $("#processingDiv").hide();
                $("#lblAlreadyExists").show();

                $("#setupYes" + c).attr("disabled", true);
                $("#setupNo" + c).attr("disabled", true);
            }
        };
        Utils.GetREST(svcUrl, successData, errorData);
    };
    Setup.setupComputerDeviceList = [];
    Setup.setupDeviceURL = [];
    return Setup;
})();
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
            alert("Actions saved successfully.");
        };
        var errorData = function (response) {
            RulePermissions.removeAllDisplayActions();
            alert("Something happened wrong, Please try again.");
        };
        Utils.PostREST(svcUrl, GroupData, successData, errorData);
    };
    RulePermissions.displayActionList = [];
    return RulePermissions;
})();
//# sourceMappingURL=CombineTypescript.js.map
