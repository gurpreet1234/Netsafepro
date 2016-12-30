<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ThankYou.aspx.cs" Inherits="Safenetpro.ThankYou" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="upPanelMain" runat="server">
        <ContentTemplate>
            <div style="margin-left: 29%; margin-top: 6%;">
                <span class="thankyou">Thank You, your order has been placed.</span>
                <p style="margin-left:14%; font-size:15px;">
                    An e-mail confirmation has been sent to you.
                </p>
                <p style="margin-left:18%; font-size:15px;">
                    Order Number:
                    <label id="lblOrderNumber" runat="server"></label>
                </p>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
