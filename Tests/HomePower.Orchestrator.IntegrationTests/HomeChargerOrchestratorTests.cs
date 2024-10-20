using HomePower.GivEnergy.Service;
using HomePower.MyEnergi.Model;
using HomePower.MyEnergi.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using System.Globalization;

namespace HomePower.Orchestrator.IntegrationTests
{
    public class HomeChargerOrchestratorTests
    {
        private Mock<IGivEnergyService> _givEnergyServiceMock;
        private Mock<IMyEnergiService> _myEnergiServiceMock;
        private Mock<ITimeProvider> _timeProviderMock;
        private OrchestratorSettings _settings;

        public HomeChargerOrchestratorTests()
        {
            _givEnergyServiceMock = new Mock<IGivEnergyService>();
            _myEnergiServiceMock = new Mock<IMyEnergiService>();
            _timeProviderMock = new Mock<ITimeProvider>();
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

        private IHomeChargerOrchestrator CreateHomeChargerOrchestrator()
        {
            var services = new ServiceCollection();
            var sp = services
                .AddSingleton(_givEnergyServiceMock.Object)
                .AddSingleton(_myEnergiServiceMock.Object)
                .AddOrchestratorServices(_settings)
                .RemoveAll<ITimeProvider>()
                .AddSingleton(_timeProviderMock.Object)
                .BuildServiceProvider();

            return sp.GetService<IHomeChargerOrchestrator>()!;
        }

        [Fact]
        public async Task UpdateChargingScheduleAsync_WhenBeforeChargeWindow_NoSchedulerTimesAreChanged()
        {
            // Arrange
            var homeChargerOrchestrator = CreateHomeChargerOrchestrator();

            _myEnergiServiceMock.Setup(m => m.GetEvChargeStatusAsync())
                .ReturnsAsync(new EvChargeStatus
                {
                    ChargerStatus = ChargerStatus.EvDisconnected,
                    ChargingStatus = ChargingStatus.Waiting,
                    ChargeRateWatts = 0
                });

            _timeProviderMock.Setup(m => m.GetCurrentTime())
                .Returns(new TimeOnly(0, 30));

            // Act
            var result = await homeChargerOrchestrator.UpdateChargingScheduleAsync();

            // Assert
            Assert.False(result);
            _myEnergiServiceMock.Verify(m => m.GetEvChargeStatusAsync(), Times.Once);
            _myEnergiServiceMock.VerifyNoOtherCalls();
            _givEnergyServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task UpdateChargingScheduleAsync_WhenDuringChargeWindow_SchedulerTimesAreUpdated()
        {
            // Arrange
            var homeChargerOrchestrator = CreateHomeChargerOrchestrator();

            _myEnergiServiceMock.Setup(m => m.GetEvChargeStatusAsync())
                .ReturnsAsync(new EvChargeStatus
                {
                    ChargerStatus = ChargerStatus.EvConnected,
                    ChargingStatus = ChargingStatus.Waiting,
                    ChargeRateWatts = 0
                });

            _timeProviderMock.Setup(m => m.GetCurrentTime())
                .Returns(new TimeOnly(2, 0));

            // Act
            var result = await homeChargerOrchestrator.UpdateChargingScheduleAsync();

            // Assert
            Assert.True(result);
            _myEnergiServiceMock.Verify(m => m.GetEvChargeStatusAsync(), Times.Once);
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeStartTimeAsync(It.Is<TimeOnly>(t => t == _settings.HouseChargeWindowStart)), Times.Once);
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeEndTimeAsync(It.Is<TimeOnly>(t => t == _settings.HouseChargeWindowEnd)), Times.Once);
        }

        [Fact]
        public async Task UpdateChargingScheduleAsync_WhenAfterChargeWindow_SchedulerTimesAreNotUpdated()
        {
            // Arrange
            var homeChargerOrchestrator = CreateHomeChargerOrchestrator();

            _myEnergiServiceMock.Setup(m => m.GetEvChargeStatusAsync())
                .ReturnsAsync(new EvChargeStatus
                {
                    ChargerStatus = ChargerStatus.EvDisconnected,
                    ChargingStatus = ChargingStatus.Complete,
                    ChargeRateWatts = 0
                });

            _timeProviderMock.Setup(m => m.GetCurrentTime())
                .Returns(new TimeOnly(23, 30));

            // Act
            var result = await homeChargerOrchestrator.UpdateChargingScheduleAsync();

            // Assert
            Assert.False(result);
            _myEnergiServiceMock.Verify(m => m.GetEvChargeStatusAsync(), Times.Once);
            _givEnergyServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task UpdateChargingScheduleAsync_WhenEvConnectedAndChargingOver4MWh_SchedulerTimesSetToEndBeforeImmersion()
        {
            // Arrange
            var homeChargerOrchestrator = CreateHomeChargerOrchestrator();

            _myEnergiServiceMock.Setup(m => m.GetEvChargeStatusAsync())
                .ReturnsAsync(new EvChargeStatus
                {
                    ChargerStatus = ChargerStatus.Charging,
                    ChargingStatus = ChargingStatus.Diverting_Charging,
                    ChargeRateWatts = 4000
                });

            _timeProviderMock.Setup(m => m.GetCurrentTime())
                .Returns(new TimeOnly(2, 0));

            // Act
            var result = await homeChargerOrchestrator.UpdateChargingScheduleAsync();

            // Assert
            Assert.True(result);
            _myEnergiServiceMock.Verify(m => m.GetEvChargeStatusAsync(), Times.Once);
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeStartTimeAsync(It.Is<TimeOnly>(t => t == _settings.HouseChargeWindowStart)), Times.Once);
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeEndTimeAsync(It.Is<TimeOnly>(t => t == _settings.PreImmersionTime)), Times.Once);
        }

        [Fact]
        public async Task UpdateChargingScheduleAsync_WhenEvConnectedAndChargingOver4MWhDuringImmersion_SchedulerTimesSetToStartAfterImmersion()
        {
            // Arrange
            var homeChargerOrchestrator = CreateHomeChargerOrchestrator();

            _myEnergiServiceMock.Setup(m => m.GetEvChargeStatusAsync())
                .ReturnsAsync(new EvChargeStatus
                {
                    ChargerStatus = ChargerStatus.Charging,
                    ChargingStatus = ChargingStatus.Diverting_Charging,
                    ChargeRateWatts = 4000
                });

            _timeProviderMock.Setup(m => m.GetCurrentTime())
                .Returns(new TimeOnly(6, 50));

            // Act
            var result = await homeChargerOrchestrator.UpdateChargingScheduleAsync();

            // Assert
            Assert.True(result);
            _myEnergiServiceMock.Verify(m => m.GetEvChargeStatusAsync(), Times.Once);
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeStartTimeAsync(It.Is<TimeOnly>(t => t == _settings.PostImmersionTime)), Times.Once);
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeEndTimeAsync(It.Is<TimeOnly>(t => t == _settings.HouseChargeWindowEnd)), Times.Once);
        }

        [Fact]
        public async Task UpdateChargingScheduleAsync_WhenEvDisconnected_SchedulerTimesAreNotUpdated()
        {
            // Arrange
            var homeChargerOrchestrator = CreateHomeChargerOrchestrator();

            _myEnergiServiceMock.Setup(m => m.GetEvChargeStatusAsync())
                .ReturnsAsync(new EvChargeStatus
                {
                    ChargerStatus = ChargerStatus.EvDisconnected,
                    ChargingStatus = ChargingStatus.Waiting,
                    ChargeRateWatts = 0
                });

            _timeProviderMock.Setup(m => m.GetCurrentTime())
                .Returns(new TimeOnly(10, 0));

            // Act
            var result = await homeChargerOrchestrator.UpdateChargingScheduleAsync();

            // Assert
            Assert.False(result);
            _myEnergiServiceMock.Verify(m => m.GetEvChargeStatusAsync(), Times.Once);
            _givEnergyServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task UpdateChargingScheduleAsync_WhenEvConnectedAndWaiting_SchedulerTimesAreUpdated()
        {
            // Arrange
            var homeChargerOrchestrator = CreateHomeChargerOrchestrator();

            _myEnergiServiceMock.Setup(m => m.GetEvChargeStatusAsync())
                .ReturnsAsync(new EvChargeStatus
                {
                    ChargerStatus = ChargerStatus.EvConnected,
                    ChargingStatus = ChargingStatus.Waiting,
                    ChargeRateWatts = 0
                });

            _timeProviderMock.Setup(m => m.GetCurrentTime())
                .Returns(new TimeOnly(5, 0));

            // Act
            var result = await homeChargerOrchestrator.UpdateChargingScheduleAsync();

            // Assert
            Assert.True(result);
            _myEnergiServiceMock.Verify(m => m.GetEvChargeStatusAsync(), Times.Once);
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeStartTimeAsync(It.Is<TimeOnly>(t => t == _settings.HouseChargeWindowStart)), Times.Once);
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeEndTimeAsync(It.Is<TimeOnly>(t => t == _settings.HouseChargeWindowEnd)), Times.Once);
        }

        [Fact]
        public async Task UpdateChargingScheduleAsync_WhenEvConnectedAndPaused_SchedulerTimesAreNotUpdated()
        {
            // Arrange
            var homeChargerOrchestrator = CreateHomeChargerOrchestrator();

            _myEnergiServiceMock.Setup(m => m.GetEvChargeStatusAsync())
                .ReturnsAsync(new EvChargeStatus
                {
                    ChargerStatus = ChargerStatus.EvConnected,
                    ChargingStatus = ChargingStatus.Paused,
                    ChargeRateWatts = 0
                });

            _timeProviderMock.Setup(m => m.GetCurrentTime())
                .Returns(new TimeOnly(5, 0));

            // Act
            var result = await homeChargerOrchestrator.UpdateChargingScheduleAsync();

            // Assert
            Assert.True(result);
            _myEnergiServiceMock.Verify(m => m.GetEvChargeStatusAsync(), Times.Once);
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeStartTimeAsync(It.Is<TimeOnly>(t => t == _settings.HouseChargeWindowStart)), Times.Once);
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeEndTimeAsync(It.Is<TimeOnly>(t => t == _settings.HouseChargeWindowEnd)), Times.Once);
        }

        [Fact]
        public async Task UpdateChargingScheduleAsync_WhenEvConnectedAndComplete_SchedulerTimesIsSetToMaximum()
        {
            // Arrange
            var homeChargerOrchestrator = CreateHomeChargerOrchestrator();

            _myEnergiServiceMock.Setup(m => m.GetEvChargeStatusAsync())
                .ReturnsAsync(new EvChargeStatus
                {
                    ChargerStatus = ChargerStatus.EvConnected,
                    ChargingStatus = ChargingStatus.Complete,
                    ChargeRateWatts = 0
                });

            _timeProviderMock.Setup(m => m.GetCurrentTime())
                .Returns(new TimeOnly(5, 0));

            // Act
            var result = await homeChargerOrchestrator.UpdateChargingScheduleAsync();

            // Assert
            Assert.True(result);
            _myEnergiServiceMock.Verify(m => m.GetEvChargeStatusAsync(), Times.Once);
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeStartTimeAsync(It.Is<TimeOnly>(t => t == _settings.HouseChargeWindowStart)), Times.Once);
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeEndTimeAsync(It.Is<TimeOnly>(t => t == _settings.HouseChargeWindowEnd)), Times.Once);
        }

        [Fact]
        public async Task UpdateChargingScheduleAsync_WhenDuringChargeWindowAndEvConnected_SchedulerTimesAreUpdated()
        {
            // Arrange
            var homeChargerOrchestrator = CreateHomeChargerOrchestrator();

            _myEnergiServiceMock.Setup(m => m.GetEvChargeStatusAsync())
                .ReturnsAsync(new EvChargeStatus
                {
                    ChargerStatus = ChargerStatus.EvConnected,
                    ChargingStatus = ChargingStatus.Waiting,
                    ChargeRateWatts = 0
                });

            _timeProviderMock.Setup(m => m.GetCurrentTime())
                .Returns(new TimeOnly(3, 0));

            // Act
            var result = await homeChargerOrchestrator.UpdateChargingScheduleAsync();

            // Assert
            Assert.True(result);
            _myEnergiServiceMock.Verify(m => m.GetEvChargeStatusAsync(), Times.Once);
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeStartTimeAsync(It.Is<TimeOnly>(t => t == _settings.HouseChargeWindowStart)), Times.Once);
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeEndTimeAsync(It.Is<TimeOnly>(t => t == _settings.HouseChargeWindowEnd)), Times.Once);
        }

        [Fact]
        public async Task UpdateChargingScheduleAsync_WhenDuringChargeWindowAndEvDisconnected_SchedulerTimesAreUpdated()
        {
            // Arrange
            var homeChargerOrchestrator = CreateHomeChargerOrchestrator();

            _myEnergiServiceMock.Setup(m => m.GetEvChargeStatusAsync())
                .ReturnsAsync(new EvChargeStatus
                {
                    ChargerStatus = ChargerStatus.EvDisconnected,
                    ChargingStatus = ChargingStatus.Waiting,
                    ChargeRateWatts = 0
                });

            _timeProviderMock.Setup(m => m.GetCurrentTime())
                .Returns(new TimeOnly(4, 0));

            // Act
            var result = await homeChargerOrchestrator.UpdateChargingScheduleAsync();

            // Assert
            Assert.True(result);
            _myEnergiServiceMock.Verify(m => m.GetEvChargeStatusAsync(), Times.Once);
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeStartTimeAsync(It.Is<TimeOnly>(t => t == _settings.HouseChargeWindowStart)), Times.Once);
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeEndTimeAsync(It.Is<TimeOnly>(t => t == _settings.HouseChargeWindowEnd)), Times.Once);
        }

        [Fact]
        public async Task UpdateChargingScheduleAsync_WhenDuringChargeWindowAndEvCharging_SchedulerTimesAreUpdated()
        {
            // Arrange
            var homeChargerOrchestrator = CreateHomeChargerOrchestrator();

            _myEnergiServiceMock.Setup(m => m.GetEvChargeStatusAsync())
                .ReturnsAsync(new EvChargeStatus
                {
                    ChargerStatus = ChargerStatus.Charging,
                    ChargingStatus = ChargingStatus.Diverting_Charging,
                    ChargeRateWatts = 1000
                });

            _timeProviderMock.Setup(m => m.GetCurrentTime())
                .Returns(new TimeOnly(6, 0));

            // Act
            var result = await homeChargerOrchestrator.UpdateChargingScheduleAsync();

            // Assert
            Assert.True(result);
            _myEnergiServiceMock.Verify(m => m.GetEvChargeStatusAsync(), Times.Once);
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeStartTimeAsync(It.Is<TimeOnly>(t => t == _settings.HouseChargeWindowStart)), Times.Once);
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeEndTimeAsync(It.Is<TimeOnly>(t => t == _settings.HouseChargeWindowEnd)), Times.Once);
        }

        [Fact]
        public async Task UpdateChargingScheduleAsync_WhenDuringChargeWindowAndEvPaused_SchedulerTimesAreUpdated()
        {
            // Arrange
            var homeChargerOrchestrator = CreateHomeChargerOrchestrator();

            _myEnergiServiceMock.Setup(m => m.GetEvChargeStatusAsync())
                .ReturnsAsync(new EvChargeStatus
                {
                    ChargerStatus = ChargerStatus.EvConnected,
                    ChargingStatus = ChargingStatus.Paused,
                    ChargeRateWatts = 0
                });

            _timeProviderMock.Setup(m => m.GetCurrentTime())
                .Returns(new TimeOnly(7, 0));

            // Act
            var result = await homeChargerOrchestrator.UpdateChargingScheduleAsync();

            // Assert
            Assert.True(result);
            _myEnergiServiceMock.Verify(m => m.GetEvChargeStatusAsync(), Times.Once);
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeStartTimeAsync(It.Is<TimeOnly>(t => t == _settings.HouseChargeWindowStart)), Times.Once);
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeEndTimeAsync(It.Is<TimeOnly>(t => t == _settings.HouseChargeWindowEnd)), Times.Once);
        }

        [Fact]
        public async Task UpdateChargingScheduleAsync_WhenDuringChargeWindowAndEvComplete_SchedulerTimesAreUpdated()
        {
            // Arrange
            var homeChargerOrchestrator = CreateHomeChargerOrchestrator();

            _myEnergiServiceMock.Setup(m => m.GetEvChargeStatusAsync())
                .ReturnsAsync(new EvChargeStatus
                {
                    ChargerStatus = ChargerStatus.EvConnected,
                    ChargingStatus = ChargingStatus.Complete,
                    ChargeRateWatts = 0
                });

            _timeProviderMock.Setup(m => m.GetCurrentTime())
                .Returns(new TimeOnly(7, 30));

            // Act
            var result = await homeChargerOrchestrator.UpdateChargingScheduleAsync();

            // Assert
            Assert.True(result);
            _myEnergiServiceMock.Verify(m => m.GetEvChargeStatusAsync(), Times.Once);
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeStartTimeAsync(It.Is<TimeOnly>(t => t == _settings.HouseChargeWindowStart)), Times.Once);
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeEndTimeAsync(It.Is<TimeOnly>(t => t == _settings.HouseChargeWindowEnd)), Times.Once);
        }

        [Fact]
        public async Task UpdateChargingScheduleAsync_WhenDuringChargeWindowAndEvUnknown_SchedulerTimesAreUpdated()
        {
            // Arrange
            var homeChargerOrchestrator = CreateHomeChargerOrchestrator();

            _myEnergiServiceMock.Setup(m => m.GetEvChargeStatusAsync())
                .ReturnsAsync(new EvChargeStatus
                {
                    ChargerStatus = ChargerStatus.EvConnected,
                    ChargingStatus = ChargingStatus.Unknown,
                    ChargeRateWatts = 0
                });

            _timeProviderMock.Setup(m => m.GetCurrentTime())
                .Returns(new TimeOnly(5, 0));

            // Act
            var result = await homeChargerOrchestrator.UpdateChargingScheduleAsync();

            // Assert
            Assert.True(result);
            _myEnergiServiceMock.Verify(m => m.GetEvChargeStatusAsync(), Times.Once);
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeStartTimeAsync(It.Is<TimeOnly>(t => t == _settings.HouseChargeWindowStart)), Times.Once);
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeEndTimeAsync(It.Is<TimeOnly>(t => t == _settings.HouseChargeWindowEnd)), Times.Once);
        }
    }
}
