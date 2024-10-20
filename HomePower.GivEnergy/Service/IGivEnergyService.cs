namespace HomePower.GivEnergy.Service;

public interface IGivEnergyService
{
    /// <summary>
    /// Checks if the AC battery charging is enabled.
    /// </summary>
    /// <returns>A boolean indicating if AC charging is enabled.</returns>
    Task<bool> GetACChargeEnabledAsync();

    /// <summary>
    /// Gets the time when the AC battery charging ends.
    /// </summary>
    /// <returns>The time when AC charging ends in HH:mm format.</returns>
    Task<string> GetBatteryChargeEndTimeAsync();

    /// <summary>
    /// Gets the time when the AC battery charging starts.
    /// </summary>
    /// <returns>The time when AC charging starts in HH:mm format.</returns>
    Task<string> GetBatteryChargeStartTimeAsync();

    /// <summary>
    /// Updates the AC battery charging enabled status.
    /// </summary>
    /// <param name="enabled">A boolean indicating if AC charging should be enabled.</param>
    /// <returns>A boolean indicating if the update was successful.</returns>
    Task<bool> UpdateACChargeEnabledAsync(bool enabled);

    /// <summary>
    /// Updates the time when the AC battery charging ends.
    /// </summary>
    /// <param name="hour">The hour when AC charging ends (0-23).</param>
    /// <param name="minute">The minute when AC charging ends (0-59).</param>
    /// <returns>A boolean indicating if the update was successful.</returns>
    Task<bool> UpdateBatteryChargeEndTimeAsync(TimeOnly startTime);

    /// <summary>
    /// Updates the time when the AC battery charging starts.
    /// </summary>
    /// <param name="hour">The hour when AC charging starts (0-23).</param>
    /// <param name="minute">The minute when AC charging starts (0-59).</param>
    /// <returns>A boolean indicating if the update was successful.</returns>
    Task<bool> UpdateBatteryChargeStartTimeAsync(TimeOnly endTime);
}
