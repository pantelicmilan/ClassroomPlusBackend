using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassroomPlus.Entities;

public class ClassroomEnrollment
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ClassroomId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; }

    [ForeignKey("ClassroomId")]
    public Classroom Classroom { get; set; }
}
