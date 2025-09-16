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
        private Int32 _idCuenta;
        private String _activo;
        private Int32 _cantidad;
        private Decimal _precio;
        private Char _operacion;
        private Int16 _estado;
        private Decimal _montoTotal;

        public Orden(int idCuenta, string activo, int cantidad, decimal precio, char operacion, short estado, decimal montoTotal)
        {
            _idCuenta = idCuenta;
            _activo = activo;
            _cantidad = cantidad;
            _precio = precio;
            _operacion = operacion;
            _estado = estado;
            _montoTotal = montoTotal;
        }

        public int Id { get => _id; set => _id = value; }
        public int IdCuenta { get => _idCuenta; set => _idCuenta = value; }
        public int Cantidad { get => _cantidad; set => _cantidad = value; }
        public decimal Precio { get => _precio; set => _precio = value; }
        public char Operacion { get => _operacion; set => _operacion = value; }
        public decimal MontoTotal { get => _montoTotal; set => _montoTotal = value; }
        internal String Activo { get => _activo; set => _activo = value; }
        internal Int16 Estado { get => _estado; set => _estado = value; }
    }
}
