using ImagX_API.Entities;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task NewNotificationReceived(Notification newNotification, string receiverConnectionId)
        {
            await Clients.Client(receiverConnectionId).SendAsync("NewNotificationReceived" , newNotification);
        }

        public string GetDonnectionId() => Context.ConnectionId;
    }
}
