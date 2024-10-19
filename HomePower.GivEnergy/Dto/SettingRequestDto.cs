using System.Text.Json.Serialization;

namespace HomePower.GivEnergy.Dto;

/// <summary>
/// Represents a request to the GivEnergy API for a specific setting.
/// </summary>
internal record SettingRequestDto
{
    /// <summary>
    /// Gets or sets the context of the request.
    /// </summary>
    [JsonPropertyName("context")]
    public required string Context { get; set; }
}
