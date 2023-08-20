using ClassroomPlus.DTOs.PostDTOs;
using ClassroomPlus.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClassroomPlus.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly IPostService _postService;

    public PostController(IPostService postService)
    {
        _postService = postService;
    }

    [HttpPost]
    [Authorize(Policy = "RequireId")]
    public async Task<IActionResult> createPost([FromBody] CreatePostDTO postDTO) 
    {
        int currentUserId = Convert.ToInt32(HttpContext.User.FindFirst("Id")?.Value);
        postDTO.CreatorId = currentUserId;
        var post = await _postService.createPostAsync(postDTO);       
        return Ok(post);
    }

    [HttpDelete]
    [Authorize(Policy = "RequireId")]
    [Route("{id}")]
    public async Task<IActionResult> deletePostById([FromRoute] int id)
    {
        int currentUserId = Convert.ToInt32(HttpContext.User.FindFirst("Id")?.Value);
        var delete = await _postService.deletePostAsync(id, currentUserId);
        return Ok(delete);
    }
    
    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> getAll()
    { 
        var posts = await _postService.getAllAsync();
        return Ok(posts);
    }

    [HttpGet]
    [Authorize(Policy = "RequireId")]
    [Route("{id}")]
    public async Task<IActionResult> getPostById(int id)
    {
        int currentUserId = Convert.ToInt32(HttpContext.User.FindFirst("Id")?.Value);
        var post = await _postService.getPostByIdAsync(id, currentUserId);
        return Ok(post);
    }

    [HttpGet]
    [Authorize(Policy = "RequireId")]
    [Route("classroom/{classroomId}")]
    public async Task<IActionResult> getPostByClassroomId(int classroomId)
    {
        int currentUserId = Convert.ToInt32(HttpContext.User.FindFirst("Id")?.Value);
        var post = await _postService.getPostsByClassroomIdAsync(classroomId, currentUserId);
        return Ok(post);
    }

    [HttpGet]
    [Authorize(Policy = "RequireId")]
    [Route("classroom/{classroomId}/pageSize={pageSize}/currentPage={currentPage}")]
    public async Task<IActionResult> getPaginatedPostListByClassroomId(
        [FromRoute] int classroomId, 
        [FromRoute] int pageSize, 
        [FromRoute] int currentPage)
    {
        int currentUserId = Convert.ToInt32(HttpContext.User.FindFirst("Id")?.Value);
        var posts = await _postService.getPostsByClassroomIdAsync(
            classroomId, 
            pageSize, 
            currentPage, 
            currentUserId);
        return Ok(posts);
    }

    [HttpPatch]
    [Authorize(Policy = "RequireId")]
    public async Task<IActionResult> editPost([FromBody] EditPostDTO postDTO)
    {
        int currentUserId = Convert.ToInt32(HttpContext.User.FindFirst("Id")?.Value);
        postDTO.EditorId = currentUserId;

        var post = await _postService.editPostAsync(postDTO);
        return Ok(post);
    }

}
