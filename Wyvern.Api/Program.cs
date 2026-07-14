using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;
using Wyvern.Api.Extensions;
using Wyvern.Application.Mappings;
using Wyvern.Application.Services;
using Wyvern.Domain.Entities;
using Wyvern.Infrastructure.Data;
using Wyvern.Infrastructure.Repositories;
using Wyvern.Infrastructure.Repositories.Campanha;


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
builder.Services.AddScoped<IPdfParserService, PdfParserService>();
builder.Services.AddScoped<IPdfExportService, PdfExportService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddOpenApi();
builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(AtributoProfile).Assembly));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<WyvernDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddScoped<CampanhaRepository>();
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
app.UseCors("AllowAll");

app.MapControllers();


app.Run();

[JsonSerializable(typeof(IEnumerable<Usuario>))]
[JsonSerializable(typeof(Usuario))]
[JsonSerializable(typeof(IEnumerable<Wyvern.Application.DTOs.Personagem.PersonagemResponseDto>))]
[JsonSerializable(typeof(Wyvern.Application.DTOs.Personagem.PersonagemResponseDto))]
[JsonSerializable(typeof(IEnumerable<Wyvern.Application.DTOs.Campanha.CampanhaResponseDto>))]
[JsonSerializable(typeof(Wyvern.Application.DTOs.Campanha.CampanhaResponseDto))]
[JsonSerializable(typeof(IEnumerable<Wyvern.Application.DTOs.Sessao.SessaoResponseDto>))]
[JsonSerializable(typeof(Wyvern.Application.DTOs.Sessao.SessaoResponseDto))]
[JsonSerializable(typeof(Wyvern.Domain.Entities.Combate))]
[JsonSerializable(typeof(IEnumerable<Wyvern.Domain.Entities.Combate>))]
[JsonSerializable(typeof(Wyvern.Domain.Entities.CombateParticipante))]
[JsonSerializable(typeof(IEnumerable<Wyvern.Domain.Entities.CombateParticipante>))]
internal partial class AppJsonContext : JsonSerializerContext
{
}