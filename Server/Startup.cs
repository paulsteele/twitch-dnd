using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using twitchDnd.Server.Configuration;
using twitchDnd.Server.Database;
using twitchDnd.Shared.Registration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using twitchDnd.Server.Hubs;
using twitchDnd.Server.Services;
using twitchDnd.Server.Tasks;

namespace twitchDnd.Server
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public ILifetimeScope AutofacContainer { get; private set; }

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			AutofacContainer = app.ApplicationServices.GetAutofacRoot();
			app.UseResponseCompression();
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseWebAssemblyDebugging();
			}
			else
			{
				app.UseExceptionHandler("/Error");
			}

			app.UseBlazorFrameworkFiles();
			app.UseStaticFiles();

			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints => {
					endpoints.MapRazorPages();
					endpoints.MapControllers();
					endpoints.MapFallbackToFile("index.html");
					endpoints.MapHub<ServerSignalRHub>("/hub", options =>
					{
						options.CloseOnAuthenticationExpiration = true;
					});
			});

			var tasks = AutofacContainer.Resolve<IEnumerable<ITask>>();
			foreach (var task in tasks)
			{
				task.Start();
			}
		}

		public void ConfigureServices(IServiceCollection services) {
			var configuration = new EnvironmentVariableConfiguration();
			services
				.AddDefaultIdentity<IdentityUser>((options => {
					options.Password.RequireDigit = false;
					options.Password.RequiredLength = 0;
					options.Password.RequireLowercase = false;
					options.Password.RequireUppercase = false;
					options.Password.RequiredUniqueChars = 0;
					options.Password.RequireNonAlphanumeric = false;
				}))
				.AddRoles<IdentityRole>()
				.AddEntityFrameworkStores<DatabaseContext>();

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,
						ValidIssuer = configuration.JwtIssuer,
						ValidAudience = configuration.JwtAudience,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.JwtSecurityKey))
					};
					
					options.Events = new JwtBearerEvents
					{
						OnMessageReceived = context =>
						{
							var accessToken = context.Request.Query["access_token"];

							// If the request is for our hub...
							var path = context.HttpContext.Request.Path;
							if (!string.IsNullOrEmpty(accessToken) &&
								path.StartsWithSegments("/hub"))
							{
								// Read the token out of the query string
								context.Token = accessToken;
							}
							return Task.CompletedTask;
						}
					};
				});
			services.AddControllersWithViews(options => {
				var policy = new AuthorizationPolicyBuilder()
					.RequireAuthenticatedUser()
					.Build();
				options.Filters.Add(new AuthorizeFilter(policy));
			});
			services.AddRazorPages();
			services.AddResponseCompression(opts =>
			{
				opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
					new[] { "application/octet-stream" });
			});
			services.AddSignalR();
		}

		public void ConfigureContainer(ContainerBuilder builder)
		{
			var assembly = Assembly.GetExecutingAssembly();

			builder.RegisterAssemblyTypes(assembly);
			builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces();

			builder.Register(context => LoggerFactory.Create(logBuilder => logBuilder.AddConsole())).As<ILoggerFactory>();

			builder.RegisterType<Db>().As<IDb>().SingleInstance();
			builder.RegisterType<InitDbTask>().As<ITask>().As<InitDbTask>().SingleInstance();
			builder.RegisterType<SessionService>().As<SessionService>().SingleInstance();
			builder.RegisterType<EnvironmentVariableConfiguration>().As<IEnvironmentVariableConfiguration>().SingleInstance();
			CommonContainer.Register(builder);
		}
	}
}
