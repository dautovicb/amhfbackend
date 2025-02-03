using MentalHealth.Models;
using MentalHealth.Models.Common;
using MentalHealth.Models.DTOs;
namespace MentalHealth.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryWithPostCountDto>> GetAllCategoriesWithPostCountAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<Category> CreateAsync(Category category);
        Task<Category> UpdateAsync(Category category);
        Task DeleteAsync(int id);
    }
} 