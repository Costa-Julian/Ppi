using Application.Dto;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IOrdenService
    {
        Task<int> CreateOrderAsynck(OrdenRequest Or, CancellationToken ct);
        Task<List<DtoOrdenResponse>> GetAll(CancellationToken ct);
        Task<Orden> GetById(int id, CancellationToken ct);
        Task UpdateOrdenEstado(int id, int estado, CancellationToken ct);
    }
}
