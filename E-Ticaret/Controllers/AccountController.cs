using Entities.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ETicaret.Models;

namespace ETicaret.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login([FromQuery(Name = "ReturnUrl")] string ReturnUrl = "/")
        {
            var login = new LoginModel()
            {
                ReturnUrl = ReturnUrl
            };
            return View(new UserModel()
            {
                Login = login,
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] UserModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await _userManager.FindByNameAsync(model.Login.Name);
                if(user != null)
                {
                    await _signInManager.SignOutAsync();
                    if((await _signInManager.PasswordSignInAsync(user, model.Login.Password, false, false)).Succeeded)
                    {
                        return Redirect(model?.Login.ReturnUrl ?? "/");
                    }
                }
                ModelState.AddModelError("Error", "Invalid username or password");

            }
            model.Login.ReturnUrl = model.Login.ReturnUrl ?? "/";
            return View(model);
        }

        public async Task<IActionResult> Logout([FromQuery(Name ="ReturnUrl")] string ReturnUrl="/")
        {
            await _signInManager.SignOutAsync();
            return Redirect(ReturnUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromForm] UserModel model)
        {
            var user = new IdentityUser
            {
                UserName = model.Register.UserName,
                Email = model.Register.Email
            };

            var result = await _userManager.CreateAsync(user, model.Register.Password);
            if (result.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(user, "User");
                if (roleResult.Succeeded)
                {
                    TempData["success"] = "Başarıyla kayıt oldunuz, giriş yapın.";
                    return RedirectToAction("Login");
                }
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            model.isRegister = true;
            return View("Login", model);
        }


        public IActionResult AccessDenied([FromQuery(Name="ReturnUrl")] string returnUrl)
        {
            return View();
        }
    }
}
