using WebUtils.Frmwk.Security;

namespace UnitTests.Frmwk.Security
{
    [TestClass]
    public sealed class ProtectionProviderTests
    {
        [TestMethod]
        public void ProtectionProvider_ProtectAndUnProtect_ReturnsOriginalString()
        {
            // Arrange
            var provider = new ProtectionProvider();
            var protector = provider.CreateProtection("TestPurpose");
            var originalString = "Hello, World!";
            
            // Act
            var encryptedString = protector.Protect(originalString);
            var decryptedString = protector.Unprotect(encryptedString);
            
            // Assert
            Assert.AreEqual(originalString, decryptedString);
        }
    }
}
