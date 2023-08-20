using ClassroomPlus.DTOs;
using ClassroomPlus.DTOs.UsersClassroomsDTOs;

namespace ClassroomPlus.Services.Interfaces;

public interface IClassroomEnrollmentService
{
    public Task<IEnumerable<ResponseClassroomEnrollmentDTO>> getAll();
    public Task<ResponseClassroomEnrollmentDTO> getClassroomEnrollmentById(int id);
    public Task<IEnumerable<ResponseClassroomEnrollmentDTO>> getClassroomsEnrollmentsByUserId(int userId);
    public Task<IEnumerable<ResponseClassroomEnrollmentDTO>> getClassroomsEnrollmentsByClassroomId(int classroomId);
    public Task<ResponseClassroomEnrollmentDTO> deleteClassroomEnrollment(int id, int currentUserId);
    public Task<ResponseClassroomEnrollmentDTO> createClassroomEnrollment(CreateClassroomEnrollmentDTO classroomEnrollmentDTO);
    public Task<ResponseClassroomEnrollmentDTO> deleteClassroomEnrollmentWhereClassroomIdAndUserId(int classroomId, int currentUserId);
}
