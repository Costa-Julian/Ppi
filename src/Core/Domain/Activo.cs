namespace Domain
{
    public  class Activo
    {
        private int _id;
        private String _ticker;
        private String _nombre;
        private Int32 _tipoActivo;
        private decimal _precioUnitario;

        public Activo() { }
        public Activo(string ticker, string nombre, decimal precioUnitario)
        {
            Ticker = ticker;
            Nombre = nombre;
            PrecioUnitario = precioUnitario;
        }

        public string Ticker { get => _ticker; set => _ticker = value; }
        public string Nombre { get => _nombre; set => _nombre = value; }
        public int TipoActivo { get => _tipoActivo; set => _tipoActivo = value; }
        public decimal PrecioUnitario { get => _precioUnitario; set => _precioUnitario = value; }
        public int Id { get => _id; set => _id = value; }
    }
}