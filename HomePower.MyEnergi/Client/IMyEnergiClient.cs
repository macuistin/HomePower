using HomePower.MyEnergi.Dto;
using HomePower.MyEnergi.Model;

namespace HomePower.MyEnergi.Client;
/// <summary>
/// Defines methods to interact with MyEnergi services.
/// </summary>
public interface IMyEnergiClient
{
    /// <summary>
    /// Gets the status of the Zappi device asynchronously.
    /// </summary>
    /// <returns>A <see cref="ZappiStatusResult"/> containing the status of the Zappi device.</returns>
    Task<ZappiStatusResult> GetZappiStatusAsync();
}
