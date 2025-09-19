using Application;
using Application.Dto;
using Application.Interfaces;
using Infraestructure;
using Infraestructure.Seeds;
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

//Conexion con db
//builder.Services.AddDbContext<EfAppDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection")));
var provider = builder.Configuration.GetValue<string>("Database:Provider")??"SqlServer";

if (provider.Equals("Sqlite",StringComparison.OrdinalIgnoreCase))
{
    builder.Services.AddDbContext<EfAppDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection")));
}else
{
    builder.Services.AddDbContext<EfAppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection")));
}

//Repos
builder.Services.AddScoped<IActivoRepository, EfActivoRepository>();
builder.Services.AddScoped<IOrdenRepository, EfOrdenRepository>();
builder.Services.AddScoped<IEstadoRepository, EfEstadoRepository>();
builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork>();

//Servicios
builder.Services.AddScoped<IActivoService,ActivoService>();
builder.Services.AddScoped<IOrdenService, OrdenService>();
builder.Services.AddScoped<IEstadoService,EstadoService>();
builder.Services.AddScoped<ICalculoTotal,CalculoFciHandler>();
builder.Services.AddScoped<ICalculoTotal, CalculoBonoHandler>();
builder.Services.AddScoped<ICalculoTotal, CalculoAccionHandler>();
builder.Services.AddScoped<CalculoOrdenFactory>();

// Seeder
builder.Services.AddScoped<DataSeeder>();

var app = builder.Build();

using (var scope = app.Services.CreateScope()) 
{ 
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<EfAppDbContext>();
    var cfg = services.GetRequiredService<IConfiguration>();
    var env = services.GetRequiredService<IWebHostEnvironment>();
    if (provider.Equals("Sqlite", StringComparison.OrdinalIgnoreCase)) 
    {
        var pending = (await db.Database.GetPendingMigrationsAsync()).Any();
        if (pending)
            await db.Database.MigrateAsync();
        else
            await db.Database.EnsureCreatedAsync();

        if (cfg.GetValue<bool>("Seed:Enabled"))
        {
            var seeder = services.GetRequiredService<DataSeeder>();
            await seeder.SeedAsync(env.ContentRootPath, CancellationToken.None);
        }
    }

}

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
