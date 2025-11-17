using System;
using System.Web.Services;
using Logica;

namespace WS_Integracion_Servicios
{
    [WebService(Namespace = "http://integracion.rentaautos.com/booking")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WS_DisponibilidadAutos : WebService
    {
        private readonly ReservaLogica _reservaLN = new ReservaLogica();

        /// <summary>
        /// Verifica disponibilidad de un vehículo entre dos fechas.
        /// Regla: fechaInicio inclusive, fechaFin exclusiva (noches = fin - inicio).
        /// </summary>
        [WebMethod(Description = "Verifica disponibilidad de un vehículo.")]
        public bool validarDisponibilidadAuto(int idVehiculo, DateTime fechaInicio, DateTime fechaFin)
        {
            // Validaciones rápidas (mismo criterio que usa tu LN)
            if (idVehiculo <= 0) throw new Exception("idVehiculo inválido.");
            var ini = fechaInicio.Date;
            var fin = fechaFin.Date;
            if (fin <= ini) throw new Exception("Rango de fechas inválido.");

            // Llama a tu capa de negocio (no tocamos repos/EF directo)
            // True  => disponible
            // False => hay solape (no disponible)
            return _reservaLN.ValidarDisponibilidad(idVehiculo, ini, fin);
        }
    }
}