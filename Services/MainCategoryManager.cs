using AutoMapper;
using Entities.Dtos;
using Entities.Exceptions;
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

        public async Task CreateCategoryAsync(MainCategoryDtoForInsertion categoryDto)
        {
            MainCategory category = _mapper.Map<MainCategory>(categoryDto);
            _manager.MainCategory.CreateCategory(category);
            await _manager.SaveAsync();
        }

        public async Task<IEnumerable<MainCategoryDto>> GetAllCategoriesAsync(bool trackChanges)
        {
            var categories = await _manager.MainCategory.GetAllCategoriesAsync(trackChanges);
            return _mapper.Map<IEnumerable<MainCategoryDto>>(categories);
        }

        public async Task UpdateCategoryAsync(MainCategoryDtoForUpdate categoryDto)
        {
            var category = _mapper.Map<MainCategory>(categoryDto);
            _manager.MainCategory.UpdateOneCategory(category);
            await _manager.SaveAsync();
        }

        public async Task<MainCategoryDto?> GetOneCategoryAsync(int id, bool trackChanges)
        {
            var category = await _manager.MainCategory.GetOneCategoryAsync(id, trackChanges);
            if (category == null)
            {
                throw new CategoryNotFoundException(id);
            }
            return _mapper.Map<MainCategoryDto>(category);
        }

        public async Task<MainCategoryDtoForUpdate> GetOneCategoryForUpdateAsync(int id, bool trackChanges)
        {
            var category = await GetOneCategoryAsync(id, trackChanges);
            var categoryDto = _mapper.Map<MainCategoryDtoForUpdate>(_mapper.Map<MainCategory>(category));
            return categoryDto;
        }

        public async Task DeleteOneCategoryAsync(int id)
        {
            var category = await GetOneCategoryAsync(id, false);
            if (category != null)
            {
                _manager.MainCategory.DeleteOneCategory(_mapper.Map<MainCategory>(category));
                await _manager.SaveAsync();
            }
        }
    }
}
