namespace HomePower.MyEnergy.Authentication;

internal class DigestChallenge
{
    public string Realm { get; set; }
    public string Nonce { get; set; }
    public string Qop { get; set; }
    public string Opaque { get; set; }
    public string Algorithm { get; set; }
}
