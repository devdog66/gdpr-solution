using Microsoft.AspNetCore.Http;
using Moq;
using UnitTests.Fakes;
using WebUtils.Core.Web;
using WebUtils.Web;

namespace UnitTests.WebUtils.Core.Web
{
    [TestClass]
    public sealed class RequestCookieAccessorTests
    {
        [TestMethod]
        public void Get_ThrowsNotImplemented()
        {
            //Arrange
            var httpContext = new DefaultHttpContext();
            var httpRequest = httpContext.Request;
            var cookies = httpRequest.Cookies;
            var accessor = new RequestCookieAccessor(cookies);
            //Act & Assert
            Assert.ThrowsException<NotImplementedException>(() => accessor.Get(t => t.Name == "testCookie"));
        }

        [TestMethod]
        public void Set_ThrowsNotImplemented()
        {
            //Arrange
            var httpContext = new DefaultHttpContext();
            var httpRequest = httpContext.Request;
            var cookies = httpRequest.Cookies;
            var accessor = new RequestCookieAccessor(cookies);
            //Act
            var testCookie = new TestCookie
            {
                Name = "testCookie",
                Value = "testValue",
                Domain = "localhost",
                Expires = DateTime.Now,
                Path = "/",
                Secure = false,
                HttpOnly = true,
                IsEssential = true
            };
            Assert.ThrowsException<NotImplementedException>(() => accessor.Set("testkey", testCookie));
        }

        [TestMethod]
        public void Get_ReturnsICookie()
        {
            //Arrange
            var cookies = new Mock<IRequestCookieCollection>();
            cookies.Setup(t => t.ContainsKey("mycookie")).Returns(true);
            cookies.Setup(t => t["mycookie"]).Returns("mycookievalue");

            var accessor = new RequestCookieAccessor(cookies.Object);
            //Act
            var actual = accessor.Get("mycookie");
            //Assert
            Assert.IsInstanceOfType(actual, typeof(ICookie));
        }

        [TestMethod]
        public void GetKeys_ReturnsIEnumerable()
        {
            //Arrange
            var cookies = new Mock<IRequestCookieCollection>();
            var cookieKeys = new List<string> { "mycookie", "secondcookie" };
            cookies.Setup(t => t.Keys).Returns(cookieKeys);

            var accessor = new RequestCookieAccessor(cookies.Object);
            //Act
            var actual = accessor.GetKeys();
            //Assert
            Assert.IsInstanceOfType<IEnumerable<string>>(actual);
            Assert.AreEqual(2, actual.Count());
        }
    }
}