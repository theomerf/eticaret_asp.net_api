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
            if (ModelState.IsValid && model?.Login?.Name != null)
            {
                var user = await _userManager.FindByNameAsync(model.Login.Name);
                if (user != null && user.UserName != null && model.Login.Password != null)
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
                        .Where(x => x.Value?.Errors.Count > 0)
                        .Select(x => new {
                            Key = x.Key,
                            Errors = x.Value?.Errors.Select(e => e.ErrorMessage).ToList()
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
            // Null kontrolü eklenerek warning giderildi
            if (model?.Login != null)
            {
                model.Login.ReturnUrl = model.Login.ReturnUrl ?? "/";
            }
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
            var birthDate = model.Register?.BirthDate ?? DateTime.Now;

            var user = new Account
            {
                UserName = model.Register?.UserName,
                FirstName = model.Register?.FirstName,
                LastName = model.Register?.LastName,
                PhoneNumber = model.Register?.PhoneNumber,
                BirthDate = DateTime.SpecifyKind(birthDate, DateTimeKind.Utc),
                Email = model.Register?.Email
            };

            if(model.Register?.Password == null)
            {
                if (isAjaxRequest)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Şifre alanı boş bırakılamaz.",
                        errors = new List<object> { new { Key = "Register.Password", Errors = new List<string> { "Şifre alanı boş bırakılamaz." } } }
                    });
                }
                ModelState.AddModelError("Register.Password", "Şifre alanı boş bırakılamaz.");
                model.IsRegister = true;
                return View("Login", model);
            }

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
                    .Where(x => x.Value?.Errors.Count > 0)
                    .Select(x => new {
                        Key = x.Key,
                        Errors = x.Value?.Errors.Select(e => e.ErrorMessage).ToList()
                    })
                    .ToList();

                return Json(new
                {
                    success = false,
                    message = "Form doğrulama hatası.",
                    errors = modelErrors
                });
            }

            model.IsRegister = true;
            return View("Login", model);
        }


        public IActionResult AccessDenied([FromQuery(Name = "ReturnUrl")] string returnUrl)
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var user = await _manager.AuthService.GetOneUserForUpdateAsync(userName);
            var orders = await _manager.OrderService.GetUserOrdersAsync(userName);
            foreach (var order in orders)
            {
                foreach (var line in order.Lines)
                {
                    var product = await _manager.ProductService.GetOneProductAsync(line.ProductId, false);
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
                User = await _manager.AuthService.GetOneUserAsync(userName),
                Orders = orders,
                UserReviews = await _manager.UserReviewService.GetAllUserReviewsOfOneUserAsync(userId, false),
                UserDtoForUpdate = user
            });
        }
        public async Task<IActionResult> DeleteComment([FromRoute(Name = "id")] int id)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userReviews = await _manager.UserReviewService.GetAllUserReviewsAsync(false);

            if (userReviews.Where(x => x.UserId == userId && x.UserReviewId == id).Any())
            {
                var userReview = await _manager.UserReviewService.GetOneUserReviewAsync(id, false);
                if (userReview != null)
                {
                    await _manager.UserReviewService.DeleteOneUserReviewAsync(id);
                    return RedirectToAction("Profile");
                }
            }
            return RedirectToAction("Profile");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateComment([FromForm] UserReviewDtoForUpdate userReviewDto, IFormFile? file = null)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var reviews = await _manager.UserReviewService.GetAllUserReviewsAsync(false);
            var review = reviews.FirstOrDefault(x => x.UserId == userId && x.UserReviewId == userReviewDto.UserReviewId);

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

            await _manager.UserReviewService.UpdateOneUserReviewAsync(userReviewDto);
            TempData["success"] = "Yorumunuz başarıyla güncellendi.";
            return RedirectToAction("Profile");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAvatar(IFormFile? file = null)
        {
            string? userName = User.FindFirstValue(ClaimTypes.Name);
            var userDto = await _manager.AuthService.GetOneUserForUpdateAsync(userName);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            userDto.AvatarUrl = $"avatars/{userId}.png";

            if (file != null)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/avatars", $"{userId}.png");
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                await _manager.AuthService.UpdateAvatarAsync(userDto);
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

            var result = await _manager.AuthService.UpdateAsync(userDto);
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
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string? userName = User.FindFirstValue(ClaimTypes.Name);
            var user = await _manager.AuthService.GetOneUserForUpdateAsync(userName);

            if (!ModelState.IsValid)
            {
                TempData["error"] = "Lütfen geçerli bilgiler girin.";
                TempData["OpenModal"] = "editPasswordModal"; // modal tekrar açılsın
                return View("Profile", new ProfileModel()
                {
                    User = await _manager.AuthService.GetOneUserAsync(userName),
                    Orders = await _manager.OrderService.GetUserOrdersAsync(userName),
                    UserReviews = await _manager.UserReviewService.GetAllUserReviewsOfOneUserAsync(userId, false),
                    UserDtoForUpdate = user,
                    ChangePasswordDto = model
                });
            }

            try
            {
                var result = await _manager.AuthService.ChangePasswordAsync(model);

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
                    User = await _manager.AuthService.GetOneUserAsync(userName),
                    Orders = await _manager.OrderService.GetUserOrdersAsync(userName),
                    UserReviews = await _manager.UserReviewService.GetAllUserReviewsOfOneUserAsync(userId, false),
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
            string? userName = User.FindFirstValue(ClaimTypes.Name);
            if (userId == null)
            {
                TempData["error"] = "Kullanıcı oturumu bulunamadı.";
                return RedirectToAction("Login");
            }

            // Kullanıcı bilgilerini veritabanından al
            var user = await _manager.AuthService.GetOneUsersFavouritesAsync(userName);
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
                var dbFavorites = user.FavouriteProductsId ?? new List<int>();

                // Cookie'deki verilerle veritabanındakiler aynı değilse güncelle
                if (!favouriteProductIds.OrderBy(id => id).SequenceEqual(dbFavorites.OrderBy(id => id)))
                {
                    var favouriteProductsDto = new UserDtoForFavourites
                    {
                        FavouriteProductsId = favouriteProductIds
                    };
                    await _manager.AuthService.UpdateUserFavouritesAsync(favouriteProductsDto, userName);

                    // Güncellenmiş kullanıcı bilgilerini tekrar al
                    user = await _manager.AuthService.GetOneUsersFavouritesAsync(userName);
                }
            }

            // Kullanıcının favori ürünlerini göster
            var favouriteProducts = await _manager.ProductService.GetFavouriteProductsAsync(user, false);
            return View(favouriteProducts);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddToFavourites(int id)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string? userName = User.FindFirstValue(ClaimTypes.Name);
            if (userId == null)
            {
                return Json(new { success = false, message = "Kullanıcı oturumu bulunamadı.", type = "warning" });
            }

            var user = await _manager.AuthService.GetOneUserForUpdateAsync(userName);
            if (user == null)
            {
                return Json(new { success = false, message = "Kullanıcı bilgileri alınamadı.", type = "danger" });
            }

            if (!user.FavouriteProductsId.Contains(id))
            {
                user.FavouriteProductsId.Add(id);
                await _manager.AuthService.UpdateAsync(user);
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
            string? userName = User.FindFirstValue(ClaimTypes.Name);
            if (userId == null)
            {
                return Json(new { success = false, message = "Kullanıcı oturumu bulunamadı.", type = "warning" });
            }

            var user = await _manager.AuthService.GetOneUserForUpdateAsync(userName);
            if (user == null)
            {
                return Json(new { success = false, message = "Kullanıcı bilgileri alınamadı.", type = "danger" });
            }

            if (user.FavouriteProductsId.Contains(id))
            {
                user.FavouriteProductsId.Remove(id);
                await _manager.AuthService.UpdateAsync(user);
                return Json(new { success = true, message = "Ürün favorilerden kaldırıldı.", type = "success" });
            }
            else
            {
                return Json(new { success = false, message = "Ürün favorilerde yok.", type = "info" });
            }
        }
        public async Task<IActionResult> Summary()
        {
            // Eğer AJAX isteği değilse 404 döndür
            if (!Request.Headers["X-Requested-With"].Equals("XMLHttpRequest"))
            {
                return View("AccessDenied","Account"); // veya Forbid() da kullanılabilir
            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string? userName = User.FindFirstValue(ClaimTypes.Name);
            int favouritesCount = 0;

            if (userId != null)
            {
                var user = await _manager.AuthService.GetOneUsersFavouritesAsync(userName);
                favouritesCount = user.FavouriteProductsId.Count();
            }

            return Json(favouritesCount);
        }

        public IActionResult Notifications()
        {
            return View();
        }

    }
}

