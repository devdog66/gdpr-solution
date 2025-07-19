using NLog;
using System;
using WebUtils.Logging;

namespace WebUtils.Frmwk.Logging
{
    public class NLogManagerAdapter : ILogManager
    {
        public ILogContract<T> GetLogger<T>()
        {
            return new NLogAdapter<T>();
        }
    }

    internal class NLogAdapter<T> : ILogContract<T>
    {
        private readonly Logger logger;

        internal NLogAdapter()
        {
            string name = typeof(T).FullName;
            logger = LogManager.GetLogger(name);
        }

        public void Debug(string message)
        {
            logger.Debug(message);
        }

        public void Error(string message)
        {
            logger.Error(message);
        }

        public void Error(string message, Exception exception)
        {
            logger.Error(exception, message);
        }

        public void Fatal(string message)
        {
            logger.Fatal(message);
        }

        public void Fatal(string message, Exception exception)
        {
            logger.Fatal(exception, message);
        }

        public void Info(string message)
        {
            logger.Info(message);
        }

        public void Warn(string message)
        {
            logger.Warn(message);
        }

        public void Warn(string message, Exception exception)
        {
            logger.Warn(exception, message);
        }
    }
}
