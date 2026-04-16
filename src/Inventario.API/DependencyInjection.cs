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
                    Version = "1.0.2",
                    Description = "API de inventario",
                });
            });

            services.AddCors(options =>
            {
                options.AddPolicy(AngularCorsPolicy, builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            return services;
        }
    }
}
