using HomePower.Orchestrator;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace HomePower.ScheduledFunction
{
    public class UpdateScheduleFunction
    {
        private readonly ILogger _logger;
        private readonly IHomeChargerOrchestrator _orchestrator;

        public UpdateScheduleFunction(
            ILoggerFactory loggerFactory,
            IHomeChargerOrchestrator orchestrator)
        {
            _logger = loggerFactory.CreateLogger<UpdateScheduleFunction>();
            _orchestrator = orchestrator;
        }

        [Function("UpdateScheduleFunction")]
        public void Run([TimerTrigger("0 */5 22-23,0-8 * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation("Trigger function executed at: {Time}", DateTime.Now);

            _orchestrator.UpdateChargingScheduleAsync();

            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation("Next timer schedule at: {NextTime}", myTimer.ScheduleStatus.Next);
            }
        }
    }
}
