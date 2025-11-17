using System.Web.Http.Routing;
using AccesoDatos.DTO;

namespace API_REST_GESTION.Hateoas.Builders
{
    public class VehiculoHateoas
    {
        public VehiculoDto GenerarLinks(VehiculoDto v, UrlHelper url)
        {
            v.Links.Clear();

            v.AddLink("self",
                url.Link("GetVehiculoById", new { id = v.IdVehiculo }),
                "GET");

            v.AddLink("update",
                url.Link("UpdateVehiculo", new { id = v.IdVehiculo }),
                "PUT");

            v.AddLink("delete",
                url.Link("DeleteVehiculo", new { id = v.IdVehiculo }),
                "DELETE");

            v.AddLink("imagenes",
                url.Link("GetImagenesPorVehiculo", new { idVehiculo = v.IdVehiculo }),
                "GET");

            return v;
        }
    }
}
