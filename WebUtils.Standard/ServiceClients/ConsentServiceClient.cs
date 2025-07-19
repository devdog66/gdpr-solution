using System;
using System.Net.Http;
using System.Text.Json;
using WebUtils.Config;
using WebUtils.Logging;
using WebUtils.Security;
using WebUtils.Services;
using WebUtils.Standard.Web;
using WebUtils.Web;

namespace WebUtils.Standard.ServiceClients
{
    public class ConsentServiceClient : IConsentServiceClient
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogContract<ConsentServiceClient> logger;
        private readonly IConfigContract configuration;
        private readonly IProtectionProvider protectionProvider;
        private readonly IVault vault;
        private readonly ICacheFacade memoryCache;

        public ConsentServiceClient(IHttpClientFactory httpClientFactory, ILogManager logManager, 
            IConfigContract configContract, IProtectionProvider protectionProvider,
            IVault vault, ICacheFacade memoryCache)
        {
            this.httpClientFactory = httpClientFactory;
            this.configuration = configContract;
            this.protectionProvider = protectionProvider;
            this.vault = vault;
            this.memoryCache = memoryCache;
            this.logger = logManager.GetLogger<ConsentServiceClient>();
        }

        public ConsentResponse GetConsent(IHttpContextFacade contextFacade)
        {
            var consentResponse = new ConsentResponse();
            try
            {
                var cookieUid = GetCookieUid(contextFacade);

                if (! string.IsNullOrEmpty(cookieUid))
                {
                    // Check cache first
                    if (memoryCache.TryGetValue(cookieUid, out ConsentResponse cachedConsents))
                    {
                        logger.Debug($"Consent found in cache for UID: {cookieUid}");
                        return cachedConsents;
                    }

                    // If not in cache, fetch from API
                    using (var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get,
                        $"{configuration.GetValue<string>("Consent:ApiUrl")}?uid={cookieUid}")
                    {
                        Headers = {
                            { "X-API-Key", vault.GetSecret("Consent:ApiKey") }
                        }
                    })
                    {

                        using (var client = httpClientFactory.CreateClient())
                        {
                            var httpResponse = client.SendAsync(httpRequestMessage).Result;
                            if (httpResponse.IsSuccessStatusCode)
                            {
                                var responseContent = httpResponse.Content.ReadAsStringAsync().Result;
                                var cookiePrefs = JsonSerializer.Deserialize<CookiePrefsModel>(responseContent);
                                consentResponse.HasCookie = true;
                                consentResponse.CookiePrefs = cookiePrefs ?? new CookiePrefsModel();
                                memoryCache.Set(cookieUid, consentResponse, DateTimeOffset.Now.AddMinutes(30)); // Cache for 30 minutes
                            }
                            else
                            {
                                logger.Warn($"Failed to get consent. Status code: {httpResponse.StatusCode}");
                            }
                        }
                    }
                }

                return consentResponse;
            }
            catch (Exception ex)
            {
                logger.Error("An error occurred while getting consent.", ex);
                return consentResponse;
            }
        }

        public void SaveConsent(CookiePrefsModel model, IHttpContextFacade contextFacade)
        {
            try
            {
                var cookieUid = GetCookieUid(contextFacade);
                if (string.IsNullOrEmpty(cookieUid))
                {
                    cookieUid = Guid.NewGuid().ToString();
                }

                var cookiePrefsJson = JsonSerializer.Serialize(model);
                var url = $"{configuration.GetValue<string>("Consent:ApiUrl")}?uid={cookieUid}&cookiePreferences={cookiePrefsJson}";
                using (var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Headers = {
                        { "X-API-Key", vault.GetSecret("Consent:ApiKey") }
                    }
                })
                {

                    using (var client = httpClientFactory.CreateClient())
                    {
                        var httpResponse = client.SendAsync(httpRequestMessage).Result;
                        if (!httpResponse.IsSuccessStatusCode)
                        {
                            logger.Warn($"Failed to save consent. Status code: {httpResponse.StatusCode}");
                        }
                        else
                        {
                            memoryCache.Set(cookieUid, new ConsentResponse
                            {
                                HasCookie = true,
                                CookiePrefs = model
                            }, DateTimeOffset.Now.AddMinutes(30)); // Cache for 30 minutes); // Clear cache for this UID

                            SetCookie(contextFacade, cookieUid);
                            logger.Debug($"Consent saved successfully for UID: {cookieUid}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("An error occurred while saving consent.", ex);
            }
        }

        internal void SetCookie(IHttpContextFacade contextFacade, string cookieUid)
        {
            var dataProtectionName = configuration.GetValue<string>("DataProtectionName");
            var cookieName = configuration.GetValue<string>("Consent:CookieName");
            var cookieDomainName = configuration.GetValue<string>("Consent:CookieDomainName");

            var dp = protectionProvider.CreateProtection(dataProtectionName);
            var protectedUid = dp.Protect(cookieUid);
            var cookie = new CookieImpl
            {
                Name = cookieName,
                Value = protectedUid,
                Domain = cookieDomainName,
                Path = "/",
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteEnum.Strict,
                Expires = DateTime.UtcNow.AddYears(1)
            };
            contextFacade.Response.Cookies.Set(cookieName, cookie);
        }

        internal string GetCookieUid(IHttpContextFacade httpContextFacade)
        {
            string cookieUid = null;

            var request = httpContextFacade.Request;
            var cookieName = configuration.GetValue<string>("Consent:CookieName");
            if (request.Cookies != null)
            {
                var cookie = request.Cookies.Get(cookieName);
                if (cookie != null)
                {
                    var dpName = configuration.GetValue<string>("DataProtectionName");
                    var dp = protectionProvider.CreateProtection(dpName);
                    cookieUid = dp.Unprotect(cookie.Value);
                }
            }
            return cookieUid;
        }
    }
}
