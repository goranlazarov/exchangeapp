using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ExchangeApp.Startup))]
namespace ExchangeApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
