using CAS_API.Models;
using CAS_API.Models.Interfaces;
using Microsoft.Extensions.Options;

namespace CAS_API.Services
{
    static class ApiConfigurationService
    {
        public static IServiceCollection AddAppConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApiSettings>(configuration.GetSection("ApiSettings"));
            var myAppConfiguration = services.BuildServiceProvider().GetRequiredService<IOptions<ApiSettings>>().Value;
            services.AddSingleton<IApiSettings>(myAppConfiguration);

            return services;
        }
    }
}
