using AutoMapper;
using backend.Interfaces;
using backend.Models;
using backend.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/topics")]
    public class TopicsController : ControllerBase
    {
        private readonly ITopicService _topicService;
        private readonly IMapper _mapper;

        public TopicsController(ITopicService topicService, IMapper mapper)
        {
            _topicService = topicService;
            _mapper = mapper;
        }

        /// <summary>
        /// Busca a árvore completa de tópicos de um determinado tipo (público ou privado).
        /// </summary>
        [HttpGet("tree/{type}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTopicTree(string type)
        {
            var tree = await _topicService.GetTopicTree(type);
            return Ok(tree);
        }

        /// <summary>
        /// Busca um tópico específico pelo seu ID.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TopicModel>> GetTopicById(string id)
        {
            var topic = await _topicService.GetTopicByIdAsync(id);

            if (topic == null)
            {
                return NotFound();
            }

            return Ok(topic);
        }

        /// <summary>
        /// Cria um novo tópico.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TopicModel>> CreateTopic([FromBody] CreateUpdateTopicDto topicDto)
        {
            if (string.IsNullOrEmpty(topicDto.ParentId))
            {
                topicDto.ParentId = null;
            }

            var topicToCreate = _mapper.Map<TopicModel>(topicDto);

            var createdTopic = await _topicService.CreateTopicAsync(topicToCreate);

            return CreatedAtAction(nameof(GetTopicById), new { id = createdTopic.Id }, createdTopic);
        }

        /// <summary>
        /// Atualiza um tópico existente.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTopic(string id, [FromBody] CreateUpdateTopicDto topicDto)
        {
            if (string.IsNullOrEmpty(topicDto.ParentId))
            {
                topicDto.ParentId = null;
            }

            var topicToUpdate = _mapper.Map<TopicModel>(topicDto);

            var success = await _topicService.UpdateTopicAsync(id, topicToUpdate);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        /// <summary>
        /// Busca todos os tópicos (em formato de lista, não árvore).
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<TopicModel>> GetAllTopics()
        {
            return await _topicService.GetAllTopics(string.Empty);
        }

        /// <summary>
        /// Busca todos os tópicos por tipo.
        /// </summary>
        [HttpGet("byType/{type}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<TopicModel>> GetTopicsByType(string type)
        {
            return await _topicService.GetAllTopics(type);
        }

        /// <summary>
        /// Exclui um tópico pelo seu ID.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTopic(string id)
        {
            var success = await _topicService.DeleteTopicAsync(id);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPatch("{id}/type")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTopicType(string id, [FromBody] UpdateTopicTypeDto dto)
        {
            var success = await _topicService.UpdateTopicTypeAsync(id, dto.Type);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}