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
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, ProductWithRatingDto>();
            CreateMap<ProductDtoForUpdate, Product>().ReverseMap();
            CreateMap<Account, UserDto>().ReverseMap();
            CreateMap<UserDtoForCreation, Account>();
            CreateMap<UserDtoForUpdate, Account>().ReverseMap();
            CreateMap<MainCategoryDtoForInsertion, MainCategory>();
            CreateMap<MainCategory, MainCategoryDto>().ReverseMap();
            CreateMap<MainCategoryDtoForUpdate, MainCategory>().ReverseMap();
            CreateMap<SubCategoryDtoForInsertion, SubCategory>();
            CreateMap<SubCategory, SubCategoryDto>().ReverseMap();
            CreateMap<SubCategoryDtoForUpdate, SubCategory>().ReverseMap();
            CreateMap<UserReviewDtoForInsertion, UserReview>();
            CreateMap<UserReview, UserReviewDto>().ReverseMap();
            CreateMap<UserReviewDtoForUpdate, UserReview>().ReverseMap();
            CreateMap<CartDto, Cart>();
            CreateMap<CartLineDto, CartLine>();
            CreateMap<Order, OrderDto>().ReverseMap();
        }
    }
}
