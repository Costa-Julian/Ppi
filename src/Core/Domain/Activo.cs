namespace Domain
{
    public abstract class Activo
    {
        private Int32 _id;
        private String _ticker;
        private String _nombre;
        private Int16 _tipoActivo;
        private decimal _precioUnitarios;

        protected Activo(string ticker, string nombre, decimal precioUnitarios)
        {
            Ticker = ticker;
            Nombre = nombre;
            PrecioUnitarios = precioUnitarios;
        }

        public string Ticker { get => _ticker; set => _ticker = value; }
        public string Nombre { get => _nombre; set => _nombre = value; }
        public short TipoActivo { get => _tipoActivo; set => _tipoActivo = value; }
        public decimal PrecioUnitarios { get => _precioUnitarios; set => _precioUnitarios = value; }
    }
}