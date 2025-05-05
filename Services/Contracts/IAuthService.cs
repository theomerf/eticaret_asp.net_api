using Entities.Dtos;
using Entities.Models;
using Microsoft.AspNetCore.Identity;

namespace Services.Contracts
{
    public interface IAuthService
    {
        IEnumerable<IdentityRole> Roles { get; }
        IEnumerable<Entities.Models.Account> GetAllUsers();
        Task<IdentityResult> CreateUser(UserDtoForCreation userDto);
        Task<Entities.Models.Account> GetOneUser(string userName);
        Task<UserDtoForUpdate> GetOneUserForUpdate(string userName);
        Task<IdentityResult> Update(UserDtoForUpdate userDto);
        Task<IdentityResult> UpdateAvatar(UserDtoForUpdate userDto);
        Task<IdentityResult> ResetPassword(ResetPasswordDto model);
        Task<IdentityResult> ChangePassword(ChangePasswordDto model);
        Task<IdentityResult> DeleteOneUser(string userName);
        Task<ICollection<int>> GetOneUsersFavourites(string userName);
        Task<IdentityResult> UpdateUserFavourites(List<int> favouritesId, string userName);
    }
}
