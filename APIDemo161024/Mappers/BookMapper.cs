using APIDemo161024.DTOS;
using APIDemo161024.Models;
using AutoMapper;

namespace APIDemo161024.Mappers
{
    public class BookMapper : Profile
    {
        public BookMapper() {
            CreateMap<Book, BookModel>().ReverseMap();
        } 
    }
}
