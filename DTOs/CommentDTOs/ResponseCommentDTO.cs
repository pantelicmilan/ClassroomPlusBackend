using ClassroomPlus.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassroomPlus.DTOs.CommentDTOs;

public class ResponseCommentDTO
{
    public int Id { get; set; }
    public string Content { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public User User { get; set; }
}
