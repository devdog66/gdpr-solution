using Microsoft.AspNetCore.Http;
using UnitTests.Fakes;
using WebUtils.Core.Web;

namespace UnitTests.WebUtils.Core.Web
{
    [TestClass]
    public sealed class ResponseCookiesAccessorTests
    {
        [TestMethod]
        public void Count_ReturnsOne()
        {
            //Arrange
            var httpContext = new DefaultHttpContext();
            var httpResponse = httpContext.Response;
            httpResponse.Cookies.Append("testkey", "testvalue");
            var headersAccessor = new HeadersAccessor(httpResponse.Headers);
            var accessor = new ResponseCookiesAccessor(httpResponse.Cookies, headersAccessor);
            //Act
            var actual = accessor.Count;
            //Assert
            Assert.AreEqual(1, actual);
        }

        [TestMethod]
        public void GetKeys_ReturnsListOfOne()
        {
            //Arrange
            var httpContext = new DefaultHttpContext();
            var httpResponse = httpContext.Response;
            httpResponse.Cookies.Append("testkey", "testvalue");
            var headersAccessor = new HeadersAccessor(httpResponse.Headers);
            var accessor = new ResponseCookiesAccessor(httpResponse.Cookies, headersAccessor);
            //Act
            var actual = accessor.GetKeys();
            //Assert
            Assert.AreEqual(1, actual.Count());
        }

        [TestMethod]
        public void Set_AddsOne()
        {
            //Arrange
            var httpContext = new DefaultHttpContext();
            var httpResponse = httpContext.Response;
            var headersAccessor = new HeadersAccessor(httpResponse.Headers);
            var accessor = new ResponseCookiesAccessor(httpResponse.Cookies, headersAccessor);
            var testCookie = new TestCookie
            {
                Name = "testkey",
                Value = "testvalue"
            };

            //Act
            accessor.Set("testkey", testCookie);

            //Assert
            Assert.AreEqual(1, httpResponse.Headers.Count);
        }

        [TestMethod]
        public void Contains_ReturnsTrue()
        {
            //Arrange
            var httpContext = new DefaultHttpContext();
            var httpResponse = httpContext.Response;
            httpResponse.Cookies.Append("testkey", "testvalue");
            var headersAccessor = new HeadersAccessor(httpResponse.Headers);
            var accessor = new ResponseCookiesAccessor(httpResponse.Cookies, headersAccessor);
            //Act
            var actual = accessor.Contains("testkey");
            //Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void Get_ThrowsException()
        {
            //Arrange
            var httpContext = new DefaultHttpContext();
            var httpResponse = httpContext.Response;
            var headersAccessor = new HeadersAccessor(httpResponse.Headers);
            var accessor = new ResponseCookiesAccessor(httpResponse.Cookies, headersAccessor);

            //Act and Assert
            Assert.ThrowsException<NotImplementedException>(() => accessor.Get("testkey"));
        }

        [TestMethod]
        public void GetEnumerable_ThrowsException()
        {
            //Arrange
            var httpContext = new DefaultHttpContext();
            var httpResponse = httpContext.Response;
            var headersAccessor = new HeadersAccessor(httpResponse.Headers);
            var accessor = new ResponseCookiesAccessor(httpResponse.Cookies, headersAccessor);

            //Act and Assert
            Assert.ThrowsException<NotImplementedException>(() => accessor.Get(t => t.Name == "testkey"));
        }
    }
}