using ClassroomPlus.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ClassroomPlus.DTOs.CommentDTOs;

public class CreateCommentDTO
{
    public string Content { get; set; }
    [JsonIgnore]
    public int UserId { get; set; }
    public int PostId { get; set; }
}
