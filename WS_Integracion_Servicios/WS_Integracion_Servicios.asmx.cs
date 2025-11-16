using System;
using System.Linq;
using System.Web.Services;
using AccesoDatos;
using AccesoDatos.DTO;

namespace WS_Integracion_Servicios
{
    /// <summary>
    /// Servicio SOAP para gestión de reservas de autos.
    /// </summary>
    [WebService(Namespace = "http://integracion.rentaautos.com.ec/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class WS_Integracion_Servicios : System.Web.Services.WebService
    {
        [WebMethod(Description = "Crea una reserva de auto a partir de un Hold activo.")]
        public ReservaDto ReservarAuto(
            string id_auto,
            string id_hold,
            string nombre,
            string apellido,
            string tipo_identificacion,
            string identificacion,
            string correo,
            DateTime fecha_inicio,
            DateTime fecha_fin)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id_auto))
                    throw new Exception("Debe indicar un ID de vehículo válido.");
                if (fecha_inicio >= fecha_fin)
                    throw new Exception("La fecha de inicio debe ser anterior a la de fin.");

                using (var db = new db31808Entities1())
                {
                    if (!int.TryParse(id_auto, out int idVehiculoInt))
                        throw new Exception("El ID de vehículo debe ser numérico.");

                    var vehiculo = db.Vehiculo.FirstOrDefault(v => v.id_vehiculo == idVehiculoInt);
                    if (vehiculo == null)
                        throw new Exception("No se encontró el vehículo especificado.");

                    var reserva = new Reserva
                    {
                        id_vehiculo = idVehiculoInt,
                        fecha_inicio = fecha_inicio,
                        fecha_fin = fecha_fin,
                        total = vehiculo.precio_dia * (decimal)(fecha_fin - fecha_inicio).TotalDays,
                        estado = "Confirmada",
                        fecha_reserva = DateTime.Now
                    };

                    db.Reserva.Add(reserva);
                    db.SaveChanges();

                    // ✅ Retorna DTO con la matrícula y datos del vehículo
                    return new ReservaDto
                    {
                        IdReserva = reserva.id_reserva,
                        NombreUsuario = $"{nombre} {apellido}",
                        IdVehiculo = vehiculo.id_vehiculo,
                        VehiculoNombre = $"{vehiculo.marca} {vehiculo.modelo}",
                        NumeroMatricula = string.IsNullOrWhiteSpace(vehiculo.matricula) ? "Sin matrícula" : vehiculo.matricula,
                        FechaInicio = reserva.fecha_inicio,
                        FechaFin = reserva.fecha_fin,
                        Total = reserva.total,
                        Estado = reserva.estado
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear la reserva: {ex.Message}");
            }
        }

        [WebMethod(Description = "Prueba de conexión del servicio SOAP.")]
        public string TestConexion()
        {
            return "Servicio SOAP de integración de reservas activo ✅";
        }
    }

    // ==============================
    // DTO de respuesta
    // ==============================
    public class ReservaDto
    {
        public int IdReserva { get; set; }
        public string NombreUsuario { get; set; }
        public int IdVehiculo { get; set; }
        public string VehiculoNombre { get; set; }
        public string NumeroMatricula { get; set; } // 🔹 agregado
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; }
    }
}
