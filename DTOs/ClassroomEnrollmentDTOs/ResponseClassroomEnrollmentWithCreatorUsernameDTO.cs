using ClassroomPlus.Entities;

namespace ClassroomPlus.DTOs.UsersClassroomsDTOs
{
    public class ResponseClassroomEnrollmentWithCreatorUsernameDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ClassroomId { get; set; }
        public string ClassroomCreatorUsername { get; set; }
        public User? User { get; set; }
        public Classroom? Classroom { get; set; }
    }
}
