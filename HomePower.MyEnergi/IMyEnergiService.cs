using HomePower.MyEnergi.Model;

namespace HomePower.MyEnergi;
public interface IMyEnergiService
{
    /// <summary>
    /// Gets the status of the electric vehicle charge asynchronously.
    /// </summary>
    /// <returns>A <see cref="EvChargeStatus"/> containing the status of the electric vehicle charge.</returns>
    Task<EvChargeStatus> GetEvChargeStatusAsync();
}
