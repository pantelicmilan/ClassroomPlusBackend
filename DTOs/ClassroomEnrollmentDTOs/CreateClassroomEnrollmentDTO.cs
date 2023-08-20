using System.Text.Json.Serialization;

namespace ClassroomPlus.DTOs.UsersClassroomsDTOs;

public class CreateClassroomEnrollmentDTO
{
    [JsonIgnore]
    public int UserId { get; set; }
    public string JoinCode { get; set; }
}
