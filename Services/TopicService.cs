using MongoDB.Driver;
using MongoDB.Bson;
using backend.Interfaces;
using backend.Models;
using MongoDB.Bson.Serialization;

namespace backend.Services
{
    public class TopicService : ITopicService
    {
        private readonly IMongoCollection<TopicModel> _topicsCollection;

        public TopicService(IMongoDatabase database)
        {
            _topicsCollection = database.GetCollection<TopicModel>("Topics");
        }

        public async Task<IEnumerable<TopicModel>> GetAllTopics()
        {
            return await _topicsCollection.Find(_ => true).ToListAsync();
        }

        public async Task<TopicModel?> GetTopicByIdAsync(string id)
        {
            return await _topicsCollection.Find(t => t.Id == id).FirstOrDefaultAsync();
        }

        public async Task<TopicModel> CreateTopicAsync(TopicModel topic)
        {
            await _topicsCollection.InsertOneAsync(topic);
            return topic;
        }


        public async Task<bool> UpdateTopicAsync(string id, TopicModel topicWithNewValues)
        {
            var updateDefinition = Builders<TopicModel>.Update
                .Set(t => t.Name, topicWithNewValues.Name)
                .Set(t => t.Type, topicWithNewValues.Type)
                .Set(t => t.ParentId, topicWithNewValues.ParentId)
                .Set(t => t.Order, topicWithNewValues.Order);

            var result = await _topicsCollection.UpdateOneAsync(
                t => t.Id == id,
                updateDefinition
            );

            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<object> GetTopicTree(string type)
        {
            var pipeline = new BsonDocument[]
            {
                new BsonDocument("$match", new BsonDocument
                {
                    { "parentId", BsonNull.Value },
                    { "type", type }
                }),
                new BsonDocument("$graphLookup", new BsonDocument
                {
                    { "from", "Topics" },
                    { "startWith", "$_id" },
                    { "connectFromField", "_id" },
                    { "connectToField", "parentId" },
                    { "as", "children" },
                    { "depthField", "level" }
                }),
                new BsonDocument("$sort", new BsonDocument("order", 1))
            };

            var results = await _topicsCollection.Aggregate<BsonDocument>(pipeline).ToListAsync();

            return BsonSerializer.Deserialize<List<object>>(results.ToJson());
        }
    }
}
