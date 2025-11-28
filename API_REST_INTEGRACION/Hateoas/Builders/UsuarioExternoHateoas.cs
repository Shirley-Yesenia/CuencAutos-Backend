using AccesoDatos.DTO;
using System.Collections.Generic;

namespace API_REST_INTEGRACION.Hateoas.Builders
{
    public class UsuarioExternoHateoas
    {
        public List<LinkDto> Build(int idUsuario)
        {
            return new List<LinkDto>
            {
                new LinkDto(
                    href: $"/api/v1/integracion/autos/usuarios/externo/{idUsuario}",
                    rel: "self",
                    method: "GET"
                ),
                new LinkDto(
                    href: "/api/v1/integracion/autos/usuarios/externo",
                    rel: "crear_usuario",
                    method: "POST"
                ),
                new LinkDto(
                    href: "/api/v1/integracion/autos/book",
                    rel: "reservar_auto",
                    method: "POST"
                ),
                new LinkDto(
                    href: "/api/v1/integracion/autos/availability",
                    rel: "validar_disponibilidad",
                    method: "POST"
                )
            };
        }
    }
}
