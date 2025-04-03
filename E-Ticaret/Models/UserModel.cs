using Entities.Dtos;
using ETicaret.Models;

namespace ETicaret.Models
{
    public class UserModel
    {
        public LoginModel? Login { get; set; }
        public RegisterDto? Register { get; set; }
        public bool isRegister { get; set; } = false;
    }
}
