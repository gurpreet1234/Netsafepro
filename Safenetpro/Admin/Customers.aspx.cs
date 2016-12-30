using SafenetproAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Safenetpro.Admin
{
    public partial class Customers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["adminUserId"] == null)
                Response.Redirect("Login.aspx");
        }

        protected void btnProcessCreditCardPayment_Click(object sender, EventArgs e)
        {
            if (txtAmount.Value != "" && txtAmount.Value != null)
                DoAuthorizationAndPayment();
        }

        private void DoAuthorizationAndPayment()
        {
            Safenetpro.com.usaepay.live.usaepayService client = getClient();
            Safenetpro.com.usaepay.live.ueSecurityToken token = getToken();

            Safenetpro.com.usaepay.live.TransactionRequestObject tran = new Safenetpro.com.usaepay.live.TransactionRequestObject();

            tran.Command = "cc:sale";
            tran.Details = new Safenetpro.com.usaepay.live.TransactionDetail();
            tran.Details.Amount = Convert.ToDouble(txtAmount.Value);
            tran.Details.AmountSpecified = true;
            //tran.Details.Invoice = "1234";
            //tran.Details.Description = "Example Transaction";

            UsersController uc = new UsersController();
            var userProfile = uc.GetUserProfile(Convert.ToInt32(hdnUserId.Value));

            tran.BillingAddress = new com.usaepay.live.Address();
            tran.BillingAddress.FirstName = userProfile.FirstName;
            tran.BillingAddress.LastName = userProfile.LastName;
            tran.BillingAddress.Street = userProfile.BAddress;
            tran.BillingAddress.City = userProfile.BCity;
            tran.BillingAddress.Zip = userProfile.BZip;

            tran.CreditCardData = new Safenetpro.com.usaepay.live.CreditCardData();
            tran.CreditCardData.CardNumber = x_card_num.Value;
            tran.CreditCardData.CardExpiration = x_exp_date.Value;
            tran.CreditCardData.CardCode = x_card_cvv.Value;

            Safenetpro.com.usaepay.live.TransactionResponse response = new Safenetpro.com.usaepay.live.TransactionResponse();

            try
            {
                response = client.runTransaction(token, tran);

                if (response.ResultCode == "A")
                {
                    lblMessage.Text = string.Concat("Transaction Approved, RefNum: ", response.RefNum);
                    ProductToUserController puc = new ProductToUserController();
                    int payment = Convert.ToInt32(paymentPeriod.Value);

                    puc.UpdateProductToUser(Convert.ToInt32(hdnUserId.Value), payment, DateTime.Now, txtAmount.Value);
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

        protected void btnUpdateLicenseDate_Click(object sender, EventArgs e)
        {
            if (txtStartDate.Value != "")
            {
                ProductToUserController puc = new ProductToUserController();
                puc.UpdateLicenseStartDate(Convert.ToInt32(hdnProductToUserId.Value), Convert.ToDateTime(txtStartDate.Value));
            }
        }
    }
}