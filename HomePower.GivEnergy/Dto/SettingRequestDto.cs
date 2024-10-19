using System.Text.Json.Serialization;

namespace HomePower.GivEnergy.Dto;

internal record SettingRequestDto
{
    [JsonPropertyName("id")]
    public int Id { get;  set; }

    [JsonPropertyName("context")]
    public required string Context { get; set; }
}
