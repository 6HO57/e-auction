
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(e_auction.SStartup))]

namespace e_auction
{
    public partial class SStartup
    {
        public void Configuration(IAppBuilder app)
        {
            //Configuration(app);    
            app.MapSignalR();
        }
    }
}
