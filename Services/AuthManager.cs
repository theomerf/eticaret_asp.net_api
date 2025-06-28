using AutoMapper;
using Entities.Dtos;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AuthManager : IAuthService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<Account> _userManager;
        private readonly IMapper _mapper;

        public AuthManager(RoleManager<IdentityRole> roleManager, UserManager<Account> userManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
        }

        public IEnumerable<IdentityRole> Roles => _roleManager.Roles;

        public async Task<IdentityResult> CreateUserAsync(UserDtoForCreation userDto)
        {
            var user = _mapper.Map<Account>(userDto);

            var result = await _userManager.CreateAsync(user, userDto.Password!);

            if (!result.Succeeded)
            {
                throw new Exception("User could not be created");
            }
            if (userDto.Roles?.Count > 0)
            {
                var roleResult = await _userManager.AddToRolesAsync(user, userDto.Roles!);
                if (!roleResult.Succeeded)
                {
                    throw new Exception("User could not be created");
                }
            }

            return result;
        }

        public async Task<IdentityResult> DeleteOneUserAsync(string? userName)
        {
            var user = await GetOneUserForServiceAsync(userName);
            return await _userManager.DeleteAsync(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userManager.Users
                .OrderBy(u => u.MembershipDate)
                .ToListAsync();

            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<Account> GetOneUserForServiceAsync(string? userName)
        {
            var user = await _userManager.FindByNameAsync(userName ?? "");
            if (user != null)
            {
                return user;
            }
            throw new Exception("Kullanıcı bulunamadı.");
        }

        public async Task<UserDto> GetOneUserAsync(string? userName)
        {
            var user = await _userManager.FindByNameAsync(userName ?? "");

            if (user != null)
            {
                var userDto = _mapper.Map<UserDto>(user);
                return userDto;
            }
            throw new Exception("Kullanıcı bulunamadı.");
        }

        public async Task<UserDtoForUpdate> GetOneUserForUpdateAsync(string? userName)
        {
            var user = await GetOneUserForServiceAsync(userName);

            var userDtoForUpdate = _mapper.Map<UserDtoForUpdate>(user);
            userDtoForUpdate.RolesList = new HashSet<string?>(Roles.Select(r => r.Name).ToList());
            userDtoForUpdate.Roles = new HashSet<string?>(await _userManager.GetRolesAsync(user));
            return userDtoForUpdate;
        }

        public async Task<UserDtoForFavourites> GetOneUsersFavouritesAsync(string? userName)
        {
            var favourites = await _userManager.Users
                .Where(u => u.UserName == userName)
                .Select(u => u.FavouriteProductsId)
                .FirstOrDefaultAsync();

            if (favourites != null)
            {
                var favouritesDto = new UserDtoForFavourites
                {
                    FavouriteProductsId = favourites
                };

                return favouritesDto;
            }

            throw new Exception("User not found or no favourites available.");
        }

        public async Task<IdentityResult> UpdateUserFavouritesAsync(UserDtoForFavourites favouritesDto, string? userName)
        {
            var user = await _userManager.FindByNameAsync(userName ?? "");
            if (user == null)
                throw new Exception("Kullanıcı bulunamadı");

            user.FavouriteProductsId = favouritesDto.FavouriteProductsId;

            var result = await _userManager.UpdateAsync(user);

            return result;

        }

        public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordDto model)
        {
            var user = await GetOneUserForServiceAsync(model.UserName);

            await _userManager.RemovePasswordAsync(user);

            if(model.Password == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Şifre boş olamaz." });
            }

            var result = await _userManager.AddPasswordAsync(user, model.Password);
            return result;
        }

        public async Task<IdentityResult> ChangePasswordAsync(ChangePasswordDto model)
        {
            var user = await GetOneUserForServiceAsync(model.UserName);

            if (model.Password != null && model.NewPassword != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);
                return result;
            }
            return IdentityResult.Failed(new IdentityError { Description = "Şifre değişikliği başarısız oldu." });
        }

        public async Task<IdentityResult> UpdateAsync(UserDtoForUpdate userDtoForUpdate)
        {
            var user = await GetOneUserForServiceAsync(userDtoForUpdate.UserName);

            user.FirstName = userDtoForUpdate.FirstName;
            user.LastName = userDtoForUpdate.LastName;
            user.PhoneNumber = userDtoForUpdate.PhoneNumber;
            user.Email = userDtoForUpdate.Email;
            user.FavouriteProductsId = userDtoForUpdate.FavouriteProductsId;

            var result = await _userManager.UpdateAsync(user);
            if (userDtoForUpdate.Roles?.Count > 0)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, userRoles);
                await _userManager.AddToRolesAsync(user, userDtoForUpdate.Roles!);
            }

            return result;

        }

        public async Task<IdentityResult> UpdateAvatarAsync(UserDtoForUpdate userDtoForUpdate)
        {
            var user = await GetOneUserForServiceAsync(userDtoForUpdate.UserName);

            if(userDtoForUpdate.AvatarUrl != null)
            {
                user.AvatarUrl = userDtoForUpdate.AvatarUrl;
            }

            var result = await _userManager.UpdateAsync(user);
            return result;
        }
    }
}