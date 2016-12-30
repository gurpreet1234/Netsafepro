using SafenetproModel_new;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Data.Entity.Migrations;
using System.Net.Mail;

namespace SafenetproAPI.Controllers
{
    public class Customer_Poco
    {
        public string EmailAddress { get; set; }
    }
    public class UserProfile_Poco
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string CellPhone { get; set; }
        public string Fax { get; set; }
        public string EmailAddress { get; set; }
        public string BName { get; set; }
        public string BAddress { get; set; }
        public string BCity { get; set; }
        public string BState { get; set; }
        public string BZip { get; set; }
        public string BPhone { get; set; }
        public string BCellPhone { get; set; }
        public string BFax { get; set; }
        public string BEmailAddress { get; set; }
        public Nullable<int> ProfileType { get; set; }
        public Nullable<int> CardDigits { get; set; }
        public int OrgId { get; set; }
        public bool? BusinessInfo { get; set; }
        public string OtherOrg { get; set; }
    }
    public class UsersController : ApiController
    {
        public UserProfile PostUsers(User u, UserProfile up, UserToOrg uo)
        {
            using (var spContext = new netsEntities())
            {
                spContext.Users.Add(u);

                up.UserId = u.Id;
                spContext.UserProfiles.Add(up);

                uo.UserId = u.Id;
                spContext.UserToOrgs.Add(uo);
                spContext.SaveChanges();
                try { sendEmail(up.EmailAddress, up.FirstName, up.LastName); }
                catch { }
                return up;
            }
        }

        public void sendEmail(string email, string firstName, string lastName)
        {
            const string SERVER = "relay-hosting.secureserver.net";
            MailMessage oMail = new MailMessage();
            oMail.From = new MailAddress("support@netsafepro.com");
            oMail.To.Add("sales@netsafepro.com");
            oMail.Bcc.Add("gurpreet.khurana89@gmail.com");
            oMail.Subject = "New Customer";
            oMail.IsBodyHtml = true;
            oMail.Priority = MailPriority.High; // enumeration
            oMail.Body = "<br><br>New customer registered in Safenetpro.<br /><br />Email:" + email + "<br /><br />Name:" + firstName + " " + lastName + " <br /><br />Best Regards<br>Support Team(Safenetpro)";
            SmtpClient smtp = new SmtpClient(SERVER);
            smtp.Send(oMail);
            oMail = null; // free up resources
        }

        public UserProfile PutUsers(User u, UserProfile up, UserToOrg uo, int userId)
        {
            using (var spContext = new netsEntities())
            {
                User _user = spContext.Users.Where(us => us.Id == userId).FirstOrDefault();
                _user.UserName = u.UserName;
                _user.Password = u.Password;
                spContext.Users.AddOrUpdate(_user);

                UserProfile _userProfile = spContext.UserProfiles.Where(us => us.UserId == userId).FirstOrDefault();
                _userProfile.UserId = userId;
                _userProfile.FirstName = up.FirstName;
                _userProfile.LastName = up.LastName;
                _userProfile.Address = up.Address;
                _userProfile.Address2 = up.Address2;
                _userProfile.City = up.City;
                _userProfile.State = up.State;
                _userProfile.Zip = up.Zip;
                _userProfile.Phone = up.Phone;
                _userProfile.CellPhone = up.CellPhone;
                _userProfile.Fax = up.Fax;
                _userProfile.EmailAddress = up.EmailAddress;
                _userProfile.BName = up.BName;
                _userProfile.BAddress = up.BAddress;
                _userProfile.BCity = up.BCity;
                _userProfile.BState = up.BState;
                _userProfile.BZip = up.BZip;
                _userProfile.BPhone = up.BPhone;
                _userProfile.BCellPhone = up.BCellPhone;
                _userProfile.BFax = up.BFax;
                _userProfile.BEmailAddress = up.BEmailAddress;
                spContext.UserProfiles.AddOrUpdate(_userProfile);

                UserToOrg _userToOrg = spContext.UserToOrgs.Where(us => us.UserId == userId).FirstOrDefault();
                _userToOrg.OrgId = uo.OrgId;
                _userToOrg.OrgOther = uo.OrgOther;
                spContext.UserToOrgs.AddOrUpdate(_userToOrg);
                spContext.SaveChanges();
                return _userProfile;
            }
        }

        public UserProfile AuthenticateUser(string userName, string password)
        {
            try
            {
                using (var spContext = new netsEntities())
                {
                    return (from u in spContext.Users
                            join up in spContext.UserProfiles on u.Id equals up.UserId
                            where u.UserName == userName && u.Password == password
                            select up).OrderByDescending(p => p.UserId).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                using (var spContext = new netsEntities())
                {
                    return (from u in spContext.Users
                            join up in spContext.UserProfiles on u.Id equals up.UserId
                            where u.UserName == userName && u.Password == password
                            select up).OrderByDescending(p => p.UserId).FirstOrDefault();
                }
            }
        }

        public UserProfile GetUserProfile(int userId)
        {
            using (var spContext = new netsEntities())
            {
                return spContext.UserProfiles.Where(u => u.UserId == userId).FirstOrDefault();
            }
        }

        public UserBillingAddress GetUserBillingInformation(int userId)
        {
            using (var spContext = new netsEntities())
            {
                return spContext.UserBillingAddresses.Where(u => u.CustomerId == userId).FirstOrDefault();
            }
        }

        public UserProfile_Poco GetProfile(int userId)
        {
            using (var spContext = new netsEntities())
            {
                return (from up in spContext.UserProfiles
                        join uo in spContext.UserToOrgs
                                  on up.UserId equals uo.UserId
                        select new UserProfile_Poco
                        {
                            Address = up.Address,
                            Address2 = up.Address2,
                            BAddress = up.BAddress,
                            BCellPhone = up.BCellPhone,
                            BCity = up.BCity,
                            BEmailAddress = up.BEmailAddress,
                            BFax = up.BFax,
                            BName = up.BName,
                            BPhone = up.BPhone,
                            BState = up.BState,
                            BZip = up.BZip,
                            CardDigits = up.CardDigits,
                            CellPhone = up.CellPhone,
                            City = up.City,
                            EmailAddress = up.EmailAddress,
                            Fax = up.Fax,
                            FirstName = up.FirstName,
                            Id = up.Id,
                            LastName = up.LastName,
                            OrgId = uo.OrgId,
                            Phone = up.Phone,
                            ProfileType = up.ProfileType,
                            State = up.State,
                            UserId = up.UserId,
                            Zip = up.Zip,
                            BusinessInfo = up.BusinessInfo,
                            OtherOrg = uo.OrgOther
                        }).Where(p => p.UserId == userId).FirstOrDefault();
            }
        }

        [Route("~/api/User/{userName}/{userId}")]
        public HttpResponseMessage GetUserAvailability(string userName, int? userId)
        {
            using (var spContext = new netsEntities())
            {
                try
                {
                    responseMessage _responseMessage = new responseMessage();
                    if (userId != null)
                    {
                        if (spContext.Users.Where(u => u.UserName == userName && u.Id != userId).Count() > 0)
                        {
                            var response = new HttpResponseMessage(HttpStatusCode.Found);
                            return response;
                        }
                        else
                        {
                            var response = new HttpResponseMessage(HttpStatusCode.NotFound);
                            return response;
                        }
                    }
                    else
                    {
                        if (spContext.Users.Where(u => u.UserName == userName).Count() > 0)
                        {
                            var response = new HttpResponseMessage(HttpStatusCode.Found);
                            return response;
                        }
                        else
                        {
                            var response = new HttpResponseMessage(HttpStatusCode.NotFound);
                            return response;
                        }
                    }
                    //else
                    //{
                    //    var response = new HttpResponseMessage(HttpStatusCode.NotFound);
                    //    return response;
                    //}
                }
                catch
                {
                    throw new HttpResponseException(HttpStatusCode.InternalServerError);
                }
            }
        }

        [Route("~/api/ForgotPassword")]
        [HttpPost]
        public HttpResponseMessage PostUserAvailabilityForgotPassword(Customer_Poco _customer)
        {
            using (var spContext = new netsEntities())
            {
                try
                {
                    var _userProfile = spContext.UserProfiles.Where(u => u.EmailAddress == _customer.EmailAddress);
                    if (_userProfile.Count() > 0)
                    {
                        var _user = spContext.Users.Where(u => u.Id == _userProfile.FirstOrDefault().UserId).FirstOrDefault();
                        _user.PasswordReset = Convert.ToString(Guid.NewGuid());
                        spContext.SaveChanges();

                        var response = this.Request.CreateResponse(HttpStatusCode.Found);

                        const string SERVER = "relay-hosting.secureserver.net";
                        MailMessage oMail = new MailMessage();
                        oMail.From = new MailAddress("help@netsafepro.com");
                        oMail.To.Add(_customer.EmailAddress);
                        oMail.Subject = "Reset Password -- Netsafepro";
                        oMail.IsBodyHtml = true;
                        oMail.Priority = MailPriority.High; // enumeration
                        oMail.Body = "Please click on following link to reset your password<br />   https://www.netsafepro.com/ResetPassword.aspx?Token=" + _user.PasswordReset;
                        SmtpClient smtp = new SmtpClient(SERVER);
                        smtp.Send(oMail);
                        oMail = null; // free up resources

                        return response;
                    }
                    else
                    {
                        var response = this.Request.CreateResponse(HttpStatusCode.NotFound);
                        return response;
                    }
                }
                catch
                {
                    throw new HttpResponseException(HttpStatusCode.InternalServerError);
                }
            }
        }

        public AdminUser AuthenticateAdminUser(string userName, string password)
        {
            using (var spContext = new netsEntities())
            {
                return spContext.AdminUsers.Where(u => u.Email == userName && u.Password == password).FirstOrDefault();
            }
        }

        public AdminUser GetAdminUserProfile(int userId)
        {
            using (var spContext = new netsEntities())
            {
                return spContext.AdminUsers.Where(u => u.Id == userId).FirstOrDefault();
            }
        }

        public AdminUser PutAdminUserProfile(AdminUser au, int adminId)
        {
            using (var spContext = new netsEntities())
            {
                AdminUser adminUser = spContext.AdminUsers.Where(u => u.Id == adminId).FirstOrDefault();
                adminUser.Email = au.Email;
                adminUser.FirstName = au.FirstName;
                adminUser.LastName = au.LastName;
                adminUser.Password = au.Password;

                spContext.AdminUsers.AddOrUpdate(au);
                spContext.SaveChanges();
                return au;
            }
        }

        public void PutUserBillingInformation(UserBillingAddress ub)
        {
            using (var spContext = new netsEntities())
            {
                UserBillingAddress _UserBillingAddress = spContext.UserBillingAddresses.Where(u => u.CustomerId == ub.CustomerId).FirstOrDefault();
                if (_UserBillingAddress != null)
                {
                    _UserBillingAddress.Name = ub.Name;
                    _UserBillingAddress.Address = ub.Address;
                    _UserBillingAddress.City = ub.City;
                    _UserBillingAddress.State = ub.State;
                    _UserBillingAddress.ZipCode = ub.ZipCode;
                    _UserBillingAddress.PhoneNumber = ub.PhoneNumber;
                    _UserBillingAddress.EmailAddress = ub.EmailAddress;

                    spContext.UserBillingAddresses.AddOrUpdate(_UserBillingAddress);
                }
                else
                {
                    _UserBillingAddress = new UserBillingAddress();
                    _UserBillingAddress.CustomerId = ub.CustomerId;
                    _UserBillingAddress.Name = ub.Name;
                    _UserBillingAddress.Address = ub.Address;
                    _UserBillingAddress.City = ub.City;
                    _UserBillingAddress.State = ub.State;
                    _UserBillingAddress.ZipCode = ub.ZipCode;
                    _UserBillingAddress.PhoneNumber = ub.PhoneNumber;
                    _UserBillingAddress.EmailAddress = ub.EmailAddress;

                    spContext.UserBillingAddresses.Add(_UserBillingAddress);

                }
                spContext.SaveChanges();
            }
        }

        public User GetUserByToken(string token)
        {
            using (var spContext = new netsEntities())
            {
                return spContext.Users.Where(u => u.PasswordReset == token).FirstOrDefault();
            }
        }
        public void UpdateUserPassword(string password, int userId)
        {
            using (var spContext = new netsEntities())
            {
                var _user = spContext.Users.Where(u => u.Id == userId).FirstOrDefault();
                _user.PasswordReset = null;
                _user.Password = password;
                spContext.SaveChanges();
            }
        }
    }
}
