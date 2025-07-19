namespace WebUtils.Config
{
    public interface IConfigContract
    {
        T GetValue<T>(string configPath);
    }
}
