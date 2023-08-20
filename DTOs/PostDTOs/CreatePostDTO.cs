using System.Text.Json.Serialization;

namespace ClassroomPlus.DTOs.PostDTOs;

public class CreatePostDTO
{
    [JsonIgnore]
    public int CreatorId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int ClassroomId { get; set; }
}
