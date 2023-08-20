using ClassroomPlus.Services.Interfaces;
using ClassroomPlus.DTOs.CommentDTOs;
using ClassroomPlus.Repositories.Interfaces;
using ClassroomPlus.Exceptions;
using AutoMapper;
using ClassroomPlus.Entities;

namespace ClassroomPlus.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IClassroomEnrollmentRepository _classroomEnrollmentRepository;
    private readonly IClassroomRepository _classroomRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CommentService(
        ICommentRepository commentRepository,
        IClassroomEnrollmentRepository classroomEnrollmentRepository,
        IClassroomRepository classroomRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper
        ) 
    {
        _commentRepository = commentRepository;
        _classroomEnrollmentRepository = classroomEnrollmentRepository;
        _classroomRepository = classroomRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseCommentDTO> createCommentAsync(CreateCommentDTO commentDTO)
    {
        if(commentDTO.Content.Length < 2 || commentDTO.Content.Length > 150 || commentDTO.Content == null || commentDTO.Content == "")
        {
            throw new LimitedCountException("Comment must have min:2 max:150 characters and can't be empty! ");
        }

        var classroom = await _classroomRepository
            .getClassroomWherePostId(commentDTO.PostId);

        if (classroom == null)
        {
            throw new NotFoundException("Classroom not found!");
        }

        if (classroom.CreatorId == commentDTO.UserId)
        {
            var createdCommentIfUserOwner = await _commentRepository.createCommentAsync(_mapper.Map<Comment>(commentDTO));
            await _unitOfWork.Save();
            var commentIfUserOwner = await _commentRepository.getCommentByIdAsync(createdCommentIfUserOwner.Id);
            return _mapper.Map<ResponseCommentDTO>(commentIfUserOwner);
        }
        var usersClassroom = await _classroomEnrollmentRepository.getClassroomEnrollmentWhereUserIdAndClassroomIdAsync(commentDTO.UserId, classroom.Id);
        if (usersClassroom == null)
        {
            throw new NotFoundException("Relationn not found!");
        }
        var createdComment = await _commentRepository
            .createCommentAsync(_mapper.Map<Comment>(commentDTO));
        await _unitOfWork.Save();
        var comment = await _commentRepository.getCommentByIdAsync(createdComment.Id);
        return _mapper.Map<ResponseCommentDTO>(comment);
    }

    public async Task<ResponseCommentDTO> deleteCommentById(int id, int currentUserId)
    {
        var comment = await _commentRepository.getCommentByIdAsync(id);
        if (comment == null) throw new NotFoundException("Comment not found!");

        if (comment.UserId != currentUserId) {
            if (comment.Post.Classroom.CreatorId == currentUserId)
            {
                _commentRepository.deleteComment(comment);
                await _unitOfWork.Save();
                return _mapper.Map<ResponseCommentDTO>(comment);
            }
            throw new UnauthorizedAccessException("You are not owner of comment or classroom and you can not delete!");
        }
        _commentRepository.deleteComment(comment);
        await _unitOfWork.Save();
        return _mapper.Map<ResponseCommentDTO>(comment);
    }

    public async Task<ResponseCommentDTO> editCommentAsync(EditCommentDTO commentDTO)
    {
        if (commentDTO == null) throw new BadRequestException("U can not have a null edit data!");
        var comment = await _commentRepository.getCommentByIdAsync(commentDTO.Id);
        if (comment == null) throw new NotFoundException("Comment not found!");
        if (comment.UserId != commentDTO.UserId) throw new UnauthorizedAccessException("U are not author of this comment!");
        if (commentDTO.Content.Count() < 3 || commentDTO.Content.Count() > 200) 
        {
            throw new WrongPropertyLength("Your edited comment must be min: 3 max 200 characters!");
        } 
        await _commentRepository.editCommentAsync(_mapper.Map<Comment>(commentDTO));
        await _unitOfWork.Save();
        return _mapper.Map<ResponseCommentDTO>(commentDTO);
    }

}
