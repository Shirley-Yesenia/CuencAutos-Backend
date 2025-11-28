using System.Collections.Generic;
using System.Web.Http.Routing;

namespace API_REST_INTEGRACION.Hateoas.Builders
{
    public class IntegracionAutosHateoas
    {
        private readonly UrlHelper _url;
        private readonly int _idReserva;
        private readonly string _idHold;
        private readonly string _idAuto;

        public IntegracionAutosHateoas(UrlHelper url, int idReserva, string idHold, string idAuto)
        {
            _url = url;
            _idReserva = idReserva;
            _idHold = idHold;
            _idAuto = idAuto;
        }

        public List<object> GetLinks()
        {
            return new List<object>
            {
                new {
                    rel = "self",
                    href = _url.Link("DefaultApi", new { controller = "ReservaIntegracion" }),
                    method = "POST"
                },
                new {
                    rel = "detalle_reserva",
                    href = _url.Link("DefaultApi", new { controller = "Reserva", id = _idReserva }),
                    method = "GET"
                },
                new {
                    rel = "detalle_hold",
                    href = _url.Link("DefaultApi", new { controller = "IntegracionHold", id_hold = _idHold }),
                    method = "GET"
                },
                new {
                    rel = "detalle_auto",
                    href = _url.Link("DefaultApi", new { controller = "Autos", id_auto = _idAuto }),
                    method = "GET"
                },
                new {
                    rel = "emitir_factura",
                    href = _url.Link("DefaultApi", new { controller = "EmitirFactura" }),
                    method = "POST"
                }
            };
        }
    }
}
