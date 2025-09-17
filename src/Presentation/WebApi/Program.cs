using Application;
using Application.Interfaces;
using Infraestructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
