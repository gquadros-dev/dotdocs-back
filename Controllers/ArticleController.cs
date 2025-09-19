using AutoMapper;
using backend.Interfaces;
using backend.Models;
using backend.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/articles")]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly IMapper _mapper;

        public ArticlesController(IArticleService articleService, IMapper mapper)
        {
            _articleService = articleService;
            _mapper = mapper;
        }

        /// <summary>
        /// Busca um artigo específico pelo seu ID.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ArticleModel>> GetArticleById(string id)
        {
            var article = await _articleService.GetArticleByIdAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            return Ok(article);
        }

        /// <summary>
        /// Busca todos os artigos pelo ID do tópico.
        /// </summary>
        [HttpGet("topic/{topicId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ArticleModel>>> GetArticlesByTopicId(string topicId)
        {
            var articles = await _articleService.GetArticlesByTopicIdAsync(topicId);

            return Ok(articles);
        }

        /// <summary>
        /// Cria um novo artigo.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ArticleModel>> CreateArticle([FromBody] CreateArticleDto articleDto)
        {
            if (string.IsNullOrEmpty(articleDto.TopicId))
            {
                return BadRequest("TopicId is required.");
            }

            var articleToCreate = _mapper.Map<ArticleModel>(articleDto);

            var createdArticle = await _articleService.CreateArticleAsync(articleToCreate);

            return CreatedAtAction(nameof(GetArticleById), new { id = createdArticle.Id }, createdArticle);
        }

        /// <summary>
        /// Atualiza um artigo existente.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateArticle(string id, [FromBody] UpdateArticleDto articleDto)
        {
            var articleToUpdate = _mapper.Map<ArticleModel>(articleDto);

            var success = await _articleService.UpdateArticleAsync(id, articleToUpdate);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        /// <summary>
        /// Busca todos os artigos.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<ArticleModel>> GetAllArticles()
        {
            return await _articleService.GetAllArticles();
        }

        /// <summary>
        /// Exclui um artigo pelo seu ID.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteArticle(string id)
        {
            var success = await _articleService.DeleteArticleAsync(id);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}