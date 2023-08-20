using ClassroomPlus.Entities;
using ClassroomPlus.DTOs.PostDTOs;

namespace ClassroomPlus.Services.Interfaces;

public interface IPostService
{
    public Task<IEnumerable<ResponsePostDTO>> getAllAsync();
    public Task<ResponsePostDTO> getPostByIdAsync(int id, int currentUserId);
    public Task<ResponsePostDTO> editPostAsync(EditPostDTO post);
    public Task<ResponsePostDTO> createPostAsync(CreatePostDTO post);
    public Task<ResponsePostDTO> deletePostAsync(int id, int currentUserId);
    public Task<IEnumerable<ResponsePostDTO>> getPostsByClassroomIdAsync(int classroomId, int currentUserId);
    public Task<IEnumerable<Post>> getPostsByClassroomIdAsync(int classroomId, int pageSize, int currentPage, int currentUserId);

}
