class ProductsManagement {

    constructor() { }

    public static FormFunctions() {
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
    }

    public static ListProducts() {
        $('#loadingmessage').show();
        $("#headertext").text("Products");

        var gridList = "<table id='tblProducts' cellspacing='0' cellpadding='0' class='grid'>" +
            "<thead class='grid_header'><tr>" +
            "<th class='grid_header_first_column'>Name</th><th class='grid_header_column'>Monthly Price</th>" +
            "<th class='grid_header_column' colspan='2'>Actions</th></tr></thead><tbody>";

        var url = "Products";

        var successData = function (response) {
            $(response).find('Product').each(function () {
                gridList = gridList + "<tr><td class='first_bottom_border'>" + $(this).find('ProductName').text() + "</td>" +
                "<td class='bottom_border'>$" + $(this).find('MonthlyPrice').text() + "</td>" +
                "<td class='bottom_border'><a href='#' onclick='OpenProducts(" + $(this).find('Id').text() + ")'>Edit</a></td>" +
                "</tr>";
            });
            gridList = gridList + "</tbody></table>";
            $("#dvProducts").html(gridList);
            $('#loadingmessage').hide();
        };
        var errorData = function (response) {
            $('#loadingmessage').hide();
        };
        Utils.GetREST(url, successData, errorData);
    }

    public static addProduct() {
        if ($("#ProductDescription").val() && $("#MonthlyPrice").val() && $("#ProductName").val()) {
            var description = $("#ProductDescription").val(),
                monthlyPrice = $("#MonthlyPrice").val(),
                productName = $("#ProductName").val()
        var data = {
                Description: description,
                MonthlyPrice: monthlyPrice,
                ProductName: productName
            }
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
    }

    public static editProduct(productId) {
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
    }

    public static updateProduct() {
        if ($("#txtproductDescription").val() && $("#txtmonthlyPrice").val() && $("#txtproductName").val()) {
            $("#loadingmessage").show();
            var description = $("#txtproductDescription").val(),
                monthlyPrice = $("#txtmonthlyPrice").val(),
                productName = $("#txtproductName").val()
        var GroupData = {
                Description: description,
                MonthlyPrice: monthlyPrice,
                ProductName: productName
            }
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
    }
}