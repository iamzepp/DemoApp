namespace Demo.WebApi.Common.Configuration
{
    public interface IConfig
    {
        string MainDbConnectionString { get; set; }
        
        public string JwtSecretKey { get; set; }

        void Validate();
    }
}