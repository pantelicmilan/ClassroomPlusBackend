using System.Text.Json.Serialization;

namespace ClassroomPlus.DTOs.UserDTOs;

public class EditUserDTO
{
    [JsonIgnore]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Username { get; set; }
}
