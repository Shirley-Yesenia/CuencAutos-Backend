using System.Collections.Generic;
using System.Web.Http.Routing;
using AccesoDatos.DTO;

namespace API_REST_INTEGRACION.Hateoas.Builders
{
    public class EmitirFacturaHateoas
    {
        private readonly UrlHelper _url;
        private readonly string _idFactura;
        private readonly string _idReserva;

        public EmitirFacturaHateoas(UrlHelper url, string idFactura, string idReserva)
        {
            _url = url;
            _idFactura = idFactura;
            _idReserva = idReserva;
        }

        public List<LinkDto> GetLinks()
        {
            return new List<LinkDto>
            {
                // 📎 Self (usar el nombre de la ruta real)
                new LinkDto(
                    rel: "self",
                    href: _url.Link("EmitirFactura", null),
                    method: "POST"
                ),

                // 📎 Consultar datos de la reserva emitida
                new LinkDto(
                    rel: "reserva_detalle",
                    href: _url.Link("BuscarDatosReserva", new { id_reserva = _idReserva }),
                    method: "GET"
                ),

                // 📎 Reemitir factura (misma ruta POST)
                new LinkDto(
                    rel: "emitir_nuevamente",
                    href: _url.Link("EmitirFactura", null),
                    method: "POST"
                )
            };
        }
    }
}
