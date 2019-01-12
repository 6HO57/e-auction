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
    
    public partial class TenderItem
    {
        public TenderItem()
        {
            this.Bids = new HashSet<Bid>();
        }
    
        public int itemid { get; set; }
        public Nullable<int> Tenderid { get; set; }
        public Nullable<int> ItemType { get; set; }
        public string ItemName { get; set; }
        public string ItemDesc { get; set; }
        public Nullable<int> ItemCategory { get; set; }
        public string Location { get; set; }
        public Nullable<int> Quantity { get; set; }
        public int UOM { get; set; }
        public Nullable<int> Price { get; set; }
        public int Currency { get; set; }
        public Nullable<int> FloorPrice { get; set; }
        public int BidMethod { get; set; }
        public Nullable<int> ReducePerBid { get; set; }
        public string Remarks { get; set; }
    
        public virtual BidMethodMaster BidMethodMaster { get; set; }
        public virtual ICollection<Bid> Bids { get; set; }
        public virtual CategoryMaster CategoryMaster { get; set; }
        public virtual CurrencyMaster CurrencyMaster { get; set; }
        public virtual ItemTypeMaster ItemTypeMaster { get; set; }
        public virtual Tender Tender { get; set; }
        public virtual UOMMaster UOMMaster { get; set; }
    }
}