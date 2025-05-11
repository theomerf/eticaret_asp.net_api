using Entities.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ETicaret.Models;
using Services.Contracts;
using System.Security.Claims;
using Entities.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

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
            // AJAX isteği olup olmadığını kontrol eden yardımcı metot
            bool isAjaxRequest = Request.Headers["X-Requested-With"] == "XMLHttpRequest";
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

                        // Kullanıcı adı ve rollerini cookie'ye claims olarak ekle
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.UserName),
                            new Claim(ClaimTypes.NameIdentifier, user.Id)
                        };

                        // Kullanıcı rollerini claim olarak ekle
                        var roles = await _userManager.GetRolesAsync(user);
                        foreach (var role in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role));
                        }

                        // Favori ürünleri cookie'ye kaydet
                        if (user.FavouriteProductsId != null && user.FavouriteProductsId.Any())
                        {
                            // ICollection<int> koleksiyonunu virgülle ayrılmış bir string'e dönüştür
                            var favouriteProductIdsString = string.Join("|", user.FavouriteProductsId);
                            // SameSite=Lax ve güvenlik ayarlarıyla cookie'yi kaydet
                            Response.Cookies.Append("FavouriteProducts", favouriteProductIdsString, new CookieOptions
                            {
                                Expires = DateTime.Now.AddMonths(1),
                                IsEssential = true,
                                Path = "/",            // Tüm site genelinde erişilebilir
                                SameSite = SameSiteMode.Lax,
                                HttpOnly = false
                            });
                        }
                        else
                        {
                            // Varolan cookie'yi temizle
                            Response.Cookies.Delete("FavouriteProducts");
                            // Boş bir cookie oluştur
                            Response.Cookies.Append("FavouriteProducts", "", new CookieOptions
                            {
                                Expires = DateTime.Now.AddMonths(1),
                                IsEssential = true,
                                Path = "/",
                                SameSite = SameSiteMode.Lax,
                                HttpOnly = false
                            });
                        }

                        // ClaimsIdentity oluştur ve giriş yap
                        var claimsIdentity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
                        await HttpContext.SignInAsync(
                            IdentityConstants.ApplicationScheme,
                            new ClaimsPrincipal(claimsIdentity)
                        );

                        // AJAX isteği ise JSON yanıtı döndür
                        if (isAjaxRequest)
                        {
                            return Json(new
                            {
                                success = true,
                                redirectUrl = model.Login.ReturnUrl ?? "/",
                                message = "Giriş başarılı!"
                            });
                        }

                        // Normal istek ise yönlendirme yap
                        return Redirect(model.Login.ReturnUrl ?? "/");
                    }
                    else
                    {
                        // Giriş başarısız olduğunda
                        if (isAjaxRequest)
                        {
                            return Json(new
                            {
                                success = false,
                                message = "Kullanıcı adı veya şifre hatalı."
                            });
                        }

                        ModelState.AddModelError("Login.Name", "Kullanıcı adı veya şifre hatalı.");
                    }
                }
                else
                {
                    // Kullanıcı bulunamadığında
                    if (isAjaxRequest)
                    {
                        return Json(new
                        {
                            success = false,
                            message = "Kullanıcı adı veya şifre hatalı."
                        });
                    }

                    ModelState.AddModelError("Login.Name", "Kullanıcı adı veya şifre hatalı.");
                }
            }
            else
            {
                // Model doğrulama hatası olduğunda
                if (isAjaxRequest)
                {
                    var errors = ModelState
                        .Where(x => x.Value.Errors.Count > 0)
                        .Select(x => new {
                            Key = x.Key,
                            Errors = x.Value.Errors.Select(e => e.ErrorMessage).ToList()
                        })
                        .ToList();

                    return Json(new
                    {
                        success = false,
                        message = "Form doğrulama hatası.",
                        errors = errors
                    });
                }
            }

            // AJAX isteği değilse ve başarısızsa - normal akışa devam et
            model.Login.ReturnUrl = model.Login.ReturnUrl ?? "/";
            return View(model);
        }


        public async Task<IActionResult> Logout([FromQuery(Name = "ReturnUrl")] string ReturnUrl = "/")
        {
            await _signInManager.SignOutAsync();

            // AJAX isteği olup olmadığını kontrol eden yardımcı metot
            bool isAjaxRequest = Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            Response.Cookies.Delete("FavouriteProducts");

            if (isAjaxRequest)
            {
                return Json(new
                {
                    success = true,
                    redirectUrl = ReturnUrl,
                    message = "Başarıyla çıkış yapıldı."
                });
            }

            return Redirect(ReturnUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromForm] UserModel model)
        {
            bool isAjaxRequest = Request.Headers["X-Requested-With"] == "XMLHttpRequest";

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
                    if (isAjaxRequest)
                    {
                        return Json(new
                        {
                            success = true,
                            message = "Başarıyla kayıt oldunuz, giriş yapın."
                        });
                    }

                    TempData["visible"] = "false";
                    TempData["success"] = "Başarıyla kayıt oldunuz, giriş yapın.";
                    return RedirectToAction("Login");
                }
            }
            else
            {
                if (isAjaxRequest)
                {
                    var errors = result.Errors
                        .Select(e => new {
                            Key = "Register.UserName",
                            Errors = new List<string> { e.Description }
                        })
                        .ToList();

                    return Json(new
                    {
                        success = false,
                        message = "Kayıt işlemi başarısız.",
                        errors = errors
                    });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            if (isAjaxRequest)
            {
                var modelErrors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new {
                        Key = x.Key,
                        Errors = x.Value.Errors.Select(e => e.ErrorMessage).ToList()
                    })
                    .ToList();

                return Json(new
                {
                    success = false,
                    message = "Form doğrulama hatası.",
                    errors = modelErrors
                });
            }

            model.isRegister = true;
            return View("Login", model);
        }


        public IActionResult AccessDenied([FromQuery(Name = "ReturnUrl")] string returnUrl)
        {
            return View();
        }

        [Authorize]
        public IActionResult Profile()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _manager.AuthService.GetOneUserForUpdate(User.Identity.Name).Result;
            var orders = _manager.OrderService.GetUserOrders(User.Identity.Name).ToList();
            foreach (var order in orders)
            {
                foreach (var line in order.Lines)
                {
                    var product = _manager.ProductService.GetOneProduct(line.ProductId, false);
                    if (product != null)
                    {
                        line.ProductName = product.ProductName;
                        line.ImageUrl = product.ImageUrl;
                        line.DiscountPrice = product.DiscountPrice;
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "Ürün bulunamadı.");
                    }
                }
            }

            return View(new ProfileModel()
            {
                User = _userManager.FindByNameAsync(User.Identity.Name).Result,
                Orders = orders,
                UserReviews = _manager.UserReviewService.GetAllUserReviewsOfOneUser(userId, false),
                UserDtoForUpdate = user
            });
        }
        public IActionResult DeleteComment([FromRoute(Name = "id")] int id)
        {
            string userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var userReviews = _manager.UserReviewService.GetAllUserReviews(false);

            if (userReviews.Where(x => x.UserId == userId && x.UserReviewId == id).Any())
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

        public async Task<IActionResult> Favourites()
        {
            // Kullanıcı giriş yapmış mı kontrol et
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                TempData["error"] = "Kullanıcı oturumu bulunamadı.";
                return RedirectToAction("Login");
            }

            // Kullanıcı bilgilerini veritabanından al
            var user = await _manager.AuthService.GetOneUsersFavourites(User.Identity?.Name ?? string.Empty);
            if (user == null)
            {
                TempData["error"] = "Kullanıcı bilgileri alınamadı.";
                return RedirectToAction("Login");
            }

            // Cookie'deki favori ürünleri al
            var favouriteProductsCookie = Request.Cookies["FavouriteProducts"];
            Console.WriteLine($"Favourites - Cookie value: {favouriteProductsCookie ?? "null"}");

            // Favori ürün ID'lerini parse et
            var favouriteProductIds = new List<int>();
            if (!string.IsNullOrEmpty(favouriteProductsCookie))
            {
                try
                {
                    favouriteProductIds = favouriteProductsCookie
                        .Split('|', StringSplitOptions.RemoveEmptyEntries)
                        .Where(id => !string.IsNullOrWhiteSpace(id))
                        .Select(id =>
                        {
                            if (int.TryParse(id.Trim(), out int productId))
                                return productId;
                            Console.WriteLine($"Favourites - Invalid product ID: {id}");
                            return -1; // Geçersiz ID'ler için -1 döndür
                        })
                        .Where(id => id > 0) // Sadece geçerli ID'leri al
                        .Distinct() // Tekrarları kaldır
                        .ToList();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Favourites - Error parsing cookie: {ex.Message}");
                }
            }

            // Cookie'de değişiklik varsa kullanıcı favorilerini güncelle
            if (favouriteProductIds.Any())
            {
                // Kullanıcının veritabanındaki favori ürünlerini al
                var dbFavorites = user.ToList() ?? new List<int>();

                // Cookie'deki verilerle veritabanındakiler aynı değilse güncelle
                if (!favouriteProductIds.OrderBy(id => id).SequenceEqual(dbFavorites.OrderBy(id => id)))
                {
                    await _manager.AuthService.UpdateUserFavourites(favouriteProductIds, User.Identity.Name);

                    // Güncellenmiş kullanıcı bilgilerini tekrar al
                    user = await _manager.AuthService.GetOneUsersFavourites(User.Identity?.Name ?? string.Empty);
                }
            }

            // Kullanıcının favori ürünlerini göster
            var favouriteProducts = _manager.ProductService.GetFavouriteProducts(user, false);
            return View(favouriteProducts);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddToFavourites(int id)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Json(new { success = false, message = "Kullanıcı oturumu bulunamadı.", type = "warning" });
            }

            var user = await _manager.AuthService.GetOneUserForUpdate(User.Identity?.Name ?? string.Empty);
            if (user == null)
            {
                return Json(new { success = false, message = "Kullanıcı bilgileri alınamadı.", type = "danger" });
            }

            if (!user.FavouriteProductsId.Contains(id))
            {
                user.FavouriteProductsId.Add(id);
                await _manager.AuthService.Update(user);
                return Json(new { success = true, message = "Ürün favorilere eklendi.", type = "success" });
            }
            else
            {
                return Json(new { success = false, message = "Ürün zaten favorilerde.", type = "info" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromFavourites(int id)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Json(new { success = false, message = "Kullanıcı oturumu bulunamadı.", type = "warning" });
            }

            var user = await _manager.AuthService.GetOneUserForUpdate(User.Identity?.Name ?? string.Empty);
            if (user == null)
            {
                return Json(new { success = false, message = "Kullanıcı bilgileri alınamadı.", type = "danger" });
            }

            if (user.FavouriteProductsId.Contains(id))
            {
                user.FavouriteProductsId.Remove(id);
                await _manager.AuthService.Update(user);
                return Json(new { success = true, message = "Ürün favorilerden kaldırıldı.", type = "success" });
            }
            else
            {
                return Json(new { success = false, message = "Ürün favorilerde yok.", type = "info" });
            }
        }
        public IActionResult Summary()
        {
            // Eğer AJAX isteği değilse 404 döndür
            if (!Request.Headers["X-Requested-With"].Equals("XMLHttpRequest"))
            {
                return View("AccessDenied","Account"); // veya Forbid() da kullanılabilir
            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int favouritesCount = 0;

            if (userId != null)
            {
                var user = _manager.AuthService.GetOneUsersFavourites(User.Identity?.Name ?? string.Empty).Result;
                favouritesCount = user?.Count() ?? 0;
            }

            return Json(favouritesCount);
        }






    }
}

