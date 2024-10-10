using ProductsSample.Abstractions.IoC;
using ProductsSample.Api.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerConfiguration()
    .AddDatabaseConfiguration()
    .RegisterInternalServices();

var app = builder.Build();

if (!app.Environment.IsProduction())
{
    app.UseSwaggerSetup();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();