namespace ClassroomPlus.DTOs.UserDTOs
{
    public class RegisterUserDTO
    {
        public IFormFile? imageFile { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
