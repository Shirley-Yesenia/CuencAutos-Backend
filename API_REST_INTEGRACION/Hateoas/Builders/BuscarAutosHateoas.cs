using AccesoDatos.DTO;
using System.Web.Http.Routing;

namespace API_REST_INTEGRACION.Hateoas.Builders
{
    public class BuscarAutosHateoas
    {
        // Genera links SOLO a nivel global (no por auto individual)
        public void GenerarLinks(HateoasResourceWrapper<object> wrapper, UrlHelper url)
        {
            wrapper.Links.Clear();

            // Self
            wrapper.AddLink(
                "self",
                url.Link("BuscarAutos", null),
                "GET"
            );

            // Crear preregistro
            wrapper.AddLink(
                "crear_prereserva_auto",
                url.Link("CrearPreReservaAuto", null),
                "POST"
            );
        }
    }
}
