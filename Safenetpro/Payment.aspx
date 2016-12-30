<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Payment.aspx.cs" Inherits="Safenetpro.Payment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="js/vendor/jquery-1.11.2.min.js"></script>
    <script src="Typescript/CombineTypescript.js"></script>
    <script>
        function changeSubscription(val) {
            if (val == 1) {
                $("#ContentPlaceHolder1_hdnMilestoneAmount").val($("#ContentPlaceHolder1_hdnMonthlyAmount").val());
                $("#ContentPlaceHolder1_txtMilestoneAmount").val($("#ContentPlaceHolder1_hdnMonthlyAmount").val());
            }
            else {
                $("#ContentPlaceHolder1_hdnMilestoneAmount").val($("#ContentPlaceHolder1_hdnYearlyAmount").val());
                $("#ContentPlaceHolder1_txtMilestoneAmount").val($("#ContentPlaceHolder1_hdnYearlyAmount").val());
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="processingDiv" style="display: none;">
        <img src="img/ajax-loader.gif" class="ajax-loader">
    </div>
    <div class="thumb_terms" style="height: 580px !important; overflow-y: hidden;">
        <div class="content-body" style="float: left; width: 66%;">
            <div class="divSmall" style="margin-left: 28%; width: 75%;">
                <table style="width: 100%; font-weight: bold; margin: 10px;">
                    <tr>
                        <td>
                            <asp:Label ID="lblTransType" Text="" runat="server"></asp:Label>
                            <asp:Label ID="lblMessage" Style="font-size: 14px; color:red;" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="form-fields form-fields-address column left" style="width: 90%; margin-left: 25%; margin-top: 2%;">
                <div id="divCreditCardInformation">
                    <table id="tableCreditCardInformation" role="presentation">
                        <tr>
                            <td>
                                <table>
                                    <tbody>
                                        <tr>
                                            <td class="SpacerRow2" colspan="2">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="LabelColCC">&nbsp;</td>
                                            <td class="DataColCC">
                                                <input type="radio" id="rdbMonthly" onclick="changeSubscription(1)" runat="server" value="1" /><label style="margin-left: 5px;"> Monthly</label>
                                                <input checked style="margin-left: 20px;" type="radio" id="rdbYearly" onclick="changeSubscription(2)" runat="server" value="2" /><label style="margin-left: 5px;">Yearly</label>
                                            </td>
                                            <%--<td class="LabelColCC">&nbsp;</td>
                                <td class="DataColCC" style="font-weight: bold;">Credit Card Information</td>--%>
                                        </tr>
                                        <tr id="trCCInfoBold" style="display: none;">
                                            <td class="LabelColCC">&nbsp;</td>
                                            <td class="DataColCC" style="font-weight: bold;">Credit Card Information</td>
                                        </tr>
                                        <tr id="trAcceptedCardImgs">
                                            <td class="LabelColCC">&nbsp;</td>
                                            <td class="DataColCC" style="padding-top: 7px; padding-bottom: 7px;">
                                                <img src="img/V.gif" width="43" height="26" title="Visa" alt="Visa"><img src="img/MC.gif" width="41" height="26" title="MasterCard" alt="MasterCard"><img src="img/Amex.gif" width="40" height="26" title="American Express" alt="American Express"><img src="img/Disc.gif" width="40" height="26" title="Discover" alt="Discover"></td>
                                        </tr>

                                        <tr>
                                            <td class="LabelColCC">
                                                <label for="x_card_num"><span class="Hidden"></span>Amount($)</label>:&nbsp;</td>
                                            <td class="DataColCC" style="padding-top: 7px; padding-bottom: 7px;">
                                                <input type="text" required runat="server" readonly class="input_text" id="txtMilestoneAmount" name="txtMilestoneAmount"
                                                    maxlength="16" autocomplete="off" value="" aria-required="true" aria-invalid="false" />*&nbsp;<span class="Comment"></span>
                                                <input type="hidden" id="hdnMilestoneAmount" value="99" runat="server" />
                                                <input type="hidden" id="hdnMonthlyAmount" value="99" runat="server" />
                                                <input type="hidden" id="hdnYearlyAmount" value="99" runat="server" />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td class="LabelColCC">
                                                <label for="x_card_num"><span class="Hidden"></span>Card Number</label>:&nbsp;</td>
                                            <td class="DataColCC" style="padding-top: 7px; padding-bottom: 7px;">
                                                <input type="text" required runat="server" class="input_text" id="x_card_num" name="x_card_num" maxlength="16" autocomplete="off" value="" aria-required="true" aria-invalid="false" />*&nbsp;<span class="Comment">(enter number without spaces or dashes)</span></td>
                                        </tr>
                                        <tr>
                                            <td class="LabelColCC">
                                                <label for="x_exp_date"><span class="Hidden"></span>Expiration Date</label>:&nbsp;</td>
                                            <td class="DataColCC" style="padding-top: 7px; padding-bottom: 7px;">
                                                <input type="text" required runat="server" class="input_text" id="x_exp_date" name="x_exp_date" maxlength="20" autocomplete="off" value="" aria-required="true" aria-invalid="false" />*&nbsp;<span class="Comment">(mmyy)</span></td>
                                        </tr>
                                        <tr>
                                            <td class="LabelColCC">
                                                <label for="x_card_cvv"><span class="Hidden"></span>CVV</label>:&nbsp;</td>
                                            <td class="DataColCC" style="padding-top: 7px; padding-bottom: 7px;">
                                                <input type="text" required runat="server" class="input_text" id="x_card_cvv" name="x_card_cvv" maxlength="5" autocomplete="off" value="" aria-required="true" aria-invalid="false" />*&nbsp;<span class="Comment">(enter security code)</span></td>
                                        </tr>
                                        <tr>
                                            <td class="LabelColCC">
                                                <label for="x_card_cvv"><span class="Hidden"></span>Recurring</label>:&nbsp;</td>
                                            <td class="DataColCC" style="padding-top: 7px; padding-bottom: 7px;">
                                                <input type="checkbox" runat="server" class="input_text" id="chkRecurring" name="chkRecurring" 
                                                    aria-required="true" aria-invalid="false" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table style="display: none;">
                                                    <tr>
                                                        <td class="LabelColCC">
                                                            <label for="x_card_num" style="font-size: 13px;">Billing Address</label>:&nbsp;</td>
                                                        <td class="DataColCC" style="padding-top: 10px; padding-bottom: 10px;"></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <table>
                                                                <tr>
                                                                    <td class="LabelColCC">
                                                                        <label for="x_firstName">First Name</label>:&nbsp;</td>
                                                                    <td class="DataColCC" style="padding-top: 7px; padding-bottom: 7px;">
                                                                        <input type="text" required runat="server" class="input_text" id="x_firstName" name="x_firstName" autocomplete="off" value=""
                                                                            aria-required="true" aria-invalid="false" />*
                                                                    </td>
                                                                    <td class="LabelColCC" style="width: 74px;">
                                                                        <label for="x_lastName">Last Name</label>:&nbsp;</td>
                                                                    <td class="DataColCC" style="padding-top: 7px; padding-bottom: 7px;">
                                                                        <input type="text" required runat="server" class="input_text" id="x_lastName" name="x_lastName" autocomplete="off" value=""
                                                                            aria-required="true" aria-invalid="false" />*</td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="LabelColCC">
                                                            <label for="x_address">Address</label>:&nbsp;</td>
                                                        <td class="DataColCC" style="padding-top: 7px; padding-bottom: 7px;">
                                                            <input type="text" required runat="server" class="input_text" id="x_address" name="x_address" autocomplete="off" value=""
                                                                aria-required="true" aria-invalid="false" style="width: 471px;" />*</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <table>
                                                                <tr>
                                                                    <td class="LabelColCC">
                                                                        <label for="x_city">City</label>:&nbsp;</td>
                                                                    <td class="DataColCC" style="padding-top: 7px; padding-bottom: 7px;">
                                                                        <input type="text" required runat="server" class="input_text" id="x_city" name="x_city" autocomplete="off" value=""
                                                                            aria-required="true" aria-invalid="false" />*</td>
                                                                    <td class="LabelColCC" style="width: 38px;">
                                                                        <label for="x_state">State</label>:&nbsp;</td>
                                                                    <td class="DataColCC" style="padding-top: 7px; padding-bottom: 7px;">
                                                                        <input type="text" required runat="server" class="input_text" id="x_state" name="x_state" autocomplete="off" value=""
                                                                            aria-required="true" aria-invalid="false" />*</td>
                                                                    <td class="LabelColCC" style="width: 25px;">
                                                                        <label for="x_zip">Zip</label>:&nbsp;</td>
                                                                    <td class="DataColCC" style="padding-top: 7px; padding-bottom: 7px;">
                                                                        <input type="text" required runat="server" class="input_text" id="x_zip" name="x_zip" autocomplete="off" value=""
                                                                            aria-required="true" aria-invalid="false" />*</td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="LabelColCC">
                                                            <label for="x_Country" style="font-size: 13px;">Country</label>:&nbsp;</td>
                                                        <td class="DataColCC" style="padding-top: 7px; padding-bottom: 7px;">
                                                            <input type="text" required runat="server" class="input_text" id="x_Country" name="x_Country" autocomplete="off" value=""
                                                                aria-required="true" aria-invalid="false" />*</td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                            <td>
                                <script type='text/javascript' src='https://www.rapidscansecure.com/siteseal/siteseal.js?code=69,7DEEAE7288850F5ACF4C02C45004F779F3F5E661'></script>
                            </td>
                        </tr>
                    </table>
                    <%--<div class="form-grid separated--bottom" style="margin-left: 110px;">
                        <div class="grid1">
                            <div class="grid__item form-grid__item one-third palm-one-whole center-block">
                            </div>
                        </div>
                    </div>--%>

                    <div class="action text-right">
                        <button onclick="location.href = 'BillingPayment.aspx';" type="button" class="btn btn-darkBlue">Back</button>
                        <asp:Button ID="btnProcessCreditCardPayment" runat="server" CssClass="btn btn-orange" OnClick="btnProcessCreditCardPayment_Click" Text="Pay" />
                    </div>
                </div>
            </div>
        </div>

        <%--<div class="content-body" style="float: left; margin-top: 3%;">
                <div class="AuthorizeNetSeal">
                    <script type="text/javascript" language="javascript" src="//verify.authorize.net/anetseal/seal.js"></script>
                </div>
            </div>--%>
    </div>
</asp:Content>
