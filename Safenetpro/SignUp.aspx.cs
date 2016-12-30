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
    public partial class SignUp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if ((Session["newUserName"] != null && Session["newUserPassword"] != null) || Session["userId"] != null)
                {
                    txtUserName.Value = Convert.ToString(Session["newUserName"]);
                    txtUserPassword.Value = Convert.ToString(Session["newUserPassword"]);

                    BindOrganizations();
                    if (Session["userId"] != null)
                    {
                        FillUserProfile();
                        continueRegistration.InnerText = "Continue";
                    }
                }
                else
                    Response.Redirect("SignIn.aspx");
            }
        }
        public void BindOrganizations()
        {
            OrganizationController OC = new OrganizationController();
            mOrganization.DataSource = OC.GetOrganizations();
            mOrganization.DataTextField = "OrgName";
            mOrganization.DataValueField = "Id";
            mOrganization.DataBind();

            mOrganization.Items.Add(new ListItem("Other", "0"));
        }
        protected void continueRegistration_click(object sender, EventArgs e)
        {
            User u = new User();
            u.UserName = txtUserName.Value;
            u.Password = txtUserPassword.Value;
            u.Active = true;

            UserProfile up = new UserProfile();
            up.FirstName = txtfname.Value;
            up.LastName = txtlName.Value;
            up.Address = txtaddress.Value;
            up.Address2 = txtaddress2.Value;
            up.City = txtcity.Value;
            up.State = selectState.Value;
            up.Zip = txtZipCode.Value;
            up.Phone = txtPhoneNumber.Value;
            up.CellPhone = txtCellPhone.Value;
            up.Fax = txtFax.Value;
            up.EmailAddress = txtEmailAddress.Value;

            if (chkBusiness.Checked)
            {
                up.BName = bname.Value;
                up.BAddress = bAddress.Value;
                up.BCity = bCity.Value;
                up.BState = bState.Value;
                up.BZip = bZipCode.Value;
                up.BPhone = bPhoneNumber.Value;
                up.BCellPhone = bCellPhone.Value;
                up.BFax = bFax.Value;
                up.BEmailAddress = bEmailAddress.Value;
                up.BusinessInfo = true;
            }
            else
            {
                up.BName = "";
                up.BAddress = "";
                up.BCity = "";
                up.BState = "";
                up.BZip = "";
                up.BPhone = "";
                up.BCellPhone = "";
                up.BFax = "";
                up.BEmailAddress = "";
                up.BusinessInfo = false;
            }

            if (chkBusiness.Checked)
                up.ProfileType = 2;
            else
                up.ProfileType = 1;

            UserToOrg uo = new UserToOrg();
            uo.OrgId = Convert.ToInt32(mOrganization.Value);
            if (uo.OrgId == 0)
                uo.OrgOther = txtOtherOrganization.Value;

            UsersController cc = new UsersController();
            UserProfile userProfile;
            if (Session["userId"] != null)
                userProfile = cc.PutUsers(u, up, uo, Convert.ToInt32(Session["userId"]));
            else
                userProfile = cc.PostUsers(u, up, uo);
            if (userProfile.UserId > 0)
            {
                Session["fullName"] = userProfile.FirstName + " " + userProfile.LastName;
                Session["userName"] = txtUserName.Value;
                Session["userId"] = userProfile.UserId;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "success", "signupsuccess(" + userProfile.UserId + ");", true);
                //Response.Redirect("Setup.aspx");
            }
        }

        public void FillUserProfile()
        {
            UsersController uc = new UsersController();
            UserProfile_Poco up = uc.GetProfile(Convert.ToInt32(Session["userId"]));
            txtfname.Value = up.FirstName;
            txtlName.Value = up.LastName;
            txtaddress.Value = up.Address;
            txtaddress2.Value = up.Address2;
            txtcity.Value = up.City;
            if (selectState.Items.FindByValue(up.State) != null)
                selectState.Items.FindByValue(up.State).Selected = true;
            txtZipCode.Value = up.Zip;
            txtPhoneNumber.Value = up.Phone;
            txtCellPhone.Value = up.CellPhone;
            txtFax.Value = up.Fax;
            txtEmailAddress.Value = up.EmailAddress;
            bname.Value = up.BName;
            bAddress.Value = up.BAddress;
            bCity.Value = up.BCity;
            if (bState.Items.FindByValue(up.BState) != null)
                bState.Items.FindByValue(up.BState).Selected = true;
            bZipCode.Value = up.BZip;
            bPhoneNumber.Value = up.BPhone;
            bCellPhone.Value = up.BCellPhone;
            bFax.Value = up.BFax;
            bEmailAddress.Value = up.BEmailAddress;
            mOrganization.Value = Convert.ToString(up.OrgId);
            if (up.OrgId == 0)
            {
                txtOtherOrganization.Value = up.OtherOrg;
                txtOtherOrganization.Style.Add("Display", "Show");
            }
            else
                txtOtherOrganization.Style.Add("Display", "none");

            chkBusiness.Checked = Convert.ToBoolean(up.BusinessInfo);
            if (Convert.ToBoolean(up.BusinessInfo))
                dvBusinessInfo.Style.Add("Display", "Show");
            else
                dvBusinessInfo.Style.Add("Display", "none");
            txtUserName.Value = Convert.ToString(Session["userName"]);
        }
    }
}