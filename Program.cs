using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System.Linq.Expressions;
using WebConsoleApplication.Models;
using WebConsoleApplication.Service;

namespace WebConsoleApplication
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();
			builder.Services.AddHostedService<QuartzHostedService>();
			builder.Services.AddSingleton<IJobFactory, Service.SingletonJobFactory>();
			builder.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
			builder.Services.AddSingleton<JobReminder>();
			builder.Services.AddSingleton(new Job(type : typeof(JobReminder),expression:"0/5 0/1 * 1/1 * ? *")); //Every 30 sec

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}
	}
}