using System.ComponentModel.DataAnnotations;
namespace blackBelt.Models
{
    public class RegisterViewModel : BaseEntity
    {
        [Required]
        [MinLength(2)]
        public string FirstName { get; set; }
        [Required]
        [MinLength(2)]
        public string LastName { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Username must be 3 or more")]
        [MaxLength(20, ErrorMessage = "Username must be 20 or less")]
        public string UserName {get; set;}
        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Password and confirmation must match.")]
        public string PasswordConfirmation { get; set; }
        
    }
}



