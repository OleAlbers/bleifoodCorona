using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bleifood.BL
{
    public static class Config
    {
        private const string SecretConfigPath = "D:\\home\\bleifood.config";
        private static ConfigValues _currentConfig = null;

        public static ConfigValues Settings
        {
            get
            {
                if (_currentConfig == null) _currentConfig = ReadConfig();
                return _currentConfig;
            }
        }

        private static ConfigValues ReadConfig()
        {
            string fileContent=File.ReadAllText(SecretConfigPath);
            return JsonConvert.DeserializeObject<ConfigValues>(fileContent);
        }

    
    }
    
}
