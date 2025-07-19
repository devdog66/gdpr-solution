using Microsoft.Extensions.Logging;
using Moq;
using WebUtils.Core.Logging;
using WebUtils.Logging;

namespace UnitTests.WebUtils.Core.Logging
{
    [TestClass]
    public sealed class LogManagerTests
    {
        [TestMethod]
        public void GetLogger_ReturnsILogContract()
        {
            // Arrange
            var serviceProvider = new Mock<IServiceProvider>();
            var logger = new Mock<ILogger<LogManagerTests>>();
            serviceProvider.Setup(sp => sp.GetService(typeof(ILogger<LogManagerTests>)))
                           .Returns(logger.Object);
            
            var logManager = new LogManager(serviceProvider.Object);
            var logMessage = "Test log message";
            var logException = new Exception("Test exception");

            // Act
            var logContract = logManager.GetLogger<LogManagerTests>();
            logContract.Debug(logMessage);
            logContract.Info(logMessage);
            logContract.Warn(logMessage);
            logContract.Warn(logMessage, logException);
            logContract.Error(logMessage);
            logContract.Error(logMessage, logException);
            logContract.Fatal(logMessage);
            logContract.Fatal(logMessage, logException);    

            // Assert
            Assert.IsNotNull(logContract);
            Assert.IsInstanceOfType(logContract, typeof(ILogContract<LogManagerTests>));
        }
    }
}
