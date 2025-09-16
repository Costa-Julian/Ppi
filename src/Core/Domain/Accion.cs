using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Accion : Activo
    {
        private const Decimal _COMISION = 0.06m;
        private const Decimal _IMPUESTO = 0.21m;

        public Accion()
        {
        }

        public Accion(string ticker, string nombre, decimal PrecioUnitario) : base(ticker, nombre, PrecioUnitario)
        {
            this.TipoActivo = 1;
        }

        public static decimal COMISION => _COMISION;

        public static decimal IMPUESTO => _IMPUESTO;
    }
}
