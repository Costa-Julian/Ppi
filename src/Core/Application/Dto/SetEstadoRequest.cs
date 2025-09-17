using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public class SetEstadoRequest
    {
        public int estadoId { get; set; }

        public SetEstadoRequest(int estadoId)
        {
            this.estadoId = estadoId;
        }
    }
}
