using AutoMapper;
using Entities.Dtos;
using Entities.Models;
using Microsoft.AspNetCore.Identity;

namespace ETicaret.Infrastructe.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductDtoForInsertion, Product>();
            CreateMap<ProductDtoForUpdate, Product>().ReverseMap();
            CreateMap<UserDtoForCreation, Account>();
            CreateMap<UserDtoForUpdate, Account>().ReverseMap();
            CreateMap<MainCategoryDtoForInsertion, MainCategory>();
            CreateMap<MainCategoryDtoForUpdate, MainCategory>().ReverseMap();
            CreateMap<SubCategoryDtoForInsertion, SubCategory>();
            CreateMap<SubCategoryDtoForUpdate, SubCategory>().ReverseMap();
            CreateMap<UserReviewDtoForInsertion, UserReview>();
            CreateMap<UserReviewDtoForUpdate, UserReview>().ReverseMap();
        }
    }
}
