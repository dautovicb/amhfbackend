using MentalHealth.Models;
using MentalHealth.Models.Common;

namespace MentalHealth.Services.Interfaces
{
    public interface ICommentService
    {
        Task<PagedResult<Comment>> GetAllAsync(int? postId = null, PaginationParams? paginationParams = null);
        Task<Comment?> GetByIdAsync(int id);
        Task<Comment> CreateAsync(Comment comment);
        Task<Comment> UpdateAsync(Comment comment);
        Task DeleteAsync(int id);
    }
} 