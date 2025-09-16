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

        public OrdenService(IOrdenRepository ordenRepository, IActivoService activoService, CalculoOrdenFactory calculatorFactory, IUnitOfWork unitOfWork)
        {
            _ordenRepository = ordenRepository;
            _activoService = activoService;
            _calculatorFactory = calculatorFactory;
            _unitOfWork = unitOfWork;
        }

        public  async Task<int> CreateOrderAsynck(OrdenRequest Or, CancellationToken ct)
        {
            Orden? orden = null;

            await _unitOfWork.ExecuteInTransactionAsync(async cancelationToken =>
            {
                Activo activo =  _activoService.GetByTickerAsync(Or.NombreActivo, cancelationToken)
                ?? throw new KeyNotFoundException("Activo no encontrado");
                var modoCalculoTotal = _calculatorFactory.Resolve(activo);

                if (activo.TipoActivo != 1) { activo.PrecioUnitario = (decimal)Or.Precio; }

                var total = modoCalculoTotal.CalculoTotal(activo, Or.Cantidad);

                orden = new Orden(Or.CuentaId, activo.Ticker, Or.Cantidad,activo.PrecioUnitario, Or.Operacion, 0, total);

                await _ordenRepository.AddOrdenAsync(orden, ct);
            }, ct);

            return orden.Id;
        }
    }
}
