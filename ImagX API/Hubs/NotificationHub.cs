using ImagX_API.Entities;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.Hubs
{
    public class NotificationHub<T> : Hub
    {
        public async Task NewCallReceived(Notification<T> newNotification)
        {
            await Clients.All.SendAsync("NewCallReceived" , newNotification);
        }
    }
}
