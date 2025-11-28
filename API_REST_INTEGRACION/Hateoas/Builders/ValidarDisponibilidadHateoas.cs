using AccesoDatos.DTO;
using System.Collections.Generic;

namespace API_REST_INTEGRACION.Hateoas.Builders
{
    public class ValidarDisponibilidadHateoas
    {
        public List<LinkDto> Build(int idVehiculo)
        {
            return new List<LinkDto>
            {
                new LinkDto(
                    href: "/api/v1/integracion/autos/availability",
                    rel: "self",
                    method: "POST"
                ),
                new LinkDto(
                    href: "/api/v1/integracion/autos/book",
                    rel: "reservar_auto",
                    method: "POST"
                ),
                new LinkDto(
                    href: "/api/v1/integracion/autos/hold",
                    rel: "crear_hold",
                    method: "POST"
                ),
                new LinkDto(
                    href: $"/api/v1/integracion/autos/{idVehiculo}",
                    rel: "ver_detalle_auto",
                    method: "GET"
                )
            };
        }
    }
}
