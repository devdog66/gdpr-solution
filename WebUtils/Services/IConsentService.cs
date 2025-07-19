using System.Threading.Tasks;

namespace WebUtils.Services
{
    public interface IConsentService
    {
        string Get(string uid);
        long Post(string uid, string cookiePreferences);
    }
}
