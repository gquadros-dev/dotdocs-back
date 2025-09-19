using backend.Models;

namespace backend.Interfaces
{
    public interface ITopicService
    {
        Task<IEnumerable<TopicModel>> GetAllTopics(string type);
        Task<TopicModel?> GetTopicByIdAsync(string id);
        Task<TopicModel> CreateTopicAsync(TopicModel topic);
        Task<bool> DeleteTopicAsync(string id);
        Task<bool> UpdateTopicAsync(string id, TopicModel updatedTopic);
        Task<object> GetTopicTree(string type);
        Task<bool> UpdateTopicTypeAsync(string id, string newType);
    }
}