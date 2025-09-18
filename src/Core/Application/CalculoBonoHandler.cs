using Application.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class CalculoBonoHandler : ICalculoTotal
    {
        public bool CanHandle(Activo activo) => activo is Bono;
        public decimal CalculoTotal(Activo activo, int cantidad)
        {
            var totalTemporal = activo.PrecioUnitario * cantidad;
            var comision = Math.Round(totalTemporal * Bono.COMISION, 2, MidpointRounding.ToEven);
            var impuesto = Math.Round(comision * Bono.IMPUESTO, 2, MidpointRounding.ToEven);
            return totalTemporal + comision + impuesto;
        }

       
    }
}
