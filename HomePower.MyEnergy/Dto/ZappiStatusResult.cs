namespace HomePower.MyEnergy.Dto;
public record ZappiStatusResult
{
    public static ZappiStatusResult CreateSuccess(ZappiDto zappi)
    {
        return new ZappiStatusResult
        {
            Zappi = zappi,
            Success = true
        };
    }

    public static readonly ZappiStatusResult Failed
        = new()
        {
            Zappi = new ZappiDto(),
            Success = false
        };

    public required ZappiDto Zappi { get; set; }

    /// <summary>
    /// Indicates if the operation was successful.
    /// </summary>
    public required bool Success { get; set; }
}
