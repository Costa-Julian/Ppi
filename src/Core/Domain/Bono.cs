using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Bono : Activo
    {
        private const Decimal _COMISION = 0.02m;
        private const Decimal _IMPUESTO = 0.21m;

        public Bono()
        {
        }

        public Bono(string ticker, string nombre, decimal PrecioUnitario) : base(ticker, nombre, PrecioUnitario)
        {
            this.TipoActivo = 2;
        }

        public static decimal COMISION => _COMISION;

        public static decimal IMPUESTO => _IMPUESTO;
    }
}
