using System;

namespace WazeScraper.Utils
{
    [AppScope(Scope.SingleInstance)]
    public class EnvironmentVariableGetter
    {
        private string GetEnvironmentVariable(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return string.Empty;
            }
            var keyValues = Environment.GetEnvironmentVariables();

            if (!keyValues.Contains(key))
            {
                return string.Empty;
            }

            return keyValues[key]?.ToString();
        }

        public string GetConnectionPassword()
        {
            return GetEnvironmentVariable(Constants.ConnectionPasswordKey);
        }

        public string GetConnectionIp()
        {
            return GetEnvironmentVariable(Constants.ConnectionIpKey);
        }

        public string GetConnectionUser()
        {
            return GetEnvironmentVariable(Constants.ConnectionUserKey);
        }

        public string GetConnectionDatabase()
        {
            return GetEnvironmentVariable(Constants.ConnectionDatabaseKey);
        }
    }
}
