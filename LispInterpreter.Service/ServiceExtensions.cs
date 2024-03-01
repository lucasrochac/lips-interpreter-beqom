using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LispInterpreter.Service
{
    public static class ServiceExtensions
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<ILispInterpreterService, LispInterpreterService>();
        }
    }
}
