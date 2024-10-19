using HomePower.MyEnergy.Dto;
using HomePower.MyEnergy.Model;

namespace HomePower.MyEnergy.Service;
public interface IMyEnergiService
{
    Task<ZappiStatusResult> GetZappiStatusAsync();
    Task<EvChargeStatus> GetEvChargeStatus();
}