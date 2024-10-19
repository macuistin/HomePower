namespace HomePower.GivEnergy.Dto;

public record SettingResponseDto
{
    public static readonly SettingResponseDto Failed = new()
    {
        Success = false,
        Data = new()
        {
            Value = string.Empty
        }
    };

    public required SettingDataDto Data { get; set; }

    public bool Success { get; private set; } = true;
}
