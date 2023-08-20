using System.Text.Json.Serialization;

namespace ClassroomPlus.DTOs.PostDTOs;

public class EditPostDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int ClassroomId { get; set; }
    [JsonIgnore]
    public int EditorId { get; set; }
}
