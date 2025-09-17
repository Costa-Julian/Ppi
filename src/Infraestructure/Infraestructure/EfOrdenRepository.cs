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
    public class EfOrdenRepository : IOrdenRepository
    {
        private readonly EfAppDbContext _dbContext;

        public EfOrdenRepository(EfAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddOrdenAsync(Orden orden, CancellationToken ct) 
        { 
            await _dbContext.Ordenes.AddAsync(orden, ct);
        }
        
        public void DeleteOrden(Orden orden,CancellationToken ct) => _dbContext?.Ordenes.Remove(orden);

 
        public void DeleteOrdenById(int id, CancellationToken ct)
        {
           Orden orden = _dbContext?.Ordenes.FirstOrDefault(o => o.Id == id);
            if (orden != null) 
                {
                    DeleteOrden(orden, ct); 
            }
        }

        public Orden FindById(int id)
        {
            return _dbContext.Ordenes.Where(o => o.Id == id).FirstOrDefault();
        }

        public List<Orden> GetAll()
        {
            return _dbContext.Ordenes.ToList();
        }

        public void UpdateOrden(Orden orden,CancellationToken ct)
        {
            _dbContext.Ordenes.Update(orden);
        }
    }
}
