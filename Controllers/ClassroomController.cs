using ClassroomPlus.DTOs.ClassroomDTOs;
using ClassroomPlus.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ClassroomPlus.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClassroomController : ControllerBase
{
    private readonly IClassroomService _classroomService;

    public ClassroomController(IClassroomService classroomService) 
    {
        _classroomService = classroomService;
    }

    [HttpGet]
    [Authorize]
    [Route("/joke")]
    public IActionResult getSomething() 
    {
        return Ok("Pozdrav djaci");
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> getAll() 
    {
        var user = await _classroomService.getAllAsync();
        return Ok(user);
    }

    [HttpGet]
    [Authorize(Policy = "RequireId")]
    [Route("{id}/{pageNumber}")]
    public async Task<IActionResult> getClassroomById([FromRoute] int id, [FromRoute] int pageNumber)
    {
        var currentUserId = Convert.ToInt32(HttpContext.User.FindFirst("Id")?.Value);
        var classroom = await _classroomService.getClassroomByIdAsync(id, currentUserId, pageNumber);
        return Ok(classroom);
    }

    [HttpGet]
    [Authorize(Policy = "RequireId")]
    [Route("userId")]
    public async Task<IActionResult> getClassroomsByUserId()
    {
        var currentUserId = Convert.ToInt32(HttpContext.User.FindFirst("Id")?.Value);
        var classroom = await _classroomService.getClassroomsByUserIdAsync(currentUserId);
        return Ok(classroom);
    }
         
    [HttpPost]
    [Authorize(Policy = "RequireId")]
    public async Task<IActionResult> createClassroom([FromBody] CreateClassroomDTO classroomDTO)
    {
        var currentUserId = HttpContext.User.FindFirst("Id")?.Value;
        classroomDTO.CreatorId = Convert.ToInt32(currentUserId);
        var classroom = await _classroomService.createClassroomAsync(classroomDTO);
        return Ok(classroom);
    }

    [HttpDelete]
    [Authorize(Policy = "RequireId")]
    [Route("{id}")]
    public async Task<IActionResult> deleteClassroom([FromRoute] int id)
    {
        var currentUserId = Convert.ToInt32(HttpContext.User.FindFirst("Id")?.Value);
        var classroom = await _classroomService.deleteClassroom(id, currentUserId);
        return Ok(classroom);
    }

    [HttpPatch]
    [Authorize(Policy = "RequireId")]
    public async Task<IActionResult> editClassroom([FromBody] EditClassroomDTO classroomDTO)
    {
        var currentUserId = Convert.ToInt32(HttpContext.User.FindFirst("Id")?.Value);
        classroomDTO.CreatorId = currentUserId;
        var classroom = await _classroomService.editClassroomAsync(classroomDTO);
        return Ok(classroom);
    }

    [HttpGet]
    [Authorize(Policy = "RequireId")]
    [Route("joinCode/{joinCode}")]
    public async Task<IActionResult> getClassroomByJoinCodeAsync([FromRoute] string joinCode)
    {
        var currentUserId = Convert.ToInt32(HttpContext.User.FindFirst("Id")?.Value);
        var classroom = await _classroomService.getClassroomByJoinCodeAsync(joinCode, currentUserId);
        return Ok(classroom);
    }

}
