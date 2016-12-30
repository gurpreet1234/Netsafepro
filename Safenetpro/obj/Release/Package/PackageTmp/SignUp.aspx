<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="SignUp.aspx.cs" Inherits="Safenetpro.SignUp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="Typescript/CombineTypescript.js"></script>
    <script src="js/togglePassword.js"></script>
    <script type="text/javascript">
        function showhideDiv(val) {
            if (val.checked)
                $("#ContentPlaceHolder1_dvBusinessInfo").show();
            else
                $("#ContentPlaceHolder1_dvBusinessInfo").hide();
        }
        function sameaspersonal(val) {
            if (val.checked) {
                $("#ContentPlaceHolder1_bAddress").val($("#ContentPlaceHolder1_txtaddress").val());
                $("#ContentPlaceHolder1_bCity").val($("#ContentPlaceHolder1_txtcity").val());
                $("#ContentPlaceHolder1_bState").val($("#ContentPlaceHolder1_selectState").val());
                $("#ContentPlaceHolder1_bZipCode").val($("#ContentPlaceHolder1_txtZipCode").val());
                $("#ContentPlaceHolder1_bPhoneNumber").val($("#ContentPlaceHolder1_txtPhoneNumber").val());
                $("#ContentPlaceHolder1_bCellPhone").val($("#ContentPlaceHolder1_txtCellPhone").val());
                $("#ContentPlaceHolder1_bFax").val($("#ContentPlaceHolder1_txtFax").val());
                $("#ContentPlaceHolder1_bEmailAddress").val($("#ContentPlaceHolder1_txtEmailAddress").val());
            }
            else {
                $("#ContentPlaceHolder1_bAddress").val("");
                $("#ContentPlaceHolder1_bCity").val("");
                $("#ContentPlaceHolder1_bState").val("");
                $("#ContentPlaceHolder1_bZipCode").val("");
                $("#ContentPlaceHolder1_bPhoneNumber").val("");
                $("#ContentPlaceHolder1_bCellPhone").val("");
                $("#ContentPlaceHolder1_bFax").val("");
                $("#ContentPlaceHolder1_bEmailAddress").val("");
            }
        }
        function isValidPhone(email) {
            var regex = /^(\+?1-?)?(\([2-9]\d{2}\)|[2-9]\d{2})-?[2-9]\d{2}-?\d{4}$/;
            return regex.test(email);
        }
        function isValidEmailAddress(emailAddress) {
            var pattern = new RegExp(/^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i);
            return pattern.test(emailAddress);
        };
        function checkValidators() {
            $("#continueRegistration").attr("disabled", "disabled");
            var validForm = true;
            var firstField = "";
            var validationFields = [];
            validationFields.push("ContentPlaceHolder1_txtlName");
            validationFields.push("ContentPlaceHolder1_txtfname");
            validationFields.push("ContentPlaceHolder1_txtaddress");
            validationFields.push("ContentPlaceHolder1_txtcity");
            validationFields.push("ContentPlaceHolder1_txtZipCode");
            validationFields.push("ContentPlaceHolder1_txtPhoneNumber");
            validationFields.push("ContentPlaceHolder1_txtEmailAddress");
            validationFields.push("ContentPlaceHolder1_txtUserName");
            validationFields.push("ContentPlaceHolder1_txtUserPassword");

            var businessvalidationFields = [];
            businessvalidationFields.push("ContentPlaceHolder1_bname");
            businessvalidationFields.push("ContentPlaceHolder1_bAddress");
            businessvalidationFields.push("ContentPlaceHolder1_bCity");
            businessvalidationFields.push("ContentPlaceHolder1_bZipCode");
            businessvalidationFields.push("ContentPlaceHolder1_bPhoneNumber");
            businessvalidationFields.push("ContentPlaceHolder1_bEmailAddress");

            $(".red").hide();
            for (var v = 0; v < validationFields.length; v++) {
                $("#" + validationFields[v]).css('border-color', '');
                $("#" + validationFields[v]).css('background', '');
                if ($("#" + validationFields[v]).val() == "") {
                    $("#" + validationFields[v]).css('border-color', '#c0392b');
                    $("#" + validationFields[v]).css('background', '#FCDACD');
                    if (firstField == "")
                        firstField = validationFields[v];
                    validForm = false;
                }
            }

            if ($("#ContentPlaceHolder1_chkBusiness").is(':checked')) {
                for (var v = 0; v < businessvalidationFields.length; v++) {
                    $("#" + businessvalidationFields[v]).css('border-color', '');
                    $("#" + businessvalidationFields[v]).css('background', '');
                    if ($("#" + businessvalidationFields[v]).val() == "") {
                        $("#" + businessvalidationFields[v]).css('border-color', '#c0392b');
                        $("#" + businessvalidationFields[v]).css('background', '#FCDACD');
                        if (firstField == "")
                            firstField = businessvalidationFields[v];
                        validForm = false;
                    }
                }
            }
            if ($("#ContentPlaceHolder1_txtEmailAddress").val() != "" && !isValidEmailAddress($("#ContentPlaceHolder1_txtEmailAddress").val())) {
                $('#ContentPlaceHolder1_txtEmailAddress').css('border-color', '#c0392b');
                $('#ContentPlaceHolder1_txtEmailAddress').css('background', '#FCDACD');
                $('#ContentPlaceHolder1_txtEmailAddress').after('<div class="red">Please enter valid email address</div>');
                firstField = "ContentPlaceHolder1_txtEmailAddress";
                validForm = false;
            }
            if ($("#ContentPlaceHolder1_chkBusiness").is(':checked')) {
                if ($("#ContentPlaceHolder1_bEmailAddress").val() != "" && !isValidEmailAddress($("#ContentPlaceHolder1_bEmailAddress").val())) {
                    $('#ContentPlaceHolder1_bEmailAddress').css('border-color', '#c0392b');
                    $('#ContentPlaceHolder1_bEmailAddress').css('background', '#FCDACD');
                    $('#ContentPlaceHolder1_bEmailAddress').after('<div class="red">Please enter valid email address</div>');
                    firstField = "ContentPlaceHolder1_bEmailAddress";
                    validForm = false;
                }
            }

            if ($("#ContentPlaceHolder1_txtPhoneNumber").val() != "" && !isValidPhone($("#ContentPlaceHolder1_txtPhoneNumber").val())) {
                $('#ContentPlaceHolder1_txtPhoneNumber').css('border-color', '#c0392b');
                $('#ContentPlaceHolder1_txtPhoneNumber').css('background', '#FCDACD');
                $('#ContentPlaceHolder1_txtPhoneNumber').after('<div class="red">Please enter valid phone number</div>');
                firstField = "ContentPlaceHolder1_txtPhoneNumber";
                validForm = false;
            }
            if ($("#ContentPlaceHolder1_chkBusiness").is(':checked')) {
                if ($("#ContentPlaceHolder1_bPhoneNumber").val() != "" && !isValidPhone($("#ContentPlaceHolder1_bPhoneNumber").val())) {
                    $('#ContentPlaceHolder1_bPhoneNumber').css('border-color', '#c0392b');
                    $('#ContentPlaceHolder1_bPhoneNumber').css('background', '#FCDACD');
                    $('#ContentPlaceHolder1_bPhoneNumber').after('<div class="red">Please enter valid phone number</div>');
                    firstField = "ContentPlaceHolder1_bPhoneNumber";
                    validForm = false;
                }
            }
            if ($("#ContentPlaceHolder1_txtCellPhone").val() != "" && !isValidPhone($("#ContentPlaceHolder1_txtCellPhone").val())) {
                $('#ContentPlaceHolder1_txtCellPhone').css('border-color', '#c0392b');
                $('#ContentPlaceHolder1_txtCellPhone').css('background', '#FCDACD');
                $('#ContentPlaceHolder1_txtCellPhone').after('<div class="red">Please enter valid phone number</div>');
                firstField = "ContentPlaceHolder1_txtCellPhone";
                validForm = false;
            }
            if ($("#ContentPlaceHolder1_chkBusiness").is(':checked')) {
                if ($("#ContentPlaceHolder1_bCellPhone").val() != "" && !isValidPhone($("#ContentPlaceHolder1_bCellPhone").val())) {
                    $('#ContentPlaceHolder1_bCellPhone').css('border-color', '#c0392b');
                    $('#ContentPlaceHolder1_bCellPhone').css('background', '#FCDACD');
                    $('#ContentPlaceHolder1_bCellPhone').after('<div class="red">Please enter valid phone number</div>');
                    firstField = "ContentPlaceHolder1_bCellPhone";
                    validForm = false;
                }
            }

            if (validForm == true) {
                var userName = $("#ContentPlaceHolder1_txtUserName").val();
                Users.checkUserNameAvailable(userName, sessionStorage.getItem("userId"));
            }
            else {
                $('html,body').animate({
                    scrollTop: $("#" + firstField).offset().top - 180
                }, 'slow');
                $("#" + firstField).focus();
                $("#continueRegistration").removeAttr("disabled");
            }
        }

        function signupsuccess(userID) {
            sessionStorage.setItem("userId", userID);
            window.location.href = "Setup.aspx";
        }

        function checkUserAvailability() {
            var userName = $("#ContentPlaceHolder1_txtUserName").val();
            Users.checkUserNameAvailablity(userName, sessionStorage.getItem("userId"));
        }
        //$.toggleShowPassword({
        //    field: '#ContentPlaceHolder1_txtUserPassword',
        //    control: '#test2',
        //});
        $(document).ready(function () {
            $("#ContentPlaceHolder1_txtUserPassword").attr('type', 'password');
            $('#showPassword').change(function () {
                if (this.checked)
                    $("#ContentPlaceHolder1_txtUserPassword").attr('type', 'text');
                else
                    $("#ContentPlaceHolder1_txtUserPassword").attr('type', 'password');
            });

            $("#ContentPlaceHolder1_mOrganization").change(function () {
                if ($(this).val() == "0") {
                    $("#ContentPlaceHolder1_txtOtherOrganization").show();
                }
                else {
                    $("#ContentPlaceHolder1_txtOtherOrganization").hide();
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="upPanelMain" runat="server">
        <ContentTemplate>
            <div id="processingDiv" style="display: none;">
                <img src="../img/ajax-loader.gif" class="ajax-loader">
            </div>
            <div class="container margin-top30 formContainer">
                <h1>New User Information</h1>

                <!-- Example row of columns -->
                <div class="row">
                    <div class="col-md-5">
                        <h2>Personal Information</h2>
                         <div class="field">
                            <label>First Name</label>
                            <input type="text" autocomplete="off" id="txtfname" runat="server" class="form-control" />
                        </div>
                        <div class="field">
                            <label>Last Name</label>
                            <input type="text" autocomplete="off" id="txtlName" runat="server" class="form-control" />
                        </div>
                       
                        <div class="field">
                            <label>Address</label>
                            <input type="text" autocomplete="off" id="txtaddress" runat="server" class="form-control" />
                            <%--<textarea id="txtaddress" autocomplete="off" runat="server" class="form-control"></textarea>--%>
                        </div>
                        <div class="field">
                            <label>Address Line 2</label>
                            <input type="text" autocomplete="off" id="txtaddress2" runat="server" class="form-control" />
                            <%--<textarea id="txtaddress2" autocomplete="off" runat="server" class="form-control"></textarea>--%>
                        </div>
                        <div class="field">
                            <label>City</label>
                            <input id="txtcity" autocomplete="off" runat="server" type="text" class="form-control" />
                        </div>
                        <div class="field">
                            <div class="row">
                                <div class="col-sm-8">
                                    <label>State</label>
                                    <select class="form-control" id="selectState" runat="server">
                                        <option value="0">Select</option>
                                        <option value="2">Alabamav</option>
                                        <option value="3">Alaska</option>
                                        <option value="4">Arizona</option>
                                        <option value="5">Arkansas</option>
                                        <option value="6">California</option>
                                        <option value="7">Colorado</option>
                                        <option value="8">Connecticut</option>
                                        <option value="9">Delaware</option>
                                        <option value="10">Florida</option>
                                        <option value="11">Georgia</option>
                                        <option value="12">Hawaii</option>
                                        <option value="13">Idaho</option>
                                        <option value="14">Illinois</option>
                                        <option value="15">Indiana</option>
                                        <option value="16">Iowa</option>
                                        <option value="17">Kansas</option>
                                        <option value="18">Kentucky[D]</option>
                                        <option value="19">Louisiana</option>
                                        <option value="20">Maine</option>
                                        <option value="21">Maryland</option>
                                        <option value="22">Massachusetts[E]</option>
                                        <option value="23">Michigan</option>
                                        <option value="24">Minnesota</option>
                                        <option value="25">Mississippi</option>
                                        <option value="26">Missouri</option>
                                        <option value="27">Montana</option>
                                        <option value="28">Nebraska</option>
                                        <option value="29">Nevada</option>
                                        <option value="30">New Hampshire</option>
                                        <option value="31">New Jersey</option>
                                        <option value="32">New Mexico</option>
                                        <option value="33">New York</option>
                                        <option value="34">North Carolina</option>
                                        <option value="35">North Dakota</option>
                                        <option value="36">Ohio</option>
                                        <option value="37">Oklahoma</option>
                                        <option value="38">Oregon</option>
                                        <option value="39">Pennsylvania[F]</option>
                                        <option value="40">Rhode Island[G]</option>
                                        <option value="41">South Carolina</option>
                                        <option value="42">South Dakota</option>
                                        <option value="43">Tennessee</option>
                                        <option value="44">Texas</option>
                                        <option value="45">Utah</option>
                                        <option value="46">Vermont</option>
                                        <option value="47">Virginia[H]</option>
                                        <option value="48">Washington</option>
                                        <option value="49">West Virginia</option>
                                        <option value="50">Wisconsin</option>
                                        <option value="51">Wyoming</option>
                                    </select>
                                </div>
                                <div class="col-sm-4">
                                    <label>Zip Code</label>
                                    <input id="txtZipCode" autocomplete="off" runat="server" type="text" class="form-control" />
                                </div>
                            </div>
                        </div>
                        <div class="field">
                            <label>Phone Number</label>
                            <input id="txtPhoneNumber" autocomplete="off" runat="server" type="text" class="form-control" />
                        </div>
                        <div class="field">
                            <label>Cell Phone Number</label>
                            <input id="txtCellPhone" autocomplete="off" runat="server" type="text" class="form-control" />
                        </div>
                        <div class="field">
                            <label>Fax</label>
                            <input id="txtFax" autocomplete="off" runat="server" type="text" class="form-control" />
                        </div>
                        <div class="field">
                            <label>Email Address</label>
                            <input id="txtEmailAddress" autocomplete="off" runat="server" type="email" class="form-control" />
                        </div>


                        <div class="checkbox" style="font-size: 18px;">
                            <label>
                                <input type="checkbox" id="chkBusiness" runat="server" onchange="showhideDiv(this)" style="height: 18px;">
                                Business Information
                            </label>
                        </div>

                        <div style="display: none;" id="dvBusinessInfo" runat="server">
                            <h2>Business Information
                            
                            </h2>
                            <div class="field" style="width: 42%;">
                                <input class="chkboxsignup" onchange="sameaspersonal(this)" type="checkbox" id="chkCopy" />
                                <label style="margin-top: 4px;" class="lblSignup" for="chkCopy">Same as Personal Information</label>
                            </div>
                            <div class="field">
                                <label>Business Name</label>
                                <input type="text" id="bname" autocomplete="off" runat="server" class="form-control" />
                            </div>
                            <div class="field">
                                <label>Business Address</label>
                                <input id="bAddress" autocomplete="off" runat="server" type="text" class="form-control" />
                            </div>
                            <div class="field">
                                <label>City</label>
                                <input id="bCity" autocomplete="off" runat="server" type="text" class="form-control" />
                                <%--<textarea id="bCity" autocomplete="off" runat="server" class="form-control"></textarea>--%>
                            </div>
                            <div class="field">
                                <div class="row">
                                    <div class="col-sm-8">
                                        <label>State</label>
                                        <select id="bState" runat="server" class="form-control">
                                            <option value="0">Select</option>
                                            <option value="2">Alabamav</option>
                                            <option value="3">Alaska</option>
                                            <option value="4">Arizona</option>
                                            <option value="5">Arkansas</option>
                                            <option value="6">California</option>
                                            <option value="7">Colorado</option>
                                            <option value="8">Connecticut</option>
                                            <option value="9">Delaware</option>
                                            <option value="10">Florida</option>
                                            <option value="11">Georgia</option>
                                            <option value="12">Hawaii</option>
                                            <option value="13">Idaho</option>
                                            <option value="14">Illinois</option>
                                            <option value="15">Indiana</option>
                                            <option value="16">Iowa</option>
                                            <option value="17">Kansas</option>
                                            <option value="18">Kentucky[D]</option>
                                            <option value="19">Louisiana</option>
                                            <option value="20">Maine</option>
                                            <option value="21">Maryland</option>
                                            <option value="22">Massachusetts[E]</option>
                                            <option value="23">Michigan</option>
                                            <option value="24">Minnesota</option>
                                            <option value="25">Mississippi</option>
                                            <option value="26">Missouri</option>
                                            <option value="27">Montana</option>
                                            <option value="28">Nebraska</option>
                                            <option value="29">Nevada</option>
                                            <option value="30">New Hampshire</option>
                                            <option value="31">New Jersey</option>
                                            <option value="32">New Mexico</option>
                                            <option value="33">New York</option>
                                            <option value="34">North Carolina</option>
                                            <option value="35">North Dakota</option>
                                            <option value="36">Ohio</option>
                                            <option value="37">Oklahoma</option>
                                            <option value="38">Oregon</option>
                                            <option value="39">Pennsylvania[F]</option>
                                            <option value="40">Rhode Island[G]</option>
                                            <option value="41">South Carolina</option>
                                            <option value="42">South Dakota</option>
                                            <option value="43">Tennessee</option>
                                            <option value="44">Texas</option>
                                            <option value="45">Utah</option>
                                            <option value="46">Vermont</option>
                                            <option value="47">Virginia[H]</option>
                                            <option value="48">Washington</option>
                                            <option value="49">West Virginia</option>
                                            <option value="50">Wisconsin</option>
                                            <option value="51">Wyoming</option>
                                        </select>
                                    </div>
                                    <div class="col-sm-4">
                                        <label>Zip Code</label>
                                        <input id="bZipCode" autocomplete="off" runat="server" type="text" class="form-control" />
                                    </div>
                                </div>
                            </div>

                            <div class="field">
                                <label>Phone Number</label>
                                <input id="bPhoneNumber" autocomplete="off" runat="server" type="text" class="form-control" />
                            </div>
                            <div class="field">
                                <label>Cell Phone Number</label>
                                <input id="bCellPhone" autocomplete="off" runat="server" type="text" class="form-control" />
                            </div>
                            <div class="field">
                                <label>Fax</label>
                                <input id="bFax" autocomplete="off" runat="server" type="text" class="form-control" />
                            </div>
                            <div class="field">
                                <label>Email Address</label>
                                <input id="bEmailAddress" autocomplete="off" runat="server" type="text" class="form-control" />
                            </div>
                        </div>
                    </div>

                    <div class="col-md-5">
                        <h2>School/Kehilla</h2>
                        <div class="field">
                            <label>Organization</label>
                            <select class="form-control" id="mOrganization" runat="server">
                                <option>select</option>
                            </select>
                            <input id="txtOtherOrganization" style="display: none; margin-top: 10px;" autocomplete="off" runat="server" type="text" class="form-control" />
                        </div>

                        <h2>Login Information</h2>
                        <div class="field">
                            <label>User Name: </label>
                            <input type="text" autocomplete="off" onblur="checkUserAvailability()" id="txtUserName" runat="server" class="form-control" />
                            <label id="lblAlreadyExists" style="display: none; color: #c0392b;">user name already register try a other user name.</label>
                        </div>
                        <div class="field">
                            <label>Password</label>
                            <input type="text" autocomplete="off" id="txtUserPassword" runat="server" class="form-control" />
                            <input id="showPassword" type="checkbox" />Show password
                        </div>
                    </div>
                </div>

                <div class="action text-right">
                    <button type="submit" style="display: none;" onserverclick="continueRegistration_click" id="btnhiddenServerClick" runat="server"></button>
                    <button type="button" onclick="checkValidators()" runat="server" id="continueRegistration" class="btn btn-primary">Continue Registration</button>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
