namespace WebUtils.Security
{
    public interface IProtectionProvider
    {
        IProtection CreateProtection(string purpose);
    }

    public interface IProtection 
    {
        string Protect(string data);
        string Unprotect(string protectedData);
    }
}
