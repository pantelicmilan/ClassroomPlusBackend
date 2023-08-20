using ClassroomPlus.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ClassroomPlus.DTOs.UserDTOs;

public class ResponseUserDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Username { get; set; }
    [JsonIgnore]
    public string HashedPassword { get; set; }
    public string Email { get; set; }
    public ICollection<Classroom> Classrooms { get; set; }
    public string ProfileImageUrl { get; set; }
}
