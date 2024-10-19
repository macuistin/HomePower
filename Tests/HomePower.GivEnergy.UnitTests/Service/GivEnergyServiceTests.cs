using HomePower.GivEnergy;
using HomePower.GivEnergy.Dto;
using HomePower.GivEnergy.Service;
using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace HomePower.GivEnergy.UnitTests.Service;

public class GivEnergyServiceTests
{
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private readonly HttpClient _httpClient;

    public GivEnergyServiceTests()
    {
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("http://mockaddress/")
        };
    }

    private GivEnergyService CreateService()
    {
        return new GivEnergyService(
            _httpClient,
            new GivEnergySettings() { InverterSerialNumber = "serial"});
    }

    [Fact]
    public async Task GetBatteryChargeStartTimeAsync_ReturnsCorrectTime()
    {
        // Arrange
        var settingResponseDto = new SettingResponseDto
        {
            Data = new SettingDataDto { Value = "08:00" }
        };
        ConfigureHttpGetResponseOk(settingResponseDto);

        var service = CreateService();

        // Act
        var result = await service.GetBatteryChargeStartTimeAsync();

        // Assert
        Assert.Equal("08:00", result);
    }

    [Fact]
    public async Task GetBatteryChargeStartTimeAsync_ReturnsEmptyString_OnFailure()
    {
        // Arrange
        ConfigureHttpGetResponseFail(HttpStatusCode.InternalServerError);

        var service = CreateService();

        // Act
        var result = await service.GetBatteryChargeStartTimeAsync();

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public async Task GetBatteryChargeEndTimeAsync_ReturnsCorrectTime()
    {
        // Arrange
        var settingResponseDto = new SettingResponseDto
        {
            Data = new SettingDataDto { Value = "20:00" }
        };
        ConfigureHttpGetResponseOk(settingResponseDto);

        var service = CreateService();

        // Act
        var result = await service.GetBatteryChargeEndTimeAsync();

        // Assert
        Assert.Equal("20:00", result);
    }

    [Fact]
    public async Task GetBatteryChargeEndTimeAsync_ReturnsEmptyString_OnFailure()
    {
        // Arrange
        ConfigureHttpGetResponseFail(HttpStatusCode.InternalServerError);

        var service = CreateService();

        // Act
        var result = await service.GetBatteryChargeEndTimeAsync();

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public async Task GetACChargeEnabledAsync_ReturnsTrue()
    {
        // Arrange
        var settingResponseDto = new SettingResponseDto
        {
            Data = new SettingDataDto { Value = "true" }
        };
        ConfigureHttpGetResponseOk(settingResponseDto);

        var service = CreateService();

        // Act
        var result = await service.GetACChargeEnabledAsync();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task GetACChargeEnabledAsync_ReturnsFalse_OnFailure()
    {
        // Arrange
        ConfigureHttpGetResponseFail(HttpStatusCode.InternalServerError);

        var service = CreateService();

        // Act
        var result = await service.GetACChargeEnabledAsync();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateBatteryChargeStartTimeAsync_ReturnsTrue()
    {
        // Arrange
        var service = CreateService();

        // Act
        var result = await service.UpdateBatteryChargeStartTimeAsync(8, 0);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task UpdateBatteryChargeStartTimeAsync_ReturnsFalse_OnInvalidTime()
    {
        // Arrange
        var service = CreateService();

        // Act
        var result = await service.UpdateBatteryChargeStartTimeAsync(25, 0);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateBatteryChargeEndTimeAsync_ReturnsTrue()
    {
        // Arrange
        var service = CreateService();

        // Act
        var result = await service.UpdateBatteryChargeEndTimeAsync(20, 0);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task UpdateBatteryChargeEndTimeAsync_ReturnsFalse_OnInvalidTime()
    {
        // Arrange
        var service = CreateService();

        // Act
        var result = await service.UpdateBatteryChargeEndTimeAsync(20, 60);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateACChargeEnabledAsync_ReturnsTrue()
    {
        // Arrange
        var service = CreateService();

        // Act
        var result = await service.UpdateACChargeEnabledAsync(true);

        // Assert
        Assert.True(result);
    }


    #region Helper methods
    private void ConfigureHttpGetResponseOk<T>(T content)
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
