using ClassroomPlus.Entities;

namespace ClassroomPlus.DTOs.PostDTOs;

public class ResponsePostDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int ClassroomId { get; set; }
    public DateTime CreatedDate { get; set; }
    public ICollection<Comment> Comments { get; set; }

}
