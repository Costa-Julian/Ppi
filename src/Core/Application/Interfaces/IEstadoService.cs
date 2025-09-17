using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IEstadoService
    {
        Task<List<Estado>> GetAll(CancellationToken ct);
        Estado Get(int id,CancellationToken ct);
    }
}
