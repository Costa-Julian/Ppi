using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public class DtoOrdenResponse
    {
        private Int32 cuentaId;
        private String nombreActivo;
        private Int32 cantidad;
        private Decimal precio;
        private Char operacion;
        private String estado;
        private Decimal montoTotal;

        public DtoOrdenResponse(int cuentaId, string nombreActivo, int cantidad, decimal precio, char operacion, string estado, decimal montoTotal)
        {
            CuentaId = cuentaId;
            NombreActivo = nombreActivo;
            Cantidad = cantidad;
            Precio = precio;
            Operacion = operacion;
            Estado = estado;
            MontoTotal = montoTotal;
        }

        public int CuentaId { get => cuentaId; set => cuentaId = value; }
        public string NombreActivo { get => nombreActivo; set => nombreActivo = value; }
        public int Cantidad { get => cantidad; set => cantidad = value; }
        public decimal Precio { get => precio; set => precio = value; }
        public char Operacion { get => operacion; set => operacion = value; }
        public decimal MontoTotal { get => montoTotal; set => montoTotal = value; }
        public string Estado { get => estado; set => estado = value; }
    }
}
