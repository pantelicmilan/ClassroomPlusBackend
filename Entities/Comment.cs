using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ClassroomPlus.Entities;

public class Comment
{
    public int Id { get; set; }
    public string Content { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public bool Edited { get; set; } = false;

    [ForeignKey("PostId")]
    [JsonIgnore]
    public Post Post { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }
}
