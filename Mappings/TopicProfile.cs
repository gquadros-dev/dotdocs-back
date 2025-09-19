using AutoMapper;
using backend.Models;
using backend.Dtos;

namespace backend.Mappings
{
    public class TopicProfile : Profile
    {
        public TopicProfile()
        {
            CreateMap<CreateUpdateTopicDto, TopicModel>();
            CreateMap<TopicModel, CreateUpdateTopicDto>();
        }
    }
}