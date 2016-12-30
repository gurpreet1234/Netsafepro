using SafenetproAPI.Controllers;
using SafenetproModel_new;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;
using System.Security.Cryptography;

namespace Safenetpro
{
    public partial class Payment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userId"] == null)
                Response.Redirect("SignIn.aspx");
            else
            {
                if (!Page.IsPostBack)
                {
                    lblMessage.Text = string.Empty;
                    lblTransType.Text = string.Empty;
                    GetUserProfile(); getMilestoneAmount();
                }
            }
        }
        private void GetUserProfile()
        {
            UsersController cc = new UsersController();
            UserBillingAddress ub = cc.GetUserBillingInformation(Convert.ToInt32(Session["userId"]));
            x_firstName.Value = ub.Name;
            x_lastName.Value = ub.Name;
            x_address.Value = ub.Address;
            x_city.Value = ub.City;
            x_state.Value = ub.State;
            x_zip.Value = ub.ZipCode;
            x_Country.Value = "United States";
        }
        private void getMilestoneAmount()
        {
            ProductToUserController puc = new ProductToUserController();
            decimal totalAmount = puc.GetTotalProductMonthlyPrice(Convert.ToInt32(Session["userId"])).FirstOrDefault().Key;
            decimal totalYearlyAmount = puc.GetTotalProductMonthlyPrice(Convert.ToInt32(Session["userId"])).FirstOrDefault().Value;

            txtMilestoneAmount.Value = Convert.ToString(totalYearlyAmount);
            hdnMilestoneAmount.Value = Convert.ToString(totalYearlyAmount);
            hdnMonthlyAmount.Value = Convert.ToString(totalAmount);
            hdnYearlyAmount.Value = Convert.ToString(totalYearlyAmount);
        }
        protected void btnProcessCreditCardPayment_Click(object sender, EventArgs e)
        {
            if (txtMilestoneAmount.Value != "" && txtMilestoneAmount.Value != null)
                DoAuthorizationAndPayment();
        }
        private void DoAuthorizationAndPayment()
        {
            Safenetpro.com.usaepay.live.usaepayService client = getClient();
            Safenetpro.com.usaepay.live.ueSecurityToken token = getToken();

            Safenetpro.com.usaepay.live.TransactionRequestObject tran = new Safenetpro.com.usaepay.live.TransactionRequestObject();

            tran.Command = "cc:sale";
            tran.Details = new Safenetpro.com.usaepay.live.TransactionDetail();
            tran.Details.Amount = Convert.ToDouble(txtMilestoneAmount.Value);
            tran.Details.AmountSpecified = true;
            //tran.Details.Invoice = "1234";
            //tran.Details.Description = "Example Transaction";

            tran.BillingAddress = new com.usaepay.live.Address();
            tran.BillingAddress.FirstName = x_firstName.Value;
            tran.BillingAddress.LastName = x_lastName.Value;
            tran.BillingAddress.Street = x_address.Value;
            tran.BillingAddress.City = x_city.Value;
            tran.BillingAddress.Zip = x_zip.Value;

            tran.CreditCardData = new Safenetpro.com.usaepay.live.CreditCardData();
            tran.CreditCardData.CardNumber = x_card_num.Value;
            tran.CreditCardData.CardExpiration = x_exp_date.Value;
            tran.CreditCardData.CardCode = x_card_cvv.Value;
            tran.RecurringBilling = new com.usaepay.live.RecurringBilling();

            if (chkRecurring.Checked)
            {
                tran.RecurringBilling.Enabled = true;
                if (rdbYearly.Checked)
                    tran.RecurringBilling.Schedule = "annually";
                else
                    tran.RecurringBilling.Schedule = "monthly";
                tran.RecurringBilling.Amount = Convert.ToDouble(txtMilestoneAmount.Value);
            }
            else
                tran.RecurringBilling.Enabled = false;

            Safenetpro.com.usaepay.live.TransactionResponse response = new Safenetpro.com.usaepay.live.TransactionResponse();

            try
            {
                response = client.runTransaction(token, tran);

                if (response.ResultCode == "A")
                {
                    //lblMessage.Text = string.Concat("Transaction Approved, RefNum: ", response.RefNum);
                    ProductToUserController puc = new ProductToUserController();
                    int paymentPeriod = 1;
                    if (rdbYearly.Checked)
                        paymentPeriod = 12;
                    puc.UpdateProductToUser(Convert.ToInt32(Session["userId"]), paymentPeriod, DateTime.Now, txtMilestoneAmount.Value);
                    Session["OrderNumber"] = response.RefNum;
                    Response.Redirect("ThankYou.aspx");
                }
                else
                {
                    lblMessage.Text = string.Concat("Transaction Failed: ", response.Error);
                }
            }
            catch (Exception err)
            {
                //MessageBox.Show(err.Message);
            }
        }
        private static Safenetpro.com.usaepay.live.ueSecurityToken getToken()
        {
            Safenetpro.com.usaepay.live.ueSecurityToken token = new Safenetpro.com.usaepay.live.ueSecurityToken();

            //token.SourceKey = "2F5OsN6m88Zpr5Zow08ZuxLlr550q5fa"; //sandbox
            token.SourceKey = "6SH11rDi6b9Z0KL5134COR4UVnbP22aS"; //Production

            //string pin = "1234";
            // IP address of end user (if applicable)
            //token.ClientIP = "11.22.33.44";

            Safenetpro.com.usaepay.live.ueHash hash = new Safenetpro.com.usaepay.live.ueHash();
            hash.Type = "md5";
            hash.Seed = Guid.NewGuid().ToString();

            // Assemble string and hash
            string prehashvalue = string.Concat(token.SourceKey, hash.Seed);
            hash.HashValue = GenerateHash(prehashvalue);

            // Add hash to token
            token.PinHash = hash;

            return token;
        }

        /**
         * getClient()  provides a reusable method for creating soap client
         * 
         **/
        private static Safenetpro.com.usaepay.live.usaepayService getClient()
        {
            // Instantiate soap service client. 'usaepay' is the name you assigned
            //  when adding the web reference
            Safenetpro.com.usaepay.live.usaepayService client = new Safenetpro.com.usaepay.live.usaepayService();

            // Setup client to use a proxy server (required by some hosting providers)
            //  Uncomment the following lines if you need to use a proxy server
            /*
            System.Net.NetworkCredential proxycreds = new System.Net.NetworkCredential("user", "password");
            System.Net.WebProxy proxy = new System.Net.WebProxy("127.0.0.1", 80); //address of proxy server
            proxy.Credentials = proxycreds;
            client.Proxy = proxy;
            */

            return client;

        }

        private static string GenerateHash(string input)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            MD5 md5Hasher = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}