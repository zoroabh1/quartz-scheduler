using Quartz;
using WebConsoleApplication.Models;

namespace WebConsoleApplication.Service
{
	public class JobReminder : IJob
	{
		public JobReminder()
		{

		}
		public Task Execute(IJobExecutionContext context)
		{
			Common.Logs($"JobReminder At " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"), "JobReminder" + DateTime.Now.ToString("hhmmss"));
			return Task.CompletedTask;
		}
	}
}
