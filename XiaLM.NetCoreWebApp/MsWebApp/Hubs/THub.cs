using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MsWebApp.Hubs
{
    public class GlobalHubServer<THub> where THub : Hub
    {
        private readonly IHubContext<THub> _hubContext;

        public GlobalHubServer(IHubContext<THub> hubContext)
        {
            _hubContext = hubContext;
        }

        public Task InvokeOnGroupAsync(string groupName, string method, params object[] args) =>
            _hubContext.Clients.Group(groupName).SendAsync(method, args);

        public Task InvokeOnAllAsync(string method, params object[] args) =>
            _hubContext.Clients.All.SendAsync(method, args);
        public Task InvokeByIDAsync(string id, string method, params object[] args) =>
            _hubContext.Clients.Client(id).SendAsync(method, args);

        public Task AddConnectionIdToGroupAsync(string connectionId, string groupName) =>
            _hubContext.Groups.AddToGroupAsync(connectionId, groupName);

    }
}
