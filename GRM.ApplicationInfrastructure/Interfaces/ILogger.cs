using System;

namespace GRM.ApplicationInfrastructure
{
    public interface ILogger
    {
        void Error(string message, Exception exception);
    }
}