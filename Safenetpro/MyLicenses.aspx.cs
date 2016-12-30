using SafenetproAPI.Controllers;
using SafenetproModel_new;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Safenetpro
{
    public partial class MyLicenses : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userId"] == null)
                Response.Redirect("SignIn.aspx");
            else
            {
                if (!Page.IsPostBack)
                {
                    ProductToUserController puc = new ProductToUserController();
                    List<ProductPrice> pp = puc.GetProductPrices().ToList();
                    foreach (var p in pp)
                    {
                        ListItem li = new ListItem();
                        li.Text = p.ProductName;
                        li.Value = Convert.ToString(p.Id);
                        if (p.SettingType == 1)
                        {
                            settings0.Items.Add(li);
                        }
                        else
                        {
                            settings_device0.Items.Add(li);
                        }
                    }
                    BindProducts(Convert.ToInt32(Session["userId"]));
                }
            }
        }

        public void BindProducts(int userId)
        {
            ProductToUserController pc = new ProductToUserController();
            IEnumerable<ProductToUser_Price_POCO> userProducts = pc.GetProductLicenses(userId, true);

            if (userProducts.Count() <= 0)
            {
                Response.Redirect("Setup.aspx");
            }
            else
            {
                string license = "";
                int setting = 0;
                decimal monthlySubtotal = 0;
                decimal totalMonthlySubtotal = 0;
                decimal yearlySubtotal = 0;
                decimal totalYearlySubtotal = 0;
                Dictionary<int?, int?> _uProducts = new Dictionary<int?, int?>();
                Dictionary<int?, int?> _uProductsCount = new Dictionary<int?, int?>();

                Dictionary<int?, int?> _uProductsCountHeader = new Dictionary<int?, int?>();

                foreach (var up in userProducts)
                {
                    int? count = 0;

                    var _up = _uProducts.Where(u => u.Key == up.Settings).FirstOrDefault();
                    if (_up.Key == null)
                        _uProducts.Add(up.Settings, up.AdditionalLicenseCount);
                    int newSetting = up.Settings == null ? 0 : Convert.ToInt32(up.Settings);

                    if (newSetting != setting || (_uProductsCount.Where(u => u.Key == up.Settings).FirstOrDefault().Value == _uProducts.Where(u => u.Key == up.Settings).FirstOrDefault().Value + 1))
                    {
                        _uProductsCount.Remove(up.Settings);
                        _uProductsCount.Add(up.Settings, 0);

                        int? headerCount = 0;
                        var upch = _uProductsCountHeader.Where(u => u.Key == up.Settings).FirstOrDefault();
                        if (upch.Key != null)
                        {
                            headerCount = upch.Value;
                            _uProductsCountHeader.Remove(up.Settings);
                        }
                        _uProductsCountHeader.Add(up.Settings, headerCount + 1);

                        if (setting != 0)
                        {
                            license = license + "<div class=\"row text-right margin-top15\">"
                                    + "<span class=\"yearly\">Yearly Subtotal: <strong>$" + yearlySubtotal + "</strong></span>"
                                    + "<span class=\"monthly\">Monthly Subtotal: <strong>$" + monthlySubtotal + "</strong></span>"
                                + "</div></div>";
                        }
                        //int countProducts = userProducts.Where(u => Convert.ToInt32(u.Settings) == newSetting).Count();
                        monthlySubtotal = 0;
                        yearlySubtotal = 0;

                        var upc_ = _uProductsCount.Where(u => u.Key == up.Settings).FirstOrDefault();

                        license = license + "<div class=\"licensePkgBox\"><div class=\"row\"><h3><strong>License " + (headerCount + 1) + " " + up.ProductName + "</strong></h3></div>";

                        setting = newSetting;
                    }

                    var upc = _uProductsCount.Where(u => u.Key == up.Settings).FirstOrDefault();
                    if (upc.Key != null)
                    {
                        count = upc.Value;
                        _uProductsCount.Remove(up.Settings);
                    }
                    _uProductsCount.Add(up.Settings, count + 1);

                    monthlySubtotal = monthlySubtotal + Convert.ToDecimal(up.MonthlyPrice);
                    totalMonthlySubtotal = totalMonthlySubtotal + Convert.ToDecimal(up.MonthlyPrice);

                    yearlySubtotal = yearlySubtotal + Convert.ToDecimal(up.YearlyPrice);
                    totalYearlySubtotal = totalYearlySubtotal + Convert.ToDecimal(up.YearlyPrice);

                    license = license + "<div class=\"offerBox\">"
                        + "<div class=\"pull-right rightLink\">"

                            + "<strong>$" + up.YearlyPrice + "/year | $" + up.MonthlyPrice + "/month</strong>"
                            + "<a href=\"#\" onclick='editProduct(" + up.Id + "," + up.ProductId + ")'><i class=\"customIcon icon-edit\"></i>Edit</a>"
                            + "<a href=\"#\" onclick='deleteAlert(" + up.Id + ")'><i class=\"customIcon icon-remove\"></i>Remove</a>"
                        + "</div>";

                    if (up.ProductId != 4)
                    {
                        license = license + "<ul>"
                                + "<li>"
                                + "<img src=\"img/prodDesktop.jpg\" alt=''></li>"
                                + "<li>" + up.PrimaryUserName + "</li>"
                                + "<li>" + up.OperatingSystem + "</li>"
                                + "<li>" + up.Location + "</li>"
                            + "</ul>"
                        + "</div>";
                    }
                    else
                    {
                        license = license + "<ul>"
                                + "<li>"
                                + "<img src=\"img/prodDesktop.jpg\" alt=''></li>"
                                + "<li>" + up.PrimaryUser + "</li>"
                                + "<li>" + up.PhoneOS + "</li>"
                                + "<li>" + up.Usage + "</li>"
                            + "</ul>"
                        + "</div>";
                    }
                }
                license = license + "<div class=\"row text-right margin-top15\">"
                                     + "<span class=\"yearly\">Yearly Subtotal: <strong>$" + yearlySubtotal + "</strong></span>"
                                    + "<span class=\"monthly\">Monthly Subtotal: <strong>$" + monthlySubtotal + "</strong></span>"
                                + "</div>";

                dvMain.InnerHtml = license;
                lblMonthlyPayment.InnerText = "$" + Convert.ToString(totalMonthlySubtotal);
                lblYearlyPayment.InnerText = "$" + Convert.ToString(totalYearlySubtotal);

                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "MyFun1", "formFunctions();", true);
            }
        }

        protected void btnRefreshData_ServerClick(object sender, EventArgs e)
        {
            BindProducts(Convert.ToInt32(Session["userId"]));
        }
    }
}