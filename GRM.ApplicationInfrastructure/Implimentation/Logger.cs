using NLog;
using System;


namespace GRM.ApplicationInfrastructure
{
    public class Logger : ILogger
    {
        private NLog.Logger Nlog = LogManager.GetCurrentClassLogger();

        public void Error(string message, Exception exception)
        {
              Nlog.Error(exception, message);
        }
    }
}