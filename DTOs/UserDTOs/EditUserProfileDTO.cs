using System.Text.Json.Serialization;

namespace ClassroomPlus.DTOs.UserDTOs
{
    public class EditUserProfileDTO
    {
        public IFormFile? imageFile { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
    }
}
