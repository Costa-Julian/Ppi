using Application.Interfaces;
using Domain;
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
        public async Task AddOrdenAsync(Orden orden, CancellationToken ct) 
        { 
            await _dbContext.ordens.AddAsync(orden, ct);
        }
        
        public void DeleteOrden(Orden orden,CancellationToken ct) => _dbContext?.ordens.Remove(orden);

 
        public void DeleteOrdenById(int id, CancellationToken ct)
        {
           Orden orden = _dbContext?.ordens.FirstOrDefault(o => o.Id == id);
            if (orden != null) 
                {
                    DeleteOrden(orden, ct); 
            }
        }

        public List<Orden> GetAll()
        {
            return _dbContext.ordens.ToList();
        }

        public void UpdateOrden(Orden orden, CancellationToken ct )
        {
            _dbContext.ordens.AddAsync(orden, ct);
        }
    }
}
