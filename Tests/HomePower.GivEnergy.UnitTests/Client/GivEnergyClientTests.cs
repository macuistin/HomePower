using HomePower.GivEnergy.Client;
using HomePower.GivEnergy.Dto;
using HomePower.GivEnergy.Settings;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http.Json;

namespace HomePower.GivEnergy.UnitTests.Client;

public class GivEnergyClientTests
{
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private readonly HttpClient _httpClient;

    public GivEnergyClientTests()
    {
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("http://mockaddress/")
        };
    }

    private GivEnergyClient Client()
    {
        return new GivEnergyClient(
            _httpClient,
            new GivEnergySettings() { InverterSerialNumber = "serial", ApiBearer = "Bearer", BaseUrl = "http://baseurl" });
    }

    [Fact]
    public async Task GetTimeSettingAsync_TimeOnly_ShouldReturnSuccess_WhenResponseIsValid()
    {
        // Arrange
        var expectedTime = new TimeOnly(14, 30);
        var settingId = InverterSettingId.ACCharge1StartTime;
        ConfigureHttpGetResponseCreated("14:30");

        var client = Client();

        // Act
        var result = await client.GetTimeSettingAsync(settingId);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(expectedTime, result.Data.Value);
    }

    [Fact]
    public async Task GetSettingAsync_TimeOnly_ShouldReturnFailed_WhenResponseIsInvalid()
    {
        // Arrange
        var settingId = InverterSettingId.ACCharge1StartTime;
        ConfigureHttpGetResponseFail();

        var client = Client();

        // Act
        var result = await client.GetSettingAsync<TimeOnly>(settingId);

        // Assert
        Assert.False(result.Success);
    }

    [Fact]
    public async Task GetSettingAsync_Generic_ShouldReturnSuccess_WhenResponseIsValid()
    {
        // Arrange
        var expectedValue = 100;
        var settingId = InverterSettingId.ACChargeEnable;
        ConfigureHttpGetResponseCreated(expectedValue);

        var client = Client();

        // Act
        var result = await client.GetSettingAsync<int>(settingId);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(expectedValue, result.Data.Value);
    }

    [Fact]
    public async Task GetSettingAsync_Generic_ShouldReturnFailed_WhenResponseIsInvalid()
    {
        // Arrange
        var settingId = InverterSettingId.ACChargeEnable;
        ConfigureHttpGetResponseFail();

        var client = Client();

        // Act
        var result = await client.GetSettingAsync<int>(settingId);

        // Assert
        Assert.False(result.Success);
    }

    [Fact]
    public async Task UpdateSettingAsync_TimeOnly_ShouldThrowNotImplementedException()
    {
        // Arrange
        var settingId = InverterSettingId.ACCharge1StartTime;
        var value = new TimeOnly(14, 30);
        var client = Client();

        // Act & Assert
        await Assert.ThrowsAsync<NotImplementedException>(() => client.UpdateSettingAsync(settingId, value));
    }

    [Fact]
    public async Task UpdateSettingAsync_Generic_ShouldThrowNotImplementedException()
    {
        // Arrange
        var settingId = InverterSettingId.ACChargeEnable;
        var value = true;
        var client = Client();

        // Act & Assert
        await Assert.ThrowsAsync<NotImplementedException>(() => client.UpdateSettingAsync(settingId, value));
    }

    #region Helper methods
    private void ConfigureHttpGetResponseCreated<T>(T value)
        where T : notnull
    {
        var responseDto = SettingResponseDto<T>.CreateSuccess(value);

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.Created)
            {
                Content = JsonContent.Create(responseDto)
            });
    }

    private void ConfigureHttpGetResponseFail(HttpStatusCode statusCode = HttpStatusCode.BadRequest)
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
