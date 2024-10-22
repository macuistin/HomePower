namespace HomePower.GivEnergy;

public interface IGivEnergyService
{
    /// <summary>
    /// Checks if the AC battery charging is enabled.
    /// </summary>
    /// <returns>A boolean indicating if AC charging is enabled.</returns>
    Task<bool> GetACChargeEnabledAsync();

    /// <summary>
    /// Gets the time when the AC battery charging starts. Schedule 1.
    /// </summary>
    /// <returns>The time when AC charging starts in HH:mm format.</returns>
    Task<TimeOnly> GetBatteryChargeStartTime1Async();
    /// <summary>
    /// Gets the time when the AC battery charging ends. Schedule 1.
    /// </summary>
    /// <returns>The time when AC charging ends in HH:mm format.</returns>
    Task<TimeOnly> GetBatteryChargeEndTime1Async();

    /// <summary>
    /// Gets the time when the AC battery charging starts. Schedule 2.
    /// </summary>
    /// <returns>The time when AC charging starts in HH:mm format.</returns>
    Task<TimeOnly> GetBatteryChargeStartTime2Async();
    /// <summary>
    /// Gets the time when the AC battery charging ends. Schedule 2.
    /// </summary>
    /// <returns>The time when AC charging ends in HH:mm format.</returns>
    Task<TimeOnly> GetBatteryChargeEndTime2Async();

    /// <summary>
    /// Gets the time when the AC battery charging starts. Schedule 3.
    /// </summary>
    /// <returns>The time when AC charging starts in HH:mm format.</returns>
    Task<TimeOnly> GetBatteryChargeStartTime3Async();
    /// <summary>
    /// Gets the time when the AC battery charging ends. Schedule 3.
    /// </summary>
    /// <returns>The time when AC charging ends in HH:mm format.</returns>
    Task<TimeOnly> GetBatteryChargeEndTime3Async();

    /// <summary>
    /// Updates the AC battery charging enabled status.
    /// </summary>
    /// <param name="enabled">A boolean indicating if AC charging should be enabled.</param>
    /// <returns>A boolean indicating if the update was successful.</returns>
    Task<bool> UpdateACChargeEnabledAsync(bool enabled);
    Task<bool> UpdateACCharge1TimesAsync(TimeOnly startTime, TimeOnly endTime);
    Task<bool> UpdateACCharge2TimesAsync(TimeOnly startTime, TimeOnly endTime);
    Task<bool> UpdateACCharge3TimesAsync(TimeOnly startTime, TimeOnly endTime);
}