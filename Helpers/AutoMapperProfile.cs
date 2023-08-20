using AutoMapper;
using ClassroomPlus.DTOs.PostDTOs;
using ClassroomPlus.DTOs.UserDTOs;
using ClassroomPlus.DTOs.UsersClassroomsDTOs;
using ClassroomPlus.DTOs.ClassroomDTOs;
using ClassroomPlus.Entities;
using ClassroomPlus.DTOs.CommentDTOs;

namespace ClassroomPlus.Helpers;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile() 
    {
        CreateMap<Post, ResponsePostDTO>();
        CreateMap<CreatePostDTO, Post>();
        CreateMap<Classroom, ResponseClassroomDTO>();
        CreateMap<Classroom, ResponseClassroomWithCreatorDTO>();
        CreateMap<CreateUserDTO, User>();
        CreateMap<EditUserDTO, User>();
        CreateMap<EditPostDTO, Post>();
        CreateMap<ClassroomEnrollment, ResponseClassroomEnrollmentDTO>();
        CreateMap<User, ResponseUserDTO>();
        CreateMap<CreateClassroomEnrollmentDTO, ClassroomEnrollment>();
        CreateMap<CreateClassroomDTO, Classroom>();
        CreateMap<Classroom, ResponseClassroomOwnerDTO>();
        CreateMap<CreateCommentDTO, Comment>();
        CreateMap<CreateCommentDTO, ResponseCommentDTO>();
        CreateMap<ClassroomEnrollment, ResponseClassroomEnrollmentWithCreatorUsernameDTO>();
        CreateMap<EditCommentDTO, Comment>();
        CreateMap<EditCommentDTO, ResponseCommentDTO>();
        CreateMap<Comment, ResponseCommentDTO>();
        CreateMap<RegisterUserDTO, User>();
    }  

}
