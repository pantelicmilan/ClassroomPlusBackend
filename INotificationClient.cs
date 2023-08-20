using ClassroomPlus.DTOs.NotificationHubDTOs;

namespace ClassroomPlus
{
    public interface INotificationClient
    {
        Task PostCreated(CreatedPostNotificationDTO post);
        Task PostDeleted(int postId, int classroomId);
    }
}
