using ClassroomPlus.Entities;
using ClassroomPlus.DTOs;

namespace ClassroomPlus.Repositories.Interfaces;

public interface IClassroomRepository
{
    public Task<Classroom> getClassroomByJoinCodeAsync(string joinCode);
    public Task<IEnumerable<Classroom>> getAllAsync();
    public Task<Classroom> getClassroomByIdAsync(int id);
    public Task<Classroom> createClassroomAsync(Classroom classroom);
    public Task<Classroom> editClassroomAsync(Classroom classroom);
    public Classroom deleteClassroom(Classroom classroom);
    public Task<IEnumerable<Classroom>> getClassroomsByUserIdAsync(int userId);
    public Task<Classroom> getClassroomWithGuidAsync(string guid);
    public Task<Classroom> getClassroomWherePostId(int postId);
    public Task<Classroom> getClassroomByIdAsync(int id, int pageNumber, int pageSize);
}
