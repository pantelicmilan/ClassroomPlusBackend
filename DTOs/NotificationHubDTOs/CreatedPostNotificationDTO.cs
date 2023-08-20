using Microsoft.OpenApi.Any;

namespace ClassroomPlus.DTOs.NotificationHubDTOs
{
    public class CreatedPostNotificationDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CreatorUsername { get; set; }
        public string Description { get; set; }
        public string CreatedDate { get; set;}
        public List<IOpenApiAny> Comments { get; set; }
        public int ClassroomId { get; set; }
    }
}
