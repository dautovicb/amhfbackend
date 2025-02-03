using Microsoft.AspNetCore.Mvc;
using MentalHealth.Models;
using MentalHealth.Services.Interfaces;
using MentalHealth.Models.DTOs;
using System.ComponentModel;
using MentalHealth.Models.Common;

namespace MentalHealth.Controllers
{
    [ApiController]
    [Route("api/comments")]
    [Produces("application/json")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        /// <summary>
        /// Gets all comments with optional post filtering and pagination
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     GET /api/comments?postId=1&amp;pageNumber=1&amp;pageSize=10
        /// </remarks>
        /// <param name="postId">Optional post id to filter comments by</param>
        /// <param name="pageNumber">Page number (default: 1)</param>
        /// <param name="pageSize">Page size (default: 10, max: 50)</param>
        /// <returns>Paged list of comments</returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<Comment>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<Comment>>> GetAll(
            [FromQuery, Description("Filter comments by post id")] int? postId = null,
            [FromQuery, Description("Page number")] int pageNumber = 1,
            [FromQuery, Description("Page size")] int pageSize = 10)
        {
            var paginationParams = new PaginationParams 
            { 
                PageNumber = pageNumber, 
                PageSize = pageSize 
            };
            
            var result = await _commentService.GetAllAsync(postId, paginationParams);
            return Ok(result);
        }

        /// <summary>
        /// Gets a specific comment by id
        /// </summary>
        /// <param name="id">The comment id</param>
        /// <returns>The requested comment</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Comment), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Comment>> GetById(int id)
        {
            var comment = await _commentService.GetByIdAsync(id);
            if (comment == null)
                return NotFound();
            return Ok(comment);
        }

        [HttpPost]
        public async Task<ActionResult<Comment>> Create(CreateCommentDto dto)
        {
            var comment = new Comment
            {
                postId = dto.postId,
                content = dto.content
            };

            var createdComment = await _commentService.CreateAsync(comment);
            return CreatedAtAction(nameof(GetById), new { id = createdComment.id }, createdComment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateCommentDto dto)
        {
            var comment = await _commentService.GetByIdAsync(id);
            if (comment == null)
                return NotFound();

            comment.content = dto.content;

            await _commentService.UpdateAsync(comment);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _commentService.DeleteAsync(id);
            return NoContent();
        }
    }
} 