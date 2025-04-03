using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Config
{
    public class MainCategoryConfig : IEntityTypeConfiguration<MainCategory>
    {
        public void Configure(EntityTypeBuilder<MainCategory> builder)
        {
            builder.HasKey(c => c.MainCategoryId);
            builder.Property(c => c.CategoryName).IsRequired();

            builder.HasMany(c => c.SubCategories)
                .WithOne(sc => sc.MainCategory)
                .HasForeignKey(sc => sc.MainCategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(c => c.Products)
                .WithOne(p => p.MainCategory)
                .HasForeignKey(p => p.MainCategoryId)
                .OnDelete(DeleteBehavior.NoAction);
            
        }
    }
}
