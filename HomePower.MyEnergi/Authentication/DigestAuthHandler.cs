using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace HomePower.MyEnergi.Authentication;

/// <summary>
/// A message handler that performs HTTP Digest Authentication.
/// </summary>
[ExcludeFromCodeCoverage]
public class DigestAuthHandler : DelegatingHandler
{
    private readonly string _username;
    private readonly string _password;

    /// <summary>
    /// Initializes a new instance of the <see cref="DigestAuthHandler"/> class.
    /// </summary>
    /// <param name="username">The username for authentication.</param>
    /// <param name="password">The password for authentication.</param>
    public DigestAuthHandler(string username, string password)
    {
        _username = username;
        _password = password;
        InnerHandler = new HttpClientHandler();
    }

    /// <summary>
    /// Sends an HTTP request with Digest Authentication.
    /// </summary>
    /// <param name="request">The HTTP request message.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The HTTP response message.</returns>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);
        if (response.StatusCode != HttpStatusCode.Unauthorized)
        {
            return response;
        }

        var wwwAuthenticateHeader = response.Headers.WwwAuthenticate.FirstOrDefault(h => h.Scheme.Equals("Digest", StringComparison.OrdinalIgnoreCase));
        if (wwwAuthenticateHeader == null)
        {
            return response;
        }

        var digestHeader = ParseDigestChallenge(wwwAuthenticateHeader.Parameter);
        var digestResponse = CreateDigestResponse(digestHeader, request.Method.Method, request.RequestUri.PathAndQuery);
        var newRequest = await CloneHttpRequestMessageAsync(request);
        newRequest.Headers.Authorization = new AuthenticationHeaderValue("Digest", digestResponse);

        response.Dispose();

        response = await base.SendAsync(newRequest, cancellationToken);

        return response;
    }

    /// <summary>
    /// Parses the Digest challenge from the WWW-Authenticate header.
    /// </summary>
    /// <param name="challenge">The challenge string from the WWW-Authenticate header.</param>
    /// <returns>A <see cref="DigestChallenge"/> object containing the parsed values.</returns>
    internal static DigestChallenge ParseDigestChallenge(string challenge)
    {
        var digestChallenge = new DigestChallenge();

        var parts = challenge.Split(',');

        foreach (var part in parts)
        {
            var keyValue = part.Split(['='], 2);
            var key = keyValue[0].Trim();
            var value = keyValue[1].Trim().Trim('"');

            switch (key)
            {
                case "realm":
                    digestChallenge.Realm = value;
                    break;
                case "nonce":
                    digestChallenge.Nonce = value;
                    break;
                case "qop":
                    digestChallenge.Qop = value;
                    break;
                case "opaque":
                    digestChallenge.Opaque = value;
                    break;
                case "algorithm":
                    digestChallenge.Algorithm = value;
                    break;
            }
        }

        return digestChallenge;
    }

    /// <summary>
    /// Creates the Digest response header value.
    /// </summary>
    /// <param name="challenge">The parsed Digest challenge.</param>
    /// <param name="httpMethod">The HTTP method (e.g., GET, POST).</param>
    /// <param name="uri">The request URI.</param>
    /// <returns>The Digest response header value.</returns>
    internal string CreateDigestResponse(DigestChallenge challenge, string httpMethod, string uri)
    {
        var ha1 = CalculateMD5Hash($"{_username}:{challenge.Realm}:{_password}");
        var ha2 = CalculateMD5Hash($"{httpMethod}:{uri}");

        var nonceCount = "00000001";
        var cnonce = GenerateCNonce();

        var responseDigest = CalculateMD5Hash($"{ha1}:{challenge.Nonce}:{nonceCount}:{cnonce}:{challenge.Qop}:{ha2}");

        var header = new StringBuilder();
        header.AppendFormat("username=\"{0}\", ", _username);
        header.AppendFormat("realm=\"{0}\", ", challenge.Realm);
        header.AppendFormat("nonce=\"{0}\", ", challenge.Nonce);
        header.AppendFormat("uri=\"{0}\", ", uri);
        header.AppendFormat("algorithm=MD5, ");
        header.AppendFormat("response=\"{0}\", ", responseDigest);
        header.AppendFormat("qop={0}, ", challenge.Qop);
        header.AppendFormat("nc={0}, ", nonceCount);
        header.AppendFormat("cnonce=\"{0}\"", cnonce);

        if (!string.IsNullOrEmpty(challenge.Opaque))
        {
            header.AppendFormat(", opaque=\"{0}\"", challenge.Opaque);
        }

        return header.ToString();
    }

    /// <summary>
    /// Calculates the MD5 hash of the specified input string.
    /// </summary>
    /// <param name="input">The input string.</param>
    /// <returns>The MD5 hash as a hexadecimal string.</returns>
    private static string CalculateMD5Hash(string input)
    {
        var inputBytes = Encoding.ASCII.GetBytes(input);
        var hash = MD5.HashData(inputBytes);
        return ToHexString(hash);
    }

    /// <summary>
    /// Converts a byte array to a hexadecimal string.
    /// </summary>
    /// <param name="bytes">The byte array.</param>
    /// <returns>The hexadecimal string.</returns>
    private static string ToHexString(byte[] bytes)
    {
        var sb = new StringBuilder(bytes.Length * 2);
        foreach (var b in bytes)
            sb.AppendFormat("{0:x2}", b);
        return sb.ToString();
    }

    /// <summary>
    /// Generates a client nonce (cnonce) for the Digest authentication.
    /// </summary>
    /// <returns>The generated cnonce as a hexadecimal string.</returns>
    private static string GenerateCNonce()
    {
        var random = new byte[16];
        RandomNumberGenerator.Fill(random);
        return ToHexString(random);
    }

    /// <summary>
    /// Clones the specified <see cref="HttpRequestMessage"/>.
    /// </summary>
    /// <param name="request">The request message to clone.</param>
    /// <returns>A cloned <see cref="HttpRequestMessage"/>.</returns>    
    private static async Task<HttpRequestMessage> CloneHttpRequestMessageAsync(HttpRequestMessage request)
    {
        var clone = new HttpRequestMessage(request.Method, request.RequestUri);

        // Copy the request's content (via a MemoryStream) into the cloned object
        if (request.Content != null)
        {
            var ms = new MemoryStream();
            await request.Content.CopyToAsync(ms);
            ms.Position = 0;
            clone.Content = new StreamContent(ms);

            // Copy the content headers
            foreach (var header in request.Content.Headers)
            {
                clone.Content.Headers.Add(header.Key, header.Value);
            }
        }

        // Copy the request's headers
        foreach (var header in request.Headers)
        {
            clone.Headers.Add(header.Key, header.Value);
        }

        // Copy the request's properties (if any)
#if !NETSTANDARD
        foreach (var prop in request.Options)
        {
            clone.Options.Set(new HttpRequestOptionsKey<object>(prop.Key), prop.Value);
        }
#endif

        return clone;
    }
}
