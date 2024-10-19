namespace HomePower.GivEnergy;

/// <summary>
/// Represents the settings for GivEnergy API integration.
/// </summary>
public class GivEnergySettings
{
    /// <summary>
    /// Gets or sets the base URL for the GivEnergy API.
    /// </summary>
    public required string BaseUrl { get; set; } = "https://api.givenergy.cloud/";

    /// <summary>
    /// Gets or sets the API bearer token for authentication.
    /// </summary>
    public required string ApiBearer { get; set; }

    /// <summary>
    /// Gets or sets the serial number of the inverter.
    /// </summary>
    public required string InverterSerialNumber { get; set; }
}
