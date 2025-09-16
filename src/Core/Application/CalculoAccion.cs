using Application.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    internal class CalculoAccion : ICalculoTotal
    {
        public bool CanHandle(Activo activo) => activo is Accion;
        public decimal CalculoTotal(Activo activo, int cantidad)
        {
            var totalTemporal = activo.PrecioUnitarios * cantidad;
            var comision = Math.Round(totalTemporal * Accion.COMISION, 2, MidpointRounding.ToEven);
            var impuesto = Math.Round(comision * Accion.IMPUESTO, 2, MidpointRounding.ToEven);
            return totalTemporal + comision + impuesto;
        }
    }
}
