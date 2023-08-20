using ClassroomPlus.Data;
using ClassroomPlus.Repositories.Interfaces;

namespace ClassroomPlus.Repositories;

public class UnitOfWork : IUnitOfWork   
{
    private readonly SQLServerContext _context;
    public UnitOfWork(SQLServerContext context) 
    {
        _context = context;
    }
    public async Task Save()
    {
        await _context.SaveChangesAsync();
    }

}
