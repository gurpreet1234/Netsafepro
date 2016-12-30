using SafenetproModel_new;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Data.Entity.Migrations;
using System.Text;
using System.Net.Mail;

namespace SafenetproAPI.Controllers
{
    public class productPrice
    {
        public string Monthly { get; set; }
        public string yearly { get; set; }
    }
    public class responseMessage
    {
        public string ststus { get; set; }
        public string message { get; set; }
    }
    public class ProductToUser_POCO
    {
        public int ID { get; set; }
        public int ProductId { get; set; }
        public string OperatingSystem { get; set; }
        public string Location { get; set; }
        public string PrimaryUserName { get; set; }
        public Nullable<int> Settings { get; set; }
        public string PhoneOS { get; set; }
        public string Manufacturer { get; set; }
        public string PrimaryUser { get; set; }
        public string Usage { get; set; }
        public List<URLtoDevice_POCO> uDevicePoco { get; set; }
    }
    public class ProductToUser_Price_POCO
    {
        public int Id { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> ProductId { get; set; }
        public string ProductName { get; set; }
        public string OperatingSystem { get; set; }
        public string Location { get; set; }
        public string PrimaryUserName { get; set; }
        public int? Settings { get; set; }
        public string PhoneOS { get; set; }
        public string Manufacturer { get; set; }
        public string PrimaryUser { get; set; }
        public string Usage { get; set; }
        public string ZscalerLogin { get; set; }
        public string MonthlyPrice { get; set; }
        public bool? Paid { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastPaymentDate { get; set; }
        public int? Change { get; set; }
        public string YearlyPrice { get; set; }
        public int? AdditionalLicenseCount { get; set; }
        public DateTime? StartDate { get; set; }
        public int? PaymentPeriod { get; set; }
    }
    public class URLtoDevice_POCO
    {
        public int ID { get; set; }
        public string URL { get; set; }
        public int ProductToUser { get; set; }
    }
    public class ProductToUserController : ApiController
    {
        [Route("~/api/Users/{UserId}/ProductToUsers")]
        public HttpResponseMessage PostProductToUser(int UserId, IEnumerable<ProductToUser_POCO> productToUser_POCO)
        {
            using (var spContext = new netsEntities())
            {
                try
                {
                    IEnumerable<ProductPrice> productPrices = GetProductPrices();
                    Dictionary<int?, int?> _lastProds = new Dictionary<int?, int?>();

                    var productToUser = spContext.ProductToUsers.Where(pu => pu.UserId == UserId).ToList();

                    //int? lastModerateComputerProdId = -1;
                    //int? lastBusinessComputerProdId = -1;
                    //int? lastHomeComputerProdId = -1;
                    foreach (var ptu in productToUser.OrderByDescending(a => a.Id))
                    {
                        if (_lastProds.Where(l => l.Key == ptu.Settings).FirstOrDefault().Value == null)
                        {
                            int? addCount = 0;
                            if (ptu.AddCount != null)
                                addCount = ptu.AddCount;
                            _lastProds.Add(ptu.Settings, addCount);
                        }
                    }
                    //if (productToUser != null && productToUser.Count > 0)
                    //{
                    //    if (productToUser.Where(m => m.Settings == 1).Count() > 0)
                    //        lastModerateComputerProdId = productToUser.Where(m => m.Settings == 1).LastOrDefault().AddCount;
                    //    if (productToUser.Where(m => m.Settings == 2).Count() > 0)
                    //        lastBusinessComputerProdId = productToUser.Where(m => m.Settings == 2).LastOrDefault().AddCount;
                    //    if (productToUser.Where(m => m.Settings == 3).Count() > 0)
                    //        lastHomeComputerProdId = productToUser.Where(m => m.Settings == 3).LastOrDefault().AddCount;
                    //}

                    //int countModerateComputerProdId = 0;
                    //int countBusinessComputerProdId = 0;
                    //int countHomeComputerProdId = 0;

                    foreach (var pu in productToUser_POCO)
                    {
                        ProductToUser ptu = new ProductToUser();
                        ptu.UserId = UserId;
                        ptu.OperatingSystem = pu.OperatingSystem;
                        ptu.Location = pu.Location;
                        ptu.PrimaryUserName = pu.PrimaryUserName;
                        ptu.Settings = pu.Settings;
                        ptu.PhoneOS = pu.PhoneOS;
                        ptu.Manufacturer = pu.Manufacturer;
                        ptu.PrimaryUser = pu.PrimaryUser;
                        ptu.Usage = pu.Usage;
                        ptu.Paid = false;
                        ptu.CreatedDate = DateTime.UtcNow;

                        //if (pu.ProductId == 1)
                        //{
                        int computerLicenseType = Convert.ToInt32(pu.Settings);
                        ProductPrice pPrice = productPrices.Where(pp => pp.ProductType == computerLicenseType).FirstOrDefault();

                        var dicValue = _lastProds.Where(p => p.Key == computerLicenseType).FirstOrDefault();
                        //if (computerLicenseType == 1)
                        //{
                        if (dicValue.Value == null)
                        {
                            ptu.MonthlyPrice = pPrice.MonthlyPrice;
                            ptu.YearlyPrice = pPrice.YearlyPrice;
                            ptu.AddCount = 0;
                            _lastProds.Add(computerLicenseType, 0);
                            //lastModerateComputerProdId = 0;
                        }
                        else if (dicValue.Value < pPrice.AdditionalLicenseCount)
                        {
                            ptu.MonthlyPrice = pPrice.AdditionalLicenseMonthlyPrice;
                            ptu.YearlyPrice = pPrice.AdditionalLicenseYearlyPrice;
                            ptu.AddCount = dicValue.Value + 1;
                            _lastProds[computerLicenseType] = dicValue.Value + 1;

                            //lastModerateComputerProdId = lastModerateComputerProdId + 1;
                        }
                        else
                        {
                            ptu.MonthlyPrice = pPrice.SecondMasterMonthlyPrice;
                            ptu.YearlyPrice = pPrice.SecondmasterYearlyPrice;
                            ptu.AddCount = 0;
                            _lastProds[computerLicenseType] = 0;
                            //lastModerateComputerProdId = 0;
                        }

                        //countModerateComputerProdId = countModerateComputerProdId + 1;
                        ptu.ProductId = pu.ProductId;// countModerateComputerProdId;
                        //}
                        //if (computerLicenseType == 2)
                        //{
                        //    if (lastBusinessComputerProdId == -1)
                        //    {
                        //        ptu.MonthlyPrice = pPrice.MonthlyPrice;
                        //        ptu.YearlyPrice = pPrice.YearlyPrice;
                        //        ptu.AddCount = 0;
                        //        lastBusinessComputerProdId = 0;
                        //    }
                        //    else if (lastBusinessComputerProdId < pPrice.AdditionalLicenseCount)
                        //    {
                        //        ptu.MonthlyPrice = pPrice.AdditionalLicenseMonthlyPrice;
                        //        ptu.YearlyPrice = pPrice.AdditionalLicenseYearlyPrice;
                        //        ptu.AddCount = lastBusinessComputerProdId + 1;
                        //        lastBusinessComputerProdId = lastBusinessComputerProdId + 1;
                        //    }
                        //    else
                        //    {
                        //        ptu.MonthlyPrice = pPrice.SecondMasterMonthlyPrice;
                        //        ptu.YearlyPrice = pPrice.SecondmasterYearlyPrice;
                        //        ptu.AddCount = 0;
                        //        lastBusinessComputerProdId = 0;
                        //    }
                        //    countBusinessComputerProdId = countBusinessComputerProdId + 1;
                        //    ptu.ProductId = countBusinessComputerProdId;
                        //}
                        //if (computerLicenseType == 3)
                        //{

                        //    if (lastHomeComputerProdId == -1)
                        //    {
                        //        ptu.MonthlyPrice = pPrice.MonthlyPrice;
                        //        ptu.YearlyPrice = pPrice.YearlyPrice;
                        //        ptu.AddCount = 0;
                        //        lastHomeComputerProdId = 0;
                        //    }
                        //    else if (lastHomeComputerProdId < pPrice.AdditionalLicenseCount)
                        //    {
                        //        ptu.MonthlyPrice = pPrice.AdditionalLicenseMonthlyPrice;
                        //        ptu.YearlyPrice = pPrice.AdditionalLicenseYearlyPrice;
                        //        ptu.AddCount = lastHomeComputerProdId + 1;
                        //        lastHomeComputerProdId = lastHomeComputerProdId + 1;
                        //    }
                        //    else
                        //    {
                        //        ptu.MonthlyPrice = pPrice.SecondMasterMonthlyPrice;
                        //        ptu.YearlyPrice = pPrice.SecondmasterYearlyPrice;
                        //        ptu.AddCount = 0;
                        //        lastHomeComputerProdId = 0;
                        //    }
                        //    countHomeComputerProdId = countHomeComputerProdId + 1;
                        //    ptu.ProductId = countHomeComputerProdId;
                        //}
                        //}
                        //else
                        //{
                        //    ProductPrice pPrice = productPrices.Where(pp => pp.ProductType == 4).FirstOrDefault();
                        //    ptu.MonthlyPrice = pPrice.MonthlyPrice;
                        //    ptu.YearlyPrice = pPrice.YearlyPrice;
                        //    ptu.ProductId = 4;
                        //}

                        //if (countModerateComputerProdId == 3)
                        //    countModerateComputerProdId = 0;
                        //if (countBusinessComputerProdId == 3)
                        //    countBusinessComputerProdId = 0;
                        //if (countHomeComputerProdId == 3)
                        //    countHomeComputerProdId = 0;

                        spContext.ProductToUsers.Add(ptu);

                        if (pu.uDevicePoco != null)
                        {
                            List<URLtoDevice_POCO> ProductURLS = pu.uDevicePoco;
                            foreach (var UD in ProductURLS)
                            {
                                if (UD.URL != "")
                                {
                                    URLToDevice UDP = new URLToDevice();
                                    UDP.ProductToUser1 = ptu;
                                    UDP.URL = UD.URL;
                                    spContext.URLToDevices.Add(UDP);
                                }
                            }
                        }
                    }
                    spContext.SaveChanges();
                }
                catch
                {
                    throw new HttpResponseException(HttpStatusCode.InternalServerError);
                }
                var response = this.Request.CreateResponse(HttpStatusCode.Created, productToUser_POCO);
                return response;
            }
        }

        public IEnumerable<ProductToUser_Price_POCO> GetProductLicenses(int userId, bool paidStatus)
        {
            using (var spContext = new netsEntities())
            {
                return (from pu in spContext.ProductToUsers
                        select new ProductToUser_Price_POCO
                        {
                            CreatedDate = pu.CreatedDate,
                            PaymentPeriod = pu.PaymentPeriod,
                            Id = pu.Id,
                            LastPaymentDate = pu.LastPaymentDate,
                            Location = pu.Location,
                            Manufacturer = pu.Manufacturer,
                            MonthlyPrice = pu.MonthlyPrice,
                            OperatingSystem = pu.OperatingSystem,
                            Paid = pu.Paid,
                            PhoneOS = pu.PhoneOS,
                            PrimaryUser = pu.PrimaryUser,
                            PrimaryUserName = pu.PrimaryUserName,
                            ProductId = pu.ProductId,
                            ProductName = (spContext.ProductPrices.Where(pp => pp.Id == pu.Settings).FirstOrDefault().ProductName),
                            Settings = pu.Settings,
                            Usage = pu.Usage,
                            UserId = pu.UserId,
                            ZscalerLogin = pu.ZscalerLogin,
                            Change = (spContext.ProductToUserChanges.Where(pr => pr.ProductUserId == pu.Id).FirstOrDefault().Change),
                            YearlyPrice = pu.YearlyPrice,
                            AdditionalLicenseCount = (spContext.ProductPrices.Where(pp => pp.Id == pu.Settings).FirstOrDefault().AdditionalLicenseCount)
                        }).Where(p => p.UserId == userId && p.Paid == paidStatus).OrderBy(o => o.Settings).ThenBy(o => o.Id).ToList();
            }
        }

        [Route("~/api/ProductToUser/{Id}")]
        public void DeleteProductToUser(int Id)
        {
            using (var spContext = new netsEntities())
            {
                var productToUser = spContext.ProductToUsers.FirstOrDefault(cond => cond.Id == Id);
                if (productToUser == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                IEnumerable<URLToDevice> urlToDevice = spContext.URLToDevices.Where(cond => cond.ProductToUser == Id).ToList();
                foreach (var d in urlToDevice)
                {
                    spContext.URLToDevices.Remove(d);
                }
                //spContext.URLToDevices.RemoveRange(urlToDevice);

                spContext.ProductToUsers.Remove(productToUser);
                spContext.SaveChanges();
            }
        }

        [Route("~/api/GetProductToUser/{Id}")]
        public ProductToUser_POCO GetProductToUser(int Id)
        {
            using (var spContext = new netsEntities())
            {
                var productToUser = spContext.ProductToUsers.FirstOrDefault(cond => cond.Id == Id);
                if (productToUser == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                IEnumerable<URLToDevice> urlToDevice = spContext.URLToDevices.Where(cond => cond.ProductToUser == Id).ToList();

                ProductToUser_POCO PUC = new ProductToUser_POCO();
                PUC.ID = productToUser.Id;
                PUC.Location = productToUser.Location;
                PUC.Manufacturer = productToUser.Manufacturer;
                PUC.OperatingSystem = productToUser.OperatingSystem;
                PUC.PhoneOS = productToUser.PhoneOS;
                PUC.PrimaryUser = productToUser.PrimaryUser;
                PUC.PrimaryUserName = productToUser.PrimaryUserName;
                PUC.ProductId = Convert.ToInt32(productToUser.ProductId);
                PUC.Settings = productToUser.Settings;
                PUC.Usage = productToUser.Usage;
                PUC.uDevicePoco = new List<URLtoDevice_POCO>();
                foreach (var ud in urlToDevice)
                {
                    URLtoDevice_POCO UTD = new URLtoDevice_POCO();
                    UTD.ID = ud.Id;
                    UTD.ProductToUser = Convert.ToInt32(ud.ProductToUser);
                    UTD.URL = ud.URL;
                    PUC.uDevicePoco.Add(UTD);
                }
                return PUC;
            }
        }

        [Route("~/api/ProductToUser/{Id}")]
        public HttpResponseMessage PutProductToUser(int Id, ProductToUser_POCO pu)
        {
            using (var spContext = new netsEntities())
            {
                try
                {
                    ProductToUser ptu = spContext.ProductToUsers.FirstOrDefault(cond => cond.Id == Id);
                    //ptu.UserId = UserId;
                    ptu.OperatingSystem = pu.OperatingSystem;
                    ptu.Location = pu.Location;
                    ptu.PrimaryUserName = pu.PrimaryUserName;
                    ptu.Settings = pu.Settings;
                    ptu.PhoneOS = pu.PhoneOS;
                    ptu.Manufacturer = pu.Manufacturer;
                    ptu.PrimaryUser = pu.PrimaryUser;
                    ptu.Usage = pu.Usage;
                    //ptu.ProductId = pu.ProductId;

                    spContext.ProductToUsers.AddOrUpdate(ptu);

                    IEnumerable<URLToDevice> urlToDevice = spContext.URLToDevices.Where(cond => cond.ProductToUser == Id).ToList();
                    foreach (var d in urlToDevice)
                    {
                        spContext.URLToDevices.Remove(d);
                    }

                    if (pu.uDevicePoco != null)
                    {
                        List<URLtoDevice_POCO> ProductURLS = pu.uDevicePoco;
                        foreach (var UD in ProductURLS)
                        {
                            if (UD.URL != "")
                            {
                                URLToDevice UDP = new URLToDevice();
                                UDP.ProductToUser1 = ptu;
                                UDP.URL = UD.URL;
                                spContext.URLToDevices.Add(UDP);
                            }
                        }
                    }
                    spContext.SaveChanges();
                }
                catch
                {
                    throw new HttpResponseException(HttpStatusCode.InternalServerError);
                }
                var response = this.Request.CreateResponse(HttpStatusCode.Created, pu);
                return response;
            }
        }


        [HttpGet]
        [Route("~/api/ProductToUser/{primaryuserName}/{userId}")]
        public HttpResponseMessage GetPrimaryNameAvailability(string primaryuserName, int userId)
        {
            using (var spContext = new netsEntities())
            {
                try
                {
                    if (spContext.ProductToUsers.Where(u => (u.PrimaryUserName == primaryuserName || u.PrimaryUser == primaryuserName) && u.UserId == userId).Count() > 0)
                    {

                        var response = this.Request.CreateResponse(HttpStatusCode.Found);
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


        public Dictionary<decimal, decimal> GetTotalProductMonthlyPrice(int userId)
        {
            using (var spContext = new netsEntities())
            {
                decimal productsInCartMonthly = (from pu in spContext.ProductToUsers
                                                 join p in spContext.Products on pu.ProductId equals p.Id
                                                 where pu.UserId == userId && pu.Paid == false
                                                 select pu.MonthlyPrice).AsEnumerable()
                                       .Sum(text =>
                                       {
                                           decimal result;
                                           decimal.TryParse(text, out result);
                                           return result;
                                       });

                decimal productsInCartYearly = (from pu in spContext.ProductToUsers
                                                join p in spContext.Products on pu.ProductId equals p.Id
                                                where pu.UserId == userId && pu.Paid == false
                                                select pu.YearlyPrice).AsEnumerable()
                                       .Sum(text =>
                                       {
                                           decimal result;
                                           decimal.TryParse(text, out result);
                                           return result;
                                       });

                var dc = new Dictionary<decimal, decimal>();
                dc.Add(productsInCartMonthly, productsInCartYearly);
                return dc;
            }
        }


        [HttpGet]
        [Route("~/api/UserProductsAmount/{userId}")]
        public HttpResponseMessage GetTotalProductPrice(int userId)
        {
            using (var spContext = new netsEntities())
            {
                decimal productsInCartMonthly = (from pu in spContext.ProductToUsers
                                                 join p in spContext.Products on pu.ProductId equals p.Id
                                                 where pu.UserId == userId && pu.Paid == false
                                                 select pu.MonthlyPrice).AsEnumerable()
                                       .Sum(text =>
                                       {
                                           decimal result;
                                           decimal.TryParse(text, out result);
                                           return result;
                                       });

                decimal productsInCartYearly = (from pu in spContext.ProductToUsers
                                                join p in spContext.Products on pu.ProductId equals p.Id
                                                where pu.UserId == userId && pu.Paid == false
                                                select pu.YearlyPrice).AsEnumerable()
                                       .Sum(text =>
                                       {
                                           decimal result;
                                           decimal.TryParse(text, out result);
                                           return result;
                                       });


                productPrice _productPrice = new productPrice();
                _productPrice.Monthly = Convert.ToString(productsInCartMonthly);
                _productPrice.yearly = Convert.ToString(productsInCartYearly);

                var response = this.Request.CreateResponse(HttpStatusCode.OK, _productPrice);
                return response;
                //return dc;
            }
        }

        public void UpdateLicenseStartDate(int productUserId, DateTime licenseStartDate)
        {
            using (var spContext = new netsEntities())
            {
                try
                {
                    ProductToUser ptu = spContext.ProductToUsers.FirstOrDefault(cond => cond.Id == productUserId);
                    ptu.LicenseStartDate = licenseStartDate;
                    spContext.ProductToUsers.AddOrUpdate(ptu);
                    spContext.SaveChanges();
                }
                catch
                {
                    throw new HttpResponseException(HttpStatusCode.InternalServerError);
                }
            }
        }

        public void UpdateProductToUser(int userId, int paymentPeriod, DateTime licenseStartDate, string amount)
        {
            using (var spContext = new netsEntities())
            {
                try
                {
                    List<ProductToUser> productToUser_POCO = spContext.ProductToUsers.Where(p => p.UserId == userId && p.Paid == false).ToList();
                    foreach (var pu in productToUser_POCO)
                    {
                        ProductToUser ptu = spContext.ProductToUsers.FirstOrDefault(cond => cond.Id == pu.Id);
                        ptu.Paid = true;
                        ptu.LastPaymentDate = DateTime.UtcNow;
                        ptu.PaymentPeriod = paymentPeriod;
                        if (ptu.LicenseStartDate == null)
                            ptu.LicenseStartDate = licenseStartDate;
                        spContext.ProductToUsers.AddOrUpdate(ptu);
                    }
                    spContext.SaveChanges();

                    sendPaymentEmail(userId, amount);
                }
                catch
                {
                    throw new HttpResponseException(HttpStatusCode.InternalServerError);
                }
            }
        }

        public void sendPaymentEmail(int userId, string amount)
        {
            using (var spContext = new netsEntities())
            {
                try
                {
                    UserProfile _userProfile = spContext.UserProfiles.Where(p => p.UserId == userId).FirstOrDefault();
                    if (_userProfile != null)
                    {
                        const string SERVER = "relay-hosting.secureserver.net";
                        MailMessage oMail = new MailMessage();
                        oMail.From = new MailAddress("support@netsafepro.com");
                        oMail.To.Add("sales@netsafepro.com");
                        oMail.Bcc.Add("gurpreet.khurana89@gmail.com");
                        oMail.Subject = "Payment Recieved";
                        oMail.IsBodyHtml = true;
                        oMail.Priority = MailPriority.High; // enumeration
                        oMail.Body = "<br><br>$" + amount + " recieved from Safenetpro customer.<br /><br />Email:" + _userProfile.EmailAddress + "<br /><br />Name:" + _userProfile.FirstName + " " + _userProfile.LastName + " <br /><br />Best Regards<br>Support Team(Safenetpro)";
                        SmtpClient smtp = new SmtpClient(SERVER);
                        smtp.Send(oMail);
                        oMail = null; // free up resources
                    }
                }
                catch
                {
                }
            }
        }

        [Route("~/api/ProductToUserChange/{Id}/{UserId}")]
        public HttpResponseMessage PutProductToUserChange(int Id, int UserId, ProductToUser_POCO pu)
        {
            using (var spContext = new netsEntities())
            {
                try
                {
                    ProductToUserChange ptud = spContext.ProductToUserChanges.Where(p => p.ProductUserId == Id).FirstOrDefault();
                    if (ptud != null)
                        spContext.ProductToUserChanges.Remove(ptud);

                    ProductToUserChange ptu = new ProductToUserChange();
                    ptu.UserId = UserId;
                    ptu.ProductUserId = pu.ID;
                    ptu.OperatingSystem = pu.OperatingSystem;
                    ptu.Location = pu.Location;
                    ptu.PrimaryUserName = pu.PrimaryUserName;
                    ptu.Settings = pu.Settings;
                    ptu.PhoneOS = pu.PhoneOS;
                    ptu.Manufacturer = pu.Manufacturer;
                    ptu.PrimaryUser = pu.PrimaryUser;
                    ptu.Usage = pu.Usage;
                    ptu.Change = 1;
                    ptu.ProductId = pu.ProductId;
                    ptu.CreatedDate = DateTime.UtcNow;

                    spContext.ProductToUserChanges.Add(ptu);

                    IEnumerable<URLToDeviceChange> urlToDevice = spContext.URLToDeviceChanges.Where(cond => cond.ProductToUserChange == Id).ToList();
                    foreach (var d in urlToDevice)
                    {
                        spContext.URLToDeviceChanges.Remove(d);
                    }
                    //spContext.URLToDeviceChanges.RemoveRange(urlToDevice);

                    if (pu.uDevicePoco != null)
                    {
                        List<URLtoDevice_POCO> ProductURLS = pu.uDevicePoco;
                        foreach (var UD in ProductURLS)
                        {
                            if (UD.URL != "")
                            {
                                URLToDeviceChange UDP = new URLToDeviceChange();
                                UDP.ProductToUserChange = Id;
                                UDP.URL = UD.URL;
                                spContext.URLToDeviceChanges.Add(UDP);
                            }
                        }
                    }
                    spContext.SaveChanges();
                }
                catch
                {
                    throw new HttpResponseException(HttpStatusCode.InternalServerError);
                }
                var response = this.Request.CreateResponse(HttpStatusCode.Created, pu);
                return response;
            }
        }

        [Route("~/api/ProductToUserChange/{Id}/{UserId}")]
        public void DeleteProductToUserChange(int Id, int UserId)
        {
            using (var spContext = new netsEntities())
            {
                try
                {
                    var productToUser = spContext.ProductToUsers.FirstOrDefault(cond => cond.Id == Id);
                    if (productToUser == null)
                    {
                        throw new HttpResponseException(HttpStatusCode.NotFound);
                    }

                    ProductToUserChange ptu = new ProductToUserChange();
                    ptu.UserId = UserId;
                    ptu.ProductUserId = Id;
                    ptu.OperatingSystem = productToUser.OperatingSystem;
                    ptu.Location = productToUser.Location;
                    ptu.PrimaryUserName = productToUser.PrimaryUserName;
                    ptu.Settings = productToUser.Settings;
                    ptu.PhoneOS = productToUser.PhoneOS;
                    ptu.Manufacturer = productToUser.Manufacturer;
                    ptu.PrimaryUser = productToUser.PrimaryUser;
                    ptu.Usage = productToUser.Usage;
                    ptu.Change = 2;
                    ptu.ProductId = productToUser.ProductId;
                    ptu.CreatedDate = DateTime.UtcNow;
                    spContext.ProductToUserChanges.Add(ptu);

                    IEnumerable<URLToDevice> urlToDevice = spContext.URLToDevices.Where(cond => cond.ProductToUser == Id).ToList();
                    foreach (var UD in urlToDevice)
                    {
                        if (UD.URL != "")
                        {
                            URLToDeviceChange UDP = new URLToDeviceChange();
                            UDP.ProductToUserChange = UD.ProductToUser;
                            UDP.URL = UD.URL;
                            spContext.URLToDeviceChanges.Add(UDP);
                        }
                    }

                    spContext.SaveChanges();
                }
                catch
                {
                    throw new HttpResponseException(HttpStatusCode.InternalServerError);
                }
            }
        }

        public IEnumerable<ProductPrice> GetProductPrices()
        {
            using (var spContext = new netsEntities())
            {
                return (from pu in spContext.ProductPrices select pu).ToList();
            }
        }
    }
}