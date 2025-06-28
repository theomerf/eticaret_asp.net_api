using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public record UserReviewDtoForUpdate : UserReviewDto
    {
        public string? ReviewUpdateDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");
    }
}
