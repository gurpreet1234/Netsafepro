﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class netsEntities : DbContext
    {
        public netsEntities()
            : base("name=netsEntities")
        {
            this.Configuration.ProxyCreationEnabled = false;
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Action> Actions { get; set; }
        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<AllRules_W_ID> AllRules_W_ID { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryToRule> CategoryToRules { get; set; }
        public DbSet<CustomerGroup> CustomerGroups { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Display_Category> Display_Category { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupToRule_W_ID> GroupToRule_W_ID { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductToUserChange> ProductToUserChanges { get; set; }
        public DbSet<Rule_Types> Rule_Types { get; set; }
        public DbSet<torahbletlech_docs> torahbletlech_docs { get; set; }
        public DbSet<UniqDisplayName> UniqDisplayNames { get; set; }
        public DbSet<URLToDevice> URLToDevices { get; set; }
        public DbSet<URLToDeviceChange> URLToDeviceChanges { get; set; }
        public DbSet<UserBillingAddress> UserBillingAddresses { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserToOrg> UserToOrgs { get; set; }
        public DbSet<ProductToUser> ProductToUsers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ProductPrice> ProductPrices { get; set; }
    }
}
