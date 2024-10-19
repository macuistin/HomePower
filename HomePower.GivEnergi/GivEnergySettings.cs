namespace HomePower.GivEnergi;
public class GivEnergySettings
{
    public string BaseUrl { get; set; } = "https://api.givenergy.cloud/v1";
    public string ApiBearer { get; set; }
    public string InverterSerialNumber { get; set; }
}
