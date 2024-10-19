using HomePower.MyEnergi.Dto;
using HomePower.MyEnergi.Model;
using HomePower.MyEnergi.Service;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http.Json;

namespace HomePower.MyEnergi.UnitTests.Service;

public class MyEnergiServiceTests
{
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private readonly HttpClient _httpClient;

    public MyEnergiServiceTests()
    {
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("http://mockaddress/")
        };
    }

    private MyEnergiService CreateService()
    {
        return new MyEnergiService(_httpClient);
    }

    [Fact]
    public async Task GetZappiStatusAsync_ReturnsSuccess()
    {
        // Arrange
        var zappiStatusDto = new ZappiStatusDto
        {
            Zappi = [new ZappiDto ()]
        };

        ConfigureHttpGetResponseOk(zappiStatusDto);

        var service = CreateService();

        // Act
        var result = await service.GetZappiStatusAsync();

        // Assert
        Assert.True(result.Success);
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
    }

    [Fact]
    public async Task GetEvChargeStatus_ReturnsSuccess()
    {
        // Arrange
        var zappiStatusDto = new ZappiStatusDto
        {
            Zappi = [new ZappiDto()]
        };

        ConfigureHttpGetResponseOk(zappiStatusDto);

        var service = CreateService();

        // Act
        var result = await service.GetEvChargeStatus();

        // Assert
        Assert.NotEqual(EvChargeStatus.Failed, result);
    }

    [Fact]
    public async Task GetEvChargeStatus_ReturnsFailed_OnZappiStatusFailed()
    {
        // Arrange
        ConfigureHttpGetResponseFail(HttpStatusCode.InternalServerError);

        var service = CreateService();

        // Act
        var result = await service.GetEvChargeStatus();

        // Assert
        Assert.Equal(EvChargeStatus.Failed, result);
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
