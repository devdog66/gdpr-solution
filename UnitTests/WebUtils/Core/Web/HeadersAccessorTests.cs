using Microsoft.AspNetCore.Http;
using WebUtils.Core.Web;

namespace UnitTests.WebUtils.Core.Web
{
    [TestClass]
    public sealed class HeadersAccessorTests
    {
        [TestMethod]
        public void Count_ReturnsOne()
        {
            //Arrange
            var httpContext = new DefaultHttpContext();
            var httpRequest = httpContext.Request;
            httpRequest.Headers.Append("testkey", "testvalue");
            var accessor = new HeadersAccessor(httpRequest.Headers);
            //Act
            var actual = accessor.Count;
            //Assert
            Assert.AreEqual(1, actual);
        }

        [TestMethod]
        public void Contains_ReturnsTrue()
        {
            //Arrange
            var httpContext = new DefaultHttpContext();
            var httpRequest = httpContext.Request;
            httpRequest.Headers.Append("testkey", "testvalue");
            var accessor = new HeadersAccessor(httpRequest.Headers);
            //Act
            var actual = accessor.Contains("testkey");
            //Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void Get_ReturnsValue()
        {
            //Arrange
            var httpContext = new DefaultHttpContext();
            var httpRequest = httpContext.Request;
            httpRequest.Headers.Append("testkey", "testvalue");
            var accessor = new HeadersAccessor(httpRequest.Headers);
            //Act
            var actual = accessor.Get("testkey");
            //Assert
            Assert.AreEqual("testvalue", actual);
        }

        [TestMethod]
        public void GetKeys_ReturnsListOfOne()
        {
            //Arrange
            var httpContext = new DefaultHttpContext();
            var httpRequest = httpContext.Request;
            httpRequest.Headers.Append("testkey", "testvalue");
            var accessor = new HeadersAccessor(httpRequest.Headers);
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
            var httpRequest = httpContext.Request;
            var accessor = new HeadersAccessor(httpRequest.Headers);
            //Act
            accessor.Set("testkey", "testvalue");
            //Assert
            Assert.AreEqual(1, httpRequest.Headers.Count);
        }
    }
}