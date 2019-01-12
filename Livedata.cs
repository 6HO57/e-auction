using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace e_auction
{
    public class Livedata : Hub
    {
        public void Hello()
        {
            Clients.All.introduce("HI");
        }
    }
}