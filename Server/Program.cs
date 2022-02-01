using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using twitchDnd.Server.Commands;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace twitchDnd.Server
{
	public static class Program {
		public static async Task Main(string[] args)
		{
			IHost host = Host.CreateDefaultBuilder(args)
				.UseServiceProviderFactory(new AutofacServiceProviderFactory())
				.ConfigureWebHostDefaults(webHostBuilder => {
					webHostBuilder.UseStartup<Startup>();
				})
				.Build();

			await host.Services.GetAutofacRoot().Resolve<EnsureUserHelper>().EnsureUser();

			await host.RunAsync();
		}
	}
}
