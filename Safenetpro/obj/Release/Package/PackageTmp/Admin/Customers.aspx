<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/admin.Master" AutoEventWireup="true" CodeBehind="Customers.aspx.cs" Inherits="Safenetpro.Admin.Customers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.6.3/css/font-awesome.min.css" />
    <script src="//code.jquery.com/ui/1.11.4/jquery-ui.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>

    <script src="../js/jquery.tablesorter.js"></script>
    <script type="text/javascript">
        function searchCustomers(val) {
            AdminManagement.ListFilterCustomers($("#searchValue").val(), val.value);
        }
        //Datepicker and tab function
        activeTab();
        $('#OneCustomer').hide();
        $('#tabDetails').hide();
        function activeTab() {
            var a = "#A";
            localStorage.setItem('lastTab', a);
            var lastTab = localStorage.getItem('lastTab');
            if (lastTab) {
                $('a[href="#A"]').click();
            }
        }
        $(function () {
            var currentDate = new Date();
            $('#ContentPlaceHolder1_txtStartDate').datepicker({
                dateFormat: 'yy-mm-dd'
            });
            $("#ContentPlaceHolder1_txtStartDate").datepicker("setDate", currentDate);
        });
        //Datepicker and tab function

        //Customer Information Tab View Function
        var customerIds;
        var customerNames;
        function CustomerInformation(customerId, Email, Phone, zip, firstName, lastName, address, city, state) {
            $("#searchCustomer").hide();
            $("#headertext").text("CUSTOMER INFORMATION");
            customerIds = customerId;
            customerNames = firstName + "" + lastName;
            $('#firstname').val(firstName);
            $('#lastname').val(lastName);
            $('#address').val(address);
            $('#city').val(city);
            $('#state').val(state);
            $('#zip').val(zip);
            $('#phone').val(Phone);
            $('#email').val(Email);
            $('#dvCustomers').hide();
            $('#OneCustomer').show();
            $('#tabDetails').show();
            activeTab();
            $('#report_tbody').append('<tr><td class="first_bottom_border">' + firstName + " " + lastName + '</td><td class="bottom_border">' + Email + '</td><td class="bottom_border">' + Phone + '</td><td class="bottom_border">' + zip + '</td>' +
                "<td class='bottom_border'><a href='#' onclick='DeleteCustomer(" + customerId + ")'>Delete</a></td>" +
                "</tr>");
        }
        function goBack() {
            $("#searchCustomer").show();
            $("#headertext").text("CUSTOMERS");
            $('#report_tbody').empty();
            $('#name').val("");
            $('#email').val("");
            $('#phone').val("");
            $('#zip').val("");
            $('#dvCustomers').show();
            $('#OneCustomer').hide();
            $('#tabDetails').hide();
            AdminManagement.ListAllCustomers();
        }
        function Product() {
            AdminManagement.ListCustomerProducts(customerIds, customerNames);
        }
        function Profile() {
            $("#headertext").text("CUSTOMER PROFILE");
        }
        function History() {
            $("#headertext").text("CUSTOMER HISTORY");
            AdminManagement.ListCustomerProducts(customerIds, customerNames);
        }
        //Customer Information Tab View Function
        function OpenCustomerProducts(customerId, customerName) {
            customerNames = customerName;
            customerIds = customerId;
            AdminManagement.ListCustomerProducts(customerId, customerName);
        }
        function UpdateCustomerLicense(Id) {
            $("#ContentPlaceHolder1_hdnProductToUserId").val(Id);
            $("#hiderLicense").fadeIn("slow");
            $('#licenseDate').fadeIn("slow");
        }

        function DeleteCustomer(customerId) {
            AdminManagement.DeleteCustomerProducts(customerId);
        }

        function EditCustomer(customerId) {
            AdminManagement.EditCustomer(customerId);
            $("#hiderProducts").fadeIn("slow");
            $('#updateCustomer').fadeIn("slow");

        }

        function PayCustomerProducts(customerId) {
            $("#ContentPlaceHolder1_hdnUserId").val(customerId);
            $("#ContentPlaceHolder1_x_card_num").val('');
            $("#ContentPlaceHolder1_x_exp_date").val('');
            $("#ContentPlaceHolder1_x_card_cvv").val('');
            $("#ContentPlaceHolder1_paymentPeriod").val('1');

            AdminManagement.getCustomerProductPrice(customerId);

            $("#hiderProducts").fadeIn("slow");
            $('#paymentDetails').fadeIn("slow");

            //AdminManagement.ListCustomerProducts(customerId);
        }

        function EditHistory(customerId) {
            AdminManagement.EditHistory(customerId);
            $('#historypopup').fadeIn("slow");
            $("#hiderProducts").fadeIn("slow");
        }

        function historyCancels() {
            $('#historypopup').fadeOut("slow");
            $("#hiderProducts").fadeOut("slow");
        }

        function UpdateHistory() {
            AdminManagement.UpdateHistory();
        }



        $(document).ready(function () {
            AdminManagement.FormFunctions();
            $("#btnAdd").text("Add");
            //Utils.CommonUtils.ListAllCustomersAutoComplete(false, true);
            $('#dvCustomers').bind('contentChanged', function (event, data) {
                $("#tblCustomers").tablesorter({});
                //$("#txtCustomer").autocomplete({
                //    source: Utils.CommonUtils.arrayCustomerResult
                //});
            });


        });

        function changeFilterType() {
            var filterType = $("#selField").val();
            if (filterType == 'CustomerName') {
                $("#txtCustomer").autocomplete({
                    source: Utils.CommonUtils.arrayCustomerResult
                });
            }
            else if (filterType == 'Email') {
                $("#txtCustomer").autocomplete({
                    source: Utils.CommonUtils.arrayCustomerEmailResult
                });
            }
            else {
                $("#txtCustomer").autocomplete({
                    source: Utils.CommonUtils.arrayCustomerUniqueIdResult
                });
            }
        }

        function changePeriod() {
            var calAmount = $("#ContentPlaceHolder1_paymentPeriod option:selected").val() * $("#ContentPlaceHolder1_txtActualAmount").val();
            $("#ContentPlaceHolder1_txtAmount").val(calAmount);
        }

        function newCustomerAdd() {
            $("#CustomerFirstName").val("");
            $("#CustomerLastName").val("");
            $("#CustomerEmail").val("");
            $("#CustomerPhone").val("");
            $("#CustomerZip").val("");
            $("#CustomerPassword").val("");
            $("#hiderProducts").fadeIn("slow");
            $("#addCustomer").fadeIn("slow");
        }

        function customerCancels() {
            $("#hiderProducts").fadeOut("slow");
            $("#addCustomer").fadeOut("slow");
        }

        function addnewcustomer() {
            AdminManagement.customerAdd();
        }

        function customerUpdate() {
            AdminManagement.UpdateCustomer()
        }
        var getvalue = [];
        function setupComputer() {
            $("#Setupscreen").fadeOut("slow");
            $("#TextBoxesGroup").show();
            $("#locations").show();
            $("#Usages").hide();
            $("#productusername").val(customerNames)
            $("#productuserid").val(customerIds)
            $("#userName").val(Username);
            AdminManagement.ListProducts()
            $("#hiderProducts").fadeIn("slow");
            $("#addoneCustomerproduct").fadeIn("slow");
            $("#textbox1").val("");
        };
        function setupDevice() {
            $("#Setupscreen").fadeOut("slow");
            $("#TextBoxesGroup").hide();
            $("#locations").hide();
            $("#Usages").show();
            $("#productusername").val(customerNames);
            $("#productuserid").val(customerIds);
            $("#userName").val(Username);
            AdminManagement.ListProducts()
            $("#hiderProducts").fadeIn("slow");
            $("#addoneCustomerproduct").fadeIn("slow");
            getvalue = [];
        };

        function canclesetupscreen() {
            $("#hiderProducts").fadeOut("slow");
            $("#Setupscreen").fadeOut("slow");
        }

        //Single customer product add
        function CustomerProductAdd() {
            $("#hiderProducts").fadeIn("slow");
            $("#Setupscreen").fadeIn("slow");
            getvalue = [];
        }
        function customerproductCancels() {
            $("#loadingmessage").hide();
            $("#hiderProducts").fadeOut("slow");
            $("#addoneCustomerproduct").fadeOut("slow");
            $("#PrimaryuserName").val("");
            $("#AllProductList").val("");
            $("#textbox1").val("");
            $('#location').prop('selectedIndex', 0);
            $('#operatingSystem').prop('selectedIndex', 0);
            $('#settings').prop('selectedIndex', 0);
            getvalue = [];
        }

        $(document).ready(function () {
            var counter = 2;
            $("#addButton").click(function () {
                if (counter > 10) {
                    alert("Only 10 textboxes allow");
                    return false;
                }
                var newTextBoxDiv = $(document.createElement('div')).attr("id", 'TextBoxDiv' + counter);
                newTextBoxDiv.after().html('<input type="text" style="width: 93%; float: left; margin-bottom: 5px;" class="form-control" name="textbox' + counter + '" id="textbox' + counter + '" value="" >');
                newTextBoxDiv.appendTo("#TextBoxesGroup");
                counter++;
            });
            $("#removeButton").click(function () {
                if (counter == 1) {
                    alert("No more textbox to remove");
                    return false;
                }
                counter--;
                $("#TextBoxDiv" + counter).remove();
            });
            $("#customerproductAdd").click(function () {
                var msg = '';
                for (i = 1; i < counter; i++) {
                    getvalue.push({ Url: $('#textbox' + i).val() });
                    msg += "\n Textbox #" + i + " : " + $('#textbox' + i).val();
                }
                AdminManagement.SingleCustomerProductAdd(getvalue);
            });
        });

    </script>
    <div id="loadingmessage" style="display: none;">
        <img src="../img/ajax-loader.gif" class="ajax-loader" />
    </div>

    <div id="hiderProducts" style="display: none;"></div>

    <div class="container margin-top30 formContainer" id="paymentDetails" style="display: none; background-color: #e3e3e3; border-radius: 10px;">
        <h3>
            <label id="lblComputer0">Payment Information</label></h3>
        <input type="hidden" id="hdnUserId" runat="server" />

        <div class="row">
            <div class="col-md-5" style="width: 100%;">
                <div class="field">
                    <label for="txtActualAmount"><span class="Hidden"></span>Actual Monthly Amount</label>
                    <input type="text" runat="server" class="form-control" id="txtActualAmount"
                        name="txtActualAmount" maxlength="16" autocomplete="off" value="" disabled="disabled" aria-required="true" aria-invalid="false" />
                </div>
                <div class="field">
                    <label for="x_card_num"><span class="Hidden"></span>Month</label>
                    <select id="paymentPeriod" runat="server" class="form-control" onchange="changePeriod()">
                        <option value="1">1</option>
                        <option value="2">2</option>
                        <option value="3">3</option>
                        <option value="4">4</option>
                        <option value="5">5</option>
                        <option value="6">6</option>
                        <option value="7">7</option>
                        <option value="8">8</option>
                        <option value="9">9</option>
                        <option value="10">10</option>
                        <option value="11">11</option>
                        <option value="12">12</option>
                    </select>
                </div>
                <div class="field">
                    <label for="txtAmount"><span class="Hidden"></span>Pay Amount</label>
                    <input type="text" runat="server" class="form-control" id="txtAmount"
                        name="txtAmount" maxlength="16" autocomplete="off" value="" aria-required="true" aria-invalid="false" />
                </div>
                <div class="field">
                    <label for="x_card_num"><span class="Hidden"></span>Card Number</label>
                    <input type="text" runat="server" class="form-control" id="x_card_num" name="x_card_num" maxlength="16" autocomplete="off" value="" aria-required="true" aria-invalid="false" />*&nbsp;<span class="Comment">(enter number without spaces or dashes)</span>
                </div>
                <div class="field">
                    <label for="x_exp_date"><span class="Hidden"></span>Expiration Date</label>
                    <input type="text" runat="server" class="form-control" id="x_exp_date" name="x_exp_date" maxlength="20" autocomplete="off" value="" aria-required="true" aria-invalid="false" />*&nbsp;<span class="Comment">(mmyy)</span>
                </div>
                <div class="field">
                    <label for="x_card_cvv"><span class="Hidden"></span>CVV</label>
                    <input type="text" runat="server" class="form-control" id="x_card_cvv" name="x_card_cvv" maxlength="5" autocomplete="off" value="" aria-required="true" aria-invalid="false" />*&nbsp;<span class="Comment">(enter security code)</span>
                </div>
                <div class="field" style="clear: both;">
                    <asp:Button ID="btnProcessCreditCardPayment" runat="server" CssClass="btn btn-orange" OnClick="btnProcessCreditCardPayment_Click" Text="Pay" />
                    <button type="button" id="setupCancel0" class="btn btn-darkBlue"><i class="customIcon icon-yes"></i>Cancel</button>
                </div>
            </div>
        </div>
    </div>





    <asp:Label ID="lblMessage" Style="font-size: 14px; color: red;" runat="server" Text=""></asp:Label>
    <div class="cf separated--bottom" style="margin-top: 15px; margin-left: 3%;">
        <div id="searchCustomer"></div>
        <button type="button" style="margin-right: 60px; float: right; margin-top: 1%;" id="addbutton" onclick="newCustomerAdd()" class="btn btn-darkBlue"><i class="customIcon icon-yes"></i>Add</button>
        <button type="button" style="margin-right: 60px; float: right; margin-top: 1%;" id="addbuttonproduct" onclick="CustomerProductAdd()" class="btn btn-darkBlue"><i class="customIcon icon-yes"></i>Add</button>
        <h1 id="headertext">CUSTOMER INFORMATION</h1>
    </div>

    <%-- <div style="margin-left: 3%; margin-top: 2%;">
        <div style="float: left;">
            <label>Customer </label>
            <select id="selField" onchange="changeFilterType();">
                <option value="CustomerName">Name</option>
                <option value="Email">Email Address</option>
                <option value="UniqueId">Customer Id</option>
            </select>
            <input type="text" id="txtCustomer" />
        </div>
        <div style="float: left;">
            <label>From </label>
            <input type="text" id="txtCustomerFrom" />
        </div>
        <div style="float: left; margin-left: 3%;">
            <label>To </label>
            <input type="text" id="txtCustomerTo" />
        </div>
        <div style="float: left; margin-left: 3%;">
            <div class="grid1">
                <div class="grid__item form-grid__item one-third palm-one-whole center-block">
                    <button id="btnSearch" onclick="javascript:return false;" style="width: 100% !important;" type="submit" data-button-loader="#profile-form-loader" class="btn btn--large btn--full btn--primary">
                        <span class="btn-loader collapse" id="profile-form-loader">
                            <span class="icon icon_spinner alpha"></span>
                        </span>
                        <span>Search</span>
                    </button>
                </div>
            </div>
        </div>
    </div>--%>

    <%--All Customer List--%>
    <div>
        <div class="content-body" id="dvCustomers" style="float: left; width: 100%; color: black;">
        </div>
    </div>

    <%--Single Customer Record--%>
    <hr />
    <div class="content-body" id="OneCustomer" style="float: left; width: 100%; color: black;">
        <table id="tblCustomers" cellspacing="0" cellpadding="0" class="grid">
            <thead class="grid_header">
                <tr>
                    <th class="grid_header_first_column header">Name</th>
                    <th class="grid_header_column header">Email</th>
                    <th class="grid_header_column header">Phone</th>
                    <th class="grid_header_column header">Zip</th>
                    <th class='grid_header_column'>Actions</th>
                </tr>
            </thead>
            <tbody id="report_tbody">
            </tbody>
        </table>
    </div>
    <br />
    <br />
    <br />
    <br />
    <br />
    <%--Tab View--%>
    <div class="content-body" id="tabDetails">
        <div style="float: right;">
            <span onclick="goBack()">
                <img src="https://cdn3.iconfinder.com/data/icons/stroke/53/Button-back-512.png" style="width: 34px; cursor: pointer;" /></span>
        </div>
        <ul class="nav nav-tabs">
            <li class="nav active" onclick="Profile()"><a href="#A" style="color: black" data-toggle="tab"><i class="fa fa-user"></i>&nbsp;Profile</a></li>
            <li class="nav" onclick="Product()"><a href="#Product" style="color: black" data-toggle="tab"><i class="fa fa-product-hunt" aria-hidden="true"></i>&nbsp;Products</a></li>
            <li class="nav" onclick="History()"><a href="#History" style="color: black" data-toggle="tab"><i class="fa fa-history" aria-hidden="true"></i>&nbsp;History</a></li>
        </ul>
        <div class="tab-content">
            <div class="tab-pane fade in active" id="A">
                <br />
                <div class="form-group">
                    <label for="recipient-name" class="form-control-label">First Name:</label>
                    <input type="text" disabled class="form-control" id="firstname" />
                </div>
                <div class="form-group">
                    <label for="recipient-name" class="form-control-label">Last Name:</label>
                    <input type="text" disabled class="form-control" id="lastname" />
                </div>
                <div class="form-group">
                    <label for="recipient-name" class="form-control-label">Address:</label>
                    <input type="text" disabled class="form-control" id="address" />
                </div>
                <div class="form-group">
                    <label for="recipient-name" class="form-control-label">City:</label>
                    <input type="text" disabled class="form-control" id="city" />
                </div>
                <div class="form-group">
                    <label for="recipient-name" class="form-control-label">State:</label>
                    <input type="text" disabled class="form-control" id="state" />
                </div>
                <div class="form-group">
                    <label for="recipient-name" class="form-control-label">Zip:</label>
                    <input type="text" disabled class="form-control" id="zip" />
                </div>
                <div class="form-group">
                    <label for="recipient-name" class="form-control-label">Phone:</label>
                    <input type="text" disabled class="form-control" id="phone" />
                </div>
                <div class="form-group">
                    <label for="recipient-name" class="form-control-label">Email:</label>
                    <input type="text" disabled class="form-control" id="email" />
                </div>
            </div>

            <div class="tab-pane fade" id="Product">
                <br />
                <div class="content-body" id="Customersproduct" style="float: left; width: 100%; color: black;">
                </div>
            </div>

            <div class="tab-pane fade" id="History">
                <br />
                <div class="content-body" id="historyList" style="float: left; width: 100%; color: black;">
                </div>
                <%-- <table class="table table-hover">
                    <thead>
                        <tr>
                            <th>#</th>
                            <th>Date&Time</th>
                            <th>Actual Monthly Amount</th>
                            <th>Pay Amount</th>
                            <th>Period</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>1</td>
                            <td>09/27/2016 5:16 PM</td>
                            <td>$500</td>
                            <td>$50</td>
                            <td>One Month</td>
                        </tr>
                        <tr>
                            <td>2</td>
                            <td>09/26/2016 5:16 PM</td>
                            <td>$600</td>
                            <td>$60</td>
                            <td>Two Month</td>
                        </tr>
                        <tr>
                            <td>3</td>
                            <td>09/25/2016 5:16 PM</td>
                            <td>$700</td>
                            <td>$70</td>
                            <td>Four Month</td>
                        </tr>
                    </tbody>
                </table>--%>
            </div>
        </div>
    </div>

    <div id="hiderLicense" style="display: none;"></div>
    <div class="container margin-top30 formContainer" id="licenseDate" style="display: none; background-color: #e3e3e3; border-radius: 10px; position: absolute; left: 40%; width: 38%; margin-top: -5em; margin-left: -5em; border: 1px solid #ccc; z-index: 100;">
         <h3>
            <label id="lblComputer1">Product License</label></h3>
        <input name="hdnProductToUserId" type="hidden" id="hdnProductToUserId" runat="server" />

        <div class="row">
            <div class="col-md-5" style="width: 100%;">
                <div class="field">
                    <label for="txtStartDate"><span class="Hidden"></span>Start Date</label>
                    <input name="txtStartDate" type="text" id="txtStartDate" runat="server" class="form-control" maxlength="16" autocomplete="off" aria-required="true" aria-invalid="false" />
                </div>
                <div class="field" style="clear: both;">
                    <button onserverclick="btnUpdateLicenseDate_Click" runat="server" id="ContentPlaceHolder1_btnUpdateLicenseDate1" type="submit" class="btn btn-orange"><i class="customIcon icon-yes"></i>Update</button>

                    <button type="button" id="setupCancelLicenseUpdate" class="btn btn-darkBlue"><i class="customIcon icon-yes"></i>Cancel</button>
                </div>
            </div>
        </div>
    </div>

    <div class="container margin-top30 formContainer" id="addCustomer" style="display: none; background-color: #e3e3e3; border-radius: 10px;">
        <h3>
            <label id="lblCustomer">Add Customer</label></h3>
        <input type="hidden" id="hdnProductId" />
        <div class="row">
            <div class="col-md-5" style="width: 100%;">
                <div class="field">
                    <label>First Name</label>
                    <input type="text" id="CustomerFirstName" class="form-control" />
                </div>
                <div class="field">
                    <label>First Name</label>
                    <input type="text" id="CustomerLastName" class="form-control" />
                </div>
                <div class="field">
                    <label>Email</label>
                    <input type="text" id="CustomerEmail" class="form-control" />
                </div>
                <div class="field">
                    <label>Password</label>
                    <input type="password" id="CustomerPassword" class="form-control" />
                </div>
                <div class="field">
                    <label>Phone</label>
                    <input type="text" maxlength="10" id="CustomerPhone" class="form-control" />
                </div>
                <div class="field">
                    <label>Zip</label>
                    <input type="text" id="CustomerZip" class="form-control" />
                </div>
                <div class="field" style="clear: both;">
                    <button type="button" id="customerAdd" onclick="addnewcustomer()" class="btn btn-darkBlue"><i class="customIcon icon-yes"></i>Add</button>
                    <button type="button" id="customerCancel" onclick="customerCancels()" class="btn btn-primary"><i class="customIcon icon-yes"></i>Cancel</button>
                </div>
            </div>
        </div>
    </div>

    <div class="container margin-top30 formContainer" id="updateCustomer" style="display: none; background-color: #e3e3e3; border-radius: 10px;">
        <h3>
            <label id="lblCustomerUpdate">Update Customer</label></h3>
        <input type="hidden" id="hdnCustomerId" />
        <div class="row">
            <div class="col-md-5" style="width: 100%;">
                <div class="field">
                    <label>First Name</label>
                    <input type="text" id="txtUpdateFirstName" class="form-control" />
                </div>
                <div class="field">
                    <label>First Name</label>
                    <input type="text" id="txtUpdateLastName" class="form-control" />
                </div>
                <div class="field">
                    <label>Email</label>
                    <input type="text" id="txtUpdateEmail" class="form-control" />
                </div>
                <div class="field">
                    <label>Password <span>(optional)</span></label>
                    <input type="password" id="txtUpdatePassword" class="form-control" />
                </div>

                <div class="field">
                    <label>Phone</label>
                    <input type="text" id="txtUpdatePhone" maxlength="10" class="form-control" />
                </div>
                <div class="field">
                    <label>Zip</label>
                    <input type="text" id="txtUpdateZip" class="form-control" />
                </div>
                <div class="field" style="clear: both;">
                    <button type="button" id="customerUpdates" onclick="customerUpdate()" class="btn btn-darkBlue"><i class="customIcon icon-yes"></i>Update</button>
                    <button type="button" id="customerUpdateCancel" class="btn btn-primary"><i class="customIcon icon-yes"></i>Cancel</button>
                </div>
            </div>
        </div>
    </div>

    <div class="container margin-top30 formContainer" id="historypopup" style="display: none; background-color: #e3e3e3; border-radius: 10px;">
        <h3>
            <label id="Label1">Edit History</label></h3>
        <input type="hidden" id="id" />
        <div class="row">
            <div class="col-md-5" style="width: 100%;">
                <div class="field">
                    <label>User Name</label>
                    <input type="text" id="Username" class="form-control" />
                </div>
                <div class="field">
                    <label>YearlyPrice</label>
                    <input type="text" id="YearlyPrice" class="form-control" />
                </div>
                <div class="field">
                    <label>MonthlyPrice</label>
                    <input type="text" id="MonthlyPrice" class="form-control" />
                </div>
                <div class="field">
                    <label>PaymentPeriod</label>
                    <input type="text" id="PaymentPeriod" class="form-control" />
                </div>
                <div class="field">
                    <label>Last Payment Date</label>
                    <input type="text" id="LastPaymentDate" class="form-control" />
                </div>
                <div class="field" style="clear: both;">
                    <button type="button" id="Button2" onclick="UpdateHistory()" class="btn btn-darkBlue"><i class="customIcon icon-yes"></i>Update</button>
                    <button type="button" id="Button3" onclick="historyCancels()" class="btn btn-primary"><i class="customIcon icon-yes"></i>Cancel</button>
                </div>
            </div>
        </div>
    </div>

    <div class="container margin-top30 formContainer" id="addoneCustomerproduct" style="display: none; background-color: #e3e3e3; border-radius: 10px;">
        <h3>
            <label id="lblsCustomer">Add Customer product</label></h3>
        <input type="hidden" id="productuserid" />
        <input type="hidden" id="productusername" />


        <div class="row">
            <div class="col-md-5" style="width: 100%;">

                <div class="field">
                    <label>Operating System</label>
                    <select class="form-control" id="operatingSystem">
                        <option value='0'>Select</option>
                        <option value='Windows 7 32'>Windows 7 32</option>
                        <option value='Windows 7 64'>Windows 7 64</option>
                        <option value='Windows 8/10'>Windows 8/10</option>
                    </select>
                </div>

                <div class="field">
                    <label>Primary UserName</label>
                    <input type="text" id="PrimaryuserName" class="form-control" />
                </div>


                <div class="field" id="Usages">
                    <label>Usage</label>
                    <select class="form-control" id="Usage">
                        <option value="0">Select</option>
                        <option value="Personal">Personal</option>
                        <option value="Business">Business</option>
                    </select>
                </div>

                <div class="field" id="locations">
                    <label>Location</label>
                    <select class="form-control" id="location">
                        <option value='0'>Select</option>
                        <option value='Home'>Home</option>
                        <option value='Office'>Office</option>
                    </select>
                </div>


                <div class="field">
                    <label>Filter Settings</label>
                    <select class="form-control" id="settings">
                        <option value='0'>Select</option>
                        <option value='1'>Moderate</option>
                        <option value='2'>Regular (Business)</option>
                        <option value='3'>Restricted (Home User)</option>
                    </select>
                </div>

                <div class="field" id='TextBoxesGroup'>
                    <label>URL</label>
                    <div class="field">
                        <input type='text' id='textbox1' style="width: 93%; float: left; margin-bottom: 5px;" class="form-control" />
                        <button style="padding: 4px; float: left; margin-left: 5px; display: block;" type="button" id="addButton" class="btn btn-primary">+</button>
                        <button style="padding: 4px; float: left; margin-left: 5px; display: block;" type="button" id="removeButton" class="btn btn-primary">-</button>
                    </div>
                </div>


                <div class="field" style="clear: both;">
                    <button type="button" id="customerproductAdd" class="btn btn-darkBlue"><i class="customIcon icon-yes"></i>Add</button>
                    <button type="button" id="customerproductCancel" onclick="customerproductCancels()" class="btn btn-primary"><i class="customIcon icon-yes"></i>Cancel</button>
                </div>
            </div>
        </div>
    </div>


    <div class="container margin-top30 formContainer" id="Setupscreen" style="display: none; background-color: #e3e3e3; border-radius: 10px;">
        <h3>
            <label id="Label2">SET UP A COMPUTER/DEVICE</label><span style="float: right; cursor: pointer;" onclick="canclesetupscreen()">X</span></h3>
        <p>Please select if you would like to set up a computer/laptop or another type of device</p>
        <div class="row">
            <div class="col-md-12">
                <div class="col-md-6">
                    <div class="prodBox">
                        <img src="https://www.netsafepro.com/img/desktop.png" alt="" onclick="setupComputer()" style="cursor: pointer" />
                        <p>Set up Computer</p>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="prodBox">
                        <img src="https://www.netsafepro.com/img/iPhone.png" alt="" onclick="setupDevice()" style="cursor: pointer" />
                        <p>Set up Device</p>
                    </div>
                </div>
            </div>
        </div>
        <br />
    </div>

</asp:Content>
