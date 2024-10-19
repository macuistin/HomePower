namespace HomePower.MyEnergi.Settings;
public class MyEnergiSettings
{
    public required string ZappiApiKey { get; set; }
    public required string ZappiSerialNumber { get; set; }
    public required string BaseUrl { get; set; } = "https://s18.myenergi.net";
}