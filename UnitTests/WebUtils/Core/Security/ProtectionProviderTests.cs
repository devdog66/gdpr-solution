using Microsoft.AspNetCore.DataProtection;
using Moq;
using System.Text;
using WebUtils.Core.Security;

namespace UnitTests.WebUtils.Core.Security
{
    [TestClass]
    public sealed class ProtectionProviderTests
    {
        [TestMethod]
        public void ProtectionProvider_ProtectsAndUnprotectsData()
        {
            // Arrange
            var mockProvider = new Mock<IDataProtectionProvider>();
            var mockDataProtector = new Mock<IDataProtector>();
            var provider = new ProtectionProvider(mockProvider.Object);
            var expectedPlaintext = "Sensitive Data";
            var expectedProtectedData = "Protected Data";

            mockDataProtector
                .Setup(p => p.Protect(It.IsAny<byte[]>()))
                .Returns(Encoding.UTF8.GetBytes(expectedProtectedData));
            mockDataProtector.Setup(p => p.Unprotect(It.IsAny<byte[]>()))
                .Returns(Encoding.UTF8.GetBytes(expectedPlaintext));

            mockProvider
                .Setup(p => p.CreateProtector(It.IsAny<string>()))
                .Returns(mockDataProtector.Object);

            // Act
            var protector = provider.CreateProtection("TestPurpose");
            var protectedData = protector.Protect(expectedPlaintext);
            var unprotectedData = protector.Unprotect(protectedData);
            
            // Assert
            Assert.AreEqual(expectedPlaintext, unprotectedData);
        }
    }
}
