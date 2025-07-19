using Microsoft.AspNetCore.Http;
using WebUtils.Core.Web;
using WebUtils.Web;

namespace UnitTests.WebUtils.Core.Web
{
    [TestClass]
    public sealed class HttpContextAdapterTests
    {
        [TestMethod]
        public void Request_IsFacade()
        {
            //Arrange
            var httpContext = new DefaultHttpContext();
            var adapter = new HttpContextAdapter(httpContext);
            //Act
            var actual = adapter.Request;
            //Assert
            Assert.IsInstanceOfType<IHttpRequestFacade>(actual);
        }

        [TestMethod]
        public void Response_IsFacade()
        {
            //Arrange
            var httpContext = new DefaultHttpContext();
            var adapter = new HttpContextAdapter(httpContext);
            //Act
            var actual = adapter.Response;
            //Assert
            Assert.IsInstanceOfType<IHttpResponseFacade>(actual);
        }
    }
}