using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Safenetpro
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private static Safenetpro.com.usaepay.live.ueSecurityToken getToken()
        {
            // Instantiate token object
            Safenetpro.com.usaepay.live.ueSecurityToken token = new Safenetpro.com.usaepay.live.ueSecurityToken();

            // SourceKey and Pin (created in merchant console)
            token.SourceKey = "6SH11rDi6b9Z0KL5134COR4UVnbP22aS";
            //string pin = "1234";

            // IP address of end user (if applicable)
            //token.ClientIP = "11.22.33.44";

            // Instantiate Hash
            Safenetpro.com.usaepay.live.ueHash hash = new Safenetpro.com.usaepay.live.ueHash();
            hash.Type = "md5";  // Type of encryption 
            hash.Seed = Guid.NewGuid().ToString();  // unique encryption seed

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

        protected void pay_ServerClick(object sender, EventArgs e)
        {
            Safenetpro.com.usaepay.live.usaepayService client = getClient();
            Safenetpro.com.usaepay.live.ueSecurityToken token = getToken();

            Safenetpro.com.usaepay.live.TransactionRequestObject tran = new Safenetpro.com.usaepay.live.TransactionRequestObject();

            tran.Command = "cc:sale";
            tran.Details = new Safenetpro.com.usaepay.live.TransactionDetail();
            tran.Details.Amount = 1.00;
            tran.Details.AmountSpecified = true;
            tran.Details.Invoice = "1234";
            tran.Details.Description = "Example Transaction";

            tran.CreditCardData = new Safenetpro.com.usaepay.live.CreditCardData();
            tran.CreditCardData.CardNumber = "4987654321098769";
            tran.CreditCardData.CardExpiration = "0517";
            tran.CreditCardData.CardCode = "0000000000000";

            Safenetpro.com.usaepay.live.TransactionResponse response = new Safenetpro.com.usaepay.live.TransactionResponse();

            try
            {
                response = client.runTransaction(token, tran);

                if (response.ResultCode == "A")
                {
                    //MessageBox.Show(string.Concat("Transaction Approved, RefNum: ",
                    //        response.RefNum));
                }
                else
                {
                    //MessageBox.Show(string.Concat("Transaction Failed: ",
                    //        response.Error));
                }
            }
            catch (Exception err)
            {
                //MessageBox.Show(err.Message);
            }
        }
    }
}