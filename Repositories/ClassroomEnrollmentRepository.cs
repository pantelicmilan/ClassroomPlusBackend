using ClassroomPlus.Data;
using ClassroomPlus.Entities;
using ClassroomPlus.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClassroomPlus.Repositories;

public class ClassroomEnrollmentRepository : IClassroomEnrollmentRepository
{
    private readonly SQLServerContext _context;

    public ClassroomEnrollmentRepository(SQLServerContext context) 
    {
        _context = context;
    }

    public async Task<ClassroomEnrollment> createClassroomEnrollmentAsync(ClassroomEnrollment classroomEnrollment)
    {
        await _context.ClassroomEnrollment.AddAsync(classroomEnrollment);
        return classroomEnrollment;
    }

    public ClassroomEnrollment deleteClassroomEnrollment(ClassroomEnrollment classroomEnrollment)
    {
        _context.ClassroomEnrollment.Remove(classroomEnrollment);
        return classroomEnrollment;
    }

    public async Task<ClassroomEnrollment> getClassroomEnrollmentByIdAsync(int id)
    {
        var classroomEnrollment = await _context.ClassroomEnrollment.FindAsync(id);
        return classroomEnrollment;
    }

    public async Task<IEnumerable<ClassroomEnrollment>> getClassroomEnrollmentByUserIdAsync(int userId)
    {
        var classroomsUsers = await _context.ClassroomEnrollment
            .Include(uc => uc.Classroom)
            .ThenInclude(c => c.Creator)
            .Where(uc => uc.UserId == userId).ToListAsync();
        return classroomsUsers;
    }

    public async Task<IEnumerable<ClassroomEnrollment>> getClassroomEnrollmentByClassroomIdAsync(int classroomId)
    {
        var classroomsEnrollments = await _context.ClassroomEnrollment
            .Include(uc => uc.User)
            .Where(uc => uc.ClassroomId == classroomId).ToListAsync();
        return classroomsEnrollments;
    }

    public async Task<IEnumerable<ClassroomEnrollment>> getAll()
    {
        var classroomsEnrollments = await _context.ClassroomEnrollment.ToListAsync();
        return classroomsEnrollments;
    }

    public async Task<ClassroomEnrollment> getClassroomEnrollmentWhereUserIdAndClassroomIdAsync(int userId, int classroomId)
    {
        var classroomEnrollment = await _context.ClassroomEnrollment
            .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.ClassroomId == classroomId);
        return classroomEnrollment;
    }

}
