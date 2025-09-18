using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    /// <summary>
    /// Solicitud de creadion de orden.
    /// </summary>
    public class OrdenRequest
    {
        /// <example>123</example>
        [Required]public int CuentaId { get; set; }
        /// <summary>
        /// Ticker del activo.
        /// </summary>
        /// <example>AAPL</example>
        [Required,StringLength(32)]public string NombreActivo { get; set; }
        /// <summary>'C' compra, 'V' venta.</summary>
        /// <example>C</example>
        [Required]public char Operacion { get; set; }
        /// <example>5</example>
        [Required, Range(1,int.MaxValue)] public int Cantidad { get; set; }
        /// <summary>Requerido para Bono/FCI; nulo para Acción.</summary>
        /// <example>102.75</example>
        public decimal? Precio { get; set; }
    }
}
