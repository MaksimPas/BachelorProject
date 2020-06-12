using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BachelorProject.Startup))]
namespace BachelorProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
