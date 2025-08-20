using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Proposta.API.Profiles;
using Proposta.Application.Services;
using Proposta.Domain.Services.Interfaces;
using Proposta.Infrastructure.Data;
using Proposta.Infrastructure.Repositories;
using PropostaAPI;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

//Configuração de serviços
builder.Services.AddControllers();

// Configuração do Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Proposta API",
        Version = "v1",
        Description = "Esta API contém a implementação de um sistema para gerenciamento e contratação de propostas de seguro, desenvolvido como parte de um teste técnico, seguindo a Arquitetura Hexagonal (Ports & Adapters) e boas práticas de Clean Code, DDD e SOLID.",
        Contact = new OpenApiContact
        {
            Name = "Guilherme Silva",
            Email = "guilherme.silva@siracore.com"
        }
    });
    // Adiciona o arquivo XML de documentação
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});
//Configuração RabbitMQ
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        var configuration = context.GetRequiredService<IConfiguration>();

        var host = configuration["RabbitMq:Host"];
        var virtualHost = configuration["RabbitMq:VirtualHost"];
        var username = configuration["RabbitMq:Username"];
        var password = configuration["RabbitMq:Password"];

        cfg.Host(host, virtualHost, h =>
        {
            h.Username(username);
            h.Password(password);
        });
    });

});
//Configuração CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
//Configuração DataBase
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPropostaRepository, PropostaRepository>();
builder.Services.AddScoped<IPropostaService, PropostaService>();
builder.Services.AddAutoMapper(
    cfg => {
        // Configurações adicionais se quiser
    },
    typeof(PropostaProfile).Assembly
);

var app = builder.Build();

app.UseCors("AllowAll");

//app.ConfigureSQLite();

if (app.Environment.IsDevelopment())
//Configuração do pipeline HTTP
if (app.Environment.IsDevelopment())
{
    // Ativa o middleware do Swagger
    app.UseSwagger();

    // Ativa a interface do Swagger UI
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minha API v1");
        c.RoutePrefix = string.Empty; // Swagger acessível na raiz: https://localhost:<porta>/
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();