using AccesoDatos.DTO;
using Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace WS_Integracion_Servicios
{
    [WebService(Namespace = "http://rentaautos.ec/integracion")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WS_BuscarAutos : WebService
    {
        private readonly VehiculoDatos _vehiculos = new VehiculoDatos();
        private readonly ImagenVehiculoDatos _imagenes = new ImagenVehiculoDatos();

        // ================================================================
        // 🔹 MÉTODO SOAP: buscarAutos
        // ================================================================
        [WebMethod(Description = "Devuelve autos disponibles según filtros opcionales (modo SOAP).")]
        public List<AutoBookingDto> buscarAutos(
            string categoria = null,
            string transmision = null,
            int? capacidad = null,
            decimal? precio_min = null,
            decimal? precio_max = null,
            string sort = null,
            string ciudad = null,
            string pais = null)
        {
            try
            {
                // ====================================================
                // ✔ Validación de países permitidos (Booking requirement)
                // ====================================================
                var paisesValidos = new[] { "Ecuador", "Estados Unidos" };

                if (!string.IsNullOrEmpty(pais) && !paisesValidos.Contains(pais))
                {
                    // País inválido → devolver lista vacía
                    return new List<AutoBookingDto>();
                }

                // ====================================================
                // ✔ Filtros principales
                // ====================================================
                var listaVehiculos = _vehiculos.Listar()
                    .Where(v =>
                        (v.Estado ?? "") == "Disponible" &&
                        (string.IsNullOrEmpty(categoria) || (v.CategoriaNombre ?? "").Contains(categoria)) &&
                        (string.IsNullOrEmpty(transmision) || (v.TransmisionNombre ?? "").Contains(transmision)) &&
                        (!capacidad.HasValue || v.Capacidad == capacidad) &&
                        (!precio_min.HasValue || v.PrecioDia >= precio_min) &&
                        (!precio_max.HasValue || v.PrecioDia <= precio_max) &&
                        (string.IsNullOrEmpty(ciudad) || (v.SucursalNombre ?? "").Contains(ciudad)) &&
                        (string.IsNullOrEmpty(pais) || (v.SucursalPais ?? "").Equals(pais))
                    )
                    .ToList();

                // ====================================================
                // ✔ Ordenamiento
                // ====================================================
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

                // ====================================================
                // ✔ Mapeo final al DTO
                // ====================================================
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
                        Pais = v.SucursalPais ?? "No especificado"   // ← ✔ CORREGIDO
                    };
                }).ToList();

                return listaFinal;
            }
            catch (Exception ex)
            {
                throw new SoapException("Error al buscar autos: " + ex.Message,
                    System.Xml.XmlQualifiedName.Empty);
            }
        }
    }

    // ================================================================
    // DTO igual que REST
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
