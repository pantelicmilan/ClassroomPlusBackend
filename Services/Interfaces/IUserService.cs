using ClassroomPlus.DTOs.UserDTOs;

namespace ClassroomPlus.Services.Interfaces;

public interface IUserService
{
    public Task<IEnumerable<ResponseUserDTO>> getAllUsersAsync();
    public Task<ResponseUserDTO> getUserByIdAsync(int id);
    public Task<ResponseUserDTO> registerUserAsync(RegisterUserDTO userDTO);
    public Task<ResponseUserDTO> editUserInformationAsync(EditUserDTO userDTO);
    public Task<ResponseUserDTO> deleteUserAsync(int id);
    public Task<ResponseLoginUserDTO> loginUserAsync(LoginUserDTO userDTO);
    public Task<ResponseUserDTO> getUserByUsername(string username);
    public Task<ResponseUserDTO> editUserProfilePictureAsync(EditUserProfilePictureDTO userDTO, int currentUserId);
    public Task<ResponseUserDTO> deleteCustomProfilePictureAsync(int currentUserId);
}
