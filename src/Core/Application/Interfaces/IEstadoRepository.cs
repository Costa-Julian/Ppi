using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IEstadoRepository
    {
        Task<List<Estado>> findAll(CancellationToken ct);
        Estado findById(int id, CancellationToken ct);
    }
}
