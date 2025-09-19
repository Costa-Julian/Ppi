using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public class EstadoDto
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }

        public EstadoDto(int id, string? nombre)
        {
            Id = id;
            Nombre = nombre;
        }
    }
}
