using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using WebUtils.Core.Filters;

namespace UnitTests.WebUtils.Core
{
    [TestClass]
    public sealed class ApiKeyAuthorizationFilterTests
    {
        [TestMethod]
        public void ApiKeyAttribute_Should_Be_Registered_As_ServiceFilter()
        {
            // Arrange
            var attribute = new ApiKeyAttribute();
            
            // Act & Assert
            Assert.IsInstanceOfType(attribute, typeof(ServiceFilterAttribute));
            Assert.AreEqual(typeof(ApiKeyAuthorizationFilter), attribute.ServiceType);
        }

        [TestMethod]
        public void ApiKeyAuthorizationFilter_Should_Return_Unauthorized_When_ApiKey_Is_Missing()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "ApiKey", "test-api-key" }
                })
                .Build();
            var filter = new ApiKeyAuthorizationFilter(configuration);
            var context = new AuthorizationFilterContext(
                new ActionContext(
                    new DefaultHttpContext(),
                    new Microsoft.AspNetCore.Routing.RouteData(),
                    new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()),
                new List<IFilterMetadata>());
            
            // Act
            filter.OnAuthorization(context);
            
            // Assert
            Assert.IsInstanceOfType(context.Result, typeof(UnauthorizedResult));
        }

        [TestMethod]
        public void ApiKeyAuthorizationFilter_Should_Return_Unauthorized_When_ApiKey_Is_Invalid()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "ApiKey", "test-api-key" }
                })
                .Build();
            var filter = new ApiKeyAuthorizationFilter(configuration);
            var context = new AuthorizationFilterContext(
                new ActionContext(
                    new DefaultHttpContext(),
                    new Microsoft.AspNetCore.Routing.RouteData(),
                    new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()),
                new List<IFilterMetadata>());
            
            // Simulate missing API key in request header
            context.HttpContext.Request.Headers["X-API-KEY"] = "invalid-api-key";
            
            // Act
            filter.OnAuthorization(context);
            
            // Assert
            Assert.IsInstanceOfType(context.Result, typeof(UnauthorizedResult));
        }

        [TestMethod]
        public void ApiKeyAuthorizationFilter_Should_Pass_When_ApiKey_Is_Valid()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "ApiKey", "test-api-key" }
                })
                .Build();
            var filter = new ApiKeyAuthorizationFilter(configuration);
            var context = new AuthorizationFilterContext(
                new ActionContext(
                    new DefaultHttpContext(),
                    new Microsoft.AspNetCore.Routing.RouteData(),
                    new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()),
                new List<IFilterMetadata>());
            
            // Simulate valid API key in request header
            context.HttpContext.Request.Headers["X-API-KEY"] = "test-api-key";
            
            // Act
            filter.OnAuthorization(context);
            
            // Assert
            Assert.IsNull(context.Result); // No result means authorization passed
        }
    }
}
