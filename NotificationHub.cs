using ClassroomPlus.DTOs.NotificationHubDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Runtime.InteropServices;

namespace ClassroomPlus;

public class NotificationHub : Hub<INotificationClient>
{
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public async Task JoinGroup(string groupName) 
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task LeaveGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task PublishNotificationToGroup(CreatedPostNotificationDTO post, string groupName)
    {
        await Clients.Group(groupName).PostCreated(post);
    }

    public async Task PublishPostDeletedNotificationToGroup(int postId, int classroomId, string groupName)
    {
        await Clients.Group(groupName).PostDeleted(postId, classroomId);
    }

}
