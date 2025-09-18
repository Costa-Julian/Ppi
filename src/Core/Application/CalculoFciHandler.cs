using Application.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class CalculoFciHandler : ICalculoTotal
    {
        public bool CanHandle(Activo activo) => activo is Fci;
        public decimal CalculoTotal(Activo activo, int cantidad)
        {
           return activo.PrecioUnitario * cantidad;
        }
    }
}
