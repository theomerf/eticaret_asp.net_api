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

        public async Task<IdentityResult> CreateUser(UserDtoForCreation userDto)
        {
            var user = _mapper.Map<Account>(userDto);
            var result = await _userManager.CreateAsync(user, userDto.Password);

            if (!result.Succeeded)
            {
                throw new Exception("User could not be created");
            }
            if (userDto.Roles.Count > 0)
            {
                var roleResult = await _userManager.AddToRolesAsync(user, userDto.Roles);
                if (!roleResult.Succeeded)
                {
                    throw new Exception("User could not be created");
                }
            }

            return result;
        }

        public async Task<IdentityResult> DeleteOneUser(string userName)
        {
            var user = await GetOneUser(userName);
            return await _userManager.DeleteAsync(user);
        }

        public IEnumerable<Account> GetAllUsers()
        {
            return _userManager.Users
                .OrderBy(u => u.MembershipDate)
                .ToList();
        }

        public async Task<Account> GetOneUser(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                return user;
            }
            throw new Exception("User could not be found.");
        }

        public async Task<UserDtoForUpdate> GetOneUserForUpdate(string userName)
        {
            var user = await GetOneUser(userName);

            var userDto = _mapper.Map<UserDtoForUpdate>(user);
            userDto.Roles = new HashSet<string>(Roles.Select(r => r.Name).ToList());
            userDto.UserRoles = new HashSet<string>(await _userManager.GetRolesAsync(user));
            return userDto;
        }

        public async Task<ICollection<int>> GetOneUsersFavourites(string userName)
        {
            var favourites = await _userManager.Users
                .Where(u => u.UserName == userName)
                .Select(u => u.FavouriteProductsId)
                .FirstOrDefaultAsync();

            if (favourites != null)
            {
                return favourites;
            }

            throw new Exception("User not found or no favourites available.");
        }

        public async Task<IdentityResult> ResetPassword(ResetPasswordDto model)
        {
            var user = await GetOneUser(model.UserName);
            await _userManager.RemovePasswordAsync(user);
            var result = await _userManager.AddPasswordAsync(user, model.Password);
            return result;
        }

        public async Task<IdentityResult> ChangePassword(ChangePasswordDto model)
        {
            var user = await GetOneUser(model.UserName);
            var result = await _userManager.ChangePasswordAsync(user,model.Password,model.NewPassword);
            return result;
        }

        public async Task<IdentityResult> Update(UserDtoForUpdate userDto)
        {
            var user = await GetOneUser(userDto.UserName);
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.PhoneNumber = userDto.PhoneNumber;
            user.Email = userDto.Email;
            user.FavouriteProductsId = userDto.FavouriteProductsId;

            var result = await _userManager.UpdateAsync(user);
            if (userDto.Roles.Count() > 0)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var r1 = await _userManager.RemoveFromRolesAsync(user, userRoles);
                var r2 = await _userManager.AddToRolesAsync(user, userDto.Roles);
            }

            return result;

        }

        public async Task<IdentityResult> UpdateAvatar(UserDtoForUpdate userDto)
        {
            var user = await GetOneUser(userDto.UserName);
            user.AvatarUrl = userDto.AvatarUrl;


            var result = await _userManager.UpdateAsync(user);

            return result;

        }
    }
}
