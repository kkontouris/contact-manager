
using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using Services;
using Repositories;
using RepositoryContracts;
using _16CrudExample.StartupExtensions;
using _16CrudExample.Middleware;

namespace _16CrudExample
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			//Logging
			//builder.Host.ConfigureLogging(loggingProvider =>
			//{
			//	loggingProvider.ClearProviders();
			//	loggingProvider.AddDebug();
			//});

			builder.Services.AddControllersWithViews();
			builder.Services.AddHealthChecks();
			builder.Services.ConfigureServices(builder.Configuration,builder);
			var app = builder.Build();

			//Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PersonsDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False


			//create application pipeline
			if (builder.Environment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				app.UseExceptionHandlingMiddleware();
			}
			//app.UseHttpLogging();
			//app.Logger.LogDebug("debug message");

			app.UseHsts();
			app.UseHttpsRedirection();

			app.MapGet("/test", () => Environment.GetEnvironmentVariable("MYSQLCONNSTR_localdb")).AllowAnonymous();

			app.UseHealthChecks("/healthcheck");
			app.UseStaticFiles();

			app.UseRouting();             //Identifying action method based route
            app.UseAuthentication();     //Reading Identity Cookie
			app.UseAuthorization();      //Validates access permission of the user
            app.MapControllers();         //Executing the filter pipeline (action method + filters)

			////conventional routing for areas
			//app.UseEndpoints(endpoints =>
			//{
			//	endpoints.MapControllerRoute(
			//		name: "areas",
			//		pattern: "{area:exists}/{controller=Home}/{action=Index}"
			//		);

			//endpoints.MapControllerRoute(
			//name: "default",
			//pattern: "{controller}/{action}/{id?}"
			//);
            
			//});



			app.Run();
		}
	}
}