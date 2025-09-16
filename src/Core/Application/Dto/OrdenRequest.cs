using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public class OrdenRequest
    {
        [Required]public int CuentaId { get; set; }
        [Required,StringLength(32)]public string NombreActivo { get; set; }
        [Required]public char Operacion { get; set; }
        [Required, Range(1,int.MaxValue)] public int Cantidad { get; set; }
        public decimal? Precio { get; set; }
    }
}
