using HomePower.MyEnergi.Authentication;
using System.Net;
using System.Net.Http.Headers;

namespace HomePower.MyEnergi.UnitTests.Authentication
{
    public class DigestAuthHandlerTests
    {       
        private static DigestAuthHandlerUnderTest CreateSut(
            Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsync) =>
            new ("testuser", "testpassword", sendAsync);

        [Fact]
        public async Task SendAsync_UnauthorizedResponse_RetriesWithDigestAuth()
        {
            // Arrange
            var unauthorizedResponse = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            unauthorizedResponse.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue("Digest", "realm=\"testrealm\", nonce=\"testnonce\", qop=\"auth\""));

            var successResponse = new HttpResponseMessage(HttpStatusCode.OK);

            var sut = CreateSut((request, cancellationToken) =>
            {
                if (request.Headers.Authorization?.Scheme == "Digest")
                {
                    return Task.FromResult(successResponse);
                }

                return Task.FromResult(unauthorizedResponse);
            });

            var request = new HttpRequestMessage(HttpMethod.Get, "http://test.com");

            // Act
            var response = await sut.SendAsync(request, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task SendAsync_SuccessfulResponse_DoesNotRetry()
        {
            // Arrange
            var successResponse = new HttpResponseMessage(HttpStatusCode.OK);

            var sut = CreateSut((request, cancellationToken) => Task.FromResult(successResponse));

            var request = new HttpRequestMessage(HttpMethod.Get, "http://test.com");

            // Act
            var response = await sut.SendAsync(request, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public void ParseDigestChallenge_ValidChallenge_ParsesCorrectly()
        {
            // Arrange
            var challenge = "realm=\"testrealm\", nonce=\"testnonce\", qop=\"auth\", opaque=\"testopaque\", algorithm=\"MD5\"";

            // Act
            var result = DigestAuthHandlerUnderTest.ParseDigestChallenge(challenge);

            // Assert
            Assert.Equal("testrealm", result.Realm);
            Assert.Equal("testnonce", result.Nonce);
            Assert.Equal("auth", result.Qop);
            Assert.Equal("testopaque", result.Opaque);
            Assert.Equal("MD5", result.Algorithm);
        }

        [Fact]
        public void CreateDigestResponse_ValidInputs_CreatesCorrectResponse()
        {
            // Arrange
            var challenge = new DigestChallenge
            {
                Realm = "testrealm",
                Nonce = "testnonce",
                Qop = "auth",
                Opaque = "testopaque",
                Algorithm = "MD5"
            };
            var httpMethod = "GET";
            var uri = "/test";
            
            var sut = CreateSut((request, cancellationToken) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));
            
            // Act           
            var response = sut.CreateDigestResponse(challenge, httpMethod, uri);

            // Assert
            Assert.Contains("username=\"testuser\"", response);
            Assert.Contains("realm=\"testrealm\"", response);
            Assert.Contains("nonce=\"testnonce\"", response);
            Assert.Contains("uri=\"/test\"", response);
            Assert.Contains("algorithm=MD5", response);
            Assert.Contains("response=", response);
            Assert.Contains("qop=auth", response);
            Assert.Contains("nc=00000001", response);
            Assert.Contains("cnonce=", response);
            Assert.Contains("opaque=\"testopaque\"", response);
        }
    }
}
