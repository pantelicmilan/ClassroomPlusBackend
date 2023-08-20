using ClassroomPlus.DTOs.UsersClassroomsDTOs;
using ClassroomPlus.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClassroomPlus.Controllers;

[Route("api/UsersClassroomsRelationship")]
[ApiController]
public class ClassroomEnrollmentController : ControllerBase
{
    private readonly IClassroomEnrollmentService _classroomEnrollmentService;
   
    public ClassroomEnrollmentController(IClassroomEnrollmentService classroomEnrollmentService) 
    {
        _classroomEnrollmentService = classroomEnrollmentService;
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> getAll() 
    {
        var classroomsEnrollments = await _classroomEnrollmentService.getAll();
        return Ok(classroomsEnrollments);
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    [Route("{id}")]
    public async Task<IActionResult> getClassroomEnrollmentById(int id)
    {
        var classroomEnrollment = await _classroomEnrollmentService.getClassroomEnrollmentById(id);
        return Ok(classroomEnrollment);
    }

    [HttpGet]
    [Authorize(Policy = "RequireId")]
    [Route("userId")]
    public async Task<IActionResult> getClassroomsEnrollmentsByUserId()
    {
        int currentUserId = Convert.ToInt32(HttpContext.User.FindFirst("Id")?.Value);
        var classroomEnrollment = await _classroomEnrollmentService.getClassroomsEnrollmentsByUserId(currentUserId);
        return Ok(classroomEnrollment);
    }

    [HttpGet]
    [Authorize(Policy = "RequireId")]
    [Route("classroom/{classroomId}")]
    public async Task<IActionResult> getClassroomsEnrollmentsByClassroomId([FromRoute] int classroomId)
    {
        var classroomsEnrollments = await _classroomEnrollmentService.getClassroomsEnrollmentsByClassroomId(classroomId);
        return Ok(classroomsEnrollments);
    }

    [HttpDelete]
    [Authorize(Policy = "RequireId")]
    [Route("{id}")]
    public async Task<IActionResult> deleteClassroomEnrollment([FromRoute] int id)
    {
        int currentUserId = Convert.ToInt32(HttpContext.User.FindFirst("Id")?.Value);
        var deletedClassroomEnrollment = await _classroomEnrollmentService.deleteClassroomEnrollment(id, currentUserId);
        return Ok(deletedClassroomEnrollment);
    }

    [HttpGet]
    [Route("joinCode/{joinCode}")]
    [Authorize(Policy = "RequireId")]
    public async Task<IActionResult> createClassroomEnrollment([FromRoute] string joinCode)
    {
        var classroomEnrollmentDTO = new CreateClassroomEnrollmentDTO();
        classroomEnrollmentDTO.JoinCode = joinCode;

        int currentUserId = Convert.ToInt32(HttpContext.User.FindFirst("Id")?.Value);
        classroomEnrollmentDTO.UserId = currentUserId;

        var classroomEnrollment = await _classroomEnrollmentService.createClassroomEnrollment(classroomEnrollmentDTO);
        return Ok(classroomEnrollment);
    }

    [HttpDelete]
    [Authorize(Policy = "RequireId")]
    [Route("whereClassroomIdAndUserId/{classroomId}")]
    public async Task<IActionResult> deleteClassroomEnrollmentWhereClassroomIdAndUserId(int classroomId) 
    {
        int currentUserId = Convert.ToInt32(HttpContext.User.FindFirst("Id")?.Value);
        var deletedClassroomEnrollment = await _classroomEnrollmentService.deleteClassroomEnrollmentWhereClassroomIdAndUserId(classroomId, currentUserId);
        return Ok(deletedClassroomEnrollment);
    }

}
