// This file was autogenerated by Visual Studio MVC Template
// Additional DI framework services were added by me, under "ConfigureServices" method

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bd.GithubAnalyzer.Logic;
using Bd.GithubAnalyzer.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Bd.GithubAnalyzer
{
	public class Startup
	{
		public IConfiguration Configuration { get; }
		public IWebHostEnvironment Environment { get; }

		public Startup(IConfiguration configuration, IWebHostEnvironment env)
		{
			Configuration = configuration;
			Environment = env;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllersWithViews();

			/* Added by me below */

			// Utilizes the HttpClientFactory for dotnet
			services.AddHttpClient<IGithubService, GithubService>(client => {

				// Set the Base Url from the appsettings file
				client.BaseAddress = new Uri(Configuration.GetValue<string>("GithubApiUrlBase"));

				// Encouraged to set version by GitHub API
				// client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
				client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.surtur-preview+json");

				// _Required_ by the Github API to have a user-agent. Recommended username / app.
				client.DefaultRequestHeaders.Add("User-Agent", "crimsonalba");

				// Set OAuth token in Authorization header
				client.DefaultRequestHeaders.Add("Authorization", $"token {Configuration.GetValue<string>("GithubApiOAuthToken")}");
			});

			services.AddTransient<IOrganizationDetailLogic, OrganizationDetailLogic>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
