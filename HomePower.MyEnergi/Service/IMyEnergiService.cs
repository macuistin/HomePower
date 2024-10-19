using HomePower.MyEnergi.Dto;
using HomePower.MyEnergi.Model;

namespace HomePower.MyEnergi.Service;
/// <summary>
/// Defines methods to interact with MyEnergi services.
/// </summary>
public interface IMyEnergiService
{
    /// <summary>
    /// Gets the status of the Zappi device asynchronously.
    /// </summary>
    /// <returns>A <see cref="ZappiStatusResult"/> containing the status of the Zappi device.</returns>
    Task<ZappiStatusResult> GetZappiStatusAsync();

    /// <summary>
    /// Gets the status of the electric vehicle charge asynchronously.
    /// </summary>
    /// <returns>A <see cref="EvChargeStatus"/> containing the status of the electric vehicle charge.</returns>
    Task<EvChargeStatus> GetEvChargeStatus();
}
