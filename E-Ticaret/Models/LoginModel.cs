using System.ComponentModel.DataAnnotations;

namespace ETicaret.Models
{
    public class LoginModel
    {
        private string? _returnUrl;

        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
        public bool RememberMe { get; set; }

        public string? ReturnUrl
        {
            get 
            {
                if(_returnUrl == null)
                { 
                    return "/";
                }
                else
                {
                    return _returnUrl;
                }
            }
            set
            {
                _returnUrl = value;
            }
        }
    }
}
