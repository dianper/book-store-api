using Application.Commands.Books.Commands;
using AutoMapper;

namespace Presentation.Mapper;

public class BookMapper : Profile
{
    public BookMapper()
    {
        CreateMap<Models.Book, Domain.Entities.Book>().ReverseMap();
        CreateMap<Models.Book, CreateBookCommand>();
    }
}
