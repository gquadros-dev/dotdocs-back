using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace backend.Models
{
    public class ArticleModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("content")]
        public string Content { get; set; }

        [BsonElement("topicId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string TopicId { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }
    }
}