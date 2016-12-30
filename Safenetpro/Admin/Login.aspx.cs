using SafenetproAPI.Controllers;
using SafenetproModel_new;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Safenetpro.Admin
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["adminUserId"] = null;
            Session["adminFullName"] = null;
            Session["adminUserName"] = null;
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "logoutsuccess", "logout();", true);
        }

        protected void btnContinueClick_ServerClick(object sender, EventArgs e)
        {
            UsersController cc = new UsersController();
            AdminUser u = cc.AuthenticateAdminUser(txtUserName.Value, txtPassword.Value);
            if (u != null && u.Id > 0)
            {
                Session["adminFullName"] = u.FirstName + " " + u.LastName;
                Session["adminUserName"] = txtUserName.Value;
                Session["adminUserId"] = u.Id;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "success", "loginsuccess(" + u.Id + ");", true);
            }
            else
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "MyFun1", "loginerror();", true);
        }
    }
}