using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IF.Samples.OAuth.LoginButton.Startup))]
namespace IF.Samples.OAuth.LoginButton
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
