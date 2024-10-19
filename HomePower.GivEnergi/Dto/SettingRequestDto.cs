namespace HomePower.GivEnergi.Dto;

internal record SettingRequestDto
{
    public int Id { get; set; }
    public required string Context { get; set; }
}
