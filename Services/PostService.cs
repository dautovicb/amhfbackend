using Microsoft.EntityFrameworkCore;
using MentalHealth.Data;
using MentalHealth.Models;
using MentalHealth.Services.Interfaces;
using MentalHealth.Models.Common;

namespace MentalHealth.Services
{
    public class PostService : IPostService
    {
        private readonly ApplicationDbContext _context;

        public PostService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Post>> GetAllAsync(
            int? categoryId = null,
            string? searchTerm = null,
            bool includeCategory = false,
            bool includeComments = false,
            PaginationParams? paginationParams = null,
            string sortOrder = "desc") // Added sortOrder parameter
        {
            IQueryable<Post> query = _context.Posts;
            
            // Apply category filter if provided
            if (categoryId.HasValue)
            {
                query = query.Where(p => p.categoryId == categoryId.Value);
            }

            // Apply search if provided
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim();
                
                // Split search terms and create contains expressions for each word
                var searchTerms = searchTerm.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                
                foreach (var term in searchTerms)
                {
                    var searchTermLocal = term.ToLower();
                    query = query.Where(p => 
                        EF.Functions.Like(p.title.ToLower(), $"%{searchTermLocal}%") || 
                        EF.Functions.Like(p.content.ToLower(), $"%{searchTermLocal}%"));
                }
            }
            
            // Include related data if requested
            if (includeCategory)
            {
                query = query.Include(p => p.Category);
            }
            
            if (includeComments)
            {
                query = query.Include(p => p.Comments);
            }

            // Apply sorting by created date
            query = sortOrder.ToLower() switch
            {
                "asc" => query.OrderBy(p => p.createdAt),
                _ => query.OrderByDescending(p => p.createdAt), // Default to descending
            };

            // Get total count before pagination
            var totalItems = await query.CountAsync();

            // Apply pagination
            if (paginationParams != null)
            {
                query = query.Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                           .Take(paginationParams.PageSize);
            }
            
            var items = await query.ToListAsync();
            return new PagedResult<Post>(items, totalItems, 
                paginationParams?.PageNumber ?? 1, 
                paginationParams?.PageSize ?? totalItems);
        }

        public async Task<Post?> GetByIdAsync(int id, bool includeCategory = false, bool includeComments = false)
        {
            IQueryable<Post> query = _context.Posts;
            
            if (includeCategory)
            {
                query = query.Include(p => p.Category);
            }
            
            if (includeComments)
            {
                query = query.Include(p => p.Comments);
            }
            
            return await query.FirstOrDefaultAsync(p => p.id == id);
        }

        public async Task<Post> CreateAsync(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task<Post> UpdateAsync(Post post)
        {
            _context.Entry(post).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task DeleteAsync(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
        }
    }
}
