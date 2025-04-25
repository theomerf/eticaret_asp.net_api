using AutoMapper;
using Entities.Dtos;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using Services.Contracts;

namespace Services
{
    public class MainCategoryManager : IMainCategoryService
    {
        private readonly IRepositoryManager _manager;

        private readonly IMapper _mapper;

        public MainCategoryManager(IRepositoryManager manager, IMapper mapper)
        {
            _manager = manager;
            _mapper = mapper;
        }

        public void CreateCategory(MainCategoryDtoForInsertion categoryDto)
        {
            MainCategory category = _mapper.Map<MainCategory>(categoryDto);
            _manager.MainCategory.CreateCategory(category);
            _manager.Save();
        }

        public IEnumerable<MainCategory> GetAllCategories(bool trackChanges)
        {
            return _manager.MainCategory.GetAllCategories(trackChanges);
        }

        public void UpdateCategory(MainCategoryDtoForUpdate categoryDto)
        {
            var category = _mapper.Map<MainCategory>(categoryDto);
            _manager.MainCategory.UpdateOneCategory(category);
            _manager.Save();
        }

        public MainCategory? GetOneCategory(int id, bool trackChanges)
        {
            var category = _manager.MainCategory.GetOneCategory(id, trackChanges);
            if (category == null)
            {
                throw new Exception("Category not found");
            }
            return category;
        }

        public MainCategoryDtoForUpdate GetOneCategoryForUpdate(int id, bool trackChanges)
        {
            var category = GetOneCategory(id, trackChanges);
            var categoryDto = _mapper.Map<MainCategoryDtoForUpdate>(category);
            return categoryDto;
        }

        public void DeleteOneCategory(int id)
        {
            var category = GetOneCategory(id, false);
            if (category != null)
            {
                _manager.MainCategory.DeleteOneCategory(category);
                _manager.Save();
            }
        }
    }
}
