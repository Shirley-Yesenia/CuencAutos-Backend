using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;   // ← NECESARIO PARA CORS
using Newtonsoft.Json;
using AccesoDatos;

namespace API_REST_INTEGRACION.Controllers
{
    // ⭐ HABILITA CORS PARA ESTE CONTROLADOR COMPLETO
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ReservaIntegracionController : ApiController
    {
        [HttpPost]
        [Route("api/v1/integracion/autos/book")]
        public IHttpActionResult Post(HttpRequestMessage request)
        {
            try
            {
                var contenido = request.Content.ReadAsStringAsync().Result;
                if (string.IsNullOrWhiteSpace(contenido))
                    return BadRequest("El cuerpo del request está vacío o mal formateado.");

                var dto = JsonConvert.DeserializeObject<ReservaIntegracionDto>(contenido);
                if (dto == null)
                    return BadRequest("No se pudo interpretar el JSON recibido.");

                if (string.IsNullOrWhiteSpace(dto.id_hold) || string.IsNullOrWhiteSpace(dto.id_auto))
                    return BadRequest("Debe indicar un ID de Hold y un ID de Vehículo válidos.");

                if (dto.fecha_inicio >= dto.fecha_fin)
                    return BadRequest("La fecha de inicio debe ser anterior a la de fin.");

                if (string.IsNullOrWhiteSpace(dto.nombre) ||
                    string.IsNullOrWhiteSpace(dto.apellido) ||
                    string.IsNullOrWhiteSpace(dto.tipo_identificacion) ||
                    string.IsNullOrWhiteSpace(dto.identificacion) ||
                    string.IsNullOrWhiteSpace(dto.correo))
                    return BadRequest("Faltan datos obligatorios del titular.");

                using (var db = new db31808Entities1())
                {
                    if (!int.TryParse(dto.id_auto, out int idVehiculoInt))
                        return BadRequest("El ID del vehículo debe ser numérico.");

                    var vehiculo = db.Vehiculo.FirstOrDefault(v => v.id_vehiculo == idVehiculoInt);
                    if (vehiculo == null)
                        return BadRequest("No se encontró el vehículo especificado.");

                    // ✅ Crear reserva
                    var reserva = new Reserva
                    {
                        id_usuario = db.Usuario.FirstOrDefault()?.id_usuario ?? 1,
                        id_vehiculo = idVehiculoInt,
                        fecha_inicio = dto.fecha_inicio,
                        fecha_fin = dto.fecha_fin,
                        total = vehiculo.precio_dia * (decimal)(dto.fecha_fin - dto.fecha_inicio).TotalDays,
                        estado = "Confirmada",
                        fecha_reserva = DateTime.Now,
                    };

                    db.Reserva.Add(reserva);
                    db.SaveChanges();

                    var respuesta = new
                    {
                        mensaje = "Reserva creada correctamente ✅",
                        id_reserva = reserva.id_reserva,
                        id_hold = dto.id_hold,
                        id_auto = dto.id_auto,
                        nombre_titular = $"{dto.nombre} {dto.apellido}",
                        tipo_identificacion = dto.tipo_identificacion,
                        identificacion = dto.identificacion,
                        correo = dto.correo,
                        vehiculo = $"{vehiculo.marca} {vehiculo.modelo}",
                        reserva.fecha_inicio,
                        reserva.fecha_fin,
                        reserva.total,
                        reserva.estado,
                        reserva.fecha_reserva
                    };

                    return Ok(respuesta);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al crear la reserva: {ex.Message}");
            }
        }
    }

    public class ReservaIntegracionDto
    {
        public string id_auto { get; set; }
        public string id_hold { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string tipo_identificacion { get; set; }
        public string identificacion { get; set; }
        public string correo { get; set; }
        public DateTime fecha_inicio { get; set; }
        public DateTime fecha_fin { get; set; }
    }
}
