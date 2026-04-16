using Microsoft.OpenApi;

namespace Inventario.API
{
    public static class DependencyInjection
    {
        private const string AngularCorsPolicy = "AllowAngularApp";

        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Inventario API",
                    Version = "1.0.0",
                    Description = "API de inventario",
                });
            });

            services.AddCors(options =>
            {
                options.AddPolicy(AngularCorsPolicy, builder =>
                {
                    builder
                        .WithOrigins("http://localhost:4200", "https://inventario-api.resyerf.com", "https://inventario-coingec.resyerf.com/")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            return services;
        }
    }
}
