using System.ComponentModel.DataAnnotations;

namespace GdShows.API.V1.Models
{
    public class LoginParameters
    {
        public LoginParameters()
        {

        }

        [Required]
        public string Email { get; set; }
   
        [Required]
        public string Password { get; set; }

    }
}
