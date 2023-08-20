using ClassroomPlus.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClassroomPlus.Data;

public class SQLServerContext : DbContext
{
    public SQLServerContext(DbContextOptions<SQLServerContext> options) : base(options) {}
    public DbSet<User> Users { get; set; } 
    public DbSet<Classroom> Classrooms { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<ClassroomEnrollment> ClassroomEnrollment { get; set; }
    public DbSet<Comment> Comments { get; set; }
}

