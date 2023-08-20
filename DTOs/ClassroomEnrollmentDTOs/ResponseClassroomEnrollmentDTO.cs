using ClassroomPlus.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassroomPlus.DTOs.UsersClassroomsDTOs;

public class ResponseClassroomEnrollmentDTO
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ClassroomId { get; set; }
    public User? User { get; set; }
    public Classroom? Classroom { get; set; }
}
