using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MvcMesswert.Startup))]
namespace MvcMesswert
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
