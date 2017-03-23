using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AbdulLCTest.UI.Startup))]
namespace AbdulLCTest.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
