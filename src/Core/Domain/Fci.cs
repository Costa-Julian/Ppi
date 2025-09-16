using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Fci : Activo
    {
        public Fci(string ticker, string nombre, decimal precioUnitarios) : base(ticker, nombre, precioUnitarios)
        {
            this.TipoActivo = 3;
        }
    }
}
