using System;
using System.Collections.Generic;



namespace blackBelt.Models
{
    public class User : BaseEntity
    {
        public int Id {get; set;}
        public string FirstName {get; set;}
        public string LastName {get; set;}
        public string UserName {get; set;}
        public string Password {get; set;}
        public int Wallet {get; set;}
        public DateTime CreatedAt {get; set;}
        public DateTime UpdatedAt {get; set;}

    }
}