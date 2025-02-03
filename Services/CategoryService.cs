using Microsoft.EntityFrameworkCore;
using MentalHealth.Data;
using MentalHealth.Models.DTOs; 
using MentalHealth.Models;
using MentalHealth.Services.Interfaces;
using MentalHealth.Models.Common;
using MentalHealth.Exceptions;

namespace MentalHealth.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

         public async Task<List<CategoryWithPostCountDto>> GetAllCategoriesWithPostCountAsync()
        {
            var categoriesWithPostCount = await _context.Categories
                .Select(c => new CategoryWithPostCountDto
                {
                    CategoryId = c.id,
                    CategoryName = c.name,
                    PostCount = _context.Posts.Count(p => p.categoryId == c.id)  // Count posts for each category
                })
                .OrderByDescending(c => c.PostCount)
                .ToListAsync();

            return categoriesWithPostCount;
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.id == id);
        }

        public async Task<Category> CreateAsync(Category category)
        {
            // Check for duplicate name
            var exists = await _context.Categories
                .AnyAsync(c => c.name.ToLower() == category.name.ToLower());
            
            if (exists)
            {
                throw new DuplicateNameException(category.name);
            }

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
} 