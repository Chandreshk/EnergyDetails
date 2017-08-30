using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EnergyDetails.Startup))]
namespace EnergyDetails
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
