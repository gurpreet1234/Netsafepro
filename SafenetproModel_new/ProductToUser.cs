//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SafenetproModel_new
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProductToUser
    {
        public ProductToUser()
        {
            this.URLToDevices = new HashSet<URLToDevice>();
            this.URLToDevices1 = new HashSet<URLToDevice>();
        }
    
        public int Id { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> ProductId { get; set; }
        public string OperatingSystem { get; set; }
        public string Location { get; set; }
        public string PrimaryUserName { get; set; }
        public Nullable<int> Settings { get; set; }
        public string PhoneOS { get; set; }
        public string Manufacturer { get; set; }
        public string PrimaryUser { get; set; }
        public string Usage { get; set; }
        public string ZscalerLogin { get; set; }
        public Nullable<bool> Paid { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> LastPaymentDate { get; set; }
        public string MonthlyPrice { get; set; }
        public string YearlyPrice { get; set; }
        public Nullable<int> AddCount { get; set; }
        public Nullable<int> PaymentPeriod { get; set; }
        public Nullable<System.DateTime> LicenseStartDate { get; set; }
    
        public virtual ICollection<URLToDevice> URLToDevices { get; set; }
        public virtual ICollection<URLToDevice> URLToDevices1 { get; set; }
        public virtual User User { get; set; }
    }
}
