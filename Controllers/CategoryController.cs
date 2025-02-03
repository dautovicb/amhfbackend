using Microsoft.AspNetCore.Mvc;
using MentalHealth.Models;
using MentalHealth.Services.Interfaces;
using MentalHealth.Models.DTOs;
using System.ComponentModel;
using MentalHealth.Exceptions;
using MentalHealth.Models.Common;

namespace MentalHealth.Controllers
{
    [ApiController]
    [Route("api/categories")]
    [Produces("application/json")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Gets all categories with optional related data
        /// </summary>
        /// <param name="pageNumber">Page number (default: 1)</param>
        /// <param name="pageSize">Page size (default: 10, max: 50)</param>
        /// <returns>Paged list of categories</returns>
        [HttpGet]
        public async Task<ActionResult<List<CategoryWithPostCountDto>>> GetAllCategoriesWithPostCount()
        {
            var categoriesWithPostCount = await _categoryService.GetAllCategoriesWithPostCountAsync();
            return Ok(categoriesWithPostCount);
        }

        /// <summary>
        /// Gets a specific category by id with optional related data
        /// </summary>
        /// <param name="id">The category id</param>
        /// <returns>The requested category</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Category), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Category>> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
                return NotFound();
            return Ok(category);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Category), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<Category>> Create(CreateCategoryDto dto)
        {
            var category = new Category
            {
                name = dto.name
            };

            var createdCategory = await _categoryService.CreateAsync(category);
            return CreatedAtAction(nameof(GetById), new { id = createdCategory.id }, createdCategory);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateCategoryDto dto)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
                return NotFound();

            category.name = dto.name;

            await _categoryService.UpdateAsync(category);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.DeleteAsync(id);
            return NoContent();
        }
    }
} 