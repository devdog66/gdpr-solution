
namespace WebUtils.Logging
{
    public interface ILogManager
    {
        ILogContract<T> GetLogger<T>();
    }
}
