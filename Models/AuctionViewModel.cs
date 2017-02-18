using System;
using System.ComponentModel.DataAnnotations;



namespace blackBelt.Models
{
    public class AuctionViewModel : BaseEntity
    {
        
        [Required]
        [MinLength(3, ErrorMessage = "Product must be 3 chars or longer!")]
        public string Product {get; set;}
        [Required]
        [MinLength(10, ErrorMessage = "Description must be 10 chars or longer!")]
        public string Description {get; set;}
        [Required(ErrorMessage ="Date Required")]
        public string EndDate {get; set;}
        public int HighestBid {get; set;}
        public int BidId {get; set;}
        public string Author {get; set;}
        public DateTime CreatedAt {get; set;}
        public DateTime UpdatedAt {get; set;}

    }
}