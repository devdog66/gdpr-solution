using System;
using System.Web;
using WebUtils.Frmwk.Web;

namespace UnitTests.Frmwk.Web
{
    [TestClass]
    public sealed class RequestCookiesAccessorTests
    {
        [TestMethod]
        public void Accessor_Set_ThrowsNotImplemented()
        {
            // Arrange
            var cookieCollection = new HttpCookieCollection();
            var accessor = new RequestCookiesAccessor(cookieCollection);

            // Act
            var httpCookie = new HttpCookie("testCookie", "testValue");
            var testCookie = new CookieWrapper(httpCookie)
            {
                Domain = "localhost",
                Expires = DateTime.Now,
                Path = "/",
                Secure = false,
                HttpOnly = true
            };
            Assert.ThrowsException<NotImplementedException>(() => accessor.Set("testkey", testCookie));
        }

        [TestMethod]
        public void Accessor_Get_ReturnsIEnumerable()
        {
            // Arrange
            var cookieCollection = new HttpCookieCollection();
            var httpCookie = new HttpCookie("mycookie", "mycookievalue")
            {
                Domain = "localhost",
                Expires = DateTime.Now,
                Path = "/",
                Secure = false,
                HttpOnly = true
            };
            cookieCollection.Add(httpCookie);
            var accessor = new RequestCookiesAccessor(cookieCollection);

            // Act & Assert
            Assert.ThrowsException<NotImplementedException>(() => accessor.Get(t => t.Domain == "localhost"));
        }
    }
}
