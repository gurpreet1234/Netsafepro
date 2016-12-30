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
    public partial class BillingPayment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userId"] == null)
                Response.Redirect("SignIn.aspx");
            else
            {
                if (!Page.IsPostBack)
                {
                    GetUserProfile();
                    BindProducts(Convert.ToInt32(Session["userId"]));
                }
            }
        }

        private void GetUserProfile()
        {
            UsersController cc = new UsersController();
            UserBillingAddress ub = cc.GetUserBillingInformation(Convert.ToInt32(Session["userId"]));
            if (ub != null)
            {
                txtlName.Value = ub.Name;
                txtaddress.Value = ub.Address;
                txtcity.Value = ub.City;
                if (selectState.Items.FindByValue(ub.State) != null)
                    selectState.Items.FindByValue(ub.State).Selected = true;
                txtZipCode.Value = ub.ZipCode;
                txtPhoneNumber.Value = ub.PhoneNumber;
                txtEmailAddress.Value = ub.EmailAddress;
            }
            else
            {
                UserProfile up = cc.GetUserProfile(Convert.ToInt32(Session["userId"]));
                if (Convert.ToBoolean(up.BusinessInfo))
                {
                    txtlName.Value = up.BName;
                    txtaddress.Value = up.BAddress;
                    txtcity.Value = up.BCity;
                    if (selectState.Items.FindByValue(up.BState) != null)
                        selectState.Items.FindByValue(up.BState).Selected = true;
                    txtZipCode.Value = up.BZip;
                    txtPhoneNumber.Value = up.BPhone;
                    txtEmailAddress.Value = up.BEmailAddress;
                }
                else
                {
                    txtlName.Value = up.LastName + " " + up.FirstName;
                    txtaddress.Value = up.Address;
                    txtcity.Value = up.City;
                    if (selectState.Items.FindByValue(up.State) != null)
                        selectState.Items.FindByValue(up.State).Selected = true;
                    txtZipCode.Value = up.Zip;
                    txtPhoneNumber.Value = up.Phone;
                    txtEmailAddress.Value = up.EmailAddress;
                }
            }
        }

        public void BindProducts(int userId)
        {
            ProductToUserController pc = new ProductToUserController();
            IEnumerable<ProductToUser_Price_POCO> userProducts = pc.GetProductLicenses(userId, false);
            string license = "";
            int setting = 0;
            decimal monthlySubtotal = 0;
            decimal totalMonthlySubtotal = 0;

            decimal yearlySubtotal = 0;
            decimal totalYearlySubtotal = 0;
            foreach (var up in userProducts)
            {
                int newSetting = up.Settings == null ? 0 : Convert.ToInt32(up.Settings);
                if (newSetting != setting)
                {
                    int marginTop = 0;
                    if (setting != 0)
                    {
                        license = license + "</ul></h5></div>";
                        marginTop = 30;
                    }
                    monthlySubtotal = 0;
                    if (newSetting == 1)//Moderate
                        license = license + "<div style=\"margin-left: 4%; margin-top:" + marginTop + "px\"><h5><strong>Computer Moderate License</strong></h5></div>";
                    else if (newSetting == 2)//Business
                        license = license + "<div style=\"margin-left: 4%; margin-top:" + marginTop + "px\"><h5><strong>Computer Business License</strong></h5></div>";
                    else if (newSetting == 3)//Personal
                        license = license + "<div style=\"margin-left: 4%; margin-top:" + marginTop + "px\"><h5><strong>Computer Personal License</strong></h5></div>";
                    else //Mobile Device
                        license = license + "<div style=\"margin-left: 4%; margin-top:" + marginTop + "px\"><h5><strong>Mobile Business License</strong></h5></div>";

                    license = license + "<div style=\"margin-left: 4%;\" class=\"field\"><h5><ul>";
                    setting = newSetting;
                }
                monthlySubtotal = monthlySubtotal + Convert.ToDecimal(up.MonthlyPrice);
                totalMonthlySubtotal = totalMonthlySubtotal + Convert.ToDecimal(up.MonthlyPrice);

                yearlySubtotal = yearlySubtotal + Convert.ToDecimal(up.YearlyPrice);
                totalYearlySubtotal = totalYearlySubtotal + Convert.ToDecimal(up.YearlyPrice);

                if (up.ProductId != 4)
                {
                    license = license + "<li>" + up.PrimaryUserName + " - $" + (Convert.ToDecimal(up.YearlyPrice)) + "/yr ($" + up.MonthlyPrice + "/month)</li>";
                }
                else
                {
                    license = license + "<li>" + up.PrimaryUser + " - $" + (Convert.ToDecimal(up.YearlyPrice)) + "/yr ($" + up.MonthlyPrice + "/month)</li>";
                }
            }
            license = license + "</ul></h5></div>";

            dvProducts.InnerHtml = license;

            lblMonthlyPayment.InnerText = "$" + Convert.ToString(totalMonthlySubtotal);
            lblYearlyPayment.InnerText = "$" + Convert.ToString(totalYearlySubtotal);

        }

        protected void Unnamed_ServerClick(object sender, EventArgs e)
        {
            UsersController cc = new UsersController();
            UserBillingAddress ub = new UserBillingAddress();
            ub.CustomerId = Convert.ToInt32(Session["userId"]);
            ub.Name = txtlName.Value;
            ub.Address = txtaddress.Value;
            ub.City = txtcity.Value;
            ub.State = selectState.Value;
            ub.ZipCode = txtZipCode.Value;
            ub.PhoneNumber = txtPhoneNumber.Value;
            ub.EmailAddress = txtEmailAddress.Value;

            cc.PutUserBillingInformation(ub);
            Response.Redirect("Payment.aspx");
        }
    }
}