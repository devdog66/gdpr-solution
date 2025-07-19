using Microsoft.AspNetCore.Mvc;
using WebUtils.Data;
using WebUtils.Services;
using WebUtils.Domain.Models;
using WebUtils.Core.Filters;

namespace ConsentService
{
    [Route("/")]
    [ApiController]
    [ApiKey]
    public class ServiceController(ILogger<ServiceController> logger, IRepository repository, 
        IHttpContextAccessor contextAccessor) : ControllerBase, IConsentService
    {
        private readonly ILogger<ServiceController> logger = logger;
        private readonly IRepository repository = repository;
        private readonly IHttpContextAccessor contextAccessor = contextAccessor;

        [HttpGet]
        public string Get(string uid)
        {
            try
            {
                var entity = repository.Read<Consent>(t => t.Uid == uid).FirstOrDefault();
                if (entity == null)
                {
                    contextAccessor.HttpContext!.Response.StatusCode = 404; // Not Found
                    return "Consent not found.";
                }
                return entity.CookiePreferences ?? string.Empty;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while processing the request for UID: {Uid}", uid);
                contextAccessor.HttpContext!.Response.StatusCode = 500; // Internal Server Error
                return "An error occurred while processing your request.";
            }
        }

        [HttpPost]
        public long Post(string uid, string cookiePreferences)
        {
            try
            {
                var entity = repository.Read<Consent>(t => t.Uid == uid).FirstOrDefault();

                if (entity == null)
                {
                    entity = new Consent
                    {
                        Uid = uid,
                        CookiePreferences = cookiePreferences,
                        LastUpdated = DateTime.Now
                    };
                    entity = repository.Create(entity);
                }
                else 
                {
                    entity.CookiePreferences = cookiePreferences;
                    entity.LastUpdated = DateTime.Now;
                    repository.Update(entity);
                }

                return entity.Id;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while processing the request for UID: {Uid}", uid);
                contextAccessor.HttpContext!.Response.StatusCode = 500; // Internal Server Error
                return 0; // Assuming 0 indicates an error
            }
        }
    }
}
