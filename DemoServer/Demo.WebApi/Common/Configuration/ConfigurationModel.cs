using System.ComponentModel.DataAnnotations;

namespace Demo.WebApi.Common.Configuration
{
    public class ConfigurationModel : IConfig
    {
        public string MainDbConnectionString { get; set; }
        
        public void Validate()
        {
            if (string.IsNullOrEmpty(MainDbConnectionString)) throw new ValidationException("MainDbConnectionString is null or empty.");
        }
    }
}