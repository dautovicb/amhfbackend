using MentalHealth.Models;
using MentalHealth.Models.Common;

namespace MentalHealth.Services.Interfaces
{
    public interface IPostService
    {
        Task<PagedResult<Post>> GetAllAsync(
            int? categoryId = null, 
            string? searchTerm = null,
            bool includeCategory = false, 
            bool includeComments = false, 
            PaginationParams? paginationParams = null,
            string sortOrder = "desc"); // Added sortOrder parameter

        Task<Post?> GetByIdAsync(int id, bool includeCategory = false, bool includeComments = false);
        Task<Post> CreateAsync(Post post);
        Task<Post> UpdateAsync(Post post);
        Task DeleteAsync(int id);
    }
}
