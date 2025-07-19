using System;
using System.Linq;
using System.Web;
using WebUtils.Frmwk.Web;


namespace UnitTests.Frmwk.Web
{
    [TestClass]
    public sealed class ResponseCookiesAccessorTest
    {
        [TestMethod]
        public void ResponseCookiesAccessor_SetCookie_SetsCookie()
        {
            // Arrange
            var expectedCookieName = "TestCookie";
            var expectedCookieValue = "TestValue";

            var httpCookie = new HttpCookie(expectedCookieName, expectedCookieValue);

            var cookie = new CookieWrapper(httpCookie)
            {
                Domain = "example.com",
                Path = "/",
                Expires = DateTime.UtcNow.AddDays(1),
                Secure = true,
                HttpOnly = true
            };

            var cookies = new HttpCookieCollection();
            var accessor = new ResponseCookiesAccessor(cookies);

            // Act
            accessor.Set(expectedCookieName, cookie);

            // Assert
            var setCookie = cookies[expectedCookieName];
            Assert.IsNotNull(setCookie);
            Assert.AreEqual(cookie.Value, setCookie.Value);
            Assert.AreEqual(cookie.Domain, setCookie.Domain);
            Assert.AreEqual(cookie.Path, setCookie.Path);
            Assert.AreEqual(cookie.Expires.UtcDateTime, setCookie.Expires);
            Assert.AreEqual(cookie.Secure, setCookie.Secure);
            Assert.AreEqual(cookie.HttpOnly, setCookie.HttpOnly);
        }
    }
}
