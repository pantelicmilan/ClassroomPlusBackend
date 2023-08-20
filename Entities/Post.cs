using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ClassroomPlus.Entities;

public class Post
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int ClassroomId { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    [JsonIgnore]
    [ForeignKey("ClassroomId")]
    public Classroom Classroom { get; set; }

    public ICollection<Comment> Comments { get; set; }
}
