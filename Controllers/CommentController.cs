using ClassroomPlus.DTOs.CommentDTOs;
using ClassroomPlus.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClassroomPlus.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService) 
    {
        _commentService = commentService;
    }

    [HttpPost]
    [Authorize(Policy = "RequireId")]
    public async Task<IActionResult> createComment([FromBody] CreateCommentDTO commentDTO)
    {
        var currentUserId = Convert.ToInt32(HttpContext.User.FindFirst("Id")?.Value);
        commentDTO.UserId = currentUserId;

        var comment = await _commentService.createCommentAsync(commentDTO);
        return Ok(comment);
    }

    [HttpPatch]
    [Authorize(Policy = "RequireId")]
    public async Task<IActionResult> editComment([FromBody] EditCommentDTO commentDTO)
    {
        var currentUserId = Convert.ToInt32(HttpContext.User.FindFirst("Id")?.Value);
        commentDTO.UserId= currentUserId;
        var editedComment = await _commentService.editCommentAsync(commentDTO);
        return Ok(editedComment);
    }

    [HttpDelete]
    [Authorize(Policy = "RequireId")]
    [Route("{commentId}")]
    public async Task<IActionResult> deleteCommentById([FromRoute] int commentId)
    {
        var currentUserId = Convert.ToInt32(HttpContext.User.FindFirst("Id")?.Value);
        var deletedComment = await _commentService.deleteCommentById(commentId, currentUserId);
        return Ok(deletedComment);
    }

}
