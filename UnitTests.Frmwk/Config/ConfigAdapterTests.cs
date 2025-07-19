using System.Collections.Generic;

namespace UnitTests.Frmwk.Config
{
    [TestClass]
    public sealed class ConfigAdapterTests
    {
        [TestMethod]
        public void GetValue_ShouldReturnCorrectValue_WhenKeyExists()
        {
            // Arrange
            var configAdapter = new WebUtils.Frmwk.Config.ConfigAdapter();
            string expectedValue = "testvalue";
            System.Configuration.ConfigurationManager.AppSettings["testkey"] = expectedValue;
            // Act
            var result = configAdapter.GetValue<string>("testkey");
            // Assert
            Assert.AreEqual(expectedValue, result);
        }
        
        [TestMethod]
        public void GetValue_ShouldThrowKeyNotFoundException_WhenKeyDoesNotExist()
        {
            // Arrange
            var configAdapter = new WebUtils.Frmwk.Config.ConfigAdapter();
            // Act and Assert
            Assert.ThrowsException<KeyNotFoundException>(() =>
            {
                var result = configAdapter.GetValue<string>("NonExistentKey");
            });
        }
    }
}
