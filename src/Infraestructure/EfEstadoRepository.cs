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
    public class EfEstadoRepository : IEstadoRepository
    {
        private readonly EfAppDbContext _dbContext;

        public EfEstadoRepository(EfAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<Estado>> findAll(CancellationToken ct)
        {
            return _dbContext.Estados.AsNoTracking().ToListAsync();
        }

        public Estado findById(int id, CancellationToken ct)
        {
            return _dbContext.Estados.Where(e => e.Id == id).FirstOrDefault();
        }
    }
}
