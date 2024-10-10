using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace ProductsSample.Api.Configurations;

public static class SwaggerConfiguration
{
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSwaggerGen(options =>
        {
            options.MapType<DateOnly>(() => new OpenApiSchema
            {
                Type = "string",
                Format = "date",
                Example = new OpenApiString("2024-01-01")
            });

            options.MapType<TimeOnly>(() => new OpenApiSchema
            {
                Type = "string",
                Format = "time",
                Example = new OpenApiString("12:34")
            });

            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Products Sample API",
                Description = "Swagger UI to document the Products Sample API.",
                Contact = new OpenApiContact { Name = "Enzo Godoy", Email = "enzofngodoy@hotmail.com" }
            });

            options.EnableAnnotations();

            var webApiXmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var webApiXmlFileXmlPath = Path.Combine(AppContext.BaseDirectory, webApiXmlFile);
            options.IncludeXmlComments(webApiXmlFileXmlPath);
        });

        return services;
    }

    public static void UseSwaggerSetup(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        });
    }
}