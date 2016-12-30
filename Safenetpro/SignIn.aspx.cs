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
    public partial class SignIn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["userId"] = null;
            Session["fullName"] = null;
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "logoutsuccess", "logout();", true);
        }

        protected void btnContinueClick_ServerClick(object sender, EventArgs e)
        {
            UsersController cc = new UsersController();
            UserProfile u = cc.AuthenticateUser(txtUserName.Value, txtPassword.Value);
            if (u != null && u.Id > 0)
            {
                Session["fullName"] = u.FirstName + " " + u.LastName;
                Session["userName"] = txtUserName.Value;
                Session["userId"] = u.UserId;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "success", "loginsuccess(" + u.UserId + ");", true);
                //Response.Redirect("Setup.aspx");
            }
            else
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "MyFun1", "loginerror();", true);
        }

        protected void btnNewuser_ServerClick(object sender, EventArgs e)
        {
            Session["newUserName"] = txtNewUserName.Value;
            Session["newUserPassword"] = txtNewUserPwd.Value;
            Response.Redirect("SignUp.aspx");
        }
    }
}