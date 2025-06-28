using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Config
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.ProductId);
            builder.Property(p => p.ProductName).IsRequired();
            builder.Property(p => p.ActualPrice).IsRequired();

            builder.HasOne(p => p.MainCategory)
                .WithMany(m => m.Products)
                .HasForeignKey(p => p.MainCategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.SubCategory)
                .WithMany(sc => sc.Products)
                .HasForeignKey(p => p.SubCategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(c => c.UserReviews)
                .WithOne(sc => sc.Product)
                .HasForeignKey(sc => sc.ProductId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
