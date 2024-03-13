using Data.Entities;
using WebApi.Models.Category;

namespace WebApi.Services.Interfaces
{
    public interface ICategoryControllerHelper
    {
        Task AddCategoryAsync(CategoryCreateViewModel model);
        Task UpdateCategoryAsync(CategoryUpdateViewModel model);
    }
}