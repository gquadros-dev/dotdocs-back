using System.ComponentModel.DataAnnotations;

namespace backend.Dtos
{
    public class CreateUpdateTopicDto
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "O tipo é obrigatório (publico ou privado)")]
        public string Type { get; set; } = null!;

        // O ParentId é opcional (pode ser nulo para tópicos raiz)
        public string? ParentId { get; set; }

        public int Order { get; set; }
    }
}