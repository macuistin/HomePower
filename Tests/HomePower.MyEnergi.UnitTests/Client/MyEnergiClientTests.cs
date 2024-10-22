using HomePower.MyEnergi.Client;
using HomePower.MyEnergi.Dto;
using HomePower.MyEnergi.Model;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http.Json;

namespace HomePower.MyEnergi.UnitTests.Client;

public class MyEnergiClientTests
{
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private readonly HttpClient _httpClient;

    public MyEnergiClientTests()
    {
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("http://mockaddress/")
        };
    }

    private MyEnergiClient CreateService()
    {
        return new MyEnergiClient(_httpClient);
    }

    [Fact]
    public async Task GetZappiStatusAsync_ReturnsSuccess()
    {
        // Arrange
        var zappiStatusDto = new ZappiStatusDto
        {
            Zappi = [new ZappiDto()]
        };

        ConfigureHttpGetResponseOk(zappiStatusDto);

        var service = CreateService();

        // Act
        var result = await service.GetZappiStatusAsync();

        // Assert
        Assert.True(result.Success);
    }

    [Fact]
    public async Task GetZappiStatusAsync_ShouldReturnFailed_WhenZappiCountIsZero()
    {
        // Arrange
        var zappiStatusDto = new ZappiStatusDto
        {
            Zappi = []
        };

        ConfigureHttpGetResponseOk(zappiStatusDto);

        var service = CreateService();

        // Act
        var result = await service.GetZappiStatusAsync();

        // Assert
        Assert.False(result.Success);
        Assert.Equal(ZappiStatusResult.Failed, result);
    }

    [Fact]
    public async Task GetZappiStatusAsync_ShouldReturnFailed_WhenZappiListIsNull()
    {
        // Arrange
        var zappiStatusDto = new ZappiStatusDto
        {
            Zappi = null
        };

        ConfigureHttpGetResponseOk(zappiStatusDto);

        var service = CreateService();

        // Act
        var result = await service.GetZappiStatusAsync();

        // Assert
        Assert.False(result.Success);
        Assert.Equal(ZappiStatusResult.Failed, result);
    }

    [Fact]
    public async Task GetZappiStatusAsync_ReturnsFailed_OnHttpError()
    {
        // Arrange
        ConfigureHttpGetResponseFail(HttpStatusCode.InternalServerError);

        var service = CreateService();

        // Act
        var result = await service.GetZappiStatusAsync();

        // Assert
        Assert.False(result.Success);
        Assert.Equal(ZappiStatusResult.Failed, result);
    }

    #region Helper methods
    private void ConfigureHttpGetResponseOk(ZappiStatusDto content)
    {
        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(content)
            });
    }

    private void ConfigureHttpGetResponseFail(HttpStatusCode statusCode)
    {
        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage(statusCode));
    }
    #endregion Helper methods
}
