using Inventario.API;
using Inventario.API.Extensions;
using Inventario.Application;
using Inventario.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPresentation()
    .AddInfrastructure(builder.Configuration)
    .AddApplication();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowAngularApp");
app.MapControllers();
app.ApplyMigrations();

app.Run();