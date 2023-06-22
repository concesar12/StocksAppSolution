using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using Microsoft.EntityFrameworkCore.InMemory;
using Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace StockTest
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);


            builder.UseEnvironment("Test");

            builder.ConfigureServices(services => {
                var descripter = services.SingleOrDefault(temp => temp.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if (descripter != null)
                {
                    services.Remove(descripter);
                }
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("DatbaseForTesting");
                });
            });
            //add configuration source for the host builder
            builder.ConfigureAppConfiguration((WebHostBuilderContext ctx, Microsoft.Extensions.Configuration.IConfigurationBuilder config) =>
            {
                var newConfiguration = new Dictionary<string, string>() {
                     { "FinnhubToken", "cc676uaad3i9rj8tb1s0" } //add token value
                    };

                config.AddInMemoryCollection(newConfiguration);
            });

        }
    }
}
