using Entities.Dtos;

namespace Entities.Dtos
{
    public record UserDtoForUpdate : UserDto
    {
        public HashSet<string?> RolesList { get; set; } = new HashSet<string?>();
    }
}