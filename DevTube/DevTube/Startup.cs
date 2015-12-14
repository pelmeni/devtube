using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DevTube.Startup))]
namespace DevTube
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
