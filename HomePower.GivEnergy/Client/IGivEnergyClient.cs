using HomePower.GivEnergy.Dto;

namespace HomePower.GivEnergy.Client;

public interface IGivEnergyClient
{
    Task<SettingResponseDto<TimeOnly>> GetTimeSettingAsync(InverterSettingId settingId);
    Task<SettingResponseDto<T>> GetSettingAsync<T>(int settingId)
        where T : notnull;
    Task<SettingResponseDto<T>> GetSettingAsync<T>(InverterSettingId settingId)
        where T : notnull;

    Task<bool> UpdateSettingAsync<T>(InverterSettingId settingId, TimeOnly value);
    Task<bool> UpdateSettingAsync<T>(int settingId, T value)
        where T : notnull;
    Task<bool> UpdateSettingAsync<T>(InverterSettingId settingId, T value)
        where T : notnull;
}
