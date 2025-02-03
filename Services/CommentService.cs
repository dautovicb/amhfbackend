using Microsoft.EntityFrameworkCore;
using MentalHealth.Data;
using MentalHealth.Models;
using MentalHealth.Services.Interfaces;
using MentalHealth.Models.Common;

namespace MentalHealth.Services
{
    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext _context;

        public CommentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Comment>> GetAllAsync(int? postId = null, PaginationParams? paginationParams = null)
        {
            IQueryable<Comment> query = _context.Comments;
            
            if (postId.HasValue)
            {
                query = query.Where(c => c.postId == postId.Value);
            }
            
            var totalItems = await query.CountAsync();

            if (paginationParams != null)
            {
                query = query.Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                           .Take(paginationParams.PageSize);
            }
            
            var items = await query.ToListAsync();
            return new PagedResult<Comment>(items, totalItems, 
                paginationParams?.PageNumber ?? 1, 
                paginationParams?.PageSize ?? totalItems);
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            IQueryable<Comment> query = _context.Comments;
            
            return await query.FirstOrDefaultAsync(c => c.id == id);
        }

        public async Task<Comment> CreateAsync(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment> UpdateAsync(Comment comment)
        {
            _context.Entry(comment).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task DeleteAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }
        }
    }
} 