namespace AccesoDatos.DTO
{
    public class VehiculoSimpleDto
    {
        public int IdVehiculo { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Anio { get; set; }
        public int IdCategoria { get; set; }
        public string CategoriaNombre { get; set; }
        public int IdTransmision { get; set; }
        public string TransmisionNombre { get; set; }
        public int Capacidad { get; set; }
        public decimal PrecioDia { get; set; }
        public decimal PrecioNormal { get; set; }
        public decimal? PrecioActual { get; set; }
        public string Matricula { get; set; }
        public int IdSucursal { get; set; }
        public string SucursalNombre { get; set; }
        public string UrlImagen { get; set; }
    }
}
