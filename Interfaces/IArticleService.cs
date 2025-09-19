using backend.Models;

namespace backend.Interfaces
{
    public interface IArticleService
    {
        Task<IEnumerable<ArticleModel>> GetAllArticles();
        Task<ArticleModel?> GetArticleByIdAsync(string id);
        Task<IEnumerable<ArticleModel>> GetArticlesByTopicIdAsync(string topicId);
        Task<ArticleModel> CreateArticleAsync(ArticleModel article);
        Task<bool> UpdateArticleAsync(string id, ArticleModel article);
        Task<bool> DeleteArticleAsync(string id);
    }
}