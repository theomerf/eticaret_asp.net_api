using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public record RegisterDto
    {
        [Required(ErrorMessage ="Kullanıcı adı gerekiyor")]
        public string UserName { get; init; }
        [Required(ErrorMessage = "E-Posta gerekiyor")]
        public string Email { get; init; }
        [Required(ErrorMessage = "Şifre gerekiyor")]
        public string Password { get; init; }

        [Required(ErrorMessage = "İsim gerekiyor")]
        public string FirstName { get; init; }
        [Required(ErrorMessage = "Soyisim gerekiyor")]
        public string PhoneNumber { get; init; }
        [Required(ErrorMessage = "Telefon numarası gerekiyor")]
        public string LastName { get; init; }
        [Required(ErrorMessage = "Doğum tarihi gerekiyor")]
        public DateTime BirthDate { get; init; }

    }
}
