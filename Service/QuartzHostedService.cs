﻿using Quartz;
using Quartz.Spi;
using System.Runtime.InteropServices;
using WebConsoleApplication.Models;

namespace WebConsoleApplication.Service
{
	public class QuartzHostedService : IHostedService
	{
		private readonly IScheduler _scheduler;
		private readonly IJobFactory _jobFactory;
		private readonly IEnumerable<Job> _jobs;

		public QuartzHostedService(IScheduler scheduler, IJobFactory jobFactory, IEnumerable<Job> jobs)
		{
			_scheduler = scheduler;
			_jobFactory = jobFactory;
			_jobs = jobs;
		}

		public IScheduler Scheduler { get; set; }

		public async Task StartAsync (CancellationToken cancellationToken)
		{
			//Common.Logs($"StartAsync At " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"), "StartAsync" + DateTime.Now.ToString("hhmmss"));
			//Scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
			Scheduler = _scheduler;
			Scheduler.JobFactory= _jobFactory;
			foreach(var job in _jobs)
			{
				try
				{
					var aJob = CreateJob(job);
					var trigger = CreateTrigger(job);
					await Scheduler.ScheduleJob(aJob, trigger, cancellationToken);
				}
				catch (Exception ex) {
					continue;
				}

			}
			await Scheduler.Start(cancellationToken);
		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{
			//Common.Logs($"StopAsync At " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"), "StopAsync" + DateTime.Now.ToString("hhmmss"));
			await Scheduler.Shutdown(cancellationToken);
		}

		public static IJobDetail CreateJob(Job job)
		{
			var type = job.Type;
			return JobBuilder.Create(type).WithIdentity(type.FullName).WithDescription(type.Name).Build();
		}

		public static ITrigger CreateTrigger(Job job)
		{
			var type = job.Type;
			return TriggerBuilder.Create().WithIdentity($"{job.Type.FullName}.trigger").WithCronSchedule(job.Expression).WithDescription(job.Expression).Build();
		}
	}
}
