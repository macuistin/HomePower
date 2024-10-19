namespace HomePower.GivEnergy.Dto;

/// <summary>
/// Represents the data of a setting response from the GivEnergy API.
/// </summary>
/// <typeparam name="T">
/// The type of the setting value. This can be a value type or a non-nullable reference type (e.g., string).
/// </typeparam>
public record SettingDataDto<T> where T : notnull
{
    /// <summary>
    /// Gets or sets the value of the setting.
    /// </summary>
    public required T Value { get; set; }
}
