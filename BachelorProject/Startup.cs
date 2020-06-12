using Microsoft.Owin;
using Owin;
using System.Web.Hosting;
using System.Threading.Tasks;
using BachelorProject.Controllers;
using System.Collections.Specialized;
using System.Web.Mvc;

[assembly: OwinStartupAttribute(typeof(BachelorProject.Startup))]
namespace BachelorProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            QuartzSendMailJob.Execute();
        }
    }
}
