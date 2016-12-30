using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Safenetpro
{
    public partial class Main : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userId"] == null)
            {
                accountLogin.Visible = true;
                dvWelcome.Visible = false;
                //accountSignUp.Visible = false;
            }
            else
            {
                accountLogin.Visible = false;
                dvWelcome.Visible = true;
                //accountSignUp.Visible = true;
                //lblUserName.InnerText = Convert.ToString(Session["fullName"]);
            }

            //dvheaderButtons.Visible = true;
            if (Page.Request.Url.ToString().Contains("SignIn"))
            {
                //dvheaderButtons.Visible = false;
            }
        }

        protected void logout_ServerClick(object sender, EventArgs e)
        {
            Session["userId"] = null;
            Session["fullName"] = null;
            
            Response.Redirect("SignIn.aspx");
        }
    }
}