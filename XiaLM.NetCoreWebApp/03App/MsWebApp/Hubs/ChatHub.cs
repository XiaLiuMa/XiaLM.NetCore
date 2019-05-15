using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;

namespace MsWebApp.Hubs
{
    public class ChatHub : Hub
    {
        public static IHubContext<ChatHub> GlobalContext { get; private set; }
        public ChatHub(IHubContext<ChatHub> ctx)
        {
            GlobalContext = ctx;
        }

        ///// 打印状态变化事件
        ///// </summary>
        ///// <param name="obj"></param>
        //public async void PrintStatusChange(string status)
        //{
        //    try
        //    {
        //        //await ChatHub.GlobalContext.Clients.All.InvokeAsync(...);
        //        ChatHub.GlobalContext.Clients.All.PrintStatusChange(status);


        //        //var hub = GlobalHost.ConnectionManager.GetHubContext<ExitAndEntryCardPrintHub>();
        //        //hub.Clients.All.PrintStatusChange(status); //跳转事件  
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}
    }
}
