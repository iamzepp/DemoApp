using System.ComponentModel.DataAnnotations;

namespace Demo.WebApi.Common.Configuration
{
    public class ConfigurationModel : IConfig
    {
        public string MainDbConnectionString { get; set; }
        
        public string JwtSecretKey { get; set; }
        
        public void Validate()
        {
            if (string.IsNullOrEmpty(MainDbConnectionString)) throw new ValidationException("MainDbConnectionString is null or empty.");
            if (string.IsNullOrEmpty(JwtSecretKey)) throw new ValidationException("JwtSecretKey is null or empty.");
        }
    }
}