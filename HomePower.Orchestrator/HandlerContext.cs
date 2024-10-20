using HomePower.MyEnergi.Model;

namespace HomePower.Orchestrator;

/// <summary>
/// Represents the context for handling charging requests.
/// </summary>
public class HandlerContext
{
    // Information for the handler
    public required OrchestratorSettings Settings { get; init; }
    public required EvChargeStatus EvChargeStatus { get; init; }
    public required TimeOnly CurrentTime { get; init; }

    // Result of the handler
    public TimeOnly NewChargeStartTime { get; internal set; }
    public TimeOnly NewChargeEndTime { get; internal set; }
    public bool ShouldUpdateChargeTimes { get; internal set; }
    public bool ChargeTimesUpdated { get; set; }

    public void SetNewChargeTimes(TimeOnly startTime, TimeOnly endTime)
    {
        if (startTime > endTime)
            return;
        
        NewChargeStartTime = startTime;
        NewChargeEndTime = endTime;
        ShouldUpdateChargeTimes = true;
    }
}
