using SafenetproAPI.Controllers;
using SafenetproModel_new;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Safenetpro
{
    public partial class Setup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userId"] == null)
                Response.Redirect("SignIn.aspx");
        }

        protected void setupComputer_ServerClick(object sender, EventArgs e)
        {
            setupDynamicComputer();
        }

        protected void setupDevice_ServerClick(object sender, EventArgs e)
        {
            setupDynamicDevice();
        }
        protected void setupYes_ServerClick(object sender, EventArgs e)
        {
            //setup.Style.Add("Display", "None");
        }
        protected void setupNo_ServerClick(object sender, EventArgs e)
        {
        }
        public void setupDynamicComputer()
        {
            setup.Style.Add("Display", "None");
            string lcText = ((LiteralControl)dvDynamicSetup.Controls[0]).Text;
            int c = Regex.Matches(Regex.Escape(lcText), "operatingSystem").Count;

            ProductToUserController puc = new ProductToUserController();
            List<ProductPrice> pp = puc.GetProductPrices().Where(p => p.SettingType == 1).ToList();

            string computer = "<div class=\"container margin-top30 formContainer\" id=\"computer" + c + "\">"
                 + " <h3><label id=lblComputer" + c + ">SET UP A COMPUTER</label><img class='setupImage' src=\"img/desktop.png\" alt=''></h3>"
                 + " <div class=\"row\">"
                    + "  <div class=\"col-md-5\">"
                         + " <div class=\"field\">"
                            + "  <label>Operating System</label>"
                             + " <select class=\"form-control\" id=\"operatingSystem" + c + "\"><option Value='0'>Select</option><option Value='Windows 7 32'>Windows 7 32</option><option Value='Windows 7 64'>Windows 7 64</option><option Value='Windows 8/10'>Windows 8/10</option><option value='Windows 8/10 32bit'>Windows 8/10 32bit</option></select>"
                         + " </div>"

                         + " <div class=\"field\">"
                            + "  <label>Primary User Name</label>"
                             + " <input onblur=\"checkUserNameAvailability(this.value," + c + ")\" type=\"text\" id=\"primaryusername" + c + "\" class=\"form-control\">"
                             + " <label id=\"lblAlreadyExists\" style=\"display: none; color: #c0392b;\">primary user name already exist, please try a other name.</label>"
                         + " </div>"

                         + " <div class=\"field\">"
                            + "  <label>Location</label>"
                            + " <select class=\"form-control\" id=\"location" + c + "\"><option Value='0'>Select</option><option Value='Home'>Home</option><option Value='Office'>Office</option></select>"
                         + " </div>"

                         + " <div class=\"field\">"
                            + "  <label>Filter Settings</label>"
                            + " <select class=\"form-control\" id=\"settings" + c + "\"><option Value='0'>Select</option>";
            foreach (var p in pp)
            {
                computer = computer + "<option Value='" + p.Id + "'>" + p.ProductName + "</option>";
            }
            //+ "<option Value='1'>Moderate</option><option Value='2'>Regular (Business)</option><option Value='3'>Restricted (Home User)</option>"

            computer = computer + "</select>"
         + " </div>"

      + " <div class=\"field\" id=\"dvDevice_" + c + "\">"
                 + " <label>URL</label>"
            + " <div class=\"field\" id=\"dvURL" + c + "_0\">"
                 + " <input style='width:87%; float:left; margin-bottom:5px;' type=\"text\" id=\"URL_device" + c + "_0\" class=\"form-control\">"
                 + " <button onclick='addNewURL(" + c + ",0)' style='padding:4px; float:left; margin-left:5px;' type=\"button\" id=\"addURL" + c + "_0\" class=\"btn btn-primary\">+</button>"
                 + " <button onclick='deleteURL(" + c + ",0,\"dvURL" + c + "_0\")' style='padding:4px; float:left; margin-left:5px;' type=\"button\" id=\"deleteURL" + c + "_0\" class=\"btn btn-primary\">-</button>"
            + " </div>"
         + " </div>"

         + " <div class=\"field\" style='background-color:#ececec;margin-top:10%;border-radius:10px;height:80px; clear:both;'>"
            + " <label id=lblQuestion" + c + " style='margin-left:10px;'>Do you have another computer/device?</label><br/>"
            + " <button style='margin-left:10px;' type=\"button\" onclick=\"setupYes(1," + c + ")\" id=\"setupYes" + c + "\" runat=\"server\" class=\"btn btn-darkBlue\"><i class=\"customIcon icon-yes\"></i>Yes</button>"
            + " <button style='margin-left:10px;' type=\"button\" onclick=\"setupNo(1," + c + ")\" id=\"setupNo" + c + "\" runat=\"server\" class=\"btn btn-darkBlue\"><i class=\"customIcon icon-no\"></i>No</button>"

            + " <button style='display:none; margin-left:10px;' type=\"button\" onclick=\"setupEdit(1," + c + ")\" id=\"setupEdit" + c + "\" runat=\"server\" class=\"btn btn-darkBlue\"><i class=\"customIcon icon-yes\"></i>Edit</button>"
            + " <button style='display:none; margin-left:10px;' type=\"button\" onclick=\"setupUpdate(1," + c + ")\" id=\"setupUpdate" + c + "\" runat=\"server\" class=\"btn btn-darkBlue\"><i class=\"customIcon icon-yes\"></i>Update</button>"
            + " <button style='display:none; margin-left:10px;' type=\"button\" onclick=\"setupDelete(1," + c + ")\" id=\"setupDelete" + c + "\" runat=\"server\" class=\"btn btn-darkBlue\"><i class=\"customIcon icon-no\"></i>Delete</button>"
         + " </div>"
     + " </div>"
 + " </div>"
+ " </div>";

            dvDynamicSetup.InnerHtml = dvDynamicSetup.InnerHtml + computer;

            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "MyFun1", "clearpreviousbuttons(" + c + ");", true);
        }
        public void setupDynamicDevice()
        {
            setup.Style.Add("Display", "None");
            string lcText = ((LiteralControl)dvDynamicSetup.Controls[0]).Text;
            int c = Regex.Matches(Regex.Escape(lcText), "operatingSystem").Count;

            ProductToUserController puc = new ProductToUserController();
            List<ProductPrice> pp = puc.GetProductPrices().Where(p => p.SettingType == 2).ToList();

            string computer = "<div class=\"container margin-top30 formContainer\" id=\"device" + c + "\">"
                 + " <h3><label id=lblDevice" + c + ">SET UP A DEVICE</label><img class='setupImage' src=\"img/iPhone.png\" alt=''></h3>"
                 + " <div class=\"row\">"
                    + "  <div class=\"col-md-5\">"

                         + " <div class=\"field\">"
                            + "  <label style='float:left;'>Operating System</label>"
                             + " <select class=\"form-control\" id=\"operatingSystem_Device" + c + "\"><option Value='0'>Select</option><option Value='IOS'>IOS</option><option Value='Android'>Android</option></select>"
                         + " </div>"

                         + " <div class=\"field\" style='display:none;'>"
                            + "  <label>Manufacturer</label>"
                            + " <select class=\"form-control\" id=\"manufacturer_device" + c + "\"><option Value='0'>Select</option></select>"
                         + " </div>"

                         + " <div class=\"field\">"
                            + "  <label>Primary User Name</label>"
                             + " <input onblur=\"checkUserNameAvailability(this.value," + c + ")\" type=\"text\" id=\"primaryusername_device" + c + "\" class=\"form-control\">"
                             + " <label id=\"lblAlreadyExists\" style=\"display: none; color: #c0392b;\">primary user name already exist, please try a other name.</label>"
                         + " </div>"

                         + " <div class=\"field\">"
                            + "  <label>Usage</label>"
                            + " <select class=\"form-control\" id=\"usage_device" + c + "\"><option Value='0'>Select</option><option Value='Personal'>Personal</option><option Value='Business'>Business</option></select>"
                         + " </div>"

                          + " <div class=\"field\">"
                            + "  <label>Filter Settings</label>"
                            + " <select class=\"form-control\" id=\"settings_device" + c + "\"><option Value='0'>Select</option>";
            foreach (var p in pp)
            {
                computer = computer + "<option Value='" + p.Id + "'>" + p.ProductName + "</option>";
            }

            computer = computer + "</select>"
         + " </div>"
                         + " <div class=\"field\" style='background-color:#ececec;margin-top:10%;border-radius:10px;height:80px; clear:both;'>"
                            + " <label id=lblQuestion" + c + " style='margin-left:10px;'>Do you have another computer/device?</label><br/>"
                            + " <button style='margin-left:10px;' type=\"button\" onclick=\"setupYes(2," + c + ")\" id=\"setupYes" + c + "\" runat=\"server\" class=\"btn btn-darkBlue\"><i class=\"customIcon icon-yes\"></i>Yes</button>"
                            + " <button style='margin-left:10px;' type=\"button\" onclick=\"setupNo(2," + c + ")\" id=\"setupNo" + c + "\" runat=\"server\" class=\"btn btn-darkBlue\"><i class=\"customIcon icon-no\"></i>No</button>"

                            + " <button style='display:none; margin-left:10px;' type=\"button\" onclick=\"setupEdit(2," + c + ")\" id=\"setupEdit" + c + "\" runat=\"server\" class=\"btn btn-darkBlue\"><i class=\"customIcon icon-yes\"></i>Edit</button>"
                            + " <button style='display:none; margin-left:10px;' type=\"button\" onclick=\"setupUpdate(2," + c + ")\" id=\"setupUpdate" + c + "\" runat=\"server\" class=\"btn btn-darkBlue\"><i class=\"customIcon icon-yes\"></i>Update</button>"
                            + " <button style='display:none; margin-left:10px;' type=\"button\" onclick=\"setupDelete(2," + c + ")\" id=\"setupDelete" + c + "\" runat=\"server\" class=\"btn btn-darkBlue\"><i class=\"customIcon icon-no\"></i>Delete</button>"
                         + " </div>"
                     + " </div>"
                 + " </div>"
             + " </div>";

            dvDynamicSetup.InnerHtml = dvDynamicSetup.InnerHtml + computer;

            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "MyFun1", "clearpreviousbuttons(" + c + ");", true);
        }
    }
}