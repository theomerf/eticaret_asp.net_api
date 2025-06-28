using Entities.Dtos;
using Entities.Models;
using Microsoft.AspNetCore.Identity;

namespace Services.Contracts
{
    public interface IAuthService
    {
        IEnumerable<IdentityRole> Roles { get; }
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<IdentityResult> CreateUserAsync(UserDtoForCreation userDto);
        Task<UserDto> GetOneUserAsync(string? userName);
        Task<UserDtoForUpdate> GetOneUserForUpdateAsync(string? userName);
        Task<IdentityResult> UpdateAsync(UserDtoForUpdate userDto);
        Task<IdentityResult> UpdateAvatarAsync(UserDtoForUpdate userDto);
        Task<IdentityResult> ResetPasswordAsync(ResetPasswordDto model);
        Task<IdentityResult> ChangePasswordAsync(ChangePasswordDto model);
        Task<IdentityResult> DeleteOneUserAsync(string? userName);
        Task<UserDtoForFavourites> GetOneUsersFavouritesAsync(string? userName);
        Task<IdentityResult> UpdateUserFavouritesAsync(UserDtoForFavourites favouritesDto, string? userName);
    }
}
