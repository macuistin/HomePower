using HomePower.MyEnergi.Dto;
using HomePower.MyEnergi.Model;

namespace HomePower.MyEnergi.Service;
public interface IMyEnergiService
{
    Task<ZappiStatusResult> GetZappiStatusAsync();
    Task<EvChargeStatus> GetEvChargeStatus();
}