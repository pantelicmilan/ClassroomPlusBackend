using ClassroomPlus.Entities;

namespace ClassroomPlus.DTOs.ClassroomDTOs;

public class ResponseClassroomOwnerDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CreatorId { get; set; }
    public string JoinCode { get; set; }
    public IEnumerable<Post> Posts { get; set; }
}
