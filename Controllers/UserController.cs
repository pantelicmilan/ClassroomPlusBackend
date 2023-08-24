using Microsoft.AspNetCore.Mvc;
using ClassroomPlus.Services.Interfaces;
using ClassroomPlus.DTOs.UserDTOs;
using Microsoft.AspNetCore.Authorization;

namespace ClassroomPlus.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService) 
    {
        _userService = userService;
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> getAllUsers() 
    {
        var users = await _userService.getAllUsersAsync();          
        return Ok(users);
    }

    [HttpGet]
    [Authorize(Policy = "RequireId")]
    [Route("id")]
    public async Task<IActionResult> getUserById() 
    {
        int currentUserId = Convert.ToInt32(HttpContext.User.FindFirst("Id")?.Value);
        var user = await _userService.getUserByIdAsync(currentUserId);
        return Ok(user);
    }

    [HttpPut]
    [Authorize(Policy = "RequireId")]
    public async Task<IActionResult> editUser([FromBody] EditUserDTO userDTO)
    {
        int currentUserId = Convert.ToInt32(HttpContext.User.FindFirst("Id")?.Value);
        userDTO.Id = currentUserId;

        var user = await _userService.editUserInformationAsync(userDTO);
        return Ok(user);
    }

    [HttpDelete]
    [Authorize(Policy = "RequireId")]
    public async Task<IActionResult> deleteUser()
    {
        int currentUserId = Convert.ToInt32(HttpContext.User.FindFirst("Id")?.Value);
        var user = await _userService.deleteUserAsync(currentUserId);
        return Ok(user);
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> loginUser([FromBody] LoginUserDTO userDTO)
    {
        var jwt = await _userService.loginUserAsync(userDTO);
        return Ok(jwt);
    }

    [HttpGet]
    [Authorize(Policy = "RequireId")]
    [Route("dashboard")]
    public async Task<IActionResult> getDashboard()
    {
        int currentUserId = Convert.ToInt32(HttpContext.User.FindFirst("Id")?.Value);
        var fullUser = await _userService.getUserByIdAsync(currentUserId);
        return Ok(fullUser);
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> registerUser([FromForm] RegisterUserDTO userDTO)
    {
        var user = await _userService.registerUserAsync(userDTO);
        return Ok(user);
    }

    [HttpPatch]
    [Route("editUserProfilePicture")]
    [Authorize(Policy = "RequireId")]
    public async Task<IActionResult> editUserProfile([FromForm] EditUserProfilePictureDTO userDTO)
    {
        int currentUserId = Convert.ToInt32(HttpContext.User.FindFirst("Id")?.Value);
        var user = await _userService.editUserProfilePictureAsync(userDTO, currentUserId);
        return Ok(user);

    }

    [HttpDelete]
    [Authorize(Policy = "RequireId")]
    [Route("deleteProfilePicture")]
    public async Task<IActionResult> deleteCustomProfilePictureAsync()
    {
        int currentUserId = Convert.ToInt32(HttpContext.User.FindFirst("Id")?.Value);
        var user = await _userService.deleteCustomProfilePictureAsync(currentUserId);
        return Ok(user);
    }

}


