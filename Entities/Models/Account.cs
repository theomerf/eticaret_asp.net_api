using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Account : IdentityUser
    {
        public string AvatarUrl { get; set; } = "avatars/default.png";
        public string FirstName { get; set; } 
        public string LastName { get; set; }
        [Column(TypeName = "timestamp without time zone")]
        public DateTime MembershipDate { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "timestamp without time zone")]
        public DateTime? BirthDate { get; set; }
        [Column(TypeName = "timestamp without time zone")]
        public DateTime? LastLoginDate { get; set; }
    }
}
