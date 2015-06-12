using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FPtask.Startup))]
namespace FPtask
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
