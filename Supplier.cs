//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace e_auction.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Supplier
    {
        public int id { get; set; }
        public Nullable<int> Tenderid { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> SupplierStatus { get; set; }
        public string EGuid { get; set; }
        public string fictitious_name { get; set; }
    
        public virtual company company { get; set; }
        public virtual StatusMaster StatusMaster { get; set; }
        public virtual Tender Tender { get; set; }
    }
}