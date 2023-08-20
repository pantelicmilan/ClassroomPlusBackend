using System.Text.Json.Serialization;

namespace ClassroomPlus.DTOs.CommentDTOs
{
    public class EditCommentDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        [JsonIgnore]
        public bool Edited { get; set; } = true;
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
