using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace ETicaret.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IServiceManager _manager;

        public UserController(IServiceManager manager)
        {
            _manager = manager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _manager.AuthService.GetAllUsersAsync();
            return View(users);
        }

        public IActionResult Create()
        {
            return View(new UserDtoForCreation()
            {
                Roles = new HashSet<string?>(_manager
                .AuthService
                .Roles
                .Select(r => r.Name)
                .ToList())
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] UserDtoForCreation userDto)
        {
            var result = await _manager.AuthService.CreateUserAsync(userDto);
            if (result.Succeeded)
            {
                TempData["success"] = "Kullanıcı başarıyla oluşturuldu.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["danger"] = "Kullanıcı oluştururken bir hata oluştu.";
                return View();
            }
        }

        public async Task<IActionResult> Update([FromRoute(Name = "id")] string id)
        {
            var user = await _manager.AuthService.GetOneUserForUpdateAsync(id);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromForm] UserDtoForUpdate userDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["danger"] = "Kullanıcı güncellenirken bir hata oluştu.";
                return View(userDto);
            }

            var result = await _manager.AuthService.UpdateAsync(userDto);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Error", error.Description);
                }
                TempData["danger"] = "Kullanıcı güncellenirken bir hata oluştu.";
                return View(userDto);
            }
            TempData["success"] = "Kullanıcı başarıyla güncellendi.";
            return RedirectToAction("Index");
        }


        public IActionResult ResetPassword([FromRoute(Name = "id")] string id)
        {
            return View(new ResetPasswordDto() { UserName = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                TempData["danger"] = "Kullanıcı şifresi değiştirilirken bir hata oluştu.";
                return View(model);
            }

            var result = await _manager.AuthService.ResetPasswordAsync(model);
            if (result.Succeeded)
            {
                TempData["success"] = "Kullanıcı şifresi başarıyla değiştirildi.";
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Error", error.Description);
                }
            }
            TempData["danger"] = "Kullanıcı şifresi değiştirilirken bir hata oluştu.";
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOneUser([FromForm] UserDto userDto)
        {
            var result = await _manager.AuthService.DeleteOneUserAsync(userDto.UserName);

            if (result.Succeeded)
            {
                TempData["success"] = "Kullanıcı başarıyla silindi."; 
                return RedirectToAction("Index");
            }
            else 
            {
                TempData["danger"] = "Kullanıcı silinirken bir hata oluştu.";
                return View();
            }
        }
    }
}
