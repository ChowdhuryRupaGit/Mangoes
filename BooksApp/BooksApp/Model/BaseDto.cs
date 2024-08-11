using System.ComponentModel.DataAnnotations;

namespace BooksApp.Model
{
    public class BaseDto
    {
        [Required]
        public int Id { get; set; }
    }
}
