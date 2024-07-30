using System.ComponentModel.DataAnnotations;

namespace WebAppMango.Models.DTO
{
    public class LoginRequestDTO
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty; 
    }
}
