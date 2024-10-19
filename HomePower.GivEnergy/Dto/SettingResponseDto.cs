namespace HomePower.GivEnergy.Dto;

public record SettingResponseDto<T> where T : notnull
{
    public static readonly SettingResponseDto<T> Failed = new()
    {
        Success = false,
        Data = new()
        {
            Value = default
        }
    };

    public required SettingDataDto<T> Data { get; set; }

    public bool Success { get; private set; } = true;
}