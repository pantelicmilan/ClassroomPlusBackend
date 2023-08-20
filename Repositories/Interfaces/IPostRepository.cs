using ClassroomPlus.Entities;

namespace ClassroomPlus.Repositories.Interfaces;

public interface IPostRepository
{
    public Task<IEnumerable<Post>> getAllAsync();
    public Task<Post> getPostByIdAsync(int id);
    public Task<Post> editPostAsync(Post post);
    public Task<Post> createPostAsync(Post post);
    public Post deletePost(Post post);
    public Task<IEnumerable<Post>> getPostsByClassroomIdAsync(int classroomId);
    public Task<IEnumerable<Post>> getPostsByClassroomIdAsync(int classroomId, int pageSize, int currentPage);

}
