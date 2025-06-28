using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Account : IdentityUser
    {
        public string? AvatarUrl { get; set; } = "avatars/default.png";
        public string? FirstName { get; set; } 
        public string? LastName { get; set; }
        public DateTime MembershipDate { get; set; } = DateTime.UtcNow;
        public DateTime? BirthDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public ICollection<int> FavouriteProductsId { get; set; } = new List<int>();
    }
}
