using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICalculoTotal
    {
        bool CanHandle(Activo activo);
        Decimal CalculoTotal(Activo activo, int cantidad);
    }
}
