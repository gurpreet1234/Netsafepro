<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="BillingPayment.aspx.cs" Inherits="Safenetpro.BillingPayment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="js/vendor/jquery-1.11.2.min.js"></script>
    <script src="js/setup.js"></script>
    <script src="Typescript/CombineTypescript.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="upPanelMain" runat="server">
        <ContentTemplate>
            <div id="processingDiv" style="display: none;">
                <img src="img/ajax-loader.gif" class="ajax-loader">
            </div>

            <div class="container margin-top30 formContainer">
                <h1>Billing & Payment</h1>
                <div class="row">
                    <div class="col-md-5">
                        <h2>Billing Information</h2>

                        <div style="margin-left: 4%;">
                            <h5>
                                <label>Name</label>
                                <input type="text" autocomplete="off" id="txtlName" runat="server" class="form-control" />
                                <%--<label class="labelClass" id="lblBName" runat="server"></label>--%>
                            </h5>
                        </div>
                        <div style="margin-left: 4%; margin-top:4%;">
                            <h5>
                                <label>Address</label>
                                <input type="text" autocomplete="off" id="txtaddress" runat="server" class="form-control" />
                                <%--<label class="labelClass" id="lblAddress" runat="server"></label>--%>
                            </h5>
                        </div>
                        <div style="margin-left: 4%; margin-top:4%;">
                            <h5>
                                <label>City</label>
                                <input id="txtcity" autocomplete="off" runat="server" type="text" class="form-control" />
                                <%--<label class="labelClass" id="lblCityStateZip" runat="server"></label>--%>
                            </h5>
                        </div>
                        <div style="margin-top:4%;">
                            <div>
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
                        <div style="margin-left: 4%; margin-top:21%;">
                            <h5>
                                <label>Phone Number</label>
                                <input id="txtPhoneNumber" autocomplete="off" runat="server" type="text" class="form-control" />
                                <%--<label class="labelClass" id="lblPhoneNumber" runat="server"></label>--%>
                            </h5>
                        </div>
                        <div style="margin-left: 4%; margin-top:4%;">
                            <h5>
                                <label>Email Address</label>
                                <input id="txtEmailAddress" autocomplete="off" runat="server" type="email" class="form-control" />
                                <%--<label class="labelClass" id="lblEmail" runat="server"></label>--%>
                            </h5>
                        </div>
                         <div style="clear: both;margin-left: 4%; margin-top: 20px;">
                             <button onclick="location.href = 'SignUp.aspx';" style="display:none;" type="button" class="btn btn-orange">Edit Profile</button>
                            <button onclick="location.href = 'ProductLicenses.aspx';" type="button" class="btn btn-darkBlue">Back</button>
                        </div>
                    </div>

                    <div class="col-md-5" style="margin-bottom: 30px;">
                        <h7>Your Order</h7>
                        <div id="dvProducts" runat="server">
                        </div>

                        <div style="clear: both; border: 1px solid #d7d7d7; margin-top: 14px;"></div>

                        <div style="float: right;">
                            <h5>TOTALS</h5>
                        </div>
                        <div style="clear: both; float: right; margin-top: 10px;">
                            <span class="yearly">Yearly Subtotal:<label style="margin-left: 35px;" id="lblYearlyPayment" runat="server">$0</label></span>
                        </div>
                        <div style="clear: both; float: right;">
                            <span class="monthly">Monthly Total:<label style="margin-right: 11px; margin-left: 25px;" id="lblMonthlyPayment" runat="server">$0</label></span>
                        </div>

                        <div style="clear: both; border: 1px solid #d7d7d7; margin-top: 14px;"></div>

                        <div class="pull-right" style="clear: both; margin-top: 20px;">
                            <button runat="server" onserverclick="Unnamed_ServerClick" type="button" class="btn btn-orange">Place Order</button>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
