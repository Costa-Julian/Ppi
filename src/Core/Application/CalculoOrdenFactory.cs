using Application.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class CalculoOrdenFactory
    {
        private readonly List<ICalculoTotal> _strategies;

        public CalculoOrdenFactory(List<ICalculoTotal> strategies)
        {
            _strategies = strategies.ToList();
        }
        public ICalculoTotal Resolve(Activo activo)
        {
            return _strategies.FirstOrDefault(s => s.CanHandle(activo))
            ?? throw new NotSupportedException($"No existe accion dada para {activo.GetType().Name}");
        }
    }
}
