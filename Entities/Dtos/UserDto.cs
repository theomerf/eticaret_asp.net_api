using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public record UserDto
    {
        [DataType(DataType.Text)]
        [Required(ErrorMessage= "Kullanıcı adı gerekli")]
        public String? UserName { get; init; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "E-Posta gerekli")]
        public String? Email { get; init; }

        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Telefon numarası gerekli")]
        public String? PhoneNumber { get; init; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Ad gerekli")]
        public String FirstName { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Soyad gerekli")]
        public String LastName { get; set; }

        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Doğum tarihi gerekli")]
        public DateTime BirthDate { get; set; }

        public List<int> FavouriteProductsId { get; set; } = new List<int>();

        public HashSet<String>? Roles { get; set; } = new HashSet<String>();
    }
}
