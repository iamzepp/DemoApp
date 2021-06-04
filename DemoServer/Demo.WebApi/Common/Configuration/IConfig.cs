namespace Demo.WebApi.Common.Configuration
{
    public interface IConfig
    {
        string MainDbConnectionString { get; set; }

        void Validate();
    }
}