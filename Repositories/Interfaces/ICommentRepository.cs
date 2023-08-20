using ClassroomPlus.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClassroomPlus.Repositories.Interfaces;

public interface ICommentRepository
{
    public Task<Comment> createCommentAsync(Comment comment);
    public Task<Comment> editCommentAsync(Comment comment);
    public Comment deleteComment(Comment comment);
    public Task<Comment> getCommentByIdAsync(int id);
}
