using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using backend.Models;

namespace backend.Dtos
{
    public class TopicTreeDto : TopicModel
    {
        [BsonElement("children")]
        public List<TopicTreeDto> Children { get; set; } = new List<TopicTreeDto>();

        [BsonElement("articles")]
        public List<ArticleInTopicDto> Articles { get; set; } = new List<ArticleInTopicDto>();

        [BsonElement("level")]
        public int Level { get; set; }
    }
}