using System;
using System.Net;
using System.Web.Http;
using API_REST_INTEGRACION.Hateoas.Builders;
using AccesoDatos.DTO;

namespace API_REST_INTEGRACION.Controllers
{
    public class UsuarioExternoController : ApiController
    {
        [HttpPost]
        [Route("api/v1/integracion/autos/usuarios/externo")]
        public IHttpActionResult Post([FromBody] UsuarioExternoDto nuevoUsuario)
        {
            try
            {
                // Validar si el objeto es nulo o tiene datos inválidos
                if (nuevoUsuario == null)
                {
                    return BadRequest("El cuerpo de la solicitud está vacío o mal formateado.");
                }

                if (string.IsNullOrWhiteSpace(nuevoUsuario.Nombre) ||
                    string.IsNullOrWhiteSpace(nuevoUsuario.Email))
                {
                    return BadRequest("El nombre y el correo son campos obligatorios.");
                }

                // Lógica de creación del usuario
                int idGenerado = new Random().Next(1000, 9999); // ID generado aleatoriamente

                var usuarioCreado = new
                {
                    IdUsuario = idGenerado,
                    Nombre = $"{nuevoUsuario.Nombre} {nuevoUsuario.Apellido}",
                    Email = nuevoUsuario.Email,
                    Estado = "Creado correctamente ✅",
                    _links = new UsuarioExternoHateoas().Build(idGenerado) // Enlaces HATEOAS
                };

                // Retornar la respuesta exitosa
                return Ok(usuarioCreado);
            }
            catch (Exception ex)
            {
                // Retornar un error 500 en caso de excepciones no controladas
                return InternalServerError(new Exception("Error al procesar el usuario: " + ex.Message));
            }
        }
    }

    // DTO para representar al usuario externo
    public class UsuarioExternoDto
    {
        public int BookingUserId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Pais { get; set; }
    }
}
