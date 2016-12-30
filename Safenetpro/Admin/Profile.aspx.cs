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
    public partial class Profile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["adminUserId"] != null)
                    FillUserProfile();
            }
        }

        protected void btnhiddenServerClick_ServerClick(object sender, EventArgs e)
        {
            AdminUser u = new AdminUser();
            u.Id = Convert.ToInt32(Session["adminUserId"]);
            u.Email = txtUserName.Value;
            u.Password = txtPassword.Value;
            u.FirstName = txtfname.Value;
            u.LastName = txtlName.Value;

            UsersController cc = new UsersController();
            AdminUser adminUserProfile = new AdminUser();
            if (Session["adminUserId"] != null)
                adminUserProfile = cc.PutAdminUserProfile(u, Convert.ToInt32(Session["adminUserId"]));
            if (adminUserProfile.Id > 0)
            {
                Session["adminFullName"] = adminUserProfile.FirstName + " " + adminUserProfile.LastName;
                Session["adminUserName"] = adminUserProfile.Email;
                Session["adminUserId"] = adminUserProfile.Id;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "success", "signupsuccess(" + adminUserProfile.Id + ");", true);
            }
        }

        public void FillUserProfile()
        {
            UsersController uc = new UsersController();
            AdminUser up = uc.GetAdminUserProfile(Convert.ToInt32(Session["adminUserId"]));
            txtfname.Value = up.FirstName;
            txtlName.Value = up.LastName;
            txtUserName.Value = up.Email;
        }
    }
}