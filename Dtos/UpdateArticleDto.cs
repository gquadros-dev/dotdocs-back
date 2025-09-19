using System.ComponentModel.DataAnnotations;

namespace backend.Dtos
{
    public class UpdateArticleDto
    {
        [Required(ErrorMessage = "O título é obrigatório")]
        [MaxLength(200)]
        public string Title { get; set; } = null!;

        public string Content { get; set; } = string.Empty;
    }
}