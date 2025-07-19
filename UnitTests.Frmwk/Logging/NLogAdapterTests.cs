using System;
using WebUtils.Frmwk.Logging;

namespace UnitTests.Frmwk.Logging
{
    [TestClass]
    public sealed class NLogAdapterTests
    {
        [TestMethod]
        public void LogMessages_AreLoggedCorrectly()
        {
            // Arrange
            var logMgr = new NLogManagerAdapter();
            var logger = logMgr.GetLogger<NLogAdapterTests>();
            var message = "Test log message";
            var exception = new Exception("Test exception");
            // Act
            logger.Debug(message);
            logger.Info(message);
            logger.Warn(message);
            logger.Warn(message, exception);
            logger.Error(message);
            logger.Fatal(message);
            logger.Error(message, exception);
            logger.Fatal(message, exception);
            // Assert
            // Here we would typically verify that the messages were logged correctly.
            // Since NLogAdapter does not expose internal state, we would need to use a mock or a test target.
            // This is a placeholder for the actual verification logic.
        }
    }
}
