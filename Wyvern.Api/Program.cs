using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Wyvern.Domain.Entities;
using Wyvern.Infrastructure.Data;
using Wyvern.Api.Extensions;
using Wyvern.Application.Mappings;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonContext.Default);
    });

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonContext.Default);
});

builder.Services.AddOpenApi();
builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(AtributoProfile).Assembly));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<WyvernDbContext>(options =>
    options.UseSqlServer(connectionString));
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Wyvern API")
               .WithTheme(ScalarTheme.Moon);
    });
    app.ConfigureExceptionHandler();
}

app.UseHttpsRedirection();

app.MapControllers();


app.Run();

[JsonSerializable(typeof(IEnumerable<Usuario>))]
[JsonSerializable(typeof(Usuario))]
internal partial class AppJsonContext : JsonSerializerContext
{
}