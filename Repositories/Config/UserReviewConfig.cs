using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Config
{
    public class UserReviewConfig : IEntityTypeConfiguration<UserReview>
    {
        public void Configure(EntityTypeBuilder<UserReview> builder)
        {
            builder.HasKey(ur => ur.UserReviewId);
            builder.Property(ur => ur.Rating).IsRequired();

            builder.HasOne(ur => ur.User)
                .WithMany()
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ur => ur.Product)
                .WithMany(p => p.UserReviews)
                .HasForeignKey(ur => ur.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
