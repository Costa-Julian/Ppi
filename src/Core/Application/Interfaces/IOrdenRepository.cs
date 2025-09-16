
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IOrdenRepository
    {
        List<Orden> GetAll();
        Task AddOrdenAsync(Orden orden,CancellationToken ct);
        void UpdateOrden(Orden orden, CancellationToken ct);
        void DeleteOrden(Orden orden, CancellationToken ct);
        void DeleteOrdenById(int id, CancellationToken ct);

    }
}
