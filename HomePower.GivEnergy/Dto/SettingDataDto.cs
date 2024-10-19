namespace HomePower.GivEnergy.Dto;

public record SettingDataDto<T> where T : notnull
{
    public required T Value { get; set; }
}
