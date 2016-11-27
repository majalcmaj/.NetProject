using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PistollegroMVC.Startup))]
namespace PistollegroMVC
{
	public partial class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			// ConfigureAuth(app);
		}
	}
}
