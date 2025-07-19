using ConsentService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using WebUtils.Data;
using WebUtils.Domain.Models;

namespace UnitTests.ConsentService
{
    [TestClass]
    public sealed class ConsentServiceTests
    {
        [TestMethod]
        public void Get_ShouldReturnError_WhenExceptionThrown()
        {
            // Arrange
            var expected = "An error occurred while processing your request.";
            string uid = "test-uid";

            // Mock dependencies
            var mockLogger = new Mock<ILogger<ServiceController>>();
            var mockRepository = new Mock<IRepository>();
            var mockContextAccessor = new Mock<IHttpContextAccessor>();
            var httpContext = new DefaultHttpContext();

            mockContextAccessor.Setup(c => c.HttpContext).Returns(httpContext);
            mockRepository.Setup(r => r.Read<Consent>(t => t.Uid == uid)).Throws(new Exception("Test exception"));

            var controller = new ServiceController(mockLogger.Object, mockRepository.Object, mockContextAccessor.Object);

            // Act
            var result = controller.Get(uid);

            // Assert
            Assert.AreEqual(500, httpContext.Response.StatusCode); // Internal Server Error
            Assert.AreEqual(expected, result);
            mockLogger.Verify(
                l => l.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v!.ToString()!.Contains("An error occurred while processing the request for UID")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);

        }

        [TestMethod]
        public void Get_ShouldReturnConsentNotFound_WhenNotFound()
        {
            // Arrange
            string expected = "Consent not found.";
            string uid = "test-uid";
            
            // Mock dependencies
            var mockLogger = new Mock<ILogger<ServiceController>>();
            var mockRepository = new Mock<IRepository>();
            var mockQueryable = new List<Consent>().AsQueryable();
            var mockContextAccessor = new Mock<IHttpContextAccessor>();
            var httpContext = new DefaultHttpContext(); 

            mockContextAccessor.Setup(c => c.HttpContext).Returns(httpContext);
            mockRepository.Setup(r => r.Read<Consent>(t => t.Uid == uid)).Returns(mockQueryable);
            
            var controller = new ServiceController(mockLogger.Object, mockRepository.Object, mockContextAccessor.Object);
            
            // Act
            var result = controller.Get(uid);
            
            // Assert
            Assert.AreEqual(404, httpContext.Response.StatusCode); // Not Found
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Get_ShouldReturnConsent_WhenFound()
        {
            // Arrange
            var expected = "test-preferences";
            string uid = "test-uid";
            var consent = new Consent { Uid = uid, CookiePreferences = expected};

            // Mock dependencies
            var mockLogger = new Mock<ILogger<ServiceController>>();
            var mockRepository = new Mock<IRepository>();
            var mockQueryable = new List<Consent> { consent }.AsQueryable();
            var mockContextAccessor = new Mock<IHttpContextAccessor>();
            var httpContext = new DefaultHttpContext();
            mockContextAccessor.Setup(c => c.HttpContext).Returns(httpContext);
            mockRepository.Setup(r => r.Read<Consent>(t => t.Uid == uid)).Returns(mockQueryable);
            
            var controller = new ServiceController(mockLogger.Object, mockRepository.Object, mockContextAccessor.Object);
            // Act
            var result = controller.Get(uid);
            
            // Assert
            Assert.AreEqual(200, httpContext.Response.StatusCode); // OK
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Post_ShouldHandleException()
        {
            // Arrange
            long expected = 0; // Assuming the method returns 0 on error
            string uid = "test-uid";
            string cookiePreferences = "test-preferences";

            // Mock dependencies
            var mockLogger = new Mock<ILogger<ServiceController>>();
            var mockRepository = new Mock<IRepository>();
            var mockContextAccessor = new Mock<IHttpContextAccessor>();
            var httpContext = new DefaultHttpContext();

            mockRepository.Setup(r => r.Read<Consent>(t => t.Uid == uid)).Throws(new Exception("Test exception"));
            mockContextAccessor.Setup(c => c.HttpContext).Returns(httpContext);
            var controller = new ServiceController(mockLogger.Object, mockRepository.Object, mockContextAccessor.Object);
            
            // Act
            var actual = controller.Post(uid, cookiePreferences);

            //Assert
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(500, httpContext.Response.StatusCode); // Internal Server Error
            mockLogger.Verify(
                l => l.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v!.ToString()!.Contains("An error occurred while processing the request")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [TestMethod]
        public void Post_ShouldInsertNewRecord_WhenConsentNotFound()
        {
            // Arrange
            long expected = 1; // Assuming the method returns the ID of the inserted record
            string uid = "test-uid";
            string cookiePreferences = "test-preferences";
            var consent = new Consent { Uid = uid, CookiePreferences = cookiePreferences };
            var insertedConsent = new Consent { Uid = uid, CookiePreferences = cookiePreferences, Id = expected };

            // Mock dependencies
            var mockLogger = new Mock<ILogger<ServiceController>>();
            var mockRepository = new Mock<IRepository>();
            var mockContextAccessor = new Mock<IHttpContextAccessor>();
            var httpContext = new DefaultHttpContext();

            mockRepository.Setup(t => t.Create(It.IsAny<Consent>())).Returns(insertedConsent);
            mockRepository.Setup(r => r.Read<Consent>(t => t.Uid == uid)).Returns(new List<Consent>().AsQueryable());
            mockContextAccessor.Setup(c => c.HttpContext).Returns(httpContext);
            
            var controller = new ServiceController(mockLogger.Object, mockRepository.Object, mockContextAccessor.Object);
            // Act
            var actual = controller.Post(uid, cookiePreferences);

            // Assert
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(200, httpContext.Response.StatusCode); // OK
        }

        [TestMethod]
        public void Post_ShouldUpdateExistingRecord_WhenConsentFound()
        {
            // Arrange
            long expected = 1; // Assuming the method returns the ID of the updated record
            string uid = "test-uid";
            string cookiePreferences = "test-preferences";
            var existingConsent = new Consent { Uid = uid, CookiePreferences = "old-preferences", Id = expected };
            
            // Mock dependencies
            var mockLogger = new Mock<ILogger<ServiceController>>();
            var mockRepository = new Mock<IRepository>();
            var mockContextAccessor = new Mock<IHttpContextAccessor>();
            var httpContext = new DefaultHttpContext();
            mockRepository.Setup(r => r.Read<Consent>(t => t.Uid == uid)).Returns(new List<Consent> { existingConsent }.AsQueryable());
            mockContextAccessor.Setup(c => c.HttpContext).Returns(httpContext);
            
            var controller = new ServiceController(mockLogger.Object, mockRepository.Object, mockContextAccessor.Object);
            
            // Act
            var actual = controller.Post(uid, cookiePreferences);
            // Assert
            mockRepository.Verify(t => t.Update(It.IsAny<Consent>()), Times.Once);
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(200, httpContext.Response.StatusCode); // OK
        }
    }
}
