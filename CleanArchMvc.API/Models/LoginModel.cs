using System.ComponentModel.DataAnnotations;

namespace CleanArchMvc.API.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage ="Email is required")]
        [EmailAddress(ErrorMessage = "Invalid format email")]

        public string Email { get; set; }
        
        [Required(ErrorMessage ="Password is required")]
        [StringLength(20, ErrorMessage ="The {0} must be at least {2} and at max " + 
            "{1} characteres long.", MinimumLength =8)]
        [DataType(DataType.Password)]

        public string Password { get; set; }
    }
}