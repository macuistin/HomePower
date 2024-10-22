using HomePower.GivEnergy.Client;
using HomePower.GivEnergy.Dto;
using Moq;

namespace HomePower.GivEnergy.UnitTests.Service
{
    public class GivEnergyServiceTests
    {
        private readonly Mock<IGivEnergyClient> _givEnergyClientMock;

        public GivEnergyServiceTests()
        {
            _givEnergyClientMock = new Mock<IGivEnergyClient>();
        }

        private GivEnergyService CreateService()
        {
            return new GivEnergyService(_givEnergyClientMock.Object);
        }

        [Fact]
        public async Task GetACChargeEnabledAsync_ShouldReturnTrue_WhenACChargeIsEnabled()
        {
            // Arrange
            var service = CreateService();
            _givEnergyClientMock
                .Setup(m => m.GetSettingAsync<bool>(It.IsAny<InverterSettingId>()))
                .ReturnsAsync(SettingResponseDto<bool>.CreateSuccess(true));

            // Act
            var result = await service.GetACChargeEnabledAsync();

            // Assert
            Assert.True(result);
            _givEnergyClientMock.Verify(m => m.GetSettingAsync<bool>(It.IsAny<InverterSettingId>()), Times.Once);
            _givEnergyClientMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetBatteryChargeStartTime1Async_ShouldReturnExpectedTime_WhenCalled()
        {
            // Arrange
            var service = CreateService();
            var expectedTime = new TimeOnly(8, 0);
            _givEnergyClientMock
                .Setup(m => m.GetSettingAsync<TimeOnly>(InverterSettingId.ACCharge1StartTime))
                .ReturnsAsync(SettingResponseDto<TimeOnly>.CreateSuccess(expectedTime));

            // Act
            var result = await service.GetBatteryChargeStartTime1Async();

            // Assert
            Assert.Equal(expectedTime, result);
            _givEnergyClientMock.Verify(m => m.GetSettingAsync<TimeOnly>(InverterSettingId.ACCharge1StartTime), Times.Once);
            _givEnergyClientMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetBatteryChargeEndTime1Async_ShouldReturnExpectedTime_WhenCalled()
        {
            // Arrange
            var service = CreateService();
            var expectedTime = new TimeOnly(10, 0);
            _givEnergyClientMock
                .Setup(m => m.GetSettingAsync<TimeOnly>(InverterSettingId.ACCharge1EndTime))
                .ReturnsAsync(SettingResponseDto<TimeOnly>.CreateSuccess(expectedTime));

            // Act
            var result = await service.GetBatteryChargeEndTime1Async();

            // Assert
            Assert.Equal(expectedTime, result);
            _givEnergyClientMock.Verify(m => m.GetSettingAsync<TimeOnly>(InverterSettingId.ACCharge1EndTime), Times.Once);
            _givEnergyClientMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetBatteryChargeStartTime2Async_ShouldReturnExpectedTime_WhenCalled()
        {
            // Arrange
            var service = CreateService();
            var expectedTime = new TimeOnly(12, 0);
            _givEnergyClientMock
                .Setup(m => m.GetSettingAsync<TimeOnly>(InverterSettingId.ACCharge2StartTime))
                .ReturnsAsync(SettingResponseDto<TimeOnly>.CreateSuccess(expectedTime));

            // Act
            var result = await service.GetBatteryChargeStartTime2Async();

            // Assert
            Assert.Equal(expectedTime, result);
            _givEnergyClientMock.Verify(m => m.GetSettingAsync<TimeOnly>(InverterSettingId.ACCharge2StartTime), Times.Once);
            _givEnergyClientMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetBatteryChargeEndTime2Async_ShouldReturnExpectedTime_WhenCalled()
        {
            // Arrange
            var service = CreateService();
            var expectedTime = new TimeOnly(14, 0);
            _givEnergyClientMock
                .Setup(m => m.GetSettingAsync<TimeOnly>(InverterSettingId.ACCharge2EndTime))
                .ReturnsAsync(SettingResponseDto<TimeOnly>.CreateSuccess(expectedTime));

            // Act
            var result = await service.GetBatteryChargeEndTime2Async();

            // Assert
            Assert.Equal(expectedTime, result);
            _givEnergyClientMock.Verify(m => m.GetSettingAsync<TimeOnly>(InverterSettingId.ACCharge2EndTime), Times.Once);
            _givEnergyClientMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetBatteryChargeStartTime3Async_ShouldReturnExpectedTime_WhenCalled()
        {
            // Arrange
            var service = CreateService();
            var expectedTime = new TimeOnly(16, 0);
            _givEnergyClientMock
                .Setup(m => m.GetSettingAsync<TimeOnly>(InverterSettingId.ACCharge3StartTime))
                .ReturnsAsync(SettingResponseDto<TimeOnly>.CreateSuccess(expectedTime));

            // Act
            var result = await service.GetBatteryChargeStartTime3Async();

            // Assert
            Assert.Equal(expectedTime, result);
            _givEnergyClientMock.Verify(m => m.GetSettingAsync<TimeOnly>(InverterSettingId.ACCharge3StartTime), Times.Once);
            _givEnergyClientMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetBatteryChargeEndTime3Async_ShouldReturnExpectedTime_WhenCalled()
        {
            // Arrange
            var service = CreateService();
            var expectedTime = new TimeOnly(18, 0);
            _givEnergyClientMock
                .Setup(m => m.GetSettingAsync<TimeOnly>(InverterSettingId.ACCharge3EndTime))
                .ReturnsAsync(SettingResponseDto<TimeOnly>.CreateSuccess(expectedTime));

            // Act
            var result = await service.GetBatteryChargeEndTime3Async();

            // Assert
            Assert.Equal(expectedTime, result);
            _givEnergyClientMock.Verify(m => m.GetSettingAsync<TimeOnly>(InverterSettingId.ACCharge3EndTime), Times.Once);
            _givEnergyClientMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task UpdateACChargeEnabledAsync_ShouldReturnTrue_WhenUpdateIsSuccessful()
        {
            // Arrange
            var service = CreateService();
            bool enabled = true;
            _givEnergyClientMock
                .Setup(m => m.UpdateSettingAsync(InverterSettingId.ACChargeEnable, enabled))
                .ReturnsAsync(true);

            // Act
            var result = await service.UpdateACChargeEnabledAsync(enabled);

            // Assert
            Assert.True(result);
            _givEnergyClientMock.Verify(m => m.UpdateSettingAsync(InverterSettingId.ACChargeEnable, enabled), Times.Once);
            _givEnergyClientMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task UpdateACCharge1TimesAsync_ShouldReturnTrue_WhenUpdateIsSuccessful()
        {
            // Arrange
            var service = CreateService();
            TimeOnly startTime = new(8, 0);
            TimeOnly endTime = new(10, 0);
            _givEnergyClientMock
                .Setup(m => m.UpdateSettingAsync(InverterSettingId.ACCharge1StartTime, startTime))
                .ReturnsAsync(true);
            _givEnergyClientMock
                .Setup(m => m.UpdateSettingAsync(InverterSettingId.ACCharge1EndTime, endTime))
                .ReturnsAsync(true);

            // Act
            var result = await service.UpdateACCharge1TimesAsync(startTime, endTime);

            // Assert
            Assert.True(result);
            _givEnergyClientMock.Verify(m => m.UpdateSettingAsync(InverterSettingId.ACCharge1StartTime, startTime), Times.Once);
            _givEnergyClientMock.Verify(m => m.UpdateSettingAsync(InverterSettingId.ACCharge1EndTime, endTime), Times.Once);
            _givEnergyClientMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task UpdateACCharge2TimesAsync_ShouldReturnTrue_WhenUpdateIsSuccessful()
        {
            // Arrange
            var service = CreateService();
            TimeOnly startTime = new(12, 0);
            TimeOnly endTime = new(14, 0);
            _givEnergyClientMock
                .Setup(m => m.UpdateSettingAsync(InverterSettingId.ACCharge2StartTime, startTime))
                .ReturnsAsync(true);
            _givEnergyClientMock
                .Setup(m => m.UpdateSettingAsync(InverterSettingId.ACCharge2EndTime, endTime))
                .ReturnsAsync(true);

            // Act
            var result = await service.UpdateACCharge2TimesAsync(startTime, endTime);

            // Assert
            Assert.True(result);
            _givEnergyClientMock.Verify(m => m.UpdateSettingAsync(InverterSettingId.ACCharge2StartTime, startTime), Times.Once);
            _givEnergyClientMock.Verify(m => m.UpdateSettingAsync(InverterSettingId.ACCharge2EndTime, endTime), Times.Once);
            _givEnergyClientMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task UpdateACCharge3TimesAsync_ShouldReturnTrue_WhenUpdateIsSuccessful()
        {
            // Arrange
            var service = CreateService();
            TimeOnly startTime = new(16, 0);
            TimeOnly endTime = new(18, 0);
            _givEnergyClientMock
                .Setup(m => m.UpdateSettingAsync(InverterSettingId.ACCharge3StartTime, startTime))
                .ReturnsAsync(true);
            _givEnergyClientMock
                .Setup(m => m.UpdateSettingAsync(InverterSettingId.ACCharge3EndTime, endTime))
                .ReturnsAsync(true);

            // Act
            var result = await service.UpdateACCharge3TimesAsync(startTime, endTime);

            // Assert
            Assert.True(result);
            _givEnergyClientMock.Verify(m => m.UpdateSettingAsync(InverterSettingId.ACCharge3StartTime, startTime), Times.Once);
            _givEnergyClientMock.Verify(m => m.UpdateSettingAsync(InverterSettingId.ACCharge3EndTime, endTime), Times.Once);
            _givEnergyClientMock.VerifyNoOtherCalls();
        }
    }
}
