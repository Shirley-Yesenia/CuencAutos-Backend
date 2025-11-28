using AccesoDatos.DTO;
using Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace API_REST_INTEGRACION.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/v2/integracion/autos")]
    public class BuscarAutosV2Controller : ApiController
    {
        private readonly VehiculoDatos _vehiculos = new VehiculoDatos();
        private readonly ImagenVehiculoDatos _imagenes = new ImagenVehiculoDatos();

        // ================================================================
        // GET: /api/v2/integracion/autos/search
        // ================================================================
        [HttpGet]
        [Route("search", Name = "BuscarAutos2")]
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
                // ================================================================
                // LISTA DE PAÍSES PERMITIDOS
                // ================================================================
                var paisesValidos = new[] { "Ecuador", "Estados Unidos" };

                if (!string.IsNullOrEmpty(pais) && !paisesValidos.Contains(pais))
                {
                    return Ok(new
                    {
                        Data = new List<object>(),
                        Links = new List<object>
                        {
                            new {
                                Rel = "self",
                                Href = Request.RequestUri.ToString(),
                                Method = "GET"
                            }
                        }
                    });
                }

                // ================================================================
                // FILTRO DE AUTOS
                // ================================================================
                var listaVehiculos = _vehiculos.Listar()
                    .Where(v =>
                        (v.Estado ?? "") == "Disponible" &&
                        (string.IsNullOrEmpty(categoria) || (v.CategoriaNombre ?? "").Contains(categoria)) &&
                        (string.IsNullOrEmpty(transmision) || (v.TransmisionNombre ?? "").Contains(transmision)) &&
                        (!capacidad.HasValue || v.Capacidad == capacidad) &&
                        (!precio_min.HasValue || (v.PrecioDia >= precio_min)) &&
                        (!precio_max.HasValue || (v.PrecioDia <= precio_max)) &&
                        (string.IsNullOrEmpty(ciudad) || (v.SucursalNombre ?? "").Contains(ciudad)) &&
                        (string.IsNullOrEmpty(pais) || (v.SucursalPais ?? "").Equals(pais))
                    )
                    .ToList();

                // ================================================================
                // ORDENAMIENTO
                // ================================================================
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

                // ================================================================
                // MAPEAR A DTO
                // ================================================================
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
                        Pais = v.SucursalPais ?? "No especificado"   // ✅ CORREGIDO
                    };
                }).ToList();

                // ================================================================
                // HATEOAS GLOBAL
                // ================================================================
                var wrapper = new
                {
                    Data = listaFinal,
                    Links = new List<object>
                    {
                        new
                        {
                            Rel = "self",
                            Href = Request.RequestUri.ToString(),
                            Method = "GET"
                        },
                        new
                        {
                            Rel = "crear_prereserva_auto",
                            Href = Url.Link("CrearPreReservaAuto", null),
                            Method = "POST"
                        }
                    }
                };

                return Ok(wrapper);
            }
            catch (Exception ex)
            {
                return InternalServerError(
                    new Exception("Error al buscar autos: " + ex.Message)
                );
            }
        }
    }
}
