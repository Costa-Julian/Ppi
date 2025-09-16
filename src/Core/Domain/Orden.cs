using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Orden
    {
        private Int32 _id;
        private Int32 _CuentaId;
        private String _nombreActivo;
        private Int32 _cantidad;
        private Decimal _precio;
        private Char _operacion;
        private Int16 _estadoId;
        private Decimal _montoTotal;
        private Activo activo;
        private Estado estado;

       
        public Orden() { }

        public Orden(int idCuenta, string nombreActivo, int cantidad, decimal precio, char operacion, short estadoId, decimal montoTotal)
        {
            _CuentaId = idCuenta;
            _nombreActivo = nombreActivo;
            _cantidad = cantidad;
            _precio = precio;
            _operacion = operacion;
            _estadoId = estadoId;
            _montoTotal = montoTotal;

        }

        public int Id { get => _id; set => _id = value; }
        public int CuentaId { get => _CuentaId; set => _CuentaId = value; }
        public string NombreActivo { get => _nombreActivo; set => _nombreActivo = value; }
        public int Cantidad { get => _cantidad; set => _cantidad = value; }
        public decimal Precio { get => _precio; set => _precio = value; }
        public char Operacion { get => _operacion; set => _operacion = value; }
        public short EstadoId { get => _estadoId; set => _estadoId = value; }
        public decimal MontoTotal { get => _montoTotal; set => _montoTotal = value; }
    }
}
