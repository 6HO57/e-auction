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
    
    public partial class Tracking
    {
        public int Trackid { get; set; }
        public Nullable<int> Tenderid { get; set; }
        public Nullable<int> Decisionby { get; set; }
        public Nullable<System.DateTime> Decisionon { get; set; }
        public Nullable<int> TrackStatus { get; set; }
        public string Remarks { get; set; }
    
        public virtual LoginMaster LoginMaster { get; set; }
        public virtual StatusMaster StatusMaster { get; set; }
        public virtual Tender Tender { get; set; }
    }
}
