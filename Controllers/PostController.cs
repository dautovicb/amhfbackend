using Microsoft.AspNetCore.Mvc;
using MentalHealth.Models;
using MentalHealth.Services.Interfaces;
using MentalHealth.Models.DTOs;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using MentalHealth.Models.Common;

namespace MentalHealth.Controllers
{
    /// <summary>
    /// Controller for managing blog posts
    /// </summary>
    [ApiController]
    [Route("api/posts")]
    [Produces("application/json")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        /// <summary>
        /// Gets all posts with optional filtering, search, and related data
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/posts?searchTerm=health mental&amp;categoryId=1&amp;pageNumber=1&amp;pageSize=10
        /// 
        /// Search supports multiple words and will match any post containing any of the search terms in either title or content.
        /// </remarks>
        /// <param name="categoryId">Optional category id to filter posts by</param>
        /// <param name="searchTerm">Search terms to filter posts. Multiple words will be treated as separate search terms</param>
        /// <param name="includeCategory">If true, includes the category data</param>
        /// <param name="includeComments">If true, includes the comments</param>
        /// <param name="pageNumber">Page number (default: 1)</param>
        /// <param name="pageSize">Page size (default: 10, max: 50)</param>
        /// <returns>Paged list of posts matching the search criteria</returns>
        /// <response code="200">Returns the paged list of posts</response>
        /// <response code="400">If the pagination parameters are invalid</response>
        [HttpGet]
[ProducesResponseType(typeof(PagedResult<Post>), StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
public async Task<ActionResult<PagedResult<Post>>> GetAll(
    [FromQuery, Description("Filter posts by category id")] int? categoryId = null,
    [FromQuery, Description("Search in title and content. Multiple words will match any term")] string? searchTerm = null,
    [FromQuery, Description("Include category data")] bool includeCategory = false,
    [FromQuery, Description("Include comments")] bool includeComments = false,
    [FromQuery, Description("Sort order: 'asc' for oldest first, 'desc' for newest first (default: 'desc')")] string sortOrder = "desc",
    [FromQuery, Description("Page number (min: 1)")] int pageNumber = 1,
    [FromQuery, Description("Page size (min: 1, max: 50)")] int pageSize = 10)
{
    if (pageNumber < 1)
        return BadRequest("Page number must be greater than 0");

    if (pageSize < 1 || pageSize > 50)
        return BadRequest("Page size must be between 1 and 50");

    var paginationParams = new PaginationParams 
    { 
        PageNumber = pageNumber, 
        PageSize = pageSize 
    };

    var result = await _postService.GetAllAsync(
        categoryId, 
        searchTerm,
        includeCategory, 
        includeComments, 
        paginationParams,
        sortOrder);
    return Ok(result);
}


        /// <summary>
        /// Gets a specific post by id with optional related data
        /// </summary>
        /// <param name="id">The post id</param>
        /// <param name="includeCategory">If true, includes the category data</param>
        /// <param name="includeComments">If true, includes the comments</param>
        /// <returns>The requested post</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Post), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Post>> GetById(
            int id,
            [FromQuery, Description("Include category data")] bool includeCategory = false,
            [FromQuery, Description("Include comments")] bool includeComments = false)
        {
            var post = await _postService.GetByIdAsync(id, includeCategory, includeComments);
            if (post == null)
                return NotFound();
            return Ok(post);
        }

        [HttpPost]
        public async Task<ActionResult<Post>> Create(CreatePostDto dto)
        {
            var post = new Post
            {
                categoryId = dto.categoryId,
                title = dto.title,
                content = dto.content
            };

            var createdPost = await _postService.CreateAsync(post);
            return CreatedAtAction(nameof(GetById), new { id = createdPost.id }, createdPost);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdatePostDto dto)
        {
            var post = await _postService.GetByIdAsync(id);
            if (post == null)
                return NotFound();

            post.categoryId = dto.categoryId;
            post.title = dto.title;
            post.content = dto.content;
            await _postService.UpdateAsync(post);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _postService.DeleteAsync(id);
            return NoContent();
        }
    }
} 