using Entities.Dtos;

namespace Entities.Dtos
{
    public record UserDtoForUpdate : UserDto
    {
        public HashSet<string> UserRoles { get; set; } = new HashSet<string>();
        public string? AvatarUrl { get; set; }
    }
}