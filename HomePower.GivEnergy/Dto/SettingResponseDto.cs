namespace HomePower.GivEnergy.Dto;

/// <summary>
/// Represents a response from the GivEnergy API for a specific setting.
/// </summary>
/// <typeparam name="T">
/// The type of the setting value. This can be a value type or a non-nullable reference type (e.g., string).
/// The type varies depending on the setting being requested from the API.
/// </typeparam>
public record SettingResponseDto<T> where T : notnull
{
    /// <summary>
    /// A static instance representing a failed response.
    /// </summary>
    public static readonly SettingResponseDto<T> Failed = new()
    {
        Success = false,
        Data = new()
        {
            Value = default!
        }
    };

    /// <summary>
    /// Gets or sets the data of the setting response.
    /// </summary>
    public required SettingDataDto<T> Data { get; set; }

    /// <summary>
    /// Gets a value indicating whether the request was successful.
    /// </summary>
    public bool Success { get; private set; } = true;
}
