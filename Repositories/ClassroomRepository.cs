using ClassroomPlus.Data;
using ClassroomPlus.Entities;
using ClassroomPlus.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClassroomPlus.Repositories;

public class ClassroomRepository : IClassroomRepository
{
    private readonly SQLServerContext _context;

    public ClassroomRepository(SQLServerContext context) 
    {
        _context = context;
    }

    public async Task<Classroom> createClassroomAsync(Classroom classroom)
    {
        await _context.Classrooms.AddAsync(classroom);
        await _context.Entry(classroom).Reference(c => c.Creator).LoadAsync();
        return classroom;
        //moguc problem
    }

    public Classroom deleteClassroom(Classroom classroom)
    {
        _context.Remove(classroom);
        return classroom; 
    }

    public async Task<Classroom> editClassroomAsync(Classroom classroom)
    {
        var classr = await _context.Classrooms.FirstOrDefaultAsync(cls => cls.Id == classroom.Id);

        classr.Name = classroom.Name;
        classr.CreatorId = classroom.CreatorId;
        return classr;
    }

    public async Task<IEnumerable<Classroom>> getAllAsync()
    {
        var classrooms = await _context.Classrooms.Include(c => c.Posts).ToListAsync();
        return classrooms;
    }

    public async Task<Classroom> getClassroomByIdAsync(int id, int pageNumber, int pageSize)
    {
        var classroom = await _context.Classrooms
            .Include(c => c.Creator)
            .Include(c => c.Posts)
                .ThenInclude(p => p.Comments)
                    .ThenInclude(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (classroom != null)
        {
            classroom.Posts = classroom.Posts.OrderByDescending(c => c.Id)
                .Skip((pageNumber - 1) * pageSize) // Preskoči odgovarajući broj postova na prethodnim stranicama
                .Take(pageSize) // Uzmi samo potrebni broj postova za trenutnu stranicu
                .ToList();

            foreach (var post in classroom.Posts)
            {
                post.Comments = post.Comments.OrderByDescending(c => c.Id).ToList();
            }
        }

        return classroom;
    }

    public async Task<Classroom> getClassroomByIdAsync(int id)
    {
        var classroom = await _context.Classrooms
            .Include(c => c.Creator)
            .Include(c => c.Posts)
            .ThenInclude(p => p.Comments)
            .ThenInclude(c => c.User)
            .FirstOrDefaultAsync(c=> c.Id == id);

        if (classroom != null)
        {
            classroom.Posts = classroom.Posts.OrderByDescending(c => c.Id).ToList();
            foreach (var post in classroom.Posts)
            {
                post.Comments = post.Comments.OrderByDescending(c => c.Id).ToList();
            }
        }

        return classroom;
    }

    public async Task<IEnumerable<Classroom>> getClassroomsByUserIdAsync(int id)
    {
        var classroom = await _context.Classrooms
            .Include(c => c.Creator)
            .Include(c => c.Posts)
            .Where(
            classroom => classroom.CreatorId == id)
            .OrderByDescending(c => c.Id)
            .ToListAsync();
        return classroom;
    }

    public async Task<Classroom> getClassroomWithGuidAsync(string guid)
    {
        var classroom = await _context.Classrooms
            .Include(c => c.Creator)
            .FirstOrDefaultAsync(c => c.JoinCode == guid);
        return classroom;
    }

    public async Task<Classroom> getClassroomWherePostId(int postId)
    {
        var classroom = await _context.Classrooms
            .Include(c => c.Posts)
            .FirstOrDefaultAsync(c => c.Posts.Any(p => p.Id == postId));

        return classroom;
    }

    public async Task<Classroom> getClassroomByJoinCodeAsync(string joinCode)
    {
        var classroom = await _context.Classrooms
            .Include(c=>c.Creator)
            .Where(c => c.JoinCode == joinCode)
            .FirstOrDefaultAsync();

        return classroom;
    }
}
