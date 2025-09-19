using Application.Dto;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infraestructure.Seeds
{
    public class DataSeeder
    {
        private readonly EfAppDbContext _db;
        private readonly ILogger<DataSeeder> _log;

        public DataSeeder(EfAppDbContext db, ILogger<DataSeeder> log)
        {
            _db = db;
            _log = log;
        }
        public async Task SeedAsync(string contentRootPath, CancellationToken ct)
        {
            // contentRootPath = ...\src\Presentation\WebApi
            var content = new DirectoryInfo(contentRootPath);
            var presentationDir = content.Parent;          // ...\src\Presentation
            var srcDir = presentationDir?.Parent; // ...\src

            var seedsDir = (srcDir is null)
                ? Path.Combine(contentRootPath, "src", "Infrastructure", "Seeds")
                : Path.Combine(srcDir.FullName, "Infrastructure", "Seeds");

            seedsDir = Path.GetFullPath(seedsDir);
            _log.LogInformation("Seeds directory: {dir}", seedsDir);

            var estadosPath = Path.Combine(seedsDir, "Estados.json");
            var activosPath = Path.Combine(seedsDir, "Activos.json");

            LogExists(estadosPath, "ESTADOS");
            LogExists(activosPath, "ACTIVOS");

            await SeedEstadosAsync(estadosPath, ct);
            await SeedActivosAsync(activosPath, ct);
        }

        private void LogExists(string path, string label)
        {
            if (File.Exists(path))
                _log.LogInformation("{label} seed found at: {path}", label, path);
            else
                _log.LogWarning("{label} seed NOT FOUND at: {path}", label, path);
        }

        //public async Task SeedAsync(string contentRootPath, CancellationToken ct)
        //{
        //    await SeedEstadosAsync(contentRootPath, ct);
        //    await SeedActivosAsync(contentRootPath, ct);
        //}
        private async Task SeedEstadosAsync(string root, CancellationToken ct)
        {
            var path = Path.Combine(root, "src", "Infrastructure", "Seeds", "estados.json");
            if (!File.Exists(path)) { _log.LogWarning("Seed file not found: {path}", path); return; }

            var estados = JsonSerializer.Deserialize<List<EstadoDto>>(
                await File.ReadAllTextAsync(path, ct),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            ) ?? new();

            foreach (var e in estados)
            {
                var entity = await _db.Estados.FirstOrDefaultAsync(x => x.Id == e.Id, ct);
                if (entity == null)
                    _db.Estados.Add(new Estado { Id = e.Id, DescripcionEstado = e.Nombre ?? "" });
                else
                    entity.DescripcionEstado = e.Nombre ?? entity.DescripcionEstado;
            }

            await _db.SaveChangesAsync(ct);
        }
        private async Task SeedActivosAsync(string root, CancellationToken ct)
        {
            var path = Path.Combine(root, "src", "Infrastructure", "Seeds", "activos.json");
            if (!File.Exists(path)) { _log.LogWarning("Seed file not found: {path}", path); return; }

            var activos = JsonSerializer.Deserialize<List<ActivoDto>>(
                await File.ReadAllTextAsync(path, ct),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            ) ?? new();

            foreach (var a in activos)
            {
                var exists = await _db.Activos.AnyAsync(x => x.Ticker == a.Ticker, ct);
                if (exists) continue;

                Activo nuevo = a.TipoActivoId switch
                {
                    1 => new Accion(a.Ticker!, a.Nombre ?? "", a.PrecioUnitario),
                    2 => new Bono(a.Ticker!, a.Nombre ?? "", a.PrecioUnitario),
                    3 => new Fci(a.Ticker!, a.Nombre ?? "", a.PrecioUnitario),
                    _ => throw new InvalidOperationException($"TipoActivoId inválido: {a.TipoActivoId}")
                };

                _db.Activos.Add(nuevo);
            }

            await _db.SaveChangesAsync(ct);
        }
    }
}
