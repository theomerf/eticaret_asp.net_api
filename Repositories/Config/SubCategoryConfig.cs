using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Config
{
    public class SubCategoryConfig : IEntityTypeConfiguration<SubCategory>
    {
        public void Configure(EntityTypeBuilder<SubCategory> builder)
        {
            builder.HasKey(sc => sc.SubCategoryId);
            builder.Property(sc => sc.CategoryName).IsRequired();

            builder.HasOne(sc => sc.MainCategory)
                .WithMany(m => m.SubCategories)
                .HasForeignKey(sc => sc.MainCategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(sc => sc.Products)
                .WithOne(p => p.SubCategory)
                .HasForeignKey(p => p.SubCategoryId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
