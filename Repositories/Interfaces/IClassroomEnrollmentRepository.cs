using ClassroomPlus.Entities;

namespace ClassroomPlus.Repositories.Interfaces;

public interface IClassroomEnrollmentRepository
{
    public Task<IEnumerable<ClassroomEnrollment>> getAll();
    public Task<ClassroomEnrollment> createClassroomEnrollmentAsync(ClassroomEnrollment classroomEnrollment);
    public Task<ClassroomEnrollment> getClassroomEnrollmentByIdAsync(int id);
    public ClassroomEnrollment deleteClassroomEnrollment(ClassroomEnrollment classroomEnrollment);
    public Task<IEnumerable<ClassroomEnrollment>> getClassroomEnrollmentByClassroomIdAsync(int classroomId);
    public Task<IEnumerable<ClassroomEnrollment>> getClassroomEnrollmentByUserIdAsync(int userId);
    public Task<ClassroomEnrollment> getClassroomEnrollmentWhereUserIdAndClassroomIdAsync(int userId, int classroomId);
}
