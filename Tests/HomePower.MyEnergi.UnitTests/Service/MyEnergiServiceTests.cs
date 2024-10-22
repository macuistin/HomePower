using HomePower.MyEnergi.Client;
using HomePower.MyEnergi.Dto;
using HomePower.MyEnergi.Model;
using Moq;

namespace HomePower.MyEnergi.UnitTests.Service
{
    public class MyEnergiServiceTests
    {
        private readonly Mock<IMyEnergiClient> _myEnergiClientMock;

        public MyEnergiServiceTests()
        {
            _myEnergiClientMock = new Mock<IMyEnergiClient>();
        }

        private MyEnergiService CreateService()
        {
            return new MyEnergiService(_myEnergiClientMock.Object);
        }

        [Fact]
        public async Task GetEvChargeStatusAsync_ShouldReturnEvChargeStatus_WhenZappiStatusIsSuccessful()
        {
            // Arrange
            var zappiStatus = ZappiStatusResult.CreateSuccess(new ZappiDto
            {
                ChargingStatus = 3, // Diverting/Charging
                ChargerStatus = "B1", // EV Connected
                ChargeRateWatts = 1000,
            });
            _myEnergiClientMock
                .Setup(m => m.GetZappiStatusAsync())
                .ReturnsAsync(zappiStatus);
            var service = CreateService();

            // Act
            var result = await service.GetEvChargeStatusAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1000, result.ChargeRateWatts);
            Assert.Equal(ChargerStatus.EvConnected, result.ChargerStatus);
            Assert.Equal(ChargingStatus.Diverting_Charging, result.ChargingStatus);
        }

        [Fact]
        public async Task GetEvChargeStatusAsync_ShouldReturnFailedStatus_WhenZappiStatusIsNotSuccessful()
        {
            // Arrange
            _myEnergiClientMock
                .Setup(m => m.GetZappiStatusAsync())
                .ReturnsAsync(ZappiStatusResult.Failed);
            var service = CreateService();

            // Act
            var result = await service.GetEvChargeStatusAsync();

            // Assert
            Assert.Equal(EvChargeStatus.Failed, result);
        }

        [Fact]
        public async Task GetEvChargeStatusAsync_ShouldReturnCorrectStatus_WhenChargingStatusIsUnknown()
        {
            // Arrange
            var zappiStatus = ZappiStatusResult.CreateSuccess(new ZappiDto
            {
                ChargingStatus = 1, // Paused
                ChargerStatus = "ZZZ", // Does not exist
                ChargeRateWatts = 0,
            });
            _myEnergiClientMock
                .Setup(m => m.GetZappiStatusAsync())
                .ReturnsAsync(zappiStatus);
            var service = CreateService();

            // Act
            var result = await service.GetEvChargeStatusAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.ChargeRateWatts);
            Assert.Equal(ChargerStatus.Unknown, result.ChargerStatus);
            Assert.Equal(ChargingStatus.Paused, result.ChargingStatus);
        }

        [Fact]
        public async Task GetEvChargeStatusAsync_ShouldReturnCorrectStatus_WhenChargerStatusIsDifferent()
        {
            // Arrange
            var zappiStatus = ZappiStatusResult.CreateSuccess(new ZappiDto
            {
                ChargingStatus = 3, // Diverting/Charging
                ChargerStatus = "C1", // EV ready
                ChargeRateWatts = 2000,
            });
            _myEnergiClientMock
                .Setup(m => m.GetZappiStatusAsync())
                .ReturnsAsync(zappiStatus);
            var service = CreateService();

            // Act
            var result = await service.GetEvChargeStatusAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2000, result.ChargeRateWatts);
            Assert.Equal(ChargerStatus.EvReady, result.ChargerStatus);
            Assert.Equal(ChargingStatus.Diverting_Charging, result.ChargingStatus);
        }
    }
}
