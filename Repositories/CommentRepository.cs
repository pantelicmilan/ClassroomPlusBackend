using ClassroomPlus.Data;
using ClassroomPlus.Entities;
using ClassroomPlus.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClassroomPlus.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly SQLServerContext _context;

    public CommentRepository(SQLServerContext context) 
    {
        _context = context;
    }

    public async Task<Comment> createCommentAsync(Comment comment) 
    {
        await _context.Comments.AddAsync(comment);
        return comment;
    }

    public async Task<Comment> editCommentAsync(Comment comment)
    {
        var fetchedComment = await _context.Comments.FindAsync(comment.Id);
        fetchedComment.Content = comment.Content;
        fetchedComment.Edited = true;
        return fetchedComment;
    }

    public Comment deleteComment(Comment comment) 
    {
        _context.Remove(comment);
        return comment;
    }

    public async Task<Comment> getCommentByIdAsync(int id)
    {
        var comment = await _context.Comments
            .Include(c => c.User)
            .Include(c => c.Post)
             .ThenInclude(p => p.Classroom)
             .FirstOrDefaultAsync(c => c.Id == id);
            
        return comment;
    }

}
