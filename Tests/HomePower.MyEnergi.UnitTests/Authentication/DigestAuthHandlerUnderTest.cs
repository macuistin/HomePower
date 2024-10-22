using HomePower.MyEnergi.Authentication;
using System.Diagnostics.CodeAnalysis;

namespace HomePower.MyEnergi.UnitTests.Authentication;

[ExcludeFromCodeCoverage]
internal class DigestAuthHandlerUnderTest : DigestAuthHandler
{
    public DigestAuthHandlerUnderTest(
        string username, 
        string password,
        Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsync)
        : base(username, password)
    {
        InnerHandler = new MockHttpMessageHandler(sendAsync);
    }

    internal new Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        => base.SendAsync(request, cancellationToken);
}
