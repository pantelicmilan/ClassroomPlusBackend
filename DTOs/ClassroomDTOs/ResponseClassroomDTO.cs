using ClassroomPlus.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ClassroomPlus.DTOs.ClassroomDTOs;

public class ResponseClassroomDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CreatorId { get; set; }
    public IEnumerable<Post> Posts { get; set; }
    public User Creator { get; set; }
}
