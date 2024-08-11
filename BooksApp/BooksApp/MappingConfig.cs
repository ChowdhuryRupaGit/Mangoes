using AutoMapper;
using BooksApp.Data;
using BooksApp.Model.Author;
using BooksApp.Model.Book;

namespace BooksApp
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        {
            CreateMap<Author,AuthorDto>().ReverseMap();
            CreateMap<Book,BookDto>().ReverseMap();
        }
    }
}
