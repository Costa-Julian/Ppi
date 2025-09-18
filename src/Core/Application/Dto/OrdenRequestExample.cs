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
        public OrdenRequest GetExamples() => new
        (
            cuentaId: 123,
            nombreActivo: "ABCD",
            operacion: Char.Parse("C"),
            cantidad: 5,
            precio: null
        );
    }
}
