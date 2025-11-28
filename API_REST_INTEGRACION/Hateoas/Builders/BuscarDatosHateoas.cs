using AccesoDatos.DTO;
using API_REST_INTEGRACION.Controllers;
using System.Web.Http.Routing;

namespace API_REST_INTEGRACION.Hateoas.Builders
{
    public class BuscarDatosHateoas
    {
        public ReservaInfoDto GenerarLinks(ReservaInfoDto dto, UrlHelper url, int idReserva)
        {
            dto.ClearLinks();

            // 🔗 SELF (ver la misma reserva)
            dto.AddLink(
                "self",
                url.Link("BuscarDatosReserva", new { id_reserva = idReserva }),
                "GET"
            );

            // 🔗 Link para descargar factura
            dto.AddLink(
                "factura",
                dto.UriFactura,
                "GET"
            );

            // ❌ Se elimina el link "vehiculo" para evitar Links vacíos en el GET secundario

            return dto;
        }
    }
}
