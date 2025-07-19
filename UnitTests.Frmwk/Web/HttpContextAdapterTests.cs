using Moq;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using WebUtils.Frmwk.Web;
using WebUtils.Web;

namespace UnitTests.Frmwk.Web
{
    [TestClass]
    public sealed class HttpContextAdapterTests
    {
        [TestMethod]
        public void Request_UserAgent_Returns()
        {
            //Arrange
            var expectedUserAgent = "TestUserAgent";
            var headers = new NameValueCollection
            {
                { "User-Agent", expectedUserAgent }
            };
            var mockRequest = new Mock<HttpRequestBase>();
            var mockContext = new Mock<HttpContextBase>();

            mockRequest.Setup(r => r.Headers).Returns(headers);
            mockContext.Setup(c => c.Request).Returns(mockRequest.Object);

            var adapter = new HttpContextAdapter(mockContext.Object);
            
            //Act
            var actual = adapter.Request.UserAgent;
            var actualHeaders = adapter.Request.Headers;

            //Assert
            Assert.AreEqual(expectedUserAgent, actual);
            Assert.IsNotNull(actualHeaders);
            Assert.IsTrue(actualHeaders.Contains("User-Agent"));
            Assert.IsTrue(actualHeaders.GetKeys().Contains("User-Agent"));
            Assert.AreEqual(expectedUserAgent, actualHeaders.Get("User-Agent"));
            Assert.AreEqual(1, actualHeaders.Count);
        }

        [TestMethod]
        public void Request_HostAndPath_Returns()
        {
            //Arrange
            var expectedHost = "www.example.com";
            var expectedPath = "/test/path";

            var mockRequest = new Mock<HttpRequestBase>();
            var mockContext = new Mock<HttpContextBase>();

            mockRequest.Setup(r => r.Url).Returns(new Uri($"http://{expectedHost}{expectedPath}"));
            mockContext.Setup(c => c.Request).Returns(mockRequest.Object);

            var adapter = new HttpContextAdapter(mockContext.Object);
            
            //Act
            var actualHost = adapter.Request.Host;
            var actualPath = adapter.Request.Path;
            
            //Assert
            Assert.AreEqual(expectedHost, actualHost);
            Assert.AreEqual(expectedPath, actualPath);
        }

        [TestMethod]
        public void Request_Method_Returns()
        {
            //Arrange
            var expectedMethod = "GET";

            var mockRequest = new Mock<HttpRequestBase>();
            var mockContext = new Mock<HttpContextBase>();

            mockRequest.Setup(r => r.HttpMethod).Returns(expectedMethod);
            mockContext.Setup(c => c.Request).Returns(mockRequest.Object);

            var adapter = new HttpContextAdapter(mockContext.Object);

            //Act
            var actualMethod = adapter.Request.Method;

            //Assert
            Assert.AreEqual(expectedMethod, actualMethod);
        }

        [TestMethod]
        public void Request_Referer_Returns()
        {
            //Arrange
            var expectedReferer = "http://www.example.com/referer";

            var mockRequest = new Mock<HttpRequestBase>();
            var mockContext = new Mock<HttpContextBase>();

            mockRequest.Setup(r => r.UrlReferrer).Returns(new Uri(expectedReferer));
            mockContext.Setup(c => c.Request).Returns(mockRequest.Object);

            var adapter = new HttpContextAdapter(mockContext.Object);

            //Act
            var actualReferer = adapter.Request.Referer;

            //Assert
            Assert.AreEqual(expectedReferer, actualReferer);
        }

        [TestMethod]
        public void Request_Cookies_Returns()
        {
            //Arrange
            var expectedCookieName = "TestCookie";
            var expectedCookieValue = "TestValue";
            var expectedCookieDomain = "example.com";
            var expectedCookiePath = "/";
            var expectedCookieExpires = DateTime.UtcNow.AddDays(1);
            var expectedCookieSecure = true;
            var expectedCookieHttpOnly = true;
            var expectedCookieSameSite = SameSiteEnum.Lax;

            var mockCookie = new HttpCookie(expectedCookieName)
            {
                Value = expectedCookieValue,
                Domain = expectedCookieDomain,
                Path = expectedCookiePath,
                Expires = expectedCookieExpires,
                Secure = expectedCookieSecure,
                HttpOnly = expectedCookieHttpOnly,
                SameSite = (SameSiteMode)expectedCookieSameSite
            };

            var cookies = new HttpCookieCollection { mockCookie };

            var mockRequest = new Mock<HttpRequestBase>();
            var mockContext = new Mock<HttpContextBase>();

            mockRequest.Setup(r => r.Cookies).Returns(cookies);
            mockContext.Setup(c => c.Request).Returns(mockRequest.Object);

            var adapter = new HttpContextAdapter(mockContext.Object);

            //Act
            var actualCookies = adapter.Request.Cookies;
            var actualCookie = actualCookies.Get(expectedCookieName);

            //Assert
            Assert.IsNotNull(actualCookie);
            Assert.AreEqual(1, actualCookies.Count);
            Assert.AreEqual(1, actualCookies.GetKeys().Count());
            Assert.IsTrue(actualCookies.Contains(expectedCookieName));
            Assert.AreEqual(expectedCookieValue, actualCookie.Value);
            Assert.AreEqual(expectedCookieDomain, actualCookie.Domain);
            Assert.AreEqual(expectedCookiePath, actualCookie.Path);
            Assert.AreEqual(expectedCookieExpires, actualCookie.Expires);
            Assert.AreEqual(expectedCookieSecure, actualCookie.Secure);
            Assert.AreEqual(expectedCookieHttpOnly, actualCookie.HttpOnly);
            Assert.AreEqual(expectedCookieSameSite, (SameSiteEnum)actualCookie.SameSite);

        }

        [TestMethod]
        public void Response_StatusCode_Returns()
        {
            //Arrange
            var expectedStatusCode = 200;
            var mockResponse = new Mock<HttpResponseBase>();
            var mockContext = new Mock<HttpContextBase>();
            mockResponse.Setup(r => r.StatusCode).Returns(expectedStatusCode);
            mockContext.Setup(c => c.Response).Returns(mockResponse.Object);
            var adapter = new HttpContextAdapter(mockContext.Object);

            //Act
            var actualStatusCode = adapter.Response.StatusCode;

            //Assert
            Assert.AreEqual(expectedStatusCode, actualStatusCode);
        }

        [TestMethod]
        public void Response_Headers_Returns()
        {
            //Arrange
            var expectedHeaderName = "TestHeader";
            var expectedHeaderValue = "HeaderValue";
            var headers = new NameValueCollection
            {
                { expectedHeaderName, expectedHeaderValue }
            };
            var mockResponse = new Mock<HttpResponseBase>();
            var mockContext = new Mock<HttpContextBase>();
            mockResponse.Setup(r => r.Headers).Returns(headers);
            mockContext.Setup(c => c.Response).Returns(mockResponse.Object);
            var adapter = new HttpContextAdapter(mockContext.Object);

            //Act
            var actualHeaders = adapter.Response.Headers;
            var actualHeaderValue = actualHeaders.Get(expectedHeaderName);

            //Assert
            Assert.AreEqual(expectedHeaderValue, actualHeaderValue);
        }

        [TestMethod]
        public void Response_Cookies_Returns()
        {
            //Arrange
            var expectedCookieName = "TestCookie";
            var expectedCookieValue = "TestValue";
            var expectedCookieDomain = "example.com";
            var expectedCookiePath = "/";
            var expectedCookieExpires = DateTime.UtcNow.AddDays(1);
            var expectedCookieSecure = true;
            var expectedCookieHttpOnly = true;
            var mockCookie = new HttpCookie(expectedCookieName)
            {
                Value = expectedCookieValue,
                Domain = expectedCookieDomain,
                Path = expectedCookiePath,
                Expires = expectedCookieExpires,
                Secure = expectedCookieSecure,
                HttpOnly = expectedCookieHttpOnly
            };
            var cookies = new HttpCookieCollection { mockCookie };
            var mockResponse = new Mock<HttpResponseBase>();
            var mockContext = new Mock<HttpContextBase>();
            mockResponse.Setup(r => r.Cookies).Returns(cookies);
            mockContext.Setup(c => c.Response).Returns(mockResponse.Object);
            var adapter = new HttpContextAdapter(mockContext.Object);

            //Act
            var actualCookies = adapter.Response.Cookies;
            var actualCookie = actualCookies.Get(expectedCookieName);

            //Assert
            Assert.IsNotNull(actualCookie);
            Assert.AreEqual(1, actualCookies.Count);
            Assert.IsTrue(actualCookies.Contains(expectedCookieName));
            Assert.AreEqual(1, actualCookies.GetKeys().Count());
            Assert.AreEqual(expectedCookieValue, actualCookie.Value);
            Assert.AreEqual(expectedCookieDomain, actualCookie.Domain);
            Assert.AreEqual(expectedCookiePath, actualCookie.Path);
            Assert.AreEqual(expectedCookieExpires, actualCookie.Expires);
            Assert.AreEqual(expectedCookieSecure, actualCookie.Secure);
            Assert.AreEqual(expectedCookieHttpOnly, actualCookie.HttpOnly);
        }
    }
}