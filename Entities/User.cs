using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ClassroomPlus.Entities;

public class User
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    [Required]
    public string Username { get; set; }
    [Required]
    [JsonIgnore]
    public string HashedPassword { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [JsonIgnore]
    public ICollection<Classroom> Classrooms { get; set; }
    public string ProfileImageUrl { get; set; } = "default.png";
}

