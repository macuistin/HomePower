using HomePower.GivEnergy;
using HomePower.MyEnergi;
using HomePower.MyEnergi.Model;
using HomePower.Orchestrator.Settings;
using Moq;
using System.Globalization;

namespace HomePower.Orchestrator.UnitTests;

public class HomeChargerOrchestratorTests
{
    private readonly Mock<IGivEnergyService> _givEnergyServiceMock;
    private readonly Mock<IMyEnergiService> _myEnergiServiceMock;
    private readonly OrchestratorSettings _settings;

    public HomeChargerOrchestratorTests()
    {
        _givEnergyServiceMock = new Mock<IGivEnergyService>();
        _myEnergiServiceMock = new Mock<IMyEnergiService>();
        _settings = new OrchestratorSettings
        {
            EvChargeLowPowerCutOffWatts = 3000,
            ImmersionStart = TimeOnly.Parse("06:25", CultureInfo.InvariantCulture),
            ImmersionEnd = TimeOnly.Parse("07:10", CultureInfo.InvariantCulture),
            HouseChargeWindowStart = TimeOnly.Parse("01:00", CultureInfo.InvariantCulture),
            HouseChargeWindowEnd = TimeOnly.Parse("08:00", CultureInfo.InvariantCulture),
            PreImmersionMinutes = -3,
            PostImmersionMinutes = 1
        };
    }

    private HomeChargerOrchestrator CreateSUT(ChargerStatus chargerStatus = ChargerStatus.Unknown, int? chargeRateWatts = 0)
    {
        if (chargerStatus != ChargerStatus.Unknown)
        {
            var evChargeStatus = new EvChargeStatus
            {
                ChargerStatus = chargerStatus,
                ChargeRateWatts = chargeRateWatts
            };

            _myEnergiServiceMock.Setup(m => m.GetEvChargeStatusAsync()).ReturnsAsync(evChargeStatus);
        }
        return new HomeChargerOrchestrator(_givEnergyServiceMock.Object, _myEnergiServiceMock.Object, _settings);
    }

    [Fact]
    public async Task UpdateChargingScheduleAsync_ShouldUpdateToFullWindow_WhenNoHighLoadExpected()
    {
        // Arrange
        var orchestrator = CreateSUT(ChargerStatus.EvConnected);
        _givEnergyServiceMock.Setup(m => m.UpdateACCharge1TimesAsync(It.IsAny<TimeOnly>(), It.IsAny<TimeOnly>())).ReturnsAsync(true);
        _givEnergyServiceMock.Setup(m => m.UpdateACCharge2TimesAsync(It.IsAny<TimeOnly>(), It.IsAny<TimeOnly>())).ReturnsAsync(true);

        // Act
        var result = await orchestrator.UpdateChargingScheduleAsync();

        // Assert
        Assert.True(result);
        _myEnergiServiceMock.Verify(m => m.GetEvChargeStatusAsync(), Times.Once);
        VerifyFullChargeWindowSchedule();
        _givEnergyServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task UpdateChargingScheduleAsync_ShouldUpdatePreAndPostImmersion_WhenHighLoadExpected()
    {
        // Arrange
        var orchestrator = CreateSUT(ChargerStatus.Charging, 3500);
        _givEnergyServiceMock.Setup(m => m.UpdateACCharge1TimesAsync(It.IsAny<TimeOnly>(), It.IsAny<TimeOnly>())).ReturnsAsync(true);
        _givEnergyServiceMock.Setup(m => m.UpdateACCharge2TimesAsync(It.IsAny<TimeOnly>(), It.IsAny<TimeOnly>())).ReturnsAsync(true);

        // Act
        var result = await orchestrator.UpdateChargingScheduleAsync();

        // Assert
        Assert.True(result);
        _myEnergiServiceMock.Verify(m => m.GetEvChargeStatusAsync(), Times.Once);
        VerifyPreAndPostImmersionchedule();
        _givEnergyServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task UpdateChargingScheduleAsync_ShouldNotUpdate_WhenEvChargeStatusIsNull()
    {
        // Arrange
        var orchestrator = CreateSUT();

        // Act
        var result = await orchestrator.UpdateChargingScheduleAsync();

        // Assert
        Assert.False(result);
        _myEnergiServiceMock.Verify(m => m.GetEvChargeStatusAsync(), Times.Once);
        VerifyFullChargeWindowSchedule();
        _givEnergyServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task UpdateChargingScheduleAsync_ShouldNotUpdate_WhenChargeRateWattsIsNull()
    {
        // Arrange
        var orchestrator = CreateSUT(ChargerStatus.Charging, null);

        // Act
        var result = await orchestrator.UpdateChargingScheduleAsync();

        // Assert
        Assert.False(result);
        _myEnergiServiceMock.Verify(m => m.GetEvChargeStatusAsync(), Times.Once);
        VerifyFullChargeWindowSchedule();
        _givEnergyServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task UpdateChargingScheduleAsync_ShouldNotUpdate_WhenChargerStatusIsNotCharging()
    {
        // Arrange
        var orchestrator = CreateSUT(ChargerStatus.EvConnected, 3500);

        // Act
        var result = await orchestrator.UpdateChargingScheduleAsync();

        // Assert
        Assert.False(result);
        _myEnergiServiceMock.Verify(m => m.GetEvChargeStatusAsync(), Times.Once);
        VerifyFullChargeWindowSchedule();
        _givEnergyServiceMock.VerifyNoOtherCalls();
    }

    private void VerifyFullChargeWindowSchedule()
    {
        _givEnergyServiceMock.Verify(m => m.UpdateACCharge1TimesAsync(_settings.HouseChargeWindowStart, _settings.HouseChargeWindowEnd), Times.Once);
        _givEnergyServiceMock.Verify(m => m.UpdateACCharge2TimesAsync(TimeOnly.MinValue, TimeOnly.MinValue), Times.Once);
    }

    private void VerifyPreAndPostImmersionchedule()
    {
        _givEnergyServiceMock.Verify(m => m.UpdateACCharge1TimesAsync(_settings.HouseChargeWindowStart, _settings.PreImmersionTime), Times.Once);
        _givEnergyServiceMock.Verify(m => m.UpdateACCharge2TimesAsync(_settings.PostImmersionTime, _settings.HouseChargeWindowEnd), Times.Once);
    }
}
