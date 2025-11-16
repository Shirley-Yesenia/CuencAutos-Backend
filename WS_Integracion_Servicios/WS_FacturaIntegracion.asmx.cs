using System;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Services.Protocols;
using AccesoDatos.DTO;
using Logica;

namespace WS_Integracion_Servicios
{
    /// <summary>
    /// Servicio SOAP para emitir facturas de integración del módulo de autos.
    /// </summary>
    [WebService(Namespace = "http://integracion.rentaautos.com.ec/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class WS_FacturaIntegracion : System.Web.Services.WebService
    {
        private readonly FacturaLogica _facturaLogica = new FacturaLogica();

        [WebMethod(Description = "Emite una factura a partir de una reserva confirmada.")]
        public FacturaDto EmitirFactura(
            int idReserva,
            int idUsuario,
            decimal valorTotal,
            string descripcion,
            List<DetalleFacturaDto> detalles)
        {
            try
            {
                if (idReserva <= 0)
                    throw new SoapException("ID de reserva inválido", SoapException.ClientFaultCode);
                if (idUsuario <= 0)
                    throw new SoapException("ID de usuario inválido", SoapException.ClientFaultCode);
                if (valorTotal <= 0)
                    throw new SoapException("El total debe ser mayor que cero", SoapException.ClientFaultCode);

                var factura = new FacturaDto
                {
                    IdReserva = idReserva,
                    IdUsuario = idUsuario,
                    ValorTotal = valorTotal,
                    Descripcion = descripcion,
                    FechaEmision = DateTime.Now,
                    Detalles = detalles ?? new List<DetalleFacturaDto>()
                };

                // Lógica central
                var idFactura = _facturaLogica.CrearFactura(factura);
                var facturaCreada = _facturaLogica.ObtenerFacturaPorId(idFactura);

                return facturaCreada;
            }
            catch (SoapException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new SoapException(
                    "Error al emitir la factura: " + ex.Message,
                    SoapException.ServerFaultCode);
            }
        }

        [WebMethod(Description = "Verifica que el servicio de facturación esté activo.")]
        public string TestConexion()
        {
            return "Servicio SOAP de facturación activo ✅";
        }
    }
}
