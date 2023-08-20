using ClassroomPlus.Entities;

namespace ClassroomPlus.DTOs.ClassroomDTOs;

public class ResponseClassroomWithCreatorDTO : ResponseClassroomDTO
{
    public User Creator { get; set; }
}

