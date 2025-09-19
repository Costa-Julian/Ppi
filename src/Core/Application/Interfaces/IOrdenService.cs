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
        Task<int> CreateOrderAsync(OrdenRequest Or, CancellationToken ct);
        Task<List<OrdenResponseDto>> GetAll(CancellationToken ct);
        Task<OrdenResponseDto> GetById(int id, CancellationToken ct);
        Task UpdateOrdenEstado(int id, int estado, CancellationToken ct);
        Task DeleteOrden(int id, CancellationToken ct = default);
    }
}
