using System;
using System.Web.Services;
using System.Web.Services.Protocols;
using AccesoDatos.DTO;
using Logica;

namespace WS_Integracion_Servicios
{
    [WebService(Namespace = "http://rentaautos.ec/gestion")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WS_PreReserva : WebService
    {
        private readonly IntegracionAutosLogica ln = new IntegracionAutosLogica();

        private SoapException Fault(string mensaje, Exception ex = null)
            => new SoapException(mensaje, SoapException.ClientFaultCode, ex);

        // ================================================================
        // 🔹 MÉTODO: CrearPreReservaAuto
        // Descripción: Crea un hold (pre-reserva) y devuelve id_hold y fecha expiración
        // (equivalente a REST /api/integracion/autos/hold)
        // ================================================================
        [WebMethod(Description = "Crea una pre-reserva (hold) y devuelve id_hold y fecha de expiración.")]
        public PreReservaAutoResponseDto CrearPreReservaAuto(PreReservaAutoRequestDto request)
        {
            try
            {
                if (request == null)
                    throw Fault("Solicitud inválida: no se recibieron datos.");

                var resultado = ln.CrearPreReservaAuto(request);
                return resultado;
            }
            catch (Exception ex)
            {
                throw Fault("Error al crear la pre-reserva: " + ex.Message, ex);
            }
        }
    }
}
