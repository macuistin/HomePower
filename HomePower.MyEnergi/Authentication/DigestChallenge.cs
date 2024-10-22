
namespace HomePower.MyEnergi.Authentication;

/// <summary>
/// Represents the Digest challenge parameters.
/// </summary>
internal class DigestChallenge
{
    public string Realm { get; set; }
    public string Nonce { get; set; }
    public string Qop { get; set; }
    public string Opaque { get; set; }
    public string Algorithm { get; set; }
}
