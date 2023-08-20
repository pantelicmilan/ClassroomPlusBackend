using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ClassroomPlus.Entities;

public class Classroom
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public int CreatorId { get; set; }
    public string JoinCode { get; set; }

    [ForeignKey("CreatorId")]
    public User Creator { get; set; }
    public ICollection<Post> Posts { get; set; }

}
