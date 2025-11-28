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
            => new SoapException(mensaje, SoapException.ClientFaultCode, Context.Request.Url.ToString(), ex);

        // ================================================================
        // 🔹 MÉTODO: CrearPreReservaAuto
        // ================================================================
        [WebMethod(Description = "Crea una pre-reserva (hold) y devuelve id_hold y fecha de expiración.")]
        public PreReservaAutoResponseDto CrearPreReservaAuto(PreReservaAutoRequestDto request)
        {
            try
            {
                if (request == null)
                    throw Fault("Solicitud inválida: El cuerpo está vacío.");

                // Ejecutar lógica (con validaciones internas)
                var resultado = ln.CrearPreReservaAuto(request);

                return resultado;
            }
            catch (SoapException)
            {
                // si ya es SOAP Fault, lo relanzas
                throw;
            }
            catch (Exception ex)
            {
                // convertir excepciones internas en Fault SOAP estándar
                throw Fault("Error al crear la pre-reserva: " + ex.Message, ex);
            }
        }
    }
}
