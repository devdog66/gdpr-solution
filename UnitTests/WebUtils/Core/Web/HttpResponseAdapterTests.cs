using Microsoft.AspNetCore.Http;
using WebUtils.Core.Web;

namespace UnitTests.WebUtils.Core.Web
{
    [TestClass]
    public sealed class HttpResponseAdapterTests
    {
        [TestMethod]
        public void Headers_ReturnsOne()
        {
            //Arrange
            var httpContext = new DefaultHttpContext();
            var httpResponse = httpContext.Response;
            httpResponse.Cookies.Append("testkey", "testvalue", new CookieOptions
            {
                Domain = "test",
                Expires = DateTime.Now.AddDays(1),
            });
            var adapter = new HttpResponseAdapter(httpResponse);
            //Act
            var actual = adapter.Headers;
            //Assert
            Assert.AreEqual(1, actual.Count);
        }

        [TestMethod]
        public void StatusCode_Returns200()
        {
            //Arrange
            var httpContext = new DefaultHttpContext();
            var httpResponse = httpContext.Response;
            var adapter = new HttpResponseAdapter(httpResponse);
            //Act
            var actual = adapter.StatusCode;
            //Assert
            Assert.AreEqual(200, actual);
        }

        [TestMethod]
        public void Cookies_ReturnsOne()
        {
            //Arrange
            var httpContext = new DefaultHttpContext();
            var httpResponse = httpContext.Response;
            httpResponse.Cookies.Append("testkey", "testvalue");
            var adapter = new HttpResponseAdapter(httpResponse);
            //Act
            var actual = adapter.Cookies;
            //Assert
            Assert.AreEqual(1, actual.Count);
        }
    }
}