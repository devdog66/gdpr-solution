using Microsoft.Extensions.Configuration;
using WebUtils.Core.Config;

namespace UnitTests.WebUtils.Core.Config
{
    [TestClass]
    public sealed class ConfigAdapterTests
    {
        [TestMethod]
        public void ConfigAdapter_GetThrowsInvalidOperation_If_KeyDoesNotExist()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection([])
                .Build();
            var configAdapter = new ConfigAdapter(configuration);

            // Act & Assert
            Assert.ThrowsException<InvalidOperationException>(() => configAdapter.GetValue<string>("NonExistentKey"));
        }

        [TestMethod]
        public void ConfigAdapter_GetThrowsInvalidOperation_If_KeyExistsButValueIsNull()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?> { { "NullValueKey", null } })
                .Build();
            var configAdapter = new ConfigAdapter(configuration);
            // Act & Assert
            Assert.ThrowsException<InvalidOperationException>(() => configAdapter.GetValue<string>("NullValueKey"));
        }

        [TestMethod]
        public void ConfigAdapter_GetValue_ReturnsValue_If_KeyExistsAndValueIsNotNull()
        {
            // Arrange
            var expectedValue = "TestValue";
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> { { "ValidKey", expectedValue } })
                .Build();
            var configAdapter = new ConfigAdapter(configuration);

            // Act
            var actualValue = configAdapter.GetValue<string>("ValidKey");

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}
