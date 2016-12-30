using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Safenetpro.Admin
{
    public partial class admin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
             if (Session["adminUserId"] == null)
             {
                 dvLeftBar.Style.Add("display", "none");
                 dvWelcome.Style.Add("display", "none");
             }
             else
             {
                 dvLeftBar.Style.Add("display", "display");
                 dvWelcome.Style.Add("display", "display");
                 lblUserName.InnerText = Convert.ToString(Session["adminFullName"]);
             }
             string thisURL = this.Page.GetType().Name.ToString();
             switch (thisURL)
             {
                 case "admin_profile_aspx":
                     dvProfile.Attributes.Add("class", "leftmenu activeLeftmenu");
                     dvProduct.Attributes.Add("class", "leftmenu");
                     dvCus.Attributes.Add("class", "leftmenu");
                     dvReports.Attributes.Add("class", "leftmenu");
                     break;
                 case "admin_customers_aspx":
                     dvProduct.Attributes.Add("class", "leftmenu");
                     dvCus.Attributes.Add("class", "leftmenu activeLeftmenu");
                     dvReports.Attributes.Add("class", "leftmenu");
                     dvProfile.Attributes.Add("class", "leftmenu");
                     break;
                 case "admin_products_aspx":
                     dvReports.Attributes.Add("class", "leftmenu");
                     dvProfile.Attributes.Add("class", "leftmenu");
                     dvProduct.Attributes.Add("class", "leftmenu activeLeftmenu");
                     dvCus.Attributes.Add("class", "leftmenu");
                     break;
                 case "admin_reports_aspx":
                     dvReports.Attributes.Add("class", "leftmenu activeLeftmenu");
                     dvProfile.Attributes.Add("class", "leftmenu");
                     dvProduct.Attributes.Add("class", "leftmenu");
                     dvCus.Attributes.Add("class", "leftmenu");
                     break;
             }
        }

        protected void logout_ServerClick(object sender, EventArgs e)
        {
            Session["adminUserId"] = null;
            Session["adminFullName"] = null;
            Session["adminUserName"] = null;

            Response.Redirect("Login.aspx");
        }
    }
}