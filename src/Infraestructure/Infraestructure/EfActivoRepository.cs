using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure
{
    public class EfActivoRepository : IActivoRepository
    {
        private readonly EfAppDbContext _dbContext;

        public EfActivoRepository(EfAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<Activo>> findall(CancellationToken ct)
        {
            return _dbContext.Activos.AsNoTracking().ToListAsync(ct);
        }

        public  Activo findbyName(string nombre, CancellationToken ct)
        {
            return  _dbContext.Activos.Where(a => a.Ticker == nombre.Trim()).FirstOrDefault();
        }
    }
}
