namespace HomePower.Orchestrator;

public interface IHomeChargerOrchestrator
{
    /// <summary>
    /// Updates the charging schedule asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<bool> UpdateChargingScheduleAsync();
}