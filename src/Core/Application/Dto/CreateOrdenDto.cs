using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public class CreateOrdenDto
    {
        private int _CuentaId;
        private String _NombreActivo;
        private int _Cantidad;
        private Nullable<Decimal> _Precio;
        private Char _Operacion;

        public CreateOrdenDto(int idCuenta, string nombreActivo, int cantidad, char operacion)
        {
            _CuentaId = idCuenta;
            _NombreActivo = nombreActivo;
            _Cantidad = cantidad;
            _Precio = null;
            _Operacion = operacion;
        }

        public int IdCuenta { get => _CuentaId; set => _CuentaId = value; }
        public string NombreActivo { get => _NombreActivo; set => _NombreActivo = value; }
        public int Cantidad { get => _Cantidad; set => _Cantidad = value; }
        public Decimal Precio { get => (decimal)_Precio; set => _Precio = value; }
        public Char Operacion { get => _Operacion;set => _Operacion = value; }
    }
}
