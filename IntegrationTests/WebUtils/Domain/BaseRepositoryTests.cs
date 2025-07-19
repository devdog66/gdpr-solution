using Microsoft.Extensions.Configuration;
using WebUtils.Core.Security;
using WebUtils.Domain.Models;
using WebUtils.Domain.Repositories;

namespace IntegrationTests.WebUtils.Domain
{
    [TestClass]
    public sealed class BaseRepositoryTests
    {
        private BaseRepository? repository;
        private readonly string sessionId = Guid.NewGuid().ToString();

        [TestInitialize]
        public void Setup()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("secrets.json", optional: true)
                .Build();
            var vault = new SecretsFileVault(configuration);

            repository = new BaseRepository(configuration, vault);
        }

        [TestMethod]
        public void Crud_Roundtrip()
        {
            // Arrange
            var entity = new Request
            {
                Id = 0, 
                RequestMethod = "GET",
                RequestPath = "/api/test",
                Referrer = "https://example.com",
                SessionId = sessionId,
                Host = "localhost",
                RequestCookies = "cookie1,cookie2",
                ResponseCode = "200",
                ResponseCookies = "responseCookie1,responseCookie2",
             };
            
            // Act - Create
            var createdEntity = repository!.Create(entity);
            // Assert - Create
            Assert.IsNotNull(createdEntity);
            Assert.AreEqual("GET", createdEntity.RequestMethod);

            //Act - Read
            var readEntity = repository.Read<Request>(r => r.Id == createdEntity.Id).FirstOrDefault();

            // Assert - Read
            Assert.AreEqual("GET", createdEntity.RequestMethod);

            // Act - Update
            readEntity!.ResponseCode = "404";
            var rowsAffected = repository.Update(readEntity);
            // Assert - Update
            Assert.AreEqual(1, rowsAffected);

            // Act - Delete
            rowsAffected = repository.Delete(readEntity);
            // Assert - Delete
            Assert.AreEqual(1, rowsAffected);

            // Cleanup
            repository.Dispose();
        }
    }
}
