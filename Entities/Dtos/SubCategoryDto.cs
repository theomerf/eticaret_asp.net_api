using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public record SubCategoryDto
    {
        public int SubCategoryId { get; set; }
        public int MainCategoryId { get; set; }
        [Required(ErrorMessage = "Kategori adı gereklidir!")]
        public string CategoryName { get; set; } = string.Empty;
    }
}