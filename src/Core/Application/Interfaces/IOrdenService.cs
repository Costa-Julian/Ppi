using Application.Dto;
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

    }
}
