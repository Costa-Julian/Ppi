using Application.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class EstadoService : IEstadoService
    {
        private readonly IEstadoRepository _repository;

        public EstadoService(IEstadoRepository repository)
        {
            _repository = repository;
        }

        public Estado Get(int id, CancellationToken ct)
        {
            return _repository.findById(id, ct);
        }

        public Task<List<Estado>> GetAll(CancellationToken ct)
        {
            return _repository.findAll(ct);
        }
    }
}
