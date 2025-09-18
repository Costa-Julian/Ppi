using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Filters;

namespace Application.Dto
{
    public class OrdenRequestExample : IExamplesProvider<OrdenRequest>
    {
        public OrdenRequest GetExamples() => new()
        {
            CuentaId = 123,
            NombreActivo = "ABCD",
            Operacion = Char.Parse("C"),
            Cantidad = 5,
            Precio = null 
        };
    }
}
