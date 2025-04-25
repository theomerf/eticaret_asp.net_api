using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public record ChangePasswordDto
    {
        public string? UserName { get; init; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Mevcut şifre gerekli")]
        public string? Password { get; init; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Yeni şifre gerekli")]
        public string? NewPassword { get; init; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Yeni şifrenizi doğrulayın")]
        [Compare("NewPassword", ErrorMessage = "Yeni şifre ve doğrulama uyuşmuyor")]
        public string? ConfirmPassword { get; init; }
    }
}
