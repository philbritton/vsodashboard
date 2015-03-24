using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VSOClientDashboard.Startup))]
namespace VSOClientDashboard
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
