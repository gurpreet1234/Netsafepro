using SafenetproModel_new;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Data.Entity.Migrations;
using Newtonsoft.Json;

namespace SafenetproAPI.Controllers
{
    public class AdminController : ApiController
    {
        [Route("~/api/AdminCustomersList")]
        public IEnumerable<UserProfile_Poco> GetCustomers()
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
                            BusinessInfo = up.BusinessInfo
                        }).ToList();
            }
        }

        [Route("~/api/Customer/{customerId}/AdminCustomersList")]
        public IEnumerable<ProductToUser_Price_POCO> GetProductLicenses(int customerId)
        {
            using (var spContext = new netsEntities())
            {
                return (from pu in spContext.ProductToUsers
                        join p in spContext.Products on pu.ProductId equals p.Id
                        select new ProductToUser_Price_POCO
                        {
                            CreatedDate = pu.CreatedDate,
                            PaymentPeriod=pu.PaymentPeriod,
                            Id = pu.Id,
                            LastPaymentDate = pu.LastPaymentDate,
                            Location = pu.Location,
                            Manufacturer = pu.Manufacturer,
                            MonthlyPrice = p.MonthlyPrice,
                            OperatingSystem = pu.OperatingSystem,
                            Paid = pu.Paid,
                            PhoneOS = pu.PhoneOS,
                            PrimaryUser = pu.PrimaryUser,
                            PrimaryUserName = pu.PrimaryUserName,
                            ProductId = pu.ProductId,
                            Settings = pu.Settings,
                            Usage = pu.Usage,
                            UserId = pu.UserId,
                            ZscalerLogin = pu.ZscalerLogin,
                            Change = (spContext.ProductToUserChanges.Where(pr => pr.ProductUserId == pu.Id).FirstOrDefault().Change),
                            StartDate = pu.LicenseStartDate
                        }).Where(p => p.UserId == customerId).OrderByDescending(o => o.Settings).ToList();
            }
        }

        [Route("~/api/Products")]
        public IEnumerable<Product> GetProducts()
        {
            using (var spContext = new netsEntities())
            {
                return spContext.Products.ToList();
            }
        }

        [Route("~/api/Product/{Id}")]
        public Product GetProducts(int Id)
        {
            using (var spContext = new netsEntities())
            {
                return spContext.Products.Where(p => p.Id == Id).FirstOrDefault();
            }
        }

        [HttpPost]
        [Route("~/api/updateProduct/{productId}")]
        public void PutProduct(int productId, Product p)
        {
            using (var spContext = new netsEntities())
            {
                try
                {
                    Product product = spContext.Products.Where(pr => pr.Id == productId).FirstOrDefault();
                    product.ProductName = p.ProductName;
                    product.Description = p.Description;
                    product.MonthlyPrice = p.MonthlyPrice;
                    spContext.Products.AddOrUpdate(product);
                    spContext.SaveChanges();
                }
                catch
                {
                    throw new HttpResponseException(HttpStatusCode.InternalServerError);
                }
            }
        }

        [HttpPost]
        [Route("~/api/updateCustomer/{CustomerId}")]
        public void UpdateCustomer(int CustomerId, CustomerVM cv)
        {
            using (var spContext = new netsEntities())
            {
                try
                {
                    var user = spContext.Users.Where(u => u.Id == CustomerId).FirstOrDefault();
                    if (user != null)
                    {
                        user.UserName = cv.EmailAddress;
                        user.Active = true;
                        if (cv.Password != null && cv.Password != "")
                        {
                            if (cv.Password.Trim() != "")
                            {
                                user.Password = cv.Password;
                            }
                        }
                        spContext.Users.AddOrUpdate(user);
                        spContext.SaveChanges();
                    }

                    var userprofile = spContext.UserProfiles.Where(u => u.UserId == CustomerId).FirstOrDefault();
                    if (userprofile != null)
                    {
                        userprofile.Phone = cv.CellPhone;
                        userprofile.FirstName = cv.FirstName;
                        userprofile.LastName = cv.LastName;
                        userprofile.EmailAddress = cv.EmailAddress;
                        userprofile.Zip = cv.Zip;
                        spContext.UserProfiles.AddOrUpdate(userprofile);
                        spContext.SaveChanges();
                    }
                }
                catch
                {
                    throw new HttpResponseException(HttpStatusCode.InternalServerError);
                }
            }
        }


        //[Route("~/api/Product/{productId}")]
        //public void PutProduct(int productId, Product p)
        //{
        //    using (var spContext = new netsEntities())
        //    {
        //        try
        //        {
        //            Product product = spContext.Products.Where(pr => pr.Id == productId).FirstOrDefault();
        //            product.ProductName = p.ProductName;
        //            product.Description = p.Description;
        //            product.MonthlyPrice = p.MonthlyPrice;
        //            spContext.Products.AddOrUpdate(product);
        //            spContext.SaveChanges();
        //        }
        //        catch
        //        {
        //            throw new HttpResponseException(HttpStatusCode.InternalServerError);
        //        }
        //    }
        //}


        [HttpPost]
        [Route("~/api/Addcustomers")]
        public string Addcustomers(CustomerVM CustomerVM)
        {
            using (var spContext = new netsEntities())
            {
                try
                {
                    var user = new User();
                    user.UserName = CustomerVM.EmailAddress;
                    user.Password = CustomerVM.Password;
                    user.Active = true;
                    spContext.Users.AddOrUpdate(user);
                    spContext.SaveChanges();

                    var userId = user.Id;
                    var usertoorg = new UserToOrg();
                    usertoorg.OrgId = 1;
                    usertoorg.UserId = userId;
                    spContext.UserToOrgs.AddOrUpdate(usertoorg);
                    spContext.SaveChanges();

                    var userprofile = new UserProfile();
                    userprofile.UserId = userId;
                    userprofile.Phone = CustomerVM.CellPhone;
                    userprofile.FirstName = CustomerVM.FirstName;
                    userprofile.LastName = CustomerVM.LastName;
                    userprofile.EmailAddress = CustomerVM.EmailAddress;
                    userprofile.Zip = CustomerVM.Zip;
                    spContext.UserProfiles.AddOrUpdate(userprofile);
                    spContext.SaveChanges();
                    return "Customer Added successfully";
                }
                catch (Exception e)
                {
                    return e.InnerException.Message;
                }
            }
        }


        // Add Products
        [HttpPost]
        [Route("~/api/AddProduct")]
        public string AddProducts(Product productVM)
        {
            using (var spContext = new netsEntities())
            {
                try
                {
                    var product = new Product();
                    product.ProductName = productVM.ProductName;
                    product.Description = productVM.Description;
                    product.MonthlyPrice = productVM.MonthlyPrice;
                    spContext.Products.AddOrUpdate(product);
                    spContext.SaveChanges();

                    return "Product Added successfully";
                }
                catch (Exception e)
                {
                    return e.InnerException.Message;
                }
            }
        }

        // Add Products
        [HttpPost]
        [Route("~/api/Product/{UserId}/SingleCustomerProductAdd")]
        public string SingleCustomerProductAdd(int UserId, ProductToUser_POCO pu)
        {
            using (var spContext = new netsEntities())
            {
                try
                {

                    IEnumerable<ProductPrice> productPrices = GetProductPrices();
                    Dictionary<int?, int?> _lastProds = new Dictionary<int?, int?>();

                    var productToUser = spContext.ProductToUsers.Where(a => a.UserId == UserId).ToList();

                    //int? lastModerateComputerProdId = -1;
                    //int? lastBusinessComputerProdId = -1;
                    //int? lastHomeComputerProdId = -1;
                    foreach (var item in productToUser.OrderByDescending(a => a.Id))
                    {
                        if (_lastProds.Where(l => l.Key == item.Settings).FirstOrDefault().Value == null)
                        {
                            int? addCount = 0;
                            if (item.AddCount != null)
                                addCount = item.AddCount;
                            _lastProds.Add(item.Settings, addCount);
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
                    spContext.SaveChanges();
                    return "Customer Product Added Successfully";
                }
                catch (Exception e)
                {
                    return e.InnerException.Message;
                }
            }
        }

        public class CustomerVM
        {
            public int Id { get; set; }
            public int UserId { get; set; }
            public string UserName { get; set; }
            public string MonthlyPrice { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string EmailAddress { get; set; }
            public string CellPhone { get; set; }
            public string Zip { get; set; }
            public string Password { get; set; }
            public bool Active { get; set; }

            public string YearlyPrice { get; set; }
            public int? PaymentPeriod { get; set; }
            public string LastPaymentDate { get; set; }

        }

        //Get Customer by customer Id
        [HttpGet]
        [Route("~/api/getCustomerById/{customerId}")]
        public CustomerVM getCustomerById(int customerId)
        {
            using (var spContext = new netsEntities())
            {
                var customerVM = new CustomerVM();
                var data = spContext.UserProfiles.Where(m => m.UserId == customerId).FirstOrDefault();
                customerVM.UserId = data.UserId;
                customerVM.CellPhone = data.Phone;
                customerVM.FirstName = data.FirstName;
                customerVM.LastName = data.LastName;
                customerVM.EmailAddress = data.EmailAddress;
                customerVM.Zip = data.Zip;
                return customerVM;
            }
        }

        [HttpGet]
        [Route("~/api/getCustomerhistoryById/{customerId}")]
        public CustomerVM getCustomerhistoryById(int customerId)
        {
            CustomerVM customerVM = new CustomerVM();
            using (var spContext = new netsEntities())
            {
                var data = spContext.ProductToUsers.Where(m => m.Id == customerId).FirstOrDefault();
                if (data != null)
                {
                    customerVM.Id = data.Id;
                    customerVM.UserName = data.PrimaryUserName;
                    customerVM.MonthlyPrice = data.MonthlyPrice;
                    customerVM.YearlyPrice = data.YearlyPrice;
                    customerVM.LastPaymentDate = data.LastPaymentDate.ToString();
                    customerVM.PaymentPeriod = data.PaymentPeriod;
                    return customerVM;
                }
            }
            return customerVM;
        }

        [HttpPost]

        [Route("~/api/updateHistory/{CustomerId}")]
        public void updateHistory(int CustomerId, CustomerVM cv)
        {
            using (var spContext = new netsEntities())
            {
                var data = spContext.ProductToUsers.Where(m => m.Id == CustomerId).FirstOrDefault();
                if (data != null)
                {
                    data.PrimaryUserName = cv.UserName;
                    data.MonthlyPrice = cv.MonthlyPrice;
                    data.YearlyPrice = cv.YearlyPrice;
                    data.LastPaymentDate = Convert.ToDateTime(cv.LastPaymentDate);
                    data.PaymentPeriod = data.PaymentPeriod;
                    spContext.ProductToUsers.AddOrUpdate(data);
                    spContext.SaveChanges();
                }
            }
        }
        [Route("~/api/DeleteCustomer/{Id}")]
        public void GetDeleteCustomer(int Id)
        {
            using (var spContext = new netsEntities())
            {
                IEnumerable<ProductToUser> productToUser = spContext.ProductToUsers.Where(cond => cond.UserId == Id).ToList();
                foreach (var p in productToUser)
                {
                    IEnumerable<URLToDevice> urlToDevice = spContext.URLToDevices.Where(cond => cond.ProductToUser == p.Id).ToList();
                    foreach (var d in urlToDevice)
                    {
                        spContext.URLToDevices.Remove(d);
                    }
                    spContext.ProductToUsers.Remove(p);
                }
                var UserBillingAddress = spContext.UserBillingAddresses.FirstOrDefault(cond => cond.CustomerId == Id);
                if (UserBillingAddress != null)
                    spContext.UserBillingAddresses.Remove(UserBillingAddress);

                var UserProfile = spContext.UserProfiles.FirstOrDefault(cond => cond.UserId == Id);
                if (UserProfile != null)
                    spContext.UserProfiles.Remove(UserProfile);

                var UserToOrg = spContext.UserToOrgs.FirstOrDefault(cond => cond.UserId == Id);
                if (UserToOrg != null)
                    spContext.UserToOrgs.Remove(UserToOrg);

                var Users = spContext.Users.FirstOrDefault(cond => cond.Id == Id);
                if (Users != null)
                    spContext.Users.Remove(Users);

                spContext.SaveChanges();
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
