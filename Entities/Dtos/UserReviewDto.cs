namespace Entities.Dtos
{
    public record UserReviewDto
    {
        public int UserReviewId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public string ReviewText { get; set; } = string.Empty;
        public string ReviewTitle { get; set; } = string.Empty;
        public string? ReviewPictureUrl { get; set; } = string.Empty;
    }
}
