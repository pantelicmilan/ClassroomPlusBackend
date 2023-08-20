using ClassroomPlus.Entities;
using AutoMapper;
using ClassroomPlus.Exceptions;
using ClassroomPlus.Services.Interfaces;
using ClassroomPlus.Repositories.Interfaces;
using ClassroomPlus.DTOs.PostDTOs;

namespace ClassroomPlus.Services;

public class PostService : IPostService
{
    private readonly IMapper _mapper;
    private readonly IPostRepository _postRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClassroomRepository _classroomRepository;
    private readonly IClassroomEnrollmentRepository _classroomEnrollment;

    public PostService(
        IPostRepository postRepository,
        IClassroomRepository classroomRepository,
        IClassroomEnrollmentRepository classroomEnrollmentRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper

        )
    { 
        _postRepository = postRepository;
        _classroomRepository = classroomRepository;
        _classroomEnrollment = classroomEnrollmentRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponsePostDTO> createPostAsync(CreatePostDTO postDTO)
    {
        if(
            postDTO.Description.Count() < 6  || 
            postDTO.Description.Count() > 350 || 
            postDTO.Name.Count() < 6 || 
            postDTO.Name.Count() > 60
            )
        {
            throw new LimitedCountException("Your Description or Name length was problem, " +
                "for Title min: 6 max:60, for Content min:6 max:350");
        }

        Post post = _mapper.Map<Post>(postDTO);

        var ownedClassrooms = await _classroomRepository
            .getClassroomsByUserIdAsync(postDTO.CreatorId);

        if (ownedClassrooms.Count() == 0)
            throw new NotFoundException("Create classroom to can post!");

        bool classroomOwnership = false;
        foreach (var classroom in ownedClassrooms)
        { 
            if( classroom.Id == postDTO.ClassroomId ) 
            {
                classroomOwnership = true;
                break; 
            }
        }
        if (!classroomOwnership) 
        {
            throw new NotFoundException("You are not owner of this classroom, you can not post!");
        }
        var createdPost = await _postRepository.createPostAsync(post);
        await _unitOfWork.Save();
        var finalPost = await _postRepository.getPostByIdAsync(createdPost.Id);

        ResponsePostDTO responsePostDTO = _mapper.Map<ResponsePostDTO>(finalPost);
        return responsePostDTO;
    }

    public async Task<ResponsePostDTO> deletePostAsync(int id, int currentUserId)
    {
        var post = await _postRepository.getPostByIdAsync(id);

        if (post == null)
            throw new NotFoundException("Post does not exist!");

        var classroomWithThatPost =  await _classroomRepository
            .getClassroomByIdAsync(post.ClassroomId);

        if (classroomWithThatPost == null)
            throw new NotFoundException("Classroom with this post not found!");

        if (classroomWithThatPost.CreatorId != currentUserId)
            throw new NotFoundException("U do not have permision to delete this post!");

        var deletedPost = _postRepository.deletePost(post);
        await _unitOfWork.Save();
        return _mapper.Map<ResponsePostDTO>(deletedPost);
    }

    public async Task<ResponsePostDTO> editPostAsync(EditPostDTO postDTO)
    { 
        var post = await _postRepository.getPostByIdAsync(postDTO.Id);
        var classroomConnWithPost = await _classroomRepository
            .getClassroomByIdAsync(post.ClassroomId);

        if (classroomConnWithPost.CreatorId != postDTO.EditorId)
            throw new BadRequestException("U do not have a permision to edit this product!");
        
        if (post == null)
            throw new NotFoundException("Post not found!");

        var editedPost = await _postRepository.editPostAsync(_mapper.Map<Post>(postDTO));
        await _unitOfWork.Save();

        return _mapper.Map<ResponsePostDTO>(editedPost);
    }

    public async Task<IEnumerable<ResponsePostDTO>> getAllAsync()
    {
        IEnumerable<Post> posts = await _postRepository.getAllAsync();
        if (posts.Count() == 0)
            throw new NotFoundException("Posts not found!");

        IEnumerable<ResponsePostDTO> postsDTO = posts.Select(p => _mapper.Map<ResponsePostDTO>(p));
        return postsDTO;
    }
    
    //treba uvesti proveru da li je neko owner
    public async Task<ResponsePostDTO> getPostByIdAsync(int id, int currentUserId)
    {
        var post = await _postRepository.getPostByIdAsync(id);

        if (post == null)
            throw new NotFoundException("Post not found!");

        var usersClassrooms = await _classroomEnrollment
            .getClassroomEnrollmentWhereUserIdAndClassroomIdAsync
                (post.ClassroomId, currentUserId);

        if (usersClassrooms == null)
            throw new NotFoundException("You do not have access to this classroom!");

        return _mapper.Map<ResponsePostDTO>(post);
    }

    //potencijalno problematicna metoda
    public async Task<IEnumerable<ResponsePostDTO>> getPostsByClassroomIdAsync(int classroomId, int currentUserId)
    {
        var currentUsersClassroom = await _classroomEnrollment
            .getClassroomEnrollmentWhereUserIdAndClassroomIdAsync(classroomId, currentUserId);

        if (currentUsersClassroom == null)
            throw new BadRequestException("You are not owner of this classroom!");

        IEnumerable<Post> posts = await _postRepository.getPostsByClassroomIdAsync(classroomId);

        IEnumerable<ResponsePostDTO> postsDTO = posts.Select(p => _mapper.Map<ResponsePostDTO>(p));
        return postsDTO;
    }

    public async Task<IEnumerable<Post>> getPostsByClassroomIdAsync(int classroomId, int pageSize, int currentPage, int currentUserId)
    {
        var classroomEnrollment = await _classroomEnrollment
            .getClassroomEnrollmentWhereUserIdAndClassroomIdAsync(currentUserId, classroomId);
        if(classroomEnrollment == null)
        {
            var classroomOwner = await _classroomRepository.getClassroomByIdAsync(classroomId);
            if(classroomOwner.CreatorId != currentUserId)
            {
                throw new UnauthorizedContentException("You do not have access!");
            }
        }
        var posts = await _postRepository.getPostsByClassroomIdAsync(classroomId, pageSize, currentPage);
        return posts;
    }

}
