namespace WebConsoleApplication.Models
{
	public class Job
	{
		public Job(Type type, string expression)
		{
			Common.Logs($"Job At "+ DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"), "MyJob" + DateTime.Now.ToString("hhmmss"));
			Type= type;
			Expression= expression;
		}

		public Type Type { get; set; }
		public string Expression { get; set; }
	}
}
