using System.Security.Cryptography;
using System.Text;

namespace UnitTests
{
    [TestClass]
    public sealed class ApiKeyGenerator
    {
        [TestMethod]
        public void GenerateApiKey()
        {
            // Arrange
            var guidObj = new {
                GuidOne = Guid.NewGuid(),
                GuidTwo = Guid.NewGuid()
            };

            var apiKey = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(guidObj.GuidOne.ToString() + guidObj.GuidTwo.ToString())));

            // Assert
            Assert.IsTrue(apiKey.Length >= 32, "API Key should be 32 characters long.");
            Console.WriteLine($"Generated API Key: {apiKey}");
        }
    }
}
