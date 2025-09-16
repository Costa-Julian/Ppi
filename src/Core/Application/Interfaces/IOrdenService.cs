using Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    internal interface IOrdenService
    {
        Task<int> CreateOrderAsynck(DtoCreateOrden dtoCreateOrden, CancellationToken ct);

    }
}
