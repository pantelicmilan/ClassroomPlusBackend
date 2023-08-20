using ClassroomPlus.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ClassroomPlus.DTOs.ClassroomDTOs;

public class CreateClassroomDTO
{
    public string Name { get; set; }
    [JsonIgnore]
    public int CreatorId { get; set; }
}
