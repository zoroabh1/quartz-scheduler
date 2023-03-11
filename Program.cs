using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using Quartz.Spi;
using Quartz.Spi.CosmosDbJobStore;
using System.Collections.Specialized;
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
			//builder.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
			builder.Services.AddSingleton<JobReminder>();
			builder.Services.AddSingleton(new Job(type : typeof(JobReminder),expression:"0/30 0/1 * 1/1 * ? *")); //Every 30 sec
			//builder.Services.AddSingleton<IJobListener, JobListener>();

			var properties = new NameValueCollection();
			//{
			//	{ "quartz.scheduler.idleWaitTime", "1000" }
			//};

			properties[StdSchedulerFactory.PropertySchedulerInstanceName] = "type";
			properties[StdSchedulerFactory.PropertySchedulerInstanceId] = $"{Environment.MachineName}-{Guid.NewGuid()}";
			properties[StdSchedulerFactory.PropertyJobStoreType] = typeof(CosmosDbJobStore).AssemblyQualifiedName;
			properties[$"{StdSchedulerFactory.PropertyObjectSerializer}.type"] = "json";
			properties[$"{StdSchedulerFactory.PropertyJobStorePrefix}.Endpoint"] = "https://hack-undefined.documents.azure.com:443/";
			properties[$"{StdSchedulerFactory.PropertyJobStorePrefix}.Key"] = "PzTBxFlcsf6yiapPYRaq8FNowWztuoGJN6p7oVLoDjPuUP2fuBsbXzQDHjrntHhPDN1qofa39ojEACDbKdVgvQ==";
			properties[$"{StdSchedulerFactory.PropertyJobStorePrefix}.DatabaseId"] = "ToDoList";
			properties[$"{StdSchedulerFactory.PropertyJobStorePrefix}.CollectionId"] = "quartz";
			properties[$"{StdSchedulerFactory.PropertyJobStorePrefix}.Clustered"] = "true";
			properties[StdSchedulerFactory.PropertySchedulerIdleWaitTime] = "1000";

			//var scheduler = new StdSchedulerFactory(properties);
			//return scheduler.GetScheduler();

			var schedulerFactory = new StdSchedulerFactory(properties);
			var scheduler = schedulerFactory.GetScheduler().Result;
			scheduler.ListenerManager.AddJobListener(new JobListener(), GroupMatcher<JobKey>.AnyGroup());


			scheduler.ListenerManager.AddTriggerListener(new TriggerListener(), GroupMatcher<TriggerKey>.AnyGroup());
			scheduler.ListenerManager.AddSchedulerListener(new ScheduleListener());
			//scheduler.Start().Wait();

			builder.Services.AddSingleton(scheduler);

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