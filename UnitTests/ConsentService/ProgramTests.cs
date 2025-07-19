using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;


namespace UnitTests.ConsentService
{
    [TestClass]
    public sealed class ProgramTests
    {
        [TestMethod]
        public void Program_GetLogger_ReturnsILogger()
        {
            //Given
            //When
            ILogger logger = Program.GetLogger();
            //Then
            Assert.IsInstanceOfType<ILogger>(logger);
        }


        [TestMethod]
        public void ItShouldSetupServices()
        {
            //Given
            var services = new Mock<IServiceCollection>();
            var logger = new Mock<ILogger>();

            //When
            Program.SetupServices(services.Object, logger.Object);
            //Then
            logger.Verify(
               x => x.Log(
                   LogLevel.Information,
                   It.IsAny<EventId>(),
                   It.Is<It.IsAnyType>((v, t) => v!.ToString()!.Contains("Services configured successfully.")),
                   It.IsAny<Exception>(),
                   It.IsAny<Func<It.IsAnyType, Exception, string>>()
               ),
               Times.Once
           );
        }

        [TestMethod]
        public void ItShouldSetupConfiguration()
        {
            //Given
            var configuration = new ConfigurationBuilder();
            var environment = new Mock<IWebHostEnvironment>();
            var logger = new Mock<ILogger>();
            environment.Setup(t => t.ContentRootPath).Returns("/");
            environment.Setup(t => t.EnvironmentName).Returns("Production");

            //When
            int configSources = Program.SetupConfiguration(configuration, environment.Object, logger.Object);
            //Then
            Assert.IsTrue(configSources > 0);
        }

        [TestMethod]
        public void ItShouldCreateBuilderWithOptions()
        {
            //Given
            var logger = new Mock<ILogger>();
            string[] args = [];
            var webAppOptions = new WebApplicationOptions
            {
                WebRootPath = "client-app/dist",
                Args = args
            };
            //When
            WebApplicationBuilder builder = Program.CreateBuilder(webAppOptions, logger.Object);
            //Then
            Assert.IsInstanceOfType<WebApplicationBuilder>(builder);
        }

        [TestMethod]
        public void ItShouldCreateAppWithOptions()
        {
            //Given
            var logger = new Mock<ILogger>();
            string[] args = [];
            var webAppOptions = new WebApplicationOptions
            {
                WebRootPath = "client-app/dist",
                Args = args
            };
            //When
            WebApplication app = Program.CreateApp(webAppOptions, logger.Object);
            //Then
            Assert.IsInstanceOfType<WebApplication>(app);
        }
    }
}
