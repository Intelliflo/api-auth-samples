using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IF.Samples.OAuth.RefreshToken.Startup))]
namespace IF.Samples.OAuth.RefreshToken
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
