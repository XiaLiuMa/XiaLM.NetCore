using Microsoft.AspNetCore.SignalR;
using MsWebApp.Comm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MsWebApp.Hubs
{
    public class LinkHubs : Hub
    {
        private readonly GlobalHubServer<LinkHubs> _hubMethods;

        public LinkHubs(GlobalHubServer<LinkHubs> hubMethods)
        {
            _hubMethods = hubMethods;
        }


        public override Task OnConnectedAsync()
        {
            //this.Clients.All.SendAsync("ReceiveMessage", string.Format("ID:{0} 已经连接到SignalR.", this.Context.ConnectionId));
            return base.OnConnectedAsync();
        }


        //private static readonly object lockObj = new object();
        //private static LinkHubs initialize;
        //public static LinkHubs GetInitialize()
        //{
        //    if (initialize == null)
        //    {
        //        lock (lockObj)
        //        {
        //            if (initialize == null)
        //            {
        //                initialize = new LinkHubs();
        //            }
        //        }
        //    }
        //    return initialize;
        //}

        /// <summary>
        /// 启动扫描仪
        /// </summary>
        public void StartScanner()
        {
            
        }


        /// 打印状态变化事件
        /// </summary>
        /// <param name="obj"></param>
        public void PrintStatusChange(string status)
        {
            try
            {
                Startup.GlobalHub.InvokeOnAllAsync("PrintStatusChange",status);
            }
            catch (Exception)
            {
            }
        }
    }
}
