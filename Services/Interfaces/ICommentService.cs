using ClassroomPlus.DTOs.CommentDTOs;

namespace ClassroomPlus.Services.Interfaces;

public interface ICommentService
{
    public Task<ResponseCommentDTO> createCommentAsync(CreateCommentDTO commentDTO);
    public Task<ResponseCommentDTO> editCommentAsync(EditCommentDTO commentDTO);
    public Task<ResponseCommentDTO> deleteCommentById(int id, int currentUserId);
}
