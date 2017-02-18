using System;
using System.Collections.Generic;



namespace blackBelt.Models
{
    public class Auction : BaseEntity
    {
        public int Id {get; set;}
        public string Product {get; set;}
        public string Description {get; set;}
        public string TimeAgo {get; set;}
        public string Author{get; set;}
        public int HighestBid {get; set;}
        public string BidId {get; set;}
        public DateTime EndDate {get; set;}
        public DateTime CreatedAt {get; set;}
        public DateTime UpdatedAt {get; set;}

    }
}