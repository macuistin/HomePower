using System.Text.Json.Serialization;

namespace HomePower.MyEnergi.Dto;

/// <summary>
/// Represents the status of Zappi devices.
/// </summary>
public record ZappiStatusDto
{
    /// <summary>
    /// List of Zappi devices.
    /// </summary>
    [JsonPropertyName("zappi")]
    public List<ZappiDto> Zappi { get; set; }

    /// <summary>
    /// Indicates if the operation was successful.
    /// </summary>
    public bool Success { get; set; }
}
