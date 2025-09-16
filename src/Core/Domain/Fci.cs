using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Fci : Activo
    {
        public Fci() { }
        public Fci(string ticker, string nombre, decimal PrecioUnitario) : base(ticker, nombre, PrecioUnitario)
        {
            this.TipoActivo = 3;
        }
    }
}
