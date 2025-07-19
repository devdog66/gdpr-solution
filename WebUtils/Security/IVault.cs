namespace WebUtils.Security
{
    public interface IVault
    {
        string GetSecret(string key);
    }
}
