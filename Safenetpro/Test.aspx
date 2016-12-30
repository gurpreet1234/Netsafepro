<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="Safenetpro.Test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>

    <form runat="server" id="form1">
        <%--<input type="hidden" name="UMkey" value="2F5OsN6m88Zpr5Zow08ZuxLlr550q5fa">
        <input type="hidden" name="UMcommand" value="sale">
        <input type="hidden" name="UMamount" value="1.00">
        
        Credit Card Info:<br>
        Card Type:
        <select name="cardtype">
            <option>Visa
                <option>Mastercard
                    <option>
            Amex
        </select><br>
        Card Number:
        <input type="text" name="UMcard"><br>
        Expiration:
        <input type="text" name="UMexpir"><br>
        Name On Card:
        <input type="text" name="UMname"><br>
        <br>
        <br>
        <input type="submit" value="Place Order">--%>
        <input type="submit" id="pay" runat="server" value="Pay" onserverclick="pay_ServerClick" />
    </form>

</body>
</html>
