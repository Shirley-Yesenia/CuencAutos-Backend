using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;   // 🔥 AGREGADO PARA CORS
using AccesoDatos.DTO;
using Datos;

namespace API_REST_INTEGRACION.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]   // 🔥 CORS HABILITADO
    [RoutePrefix("api/v1/integracion/autos")]
    public class IntegracionAutosController : ApiController
    {
        private readonly VehiculoDatos _vehiculos = new VehiculoDatos();
        private readonly ImagenVehiculoDatos _imagenes = new ImagenVehiculoDatos();

        // ================================================================
        // 🔹 GET: /api/integracion/autos/search
        // ================================================================
        [HttpGet]
        [Route("search")]
        public IHttpActionResult BuscarAutos(
            string categoria = "",
            string transmision = "",
            int? capacidad = null,
            decimal? precio_min = null,
            decimal? precio_max = null,
            string sort = "",
            string ciudad = "",
            string pais = ""
        )
        {
            try
            {
                var listaVehiculos = _vehiculos.Listar()
                    .Where(v =>
                        (v.Estado ?? "") == "Disponible" &&
                        (string.IsNullOrEmpty(categoria) || (v.CategoriaNombre ?? "").Contains(categoria)) &&
                        (string.IsNullOrEmpty(transmision) || (v.TransmisionNombre ?? "").Contains(transmision)) &&
                        (!capacidad.HasValue || v.Capacidad == capacidad) &&
                        (!precio_min.HasValue || (v.PrecioDia >= precio_min)) &&
                        (!precio_max.HasValue || (v.PrecioDia <= precio_max)) &&
                        (string.IsNullOrEmpty(ciudad) || (v.SucursalNombre ?? "").Contains(ciudad))
                    )
                    .ToList();

                // 🔸 Ordenamiento
                if (!string.IsNullOrEmpty(sort))
                {
                    switch (sort.ToLower())
                    {
                        case "precio_asc":
                            listaVehiculos = listaVehiculos.OrderBy(v => v.PrecioDia).ToList();
                            break;
                        case "precio_desc":
                            listaVehiculos = listaVehiculos.OrderByDescending(v => v.PrecioDia).ToList();
                            break;
                    }
                }

                // 🔸 DTO final (con ciudad y país)
                var listaFinal = listaVehiculos.Select(v =>
                {
                    var img = _imagenes.ListarPorVehiculo(v.IdVehiculo).FirstOrDefault();

                    return new AutoBookingDto
                    {
                        IdAuto = v.IdVehiculo.ToString(),
                        Tipo = $"{v.Marca} {v.Modelo} {(v.CategoriaNombre ?? "")}".Trim(),
                        Capacidad = v.Capacidad,
                        PrecioNormal = v.PrecioDia,
                        PrecioActual = null,
                        UriImagen = img?.UriImagen,
                        Ciudad = v.SucursalNombre ?? "No especificada",
                        Pais = "Ecuador"
                    };
                }).ToList();

                return Ok(listaFinal);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Error al buscar autos: " + ex.Message));
            }
        }
    }

    // ================================================================
    // 🔸 DTO compatible con Booking
    // ================================================================
    public class AutoBookingDto
    {
        public string IdAuto { get; set; }
        public string Tipo { get; set; }
        public int Capacidad { get; set; }
        public decimal PrecioNormal { get; set; }
        public decimal? PrecioActual { get; set; }
        public string UriImagen { get; set; }
        public string Ciudad { get; set; }
        public string Pais { get; set; }
    }
}
