using ClassroomPlus.Data;
using ClassroomPlus.Entities;
using ClassroomPlus.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClassroomPlus.Repositories;

public class PostRepository : IPostRepository
{
    private readonly SQLServerContext _context;

    public PostRepository(SQLServerContext context) 
    {
        _context = context;
    }

    public async Task<Post> createPostAsync(Post post)
    {
        await _context.Posts.AddAsync(post);
        return post;
    }

    public Post deletePost(Post post)
    {
        _context.Remove(post);
        return post;
    }

    public async Task<Post> editPostAsync(Post post)
    {
        var uneditedPost = await _context.Posts.FirstOrDefaultAsync(p => p.Id == post.Id);
        if (uneditedPost != null) 
        {
            uneditedPost.Name = post.Name;
            uneditedPost.Description = post.Description;
            uneditedPost.CreatedDate = DateTime.Now;
        }
        return uneditedPost;
    }

    public async Task<IEnumerable<Post>> getAllAsync()
    {
        var posts = await _context.Posts.ToListAsync();
        return posts;
    }

    public async Task<Post> getPostByIdAsync(int id)
    {
        var post = await _context.Posts.
            Include(p => p.Comments)
            .FirstOrDefaultAsync(p=> p.Id == id);
        return post;
    }

    public async Task<IEnumerable<Post>> getPostsByClassroomIdAsync(int classroomId)
    {
        IEnumerable<Post> posts = await _context.Posts
            .Where(p => p.ClassroomId == classroomId).ToListAsync();
        return posts;
    }

    public async Task<IEnumerable<Post>> getPostsByClassroomIdAsync(int classroomId, int pageSize, int currentPage)
    {
        int skipAmount = (currentPage - 1) * pageSize;

        IEnumerable<Post> posts = await _context.Posts
            .Include(p=>p.Comments)
            .Where(p => p.ClassroomId == classroomId)
            .OrderByDescending(p => p.Id) // Od novijih ka starijim
            .Skip(skipAmount) // Preskoči postove na prethodnim stranicama
            .Take(pageSize) // Uzmi postove za trenutnu stranicu
            .ToListAsync();

        return posts;
    }

}
