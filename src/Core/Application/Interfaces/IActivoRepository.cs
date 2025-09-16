using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IActivoRepository
    {
        Activo findbyName(String nombre,CancellationToken ct);
        Task<List<Activo>> findall(CancellationToken ct);
    }
}
