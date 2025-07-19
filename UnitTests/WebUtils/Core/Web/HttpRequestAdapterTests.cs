using Microsoft.AspNetCore.Http;
using WebUtils.Core.Web;
using WebUtils.Utils;
using WebUtils.Web;

namespace UnitTests.WebUtils.Core.Web
{
    [TestClass]
    public sealed class HttpRequestAdapterTests
    {
        [TestMethod]
        public void UserAgent_ReturnsAgent()
        {
            //Arrange
            var httpContext = new DefaultHttpContext();
            var httpRequest = httpContext.Request;
            httpRequest.Headers.Append("User-Agent", "UnitTest");
            var adapter = new HttpRequestAdapter(httpRequest);
            //Act
            var actual = adapter.UserAgent;
            //Assert
            Assert.AreEqual("UnitTest", actual);
        }

        [TestMethod]
        public void Headers_ReturnsAccessor()
        {
            //Arrange
            var httpContext = new DefaultHttpContext();
            var httpRequest = httpContext.Request;
            httpRequest.Headers.Append("User-Agent", "UnitTest");
            var adapter = new HttpRequestAdapter(httpRequest);
            //Act
            var actual = adapter.Headers;
            //Assert
            Assert.IsInstanceOfType(actual, typeof(IAccessor<string>));
            Assert.AreEqual(1, actual.Count);
        }

        [TestMethod]
        public void Cookies_ReturnsAccessor()
        {
            //Arrange
            var httpContext = new DefaultHttpContext();
            var httpRequest = httpContext.Request;
            var adapter = new HttpRequestAdapter(httpRequest);
            //Act
            var actual = adapter.Cookies;
            //Assert
            Assert.IsInstanceOfType(actual, typeof(IAccessor<ICookie>));
            Assert.AreEqual(0, actual.Count);
            Assert.IsFalse(actual.Contains("testcookie"));
            Assert.AreEqual(0, actual.GetKeys().Count());
        }

        [TestMethod]
        public void Host_ReturnsString()
        {
            //Arrange
            var httpContext = new DefaultHttpContext();
            var httpRequest = httpContext.Request;
            httpRequest.Host = new HostString("http://testhost");
            var adapter = new HttpRequestAdapter(httpRequest);
            //Act
            var actual = adapter.Host;
            //Assert
            Assert.AreEqual("http://testhost", actual);
        }

        [TestMethod]
        public void Method_ReturnsString()
        {
            //Arrange
            var expected = "POST";
            var httpContext = new DefaultHttpContext();
            var httpRequest = httpContext.Request;
            httpRequest.Method = expected;
            var adapter = new HttpRequestAdapter(httpRequest);
            //Act
            var actual = adapter.Method;
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Path_ReturnsString()
        {
            //Arrange
            var expected = "/api";
            var httpContext = new DefaultHttpContext();
            var httpRequest = httpContext.Request;
            httpRequest.Path = expected;
            var adapter = new HttpRequestAdapter(httpRequest);
            //Act
            var actual = adapter.Path;
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Referer_ReturnsString()
        {
            //Arrange
            var expected = "http://google.com";
            var httpContext = new DefaultHttpContext();
            var httpRequest = httpContext.Request;
            httpRequest.Headers.Append("Referer", expected);
            var adapter = new HttpRequestAdapter(httpRequest);
            //Act
            var actual = adapter.Referer;
            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}