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
    public partial class ResetPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected void btnResetPassword_ServerClick(object sender, EventArgs e)
        {
            string passwordToken = Request.QueryString["Token"];
            if (passwordToken != null && passwordToken != "")
            {
                UsersController _UsersController = new UsersController();
                User _user = _UsersController.GetUserByToken(passwordToken);
                if (_user != null)
                {
                    _UsersController.UpdateUserPassword(txtNewUserPwd.Value.Trim(), _user.Id);
                    Response.Redirect("SignIn.aspx");
                }
                else
                    errorMessage.Style.Add("display", "block");
            }
            else
            {
                errorMessage.Style.Add("display", "block");
            }
        }
    }
}