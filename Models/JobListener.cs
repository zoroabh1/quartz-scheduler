using Quartz;

namespace WebConsoleApplication.Models
{
	public class JobListener : IJobListener
	{
		public string Name => "testing";

		public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException? jobException, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}
	}
}
