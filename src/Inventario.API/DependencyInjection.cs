namespace Inventario.API
{
    public static class DependencyInjection
    {
        private const string AngularCorsPolicy = "AllowAngularApp";

        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddCors(options =>
            {
                options.AddPolicy(AngularCorsPolicy, builder =>
                {
                    builder
                        .WithOrigins("http://localhost:4200", "https://inventario-api.resyerf.com")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            return services;
        }
    }
}
