using Application.Dto;
using Application.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class OrdenService : IOrdenService
    {
        private readonly IOrdenRepository _ordenRepository;
        private readonly IActivoService _activoService;
        private readonly CalculoOrdenFactory _calculatorFactory;
        private readonly IUnitOfWork _unitOfWork;
        public  async Task<int> CreateOrderAsynck(DtoCreateOrden dtoCreateOrden, CancellationToken ct)
        {
            Orden? orden = null;

            await _unitOfWork.ExecuteInTransactionAsync(async cancelationToken =>
            {
                Activo activo =  _activoService.GetByTickerAsync(dtoCreateOrden.NombreActivo, cancelationToken)
                ?? throw new KeyNotFoundException("Activo no encontrado");
                var modoCalculoTotal = _calculatorFactory.Resolve(activo);

                if (activo.TipoActivo != 1) { activo.PrecioUnitarios = dtoCreateOrden.Precio; }

                var total = modoCalculoTotal.CalculoTotal(activo, dtoCreateOrden.Cantidad);

                orden = new Orden(dtoCreateOrden.IdCuenta, activo.Nombre, dtoCreateOrden.Cantidad, activo.PrecioUnitarios,dtoCreateOrden.Operacion,0,total);

                await _ordenRepository.AddOrdenAsync(orden, ct);
            }, ct);

            return orden.Id;
        }
    }
}
