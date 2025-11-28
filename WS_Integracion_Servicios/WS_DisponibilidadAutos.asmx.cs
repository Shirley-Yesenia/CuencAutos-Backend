using Datos;
using Logica;
using System;
using System.Web.Services;

namespace WS_Integracion_Servicios
{
    [WebService(Namespace = "http://integracion.rentaautos.com/booking")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WS_DisponibilidadAutos : WebService
    {
        private readonly ReservaLogica _reservas = new ReservaLogica();
        private readonly VehiculoDatos _vehiculos = new VehiculoDatos();


        /// <summary>
        /// Verifica disponibilidad de un vehículo entre dos fechas.
        /// Regla: fechaInicio inclusive, fechaFin exclusiva.
        /// </summary>
        [WebMethod(Description = "Verifica disponibilidad de un vehículo.")]
        public ValidarDisponibilidadSoapResponse validarDisponibilidadAuto(string idVehiculo, DateTime fechaInicio, DateTime fechaFin)
        {
            // =====================================================
            // ✔ Validar IdVehiculo (igual que REST)
            // =====================================================
            if (!int.TryParse(idVehiculo, out int idVehiculoInt))
                return new ValidarDisponibilidadSoapResponse
                {
                    Disponible = false,
                    Mensaje = "El IdVehiculo debe ser numérico."
                };

            // =====================================================
            // ✔ Validar que exista el vehículo (igual que REST)
            // =====================================================
            var v = _vehiculos.ObtenerPorId(idVehiculoInt);
            if (v == null)
            {
                return new ValidarDisponibilidadSoapResponse
                {
                    Disponible = false,
                    Mensaje = "El vehículo no existe."
                };
            }

            // =====================================================
            // ✔ Validación de fechas (igual que REST)
            // =====================================================
            var ini = fechaInicio.Date;
            var fin = fechaFin.Date;

            if (fin <= ini)
            {
                return new ValidarDisponibilidadSoapResponse
                {
                    Disponible = false,
                    Mensaje = "Rango de fechas inválido."
                };
            }

            // =====================================================
            // ✔ Llamada a la lógica (igual que REST)
            // =====================================================
            bool disponible = _reservas.ValidarDisponibilidad(idVehiculoInt, ini, fin);

            // =====================================================
            // ✔ Respuesta alineada con REST
            // =====================================================
            return new ValidarDisponibilidadSoapResponse
            {
                IdVehiculo = idVehiculoInt,
                FechaInicio = ini,
                FechaFin = fin,
                Disponible = disponible,
                Mensaje = disponible ? "Vehículo disponible" : "No disponible"
            };
        }
    }

    // ===========================================================
    // DTO SOAP (equivalente al anónimo en REST)
    // ===========================================================
    public class ValidarDisponibilidadSoapResponse
    {
        public int IdVehiculo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public bool Disponible { get; set; }
        public string Mensaje { get; set; }
    }
}
