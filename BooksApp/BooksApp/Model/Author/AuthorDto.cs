﻿using System.ComponentModel.DataAnnotations;

namespace BooksApp.Model.Author
{
    public class AuthorDto :BaseDto
    {
        [Required]
        [StringLength(50)]
        public string  FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(250)]
        public string Bio { get; set; }

    }
}
