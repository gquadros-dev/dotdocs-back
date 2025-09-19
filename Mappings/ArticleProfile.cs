using AutoMapper;
using backend.Models;
using backend.Dtos;

public class ArticleProfile : Profile
{
    public ArticleProfile()
    {
        CreateMap<CreateArticleDto, ArticleModel>();
        CreateMap<UpdateArticleDto, ArticleModel>();
    }
}