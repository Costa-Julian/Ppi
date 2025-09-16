namespace Domain
{
    public class Estado
    {
        private Int32 _id;
        private String _descripcionEstado;

        public Estado()
        {
        }

        public Estado(int id, string descripcionEstado)
        {
            _id = id;
            _descripcionEstado = descripcionEstado;
        }

        public string Nombre { get => _descripcionEstado; set => _descripcionEstado = value; }
        public int Id { get => _id; set => _id = value; }
    }
}