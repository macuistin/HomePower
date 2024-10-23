using HomePower.MyEnergi.Client;
using HomePower.MyEnergi.Extensions;
using HomePower.MyEnergi.Model;
using Microsoft.Extensions.Logging;

namespace HomePower.MyEnergi;

public class MyEnergiService(IMyEnergiClient _client) : IMyEnergiService
{
    /// <inheritdoc/>
    public async Task<EvChargeStatus> GetEvChargeStatusAsync()
    {
        var zappiStatus = await _client.GetZappiStatusAsync();

        return zappiStatus.Success 
            ? zappiStatus.Zappi.ToEvChargeStatus() 
            : EvChargeStatus.Failed;
    }
}
