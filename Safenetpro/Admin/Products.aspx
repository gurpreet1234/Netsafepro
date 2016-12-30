<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/admin.Master" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="Safenetpro.Admin.Products" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../js/jquery.tablesorter.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            ProductsManagement.FormFunctions();
            $('#dvProducts').bind('contentChanged', function (event, data) {
                $("#tblProducts").tablesorter({ debug: true });
            });
        });

        function OpenProducts(productId) {
            ProductsManagement.editProduct(productId);
            $("#hiderProducts").fadeIn("slow");
            $('#updateProduct').fadeIn("slow");
        }

        function openModalProducts() {
            $("#hiderProducts").fadeIn("slow");
            $("#addProduct").fadeIn("slow");
        }

        function ProductAdd() {
            console.log("call")
           ProductsManagement.addProduct();
        }

        function productCancels() {

            $("#hiderProducts").fadeOut("slow");
            $("#addProduct").fadeOut("slow");
            
        }

    </script>
    <div id="loadingmessage" style="display: none;">
        <img src="../img/ajax-loader.gif" class="ajax-loader" />
    </div>
    <div class="cf separated--bottom" style="margin-top: 15px; margin-left: 3%;">
      <%--  <h3><a id="btnAdd" style="margin-right: 60px; margin-top: 10px;" class="pull-right" onclick="openModalProducts()">Add</a></h3>--%>
        <button type="button" style="margin-right: 60px;float: right;" id="btnAdd" onclick="openModalProducts()" class="btn btn-darkBlue"><i class="customIcon icon-yes"></i>Add</button>
        <h1 id="headertext">PRODUCTS</h1>
    </div>

    <div id="hiderProducts" style="display: none;"></div>
    <div class="container margin-top30 formContainer" id="updateProduct" style="display: none; background-color: #e3e3e3; border-radius: 10px;">
        <h3>
            <label id="lblComputer0">Update Product</label></h3>
        <input type="hidden" id="hdnProductId" />
        <div class="row">
            <div class="col-md-5" style="width: 100%;">
                <div class="field">
                    <label>Name</label>
                    <input type="text" id="txtproductName" class="form-control" />
                </div>
                <div class="field">
                    <label>Description</label>
                    <input type="text" id="txtproductDescription" class="form-control" />
                </div>
                <div class="field">
                    <label>Price</label>
                    <input type="text" id="txtmonthlyPrice" class="form-control" />
                </div>
                <div class="field" style="clear: both;">
                    <button type="button" id="productUpdate" class="btn btn-darkBlue"><i class="customIcon icon-yes"></i>Update</button>
                    <button type="button" id="productCancel" onclick="productCancels()" class="btn btn-darkBlue"><i class="customIcon icon-yes"></i>Cancel</button>
                    <button style="display: none;" type="button" id="productRefresh" class="btn btn-darkBlue"><i class="customIcon icon-yes"></i>Refresh</button>
                </div>
            </div>
        </div>
    </div>

    <div class="container margin-top30 formContainer" id="addProduct" style="display: none; background-color: #e3e3e3; border-radius: 10px;">
        <h3>
            <label id="lblComputer">Add Product</label></h3>
        <div class="row">
            <div class="col-md-5" style="width: 100%;">
                <div class="field">
                    <label>Name</label>
                    <input type="text" id="ProductName" class="form-control" />
                </div>
                <div class="field">
                    <label>Description</label>
                    <input type="text" id="ProductDescription" class="form-control" />
                </div>
                <div class="field">
                    <label>Price</label>
                    <input type="text" id="MonthlyPrice" class="form-control" />
                </div>
                <div class="field" style="clear: both;">
                    <button type="button" id="ProductAdds" onclick="ProductAdd()" class="btn btn-darkBlue"><i class="customIcon icon-yes"></i>Add</button>
                    <button type="button" id="ProCancel" onclick="productCancels()" class="btn btn-primary"><i class="customIcon icon-yes"></i>Cancel</button>
                    <%-- <button style="display: none;" type="button" id="productRefresh" class="btn btn-darkBlue"><i class="customIcon icon-yes"></i>Refresh</button>--%>
                </div>
            </div>
        </div>
    </div>

    <div>
        <div class="content-body" id="dvProducts" style="float: left; width: 100%; color: black;">
        </div>
    </div>
</asp:Content>
