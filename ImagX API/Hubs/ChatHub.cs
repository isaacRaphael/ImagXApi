using ImagX_API.Contracts;
using ImagX_API.Entities;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IUnitOfWork _unitOFWork;

        public ChatHub(IUnitOfWork unitOFWork)
        {
            _unitOFWork = unitOFWork;
        }

        
        public async Task NewMessageReceived(int chatId, string userName, string message, string receiverConnectionId)
        {

            var payLoad = new ChatMessage() { ChatId = chatId, PayLoad = message };
            await _unitOFWork.Messages.Add(payLoad);
            await Clients.Client(receiverConnectionId).SendAsync("NewMessageReceived", userName, payLoad);
        }

        public string GetConnectionId() => Context.ConnectionId;
    }
}
