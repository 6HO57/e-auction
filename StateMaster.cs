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
    
    public partial class StateMaster
    {
        public StateMaster()
        {
            this.RegisterMasters = new HashSet<RegisterMaster>();
        }
    
        public int stateId { get; set; }
        public string StateName { get; set; }
    
        public virtual ICollection<RegisterMaster> RegisterMasters { get; set; }
    }
}
