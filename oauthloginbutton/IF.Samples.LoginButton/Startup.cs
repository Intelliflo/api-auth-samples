using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IF.Samples.LoginButton.Startup))]
namespace IF.Samples.LoginButton
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
