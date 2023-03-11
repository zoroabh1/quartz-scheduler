using Quartz;

namespace WebConsoleApplication.Models
{
    public class TriggerListener : ITriggerListener
    {
        string ITriggerListener.Name => "testing";

        Task ITriggerListener.TriggerComplete(ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task ITriggerListener.TriggerFired(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task ITriggerListener.TriggerMisfired(ITrigger trigger, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<bool> ITriggerListener.VetoJobExecution(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
