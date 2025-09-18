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
        private readonly IEstadoService _estadoService;
        private readonly CalculoOrdenFactory _calculatorFactory;
        private readonly IUnitOfWork _unitOfWork;

        public OrdenService(IOrdenRepository ordenRepository, IActivoService activoService, CalculoOrdenFactory calculatorFactory, IUnitOfWork unitOfWork, IEstadoService estadoService)
        {
            _ordenRepository = ordenRepository;
            _activoService = activoService;
            _calculatorFactory = calculatorFactory;
            _unitOfWork = unitOfWork;
            _estadoService = estadoService;
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

        public async Task DeleteOrden(int id, CancellationToken ct = default)
        {
            await _unitOfWork.ExecuteInTransactionAsync(async canceletionToken => 
            {
                var orden = _ordenRepository.FindById(id, ct);
                if (orden is null) throw new KeyNotFoundException();
                _ordenRepository.DeleteOrden(orden, ct);
            }, ct);
        }

        public async Task<List<DtoOrdenResponse>> GetAll(CancellationToken ct)
        {
            List<DtoOrdenResponse> dtoOrdenResponses = new List<DtoOrdenResponse>();
            List<Orden> ordens = new List<Orden>();

            await _unitOfWork.ExecuteInTransactionAsync(async canelationToken =>
            {
                 ordens = _ordenRepository.GetAll();
            }, ct);

            foreach (var orden in ordens)
            {
                DtoOrdenResponse or = new DtoOrdenResponse(orden.CuentaId, orden.NombreActivo, orden.Cantidad,
                    orden.Precio, orden.Operacion, _estadoService.Get(orden.EstadoId, ct).DescripcionEstado, orden.MontoTotal);
                dtoOrdenResponses.Add(or);
            }
            return dtoOrdenResponses;
        }

        public Task<DtoOrdenResponse> GetById(int id, CancellationToken ct)
        {
            var orden = _ordenRepository.FindById(id, ct);
            if (orden is null) throw new KeyNotFoundException();
            DtoOrdenResponse ordenRequest = new DtoOrdenResponse(orden.CuentaId, orden.NombreActivo, orden.Cantidad,
                    orden.Precio, orden.Operacion, _estadoService.Get(orden.EstadoId, ct).DescripcionEstado, orden.MontoTotal);
            return  Task.FromResult(ordenRequest);
        }

        public async Task UpdateOrdenEstado(int id, int estadoid, CancellationToken ct)
        {
           await _unitOfWork.ExecuteInTransactionAsync(async ct2 =>
            {
                Estado estado = _estadoService.Get(estadoid, ct)
                    ?? throw new ArgumentException("Estado inválido", nameof(estadoid));

                Orden orden = _ordenRepository.FindById(id)
                ?? throw new KeyNotFoundException("Orden no encontrada");

                if (orden.EstadoId == estadoid)
                    throw new InvalidOperationException(
                        $"La orden {orden.Id} ya se encuentra en el estado solicitado (Estado= {estado.DescripcionEstado}); la operación no produce cambios.");
                orden.EstadoId = estadoid;
                 _ordenRepository.UpdateOrden(orden,ct2);
            },ct);
        }
    }
}
