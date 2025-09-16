using Application.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class ActivoService : IActivoService
    {
        private readonly IActivoRepository _repository;
        public  Activo GetByTickerAsync(string nombre, CancellationToken ct)
        {
            return  _repository.findbyName(nombre,ct);
        }
    }
}
