namespace HomePower.GivEnergy.Dto;

internal record SettingRequestDto
{
    public int Id { get; set; }
    public required string Context { get; set; }
}
