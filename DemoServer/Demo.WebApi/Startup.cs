using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Npgsql;
using Demo.WebApi.Common.Configuration;
using Demo.WebApi.Common.DbConnection;

namespace Demo.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Demo.WebApi", Version = "v1"});
            });

            RegisterInjections(services, Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo.WebApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        public void RegisterInjections(IServiceCollection services, IConfiguration configuration)
        {
            var configurationModel = configuration.GetSection("Settings").Get<ConfigurationModel>() 
                                          ?? throw new ArgumentException($"Not found \'Settings\' section");
            
            configurationModel.Validate();
            
            services.AddSingleton<IConfig>(x => configurationModel);
            services.AddTransient<IMainDbConnection>(x => new DbProxy(new NpgsqlConnection(configurationModel.MainDbConnectionString)));
        }
    }
}