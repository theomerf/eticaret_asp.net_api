using Microsoft.AspNetCore.Identity;

namespace Entities.Models
{
    public class Account : IdentityUser
    {
        public string AvatarUrl { get; set; } = "avatars/default.png";
        public string FirstName { get; set; } 
        public string LastName { get; set; }
        public DateTime MembershipDate { get; set; } = DateTime.Now;
        public DateTime? BirthDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
    }
}
