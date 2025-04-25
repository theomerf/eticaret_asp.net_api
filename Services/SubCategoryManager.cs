using AutoMapper;
using Entities.Dtos;
using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;

namespace Services
{
    public class SubCategoryManager : ISubCategoryService
    {
        private readonly IRepositoryManager _manager;

        private readonly IMapper _mapper;

        public SubCategoryManager(IRepositoryManager manager, IMapper mapper)
        {
            _manager = manager;
            _mapper = mapper;
        }

        public void CreateCategory(SubCategoryDtoForInsertion categoryDto)
        {
            SubCategory category = _mapper.Map<SubCategory>(categoryDto);
            _manager.SubCategory.CreateCategory(category);
            _manager.Save();
        }

        public IEnumerable<SubCategory> GetAllCategories(bool trackChanges)
        {
            return _manager.SubCategory.GetAllCategories(trackChanges);
        }

        public void UpdateCategory(SubCategoryDtoForUpdate categoryDto)
        {
            var category = _mapper.Map<SubCategory>(categoryDto);
            _manager.SubCategory.UpdateOneCategory(category);
            _manager.Save();
        }

        public SubCategory? GetOneCategory(int id, bool trackChanges)
        {
            var category = _manager.SubCategory.GetOneCategory(id, trackChanges);
            if (category == null)
            {
                throw new Exception("Category not found");
            }
            return category;
        }

        public SubCategoryDtoForUpdate GetOneCategoryForUpdate(int id, bool trackChanges)
        {
            var category = GetOneCategory(id, trackChanges);
            var categoryDto = _mapper.Map<SubCategoryDtoForUpdate>(category);
            return categoryDto;
        }

        public void DeleteOneCategory(int id)
        {
            var category = GetOneCategory(id, false);
            if (category != null)
            {
                _manager.SubCategory.DeleteOneCategory(category);
                _manager.Save();
            }
        }
    }
}
