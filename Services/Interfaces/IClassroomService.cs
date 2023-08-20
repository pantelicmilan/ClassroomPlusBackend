using ClassroomPlus.DTOs;
using ClassroomPlus.DTOs.ClassroomDTOs;
using ClassroomPlus.Entities;

namespace ClassroomPlus.Services.Interfaces;

public interface IClassroomService
{
    public Task<ResponseClassroomWithCreatorDTO> getClassroomByJoinCodeAsync(string joinCode, int currentUserId);
    public Task<IEnumerable<ResponseClassroomDTO>> getAllAsync();
    public Task<ResponseClassroomDTO> getClassroomByIdAsync(int id, int currentUserId, int itemsPerPage);
    public Task<ResponseClassroomDTO> createClassroomAsync(CreateClassroomDTO classroomDTO);
    public Task<ResponseClassroomDTO> editClassroomAsync(EditClassroomDTO classroomDTO);
    public Task<ResponseClassroomDTO> deleteClassroom(int id, int currentUserId);
    public Task<IEnumerable<ResponseClassroomOwnerDTO>> getClassroomsByUserIdAsync(int id);
}
