using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ArtShop.Startup))]
namespace ArtShop
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
