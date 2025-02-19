
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

			builder.Services.AddControllersWithViews();
			builder.Services.AddHealthChecks();
			builder.Services.ConfigureServices(builder.Configuration,builder);
			var app = builder.Build();



			if (builder.Environment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				app.UseExceptionHandlingMiddleware();
			}

			app.UseHsts();
			app.UseHttpsRedirection();

			app.UseHealthChecks("/healthcheck");
			app.UseStaticFiles();

			app.UseRouting();             //Identifying action method based route
            app.UseAuthentication();     //Reading Identity Cookie
			app.UseAuthorization();      //Validates access permission of the user
            app.MapControllers();         //Executing the filter pipeline (action method + filters)

			app.Run();
		}
	}
}