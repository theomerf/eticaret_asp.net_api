using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public record MainCategoryDto
    {
        public int MainCategoryId { get; set; }
        [Required(ErrorMessage = "Kategori adı gereklidir!")]
        public string CategoryName { get; set; } = string.Empty;
    }
}
