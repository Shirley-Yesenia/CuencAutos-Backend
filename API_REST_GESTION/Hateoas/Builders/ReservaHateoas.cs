using AccesoDatos.DTO;
using System.Web.Http.Routing;

namespace API_REST_GESTION.Hateoas.Builders
{
    public class ReservaHateoas
    {
        public ReservaDto GenerarLinks(ReservaDto dto, UrlHelper url)
        {
            dto.ClearLinks();

            dto.AddLink("self",
                url.Link("GetReservaById", new { idReserva = dto.IdReserva }));

            dto.AddLink("actualizar",
                url.Link("UpdateReserva", new { idReserva = dto.IdReserva }));

            dto.AddLink("eliminar",
                url.Link("DeleteReserva", new { idReserva = dto.IdReserva }));

            dto.AddLink("cambiar_estado",
                url.Link("CambiarEstadoReserva", new { idReserva = dto.IdReserva, nuevoEstado = "Pendiente" }));

            dto.AddLink("reservas_usuario",
                url.Link("GetReservasByUsuario", new { idUsuario = dto.IdUsuario }));

            return dto;
        }
    }
}
