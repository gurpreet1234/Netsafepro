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
    
    public partial class Display_Category
    {
        public int ID { get; set; }
        public int CategoryId { get; set; }
        public int Section { get; set; }
        public int DisplayId { get; set; }
        public bool Display { get; set; }
        public int RuleTypeId { get; set; }
    
        public virtual Rule_Types Rule_Types { get; set; }
        public virtual UniqDisplayName UniqDisplayName { get; set; }
    }
}
