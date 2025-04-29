using Entities.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ETicaret.Models;
using Services.Contracts;
using System.Security.Claims;
using Entities.Models;

namespace ETicaret.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Account> _userManager;
        private readonly SignInManager<Account> _signInManager;
        private readonly IServiceManager _manager;

        public AccountController(UserManager<Account> userManager, SignInManager<Account> signInManager, IServiceManager manager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _manager = manager;
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
                var user = await _userManager.FindByNameAsync(model.Login.Name);
                if (user != null)
                {
                    await _signInManager.SignOutAsync();

                    var result = await _signInManager.PasswordSignInAsync(
                        user.UserName,
                        model.Login.Password,
                        model.Login.RememberMe,
                        lockoutOnFailure: false
                    );

                    if (result.Succeeded)
                    {
                        user.LastLoginDate = DateTime.UtcNow;
                        await _userManager.UpdateAsync(user);

                        return Redirect(model.Login.ReturnUrl ?? "/");
                    }
                }

                ModelState.AddModelError("Error", "Kullanıcı adı veya şifre hatalı.");
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

            var user = new Account
            {
                UserName = model.Register.UserName,
                FirstName = model.Register.FirstName,
                LastName = model.Register.LastName,
                PhoneNumber = model.Register.PhoneNumber,
                BirthDate = DateTime.SpecifyKind(model.Register.BirthDate, DateTimeKind.Utc),
                Email = model.Register.Email
            };

            var result = await _userManager.CreateAsync(user, model.Register.Password);
            if (result.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(user, "User");
                if (roleResult.Succeeded)
                {
                    ViewBag.SuppressLayoutNotification = true;
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

        public IActionResult Profile()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _manager.AuthService.GetOneUserForUpdate(User.Identity.Name).Result;

            return View(new ProfileModel()
            {
                User = _userManager.FindByNameAsync(User.Identity.Name).Result,
                Orders = _manager.OrderService.GetUserOrders(User.Identity.Name).ToList(),
                UserReviews = _manager.UserReviewService.GetAllUserReviewsOfOneUser(userId, false),
                UserDtoForUpdate = user
            });
        }
        public IActionResult DeleteComment([FromRoute(Name = "id")] int id)
        {
            string userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var userReviews = _manager.UserReviewService.GetAllUserReviews(false);

            if(userReviews.Where(x => x.UserId == userId && x.UserReviewId == id).Any())
            {
                var userReview = _manager.UserReviewService.GetOneUserReview(id, false);
                if (userReview != null)
                {
                    _manager.UserReviewService.DeleteOneUserReview(id);
                    return RedirectToAction("Profile");
                }
            }
            return RedirectToAction("Profile");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateComment([FromForm] UserReviewDtoForUpdate userReviewDto, IFormFile? file = null)
        {
            string userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var review = _manager.UserReviewService.GetAllUserReviews(false)
                .FirstOrDefault(x => x.UserId == userId && x.UserReviewId == userReviewDto.UserReviewId);
            if (review == null)
            {
                return RedirectToAction("Profile");
            }

            if (file != null)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/comments", $"{userReviewDto.UserReviewId.ToString()}.png");
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                userReviewDto.ReviewPictureUrl = $"{userReviewDto.UserReviewId.ToString()}.png";    
            }
            else
            {
                userReviewDto.ReviewPictureUrl = review.ReviewPictureUrl;
            }

            _manager.UserReviewService.UpdateOneUserReview(userReviewDto);
            TempData["success"] = "Yorumunuz başarıyla güncellendi.";
            return RedirectToAction("Profile");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAvatar(IFormFile? file = null)
        {
            string userName = User.Identity.Name;
            Account user = await _manager.AuthService.GetOneUser(userName);
            user.AvatarUrl = $"avatars/{User.Identity.Name}.png";
            UserDtoForUpdate userDto = new UserDtoForUpdate()
            {
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                AvatarUrl = user.AvatarUrl,
            };


            if (file != null)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/avatars", $"{User.Identity.Name}.png");
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                await _manager.AuthService.UpdateAvatar(userDto);
                TempData["success"] = "Avatarınız başarıyla güncellendi.";

            }
            return RedirectToAction("Profile");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile([FromForm] UserDtoForUpdate userDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["danger"] = "Profiliniz güncellenirken bir hata oluştu.";
                return RedirectToAction("Profile");
            }

            var result = await _manager.AuthService.Update(userDto);
            if (!result.Succeeded)
            {

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Error", error.Description);
                }
                TempData["danger"] = "Profiliniz güncellenirken bir hata oluştu.";
                return RedirectToAction("Profile");
            }
            TempData["success"] = "Profiliniz başarıyla güncellendi.";
            return RedirectToAction("Profile");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto model)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _manager.AuthService.GetOneUserForUpdate(User.Identity.Name).Result;

            if (!ModelState.IsValid)
            {
                TempData["error"] = "Lütfen geçerli bilgiler girin.";
                TempData["OpenModal"] = "editPasswordModal"; // modal tekrar açılsın
                return View("Profile", new ProfileModel()
                {
                    User = _userManager.FindByNameAsync(User.Identity.Name).Result,
                    Orders = _manager.OrderService.GetUserOrders(User.Identity.Name).ToList(),
                    UserReviews = _manager.UserReviewService.GetAllUserReviewsOfOneUser(userId, false),
                    UserDtoForUpdate = user,
                    ChangePasswordDto = model
                });
            }

            try
            {
                var result = await _manager.AuthService.ChangePassword(model);

                if (result.Succeeded)
                {
                    TempData["success"] = "Şifreniz başarıyla güncellendi.";
                    return RedirectToAction("Profile");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                TempData["error"] = "Şifre güncellenemedi.";
                TempData["OpenModal"] = "editPasswordModal";
                return View("Profile", new ProfileModel()
                {
                    User = _userManager.FindByNameAsync(User.Identity.Name).Result,
                    Orders = _manager.OrderService.GetUserOrders(User.Identity.Name).ToList(),
                    UserReviews = _manager.UserReviewService.GetAllUserReviewsOfOneUser(userId, false),
                    UserDtoForUpdate = user,
                    ChangePasswordDto = model
                });
            }
            catch (Exception ex)
            {
                TempData["error"] = $"Bir hata oluştu: {ex.Message}";
                return RedirectToAction("Profile");
            }
        }



    }
}

