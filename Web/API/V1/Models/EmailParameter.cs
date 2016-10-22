using System.ComponentModel.DataAnnotations;

namespace GdShows.API.V1.Models
{
    public class EmailParameter
    {
        [Required]
        public string Email { get; set; }
    }
}
