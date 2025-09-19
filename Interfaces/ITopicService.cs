using backend.Models;

namespace backend.Interfaces
{
    public interface ITopicService
    {
        Task<IEnumerable<TopicModel>> GetAllTopics();
        Task<TopicModel?> GetTopicByIdAsync(string id);
        Task<TopicModel> CreateTopicAsync(TopicModel topic);
        Task<bool> UpdateTopicAsync(string id, TopicModel updatedTopic);
        Task<object> GetTopicTree(string type);
    }
}