using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Config
{
    public class CartConfig : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasKey(c => c.CartId);
            builder.Property(c => c.UserId).IsRequired(false);
            
            builder.HasMany(c => c.Lines)
                .WithOne(cl => cl.Cart)
                .HasForeignKey(cl => cl.CartId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
