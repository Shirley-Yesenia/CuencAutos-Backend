using System.Collections.Generic;
using System.Web.Http.Routing;
using AccesoDatos.DTO;

namespace API_REST_INTEGRACION.Hateoas.Builders
{
    public class CrearPrereservaHateoas
    {
        private readonly UrlHelper _url;
        private readonly string _idHold;

        public CrearPrereservaHateoas(UrlHelper url, string idHold)
        {
            _url = url;
            _idHold = idHold;
        }

        public List<LinkDto> GetLinks()
        {
            // Construimos a mano la URL de reservar auto
            string urlReservar = "/api/v1/integracion/autos/book?id_hold=" + _idHold;

            return new List<LinkDto>
            {
                // SELF → este mismo endpoint
                new LinkDto(
                    rel: "self",
                    href: _url.Link("CrearPreReservaAutoV2", null),
                    method: "POST"
                ),

                // Confirmar la reserva
                new LinkDto(
                    rel: "confirmar_reserva",
                    href: urlReservar,
                    method: "POST"
                )
            };
        }
    }
}
