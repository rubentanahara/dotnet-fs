using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;

        public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage));

            var comments = await _commentRepo.GetAllAsync();
            var commentsDto = comments.Select(comment => comment.ToCommentDto());
            return Ok(commentsDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage));

            var comment = await _commentRepo.GetByIdAsync(id);
            if (comment == null)
                return NotFound();
            return Ok(comment.ToCommentDto());
        }

        [HttpPost("{stockId:int}")]
        public async Task<IActionResult> Create([FromRoute] int stockId, CreateCommentDto commentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage));

            if (!await _stockRepo.StockExists(stockId))
                return BadRequest("Stock not found");
            Comment commentModel = commentDto.ToCommentFromCreate(stockId);
            await _commentRepo.CreateAsync(commentModel);
            return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.ToCommentDto());
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentDto updatedComment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage));

            var comment = await _commentRepo.UpdateAsync(id, updatedComment.ToCommentFromUpdate());
            if (comment == null)
                return NotFound("Comment not found");
            return Ok(comment.ToCommentDto());
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage));

            var deletedComment = await _commentRepo.DeleteAsync(id);
            if (deletedComment == null)
                return NotFound("Comment not found");
            return Ok(deletedComment);
        }


    }
}
