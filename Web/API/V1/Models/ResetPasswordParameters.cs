using System.ComponentModel.DataAnnotations;

namespace GdShows.API.V1.Models
{
    public class ResetPasswordParameters
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
