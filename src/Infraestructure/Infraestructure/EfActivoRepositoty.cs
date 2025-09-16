using Application.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure
{
    public class EfActivoRepositoty : IActivoRepository
    {
        private readonly EfAppDbContext _dbContext;
        public  Activo findbyName(string nombre, CancellationToken ct)
        {
            return  _dbContext.activos.Where(a => a.Ticker == nombre.Trim()).FirstOrDefault();
        }
    }
}
