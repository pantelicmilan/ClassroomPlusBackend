using ClassroomPlus.Entities;
using System.Text.Json.Serialization;

namespace ClassroomPlus.DTOs.ClassroomDTOs;

public class EditClassroomDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    [JsonIgnore]
    public int CreatorId { get; set; }
}
