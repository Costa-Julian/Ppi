using Application;
using Application.Dto;
using Application.Interfaces;
using Infraestructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PPI .net challenge", Version = "v1" });
    c.EnableAnnotations();

    var xml = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xml);
    if (File.Exists(xmlPath))
        c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);

    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    c.ExampleFilters();
});
builder.Services.AddSwaggerExamplesFromAssemblyOf<OrdenRequestExample>();

//Conexion con sql server
builder.Services.AddDbContext<EfAppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection")));

//Repos
builder.Services.AddScoped<IActivoRepository, EfActivoRepository>();
builder.Services.AddScoped<IOrdenRepository, EfOrdenRepository>();
builder.Services.AddScoped<IEstadoRepository, EfEstadoRepository>();
builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork>();

//Servicios
builder.Services.AddScoped<IActivoService,ActivoService>();
builder.Services.AddScoped<IOrdenService, OrdenService>();
builder.Services.AddScoped<IEstadoService,EstadoService>();
builder.Services.AddScoped<ICalculoTotal,CalculoFci>();
builder.Services.AddScoped<ICalculoTotal, CalculoBono>();
builder.Services.AddScoped<ICalculoTotal, CalculoAccion>();
builder.Services.AddScoped<CalculoOrdenFactory>();

var app = builder.Build();

// Swagger config
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PPI .net challenge v1");
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
