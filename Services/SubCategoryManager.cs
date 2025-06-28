using AutoMapper;
using Entities.Dtos;
using Entities.Exceptions;
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

        public async Task CreateCategoryAsync(SubCategoryDtoForInsertion categoryDto)
        {
            SubCategory category = _mapper.Map<SubCategory>(categoryDto);
            _manager.SubCategory.CreateCategory(category);
            await _manager.SaveAsync();
        }

        public async Task<IEnumerable<SubCategoryDto>> GetAllCategoriesAsync(bool trackChanges)
        {
            var categories = await _manager.SubCategory.GetAllCategoriesAsync(trackChanges);
            return _mapper.Map<IEnumerable<SubCategoryDto>>(categories);
        }

        public async Task UpdateCategoryAsync(SubCategoryDtoForUpdate categoryDto)
        {
            var category = _mapper.Map<SubCategory>(categoryDto);
            _manager.SubCategory.UpdateOneCategory(category);
            await _manager.SaveAsync();
        }

        public async Task<SubCategoryDto?> GetOneCategoryAsync(int id, bool trackChanges)
        {
            var category = await _manager.SubCategory.GetOneCategoryAsync(id, trackChanges);
            if (category == null)
            {
                throw new CategoryNotFoundException(id);
            }
            return _mapper.Map<SubCategoryDto>(category);
        }

        public async Task<SubCategoryDtoForUpdate> GetOneCategoryForUpdateAsync(int id, bool trackChanges)
        {
            var category = await GetOneCategoryAsync(id, trackChanges);
            var categoryDto = _mapper.Map<SubCategoryDtoForUpdate>(_mapper.Map<SubCategory>(category));
            return categoryDto;
        }

        public async Task DeleteOneCategoryAsync(int id)
        {
            var category = await GetOneCategoryAsync(id, false);
            if (category != null)
            {
                _manager.SubCategory.DeleteOneCategory(_mapper.Map<SubCategory>(category));
                await _manager.SaveAsync();
            }
        }
    }
}
