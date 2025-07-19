using Moq;
using Moq.Protected;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using UnitTests.Fakes;
using WebUtils.Config;
using WebUtils.Logging;
using WebUtils.Security;
using WebUtils.Services;
using WebUtils.Standard.ServiceClients;
using WebUtils.Utils;
using WebUtils.Web;

namespace UnitTests.WebUtils.Standard
{
    [TestClass]
    public sealed class ConsentServiceClientTests
    {
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock = new();
        private readonly Mock<ILogManager> _logManagerMock = new();
        private readonly Mock<ILogContract<ConsentServiceClient>> _logContractMock = new();
        private readonly Mock<IConfigContract> _configContractMock = new();
        private readonly Mock<IProtectionProvider> _protectionProviderMock = new();
        private readonly Mock<IProtection> _protectionMock = new();
        private readonly Mock<IVault> _vaultMock = new();
        private readonly Mock<ICacheFacade> _memoryCacheMock = new();
        private ConsentServiceClient? _consentServiceClient;
        private const string cookieName = "TestCookie";
        private const string dataProtectionName = "TestDataProtection";
        private const string apiUrl = "https://example.com/api/consent";
        private const string apiKey = "test-api-key";


        [TestInitialize]
        public void Setup()
        {
            _configContractMock.Setup(c => c.GetValue<string>("Consent:CookieName")).Returns(cookieName);
            _configContractMock.Setup(c => c.GetValue<string>("DataProtectionName")).Returns(dataProtectionName);
            _configContractMock.Setup(t => t.GetValue<string>("Consent:ApiUrl")).Returns(apiUrl);
            _vaultMock.Setup(t => t.GetSecret("Consent:ApiKey")).Returns(apiKey);

            _protectionProviderMock.Setup(p => p.CreateProtection(dataProtectionName))
                .Returns(_protectionMock.Object);
            _logManagerMock.Setup(t => t.GetLogger<ConsentServiceClient>())
                .Returns(_logContractMock.Object);
            _consentServiceClient = new ConsentServiceClient(_httpClientFactoryMock.Object,
                _logManagerMock.Object, _configContractMock.Object, _protectionProviderMock.Object,
                _vaultMock.Object, _memoryCacheMock.Object);
        }

        [TestMethod]
        public void SaveConsent_LogsError_WhenExceptionThrown()
        {
            // Arrange
            var cookiePrefsModel = new CookiePrefsModel
            {
                Analytics = true,
                Marketing = false,
                Necessary = true
            };
            //Setup mocks
            var contextFacade = new Mock<IHttpContextFacade>();
            var mockRequest = new Mock<IHttpRequestFacade>();
            var mockResponse = new Mock<IHttpResponseFacade>();
            mockRequest.Setup(t => t.Cookies).Throws(new Exception("Test exception"));
            contextFacade.Setup(c => c.Request).Returns(mockRequest.Object);
            contextFacade.Setup(c => c.Response).Returns(mockResponse.Object);
            // Act
            _consentServiceClient!.SaveConsent(cookiePrefsModel, contextFacade.Object);
            //Assert
            _logContractMock.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
        }

        [TestMethod]
        public void SaveConsent_LogsWarning_WhenApiCallFails()
        {
            // Arrange
            var cookiePrefsModel = new CookiePrefsModel
            {
                Analytics = true,
                Marketing = false,
                Necessary = true
            };

            //Setup mocks
            SetupApiCall(HttpStatusCode.BadRequest, new StringContent(""));

            var contextFacade = new Mock<IHttpContextFacade>();
            var cookies = new Mock<IAccessor<ICookie>>();
            var mockRequest = new Mock<IHttpRequestFacade>();
            var mockResponse = new Mock<IHttpResponseFacade>();
            
            mockRequest.Setup(t => t.Cookies).Returns(cookies.Object);
            mockResponse.Setup(t => t.Cookies).Returns(cookies.Object);
            contextFacade.Setup(c => c.Request).Returns(mockRequest.Object);
            contextFacade.Setup(c => c.Response).Returns(mockResponse.Object);
            
            // Act
            _consentServiceClient!.SaveConsent(cookiePrefsModel, contextFacade.Object);
            
            //Assert
            _logContractMock.Verify(l => l.Warn(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void SaveConsent_SavesConsent_WithoutCookie()
        {
            // Arrange
            var cookiePrefsModel = new CookiePrefsModel
            {
                Analytics = true,
                Marketing = false,
                Necessary = true
            };

            //Setup mocks
            SetupApiCall(HttpStatusCode.OK, new StringContent(""));

            var contextFacade = new Mock<IHttpContextFacade>();
            var cookies = new Mock<IAccessor<ICookie>>();
            var mockRequest = new Mock<IHttpRequestFacade>();
            var mockResponse = new Mock<IHttpResponseFacade>();

            
            mockRequest.Setup(t => t.Cookies).Returns(cookies.Object);
            mockResponse.Setup(t => t.Cookies).Returns(cookies.Object);
            contextFacade.Setup(c => c.Request).Returns(mockRequest.Object);
            contextFacade.Setup(c => c.Response).Returns(mockResponse.Object);

            // Act
            _consentServiceClient!.SaveConsent(cookiePrefsModel, contextFacade.Object);

            //Assert
            contextFacade.Verify(t => t.Response.Cookies.Set(It.IsAny<string>(), It.IsAny<ICookie>()), Times.Once);
        }

        [TestMethod]
        public void GetConsent_LogsWarning_WhenApiCallFails()
        {
            // Arrange
            var expectedCookie = Guid.NewGuid().ToString();
            var contextFacade = new Mock<IHttpContextFacade>();
            var cookies = new Mock<IAccessor<ICookie>>();
            var mockRequest = new Mock<IHttpRequestFacade>();
            mockRequest.Setup(t => t.Cookies).Returns(cookies.Object);
            contextFacade.Setup(c => c.Request).Returns(mockRequest.Object);

            //Setup mocks for API failure
            SetupCookie(contextFacade, expectedCookie);
            SetupApiCall(HttpStatusCode.BadRequest, new StringContent(""));
            
            // Act
            var actual = _consentServiceClient!.GetConsent(contextFacade.Object);
            
            //Assert
            _logContractMock.Verify(l => l.Warn(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void GetConsent_LogsError_WhenExceptionThrown()
        {
            // Arrange
            var contextFacade = new Mock<IHttpContextFacade>();
            var mockRequest = new Mock<IHttpRequestFacade>();
            mockRequest.Setup(t => t.Cookies).Throws(new Exception("Test exception"));
            contextFacade.Setup(c => c.Request).Returns(mockRequest.Object);
            
            // Act
            var actual = _consentServiceClient!.GetConsent(contextFacade.Object);
            
            //Assert
            _logContractMock.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
        }

        [TestMethod]
        public void GetConsent_ReturnsEmptyConsent_WhenNoCookie()
        {
            // Arrange
            var contextFacade = new Mock<IHttpContextFacade>();
            var mockRequest = new Mock<IHttpRequestFacade>();
            var mockCookies = new Mock<IAccessor<ICookie>>();
            mockRequest.Setup(t => t.Cookies).Returns(mockCookies.Object);
            contextFacade.Setup(c => c.Request).Returns(mockRequest.Object);
            // Act
            var actual = _consentServiceClient!.GetConsent(contextFacade.Object);
            // Assert
            Assert.IsNotNull(actual, "Expected consent response to be not null.");
            Assert.IsFalse(actual.HasCookie, "Expected HasCookie to be false when no cookie is present.");
            Assert.IsNotNull(actual.CookiePrefs, "Expected CookiePrefs to be initialized even when no cookie is present.");
        }

        [TestMethod]
        public void GetConsent_ReturnsCachedConsent()
        {
            // Arrange
            var expectedCookie = Guid.NewGuid().ToString();
            var expectedCookiePrefs = new CookiePrefsModel
            {
                Analytics = true,
                Marketing = false,
                Necessary = true
            };
            var expected = new ConsentResponse
            {
                CookiePrefs = expectedCookiePrefs,
                HasCookie = true
            };
            var contextFacade = new Mock<IHttpContextFacade>();
            
            //Setup mocks  
            SetupCookie(contextFacade, expectedCookie);
            _memoryCacheMock.Setup(m => m.TryGetValue(expectedCookie, out expected))
                .Returns(true);

            // Act
            var actual = _consentServiceClient!.GetConsent(contextFacade.Object);
            //Assert
            Assert.AreEqual(expected.HasCookie, actual.HasCookie, "Expected HasCookie to match.");
            Assert.AreEqual(expected.CookiePrefs.Analytics, actual.CookiePrefs.Analytics, "Expected Analytics preference to match.");
            Assert.AreEqual(expected.CookiePrefs.Marketing, actual.CookiePrefs.Marketing, "Expected Marketing preference to match.");
            Assert.AreEqual(expected.CookiePrefs.Necessary, actual.CookiePrefs.Necessary, "Expected Necessary preference to match.");
        }

        [TestMethod]
        public void GetConsent_ReturnsConsent()
        {
            // Arrange
            var expectedCookie = Guid.NewGuid().ToString();
            var expectedCookiePrefs = new CookiePrefsModel
            {
                Analytics = true,
                Marketing = false,
                Necessary = true
            };
            var expected = new ConsentResponse
            {
                CookiePrefs = expectedCookiePrefs,
                HasCookie = true
            };
            var contextFacade = new Mock<IHttpContextFacade>();

            //Setup mocks  
            SetupCookie(contextFacade, expectedCookie);
            var stringContent = new StringContent(JsonSerializer.Serialize(expectedCookiePrefs));
            SetupApiCall(HttpStatusCode.OK, stringContent);

            // Act
            var actual = _consentServiceClient!.GetConsent(contextFacade.Object);

            //Assert
            Assert.AreEqual(expected.HasCookie, actual.HasCookie, "Expected HasCookie to match.");

        }

        [TestMethod]
        public void GetCookieUid_ReturnsNull()
        {
            //Arrange
            var mockRequest = new Mock<IHttpRequestFacade>();
            var contextFacade = new Mock<IHttpContextFacade>();

            contextFacade.Setup(c => c.Request).Returns(mockRequest.Object);

            //Act
            var actual = _consentServiceClient!.GetCookieUid(contextFacade.Object);

            //Assert
            Assert.IsNull(actual, "Expected cookie UID to be null, but it was not.");
        }

        [TestMethod]
        public void GetCookieUid_ReturnsUnprotectedValue()
        {
            // Arrange
            var contextFacade = new Mock<IHttpContextFacade>();
            var expected = Guid.NewGuid().ToString();

            SetupCookie(contextFacade, expected);

            // Act
            var actual = _consentServiceClient!.GetCookieUid(contextFacade.Object);

            // Assert
            Assert.AreEqual(expected, actual, "Expected cookie UID to match unprotected value.");
        }

        private void SetupCookie(Mock<IHttpContextFacade> contextFacade, string expected)
        {
            var mockCookies = new Mock<IAccessor<ICookie>>();
            var mockRequest = new Mock<IHttpRequestFacade>();
            
            var testCookie = new TestCookie
            {
                Name = cookieName,
                Value = Convert.ToBase64String(SHA512.HashData(Encoding.UTF8.GetBytes(expected))),
                Domain = "localhost",
                Path = "/",
            };

            
            _protectionMock.Setup(p => p.Unprotect(It.IsAny<string>())).Returns(expected);
            mockCookies.Setup(c => c.Get(cookieName)).Returns(testCookie);
            mockRequest.Setup(t => t.Cookies).Returns(mockCookies.Object);
            contextFacade.Setup(c => c.Request).Returns(mockRequest.Object);
        }

        private void SetupApiCall(HttpStatusCode statusCode, StringContent stringContent)
        {
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            var mockHttpClient = new HttpClient(mockMessageHandler.Object);

            mockMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = stringContent
                });

            _httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>()))
                .Returns(mockHttpClient);
        }
    }
}
