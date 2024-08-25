using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayerApi
{
    public class AudioHub : Hub
    {
        public async Task Send(int i)
        {
            await this.Clients.All.SendAsync("Send", i);
        }
    }
}
