using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public class ActivoDto
    {
        public string? Ticker { get; set; }
        public string? Nombre { get; set; }
        public int TipoActivoId { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}
