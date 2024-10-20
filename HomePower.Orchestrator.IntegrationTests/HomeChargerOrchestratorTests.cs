using HomePower.GivEnergy.Service;
using HomePower.MyEnergi.Model;
using HomePower.MyEnergi.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace HomePower.Orchestrator.IntegrationTests
{
    public class HomeChargerOrchestratorTests
    {
        private Mock<IGivEnergyService> _givEnergyServiceMock;
        private Mock<IMyEnergiService> _myEnergiServiceMock;
        private Mock<ITimeProvider> _timeProviderMock;

        public HomeChargerOrchestratorTests()
        {
            _givEnergyServiceMock = new Mock<IGivEnergyService>();
            _myEnergiServiceMock = new Mock<IMyEnergiService>();
            _timeProviderMock = new Mock<ITimeProvider>();
        }

        private IHomeChargerOrchestrator CreateHomeChargerOrchestrator()
        {
            var services = new ServiceCollection();
            var sp = services
                .AddSingleton(_givEnergyServiceMock.Object)
                .AddSingleton(_myEnergiServiceMock.Object)
                .AddOrchestratorServices()
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
            Assert.True(result);
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
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeStartTimeAsync(It.Is<int>(h => h == 1), It.Is<int>(m => m == 0)), Times.Once);
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeEndTimeAsync(It.Is<int>(h => h == 8), It.Is<int>(m => m == 0)), Times.Once);
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
            Assert.True(result);
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
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeStartTimeAsync(It.Is<int>(h => h == 1), It.Is<int>(m => m == 0)), Times.Once);
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeEndTimeAsync(It.Is<int>(h => h == 6), It.Is<int>(m => m == 22)), Times.Once);
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
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeStartTimeAsync(It.Is<int>(h => h == 7), It.Is<int>(m => m == 11)), Times.Once);
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeEndTimeAsync(It.Is<int>(h => h == 8), It.Is<int>(m => m == 0)), Times.Once);
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
            Assert.True(result);
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
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeStartTimeAsync(It.Is<int>(h => h == 1), It.Is<int>(m => m == 0)), Times.Once);
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeEndTimeAsync(It.Is<int>(h => h == 8), It.Is<int>(m => m == 0)), Times.Once);
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
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeStartTimeAsync(It.Is<int>(h => h == 1), It.Is<int>(m => m == 0)), Times.Once);
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeEndTimeAsync(It.Is<int>(h => h == 8), It.Is<int>(m => m == 0)), Times.Once);
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
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeStartTimeAsync(It.Is<int>(h => h == 1), It.Is<int>(m => m == 0)), Times.Once);
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeEndTimeAsync(It.Is<int>(h => h == 8), It.Is<int>(m => m == 0)), Times.Once);
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
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeStartTimeAsync(It.Is<int>(h => h == 1), It.Is<int>(m => m == 0)), Times.Once);
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeEndTimeAsync(It.Is<int>(h => h == 8), It.Is<int>(m => m == 0)), Times.Once);
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
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeStartTimeAsync(It.Is<int>(h => h == 1), It.Is<int>(m => m == 0)), Times.Once);
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeEndTimeAsync(It.Is<int>(h => h == 8), It.Is<int>(m => m == 0)), Times.Once);
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
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeStartTimeAsync(It.Is<int>(h => h == 1), It.Is<int>(m => m == 0)), Times.Once);
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeEndTimeAsync(It.Is<int>(h => h == 8), It.Is<int>(m => m == 0)), Times.Once);
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
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeStartTimeAsync(It.Is<int>(h => h == 1), It.Is<int>(m => m == 0)), Times.Once);
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeEndTimeAsync(It.Is<int>(h => h == 8), It.Is<int>(m => m == 0)), Times.Once);
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
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeStartTimeAsync(It.Is<int>(h => h == 1), It.Is<int>(m => m == 0)), Times.Once);
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeEndTimeAsync(It.Is<int>(h => h == 8), It.Is<int>(m => m == 0)), Times.Once);
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
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeStartTimeAsync(It.Is<int>(h => h == 1), It.Is<int>(m => m == 0)), Times.Once);
            _givEnergyServiceMock.Verify(m => m.UpdateBatteryChargeEndTimeAsync(It.Is<int>(h => h == 8), It.Is<int>(m => m == 0)), Times.Once);
        }
    }
}
