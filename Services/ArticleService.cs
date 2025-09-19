using backend.Interfaces;
using backend.Models;
using MongoDB.Driver;

namespace backend.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IMongoCollection<ArticleModel> _articlesCollection;

        public ArticleService(IMongoDatabase database)
        {
            _articlesCollection = database.GetCollection<ArticleModel>("Articles");
        }

        public async Task<ArticleModel> CreateArticleAsync(ArticleModel article)
        {
            article.CreatedAt = DateTime.UtcNow;
            await _articlesCollection.InsertOneAsync(article);
            return article;
        }

        public async Task<bool> DeleteArticleAsync(string id)
        {
            var result = await _articlesCollection.DeleteOneAsync(a => a.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<IEnumerable<ArticleModel>> GetAllArticles()
        {
            return await _articlesCollection.Find(_ => true).ToListAsync();
        }

        public async Task<ArticleModel?> GetArticleByIdAsync(string id)
        {
            return await _articlesCollection.Find(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ArticleModel>> GetArticlesByTopicIdAsync(string topicId)
        {
            return await _articlesCollection.Find(a => a.TopicId == topicId).ToListAsync();
        }

        public async Task<bool> UpdateArticleAsync(string id, ArticleModel article)
        {
            var updateDefinition = Builders<ArticleModel>.Update
                .Set(a => a.Title, article.Title)
                .Set(a => a.Content, article.Content);

            var result = await _articlesCollection.UpdateOneAsync(a => a.Id == id, updateDefinition);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
    }
}