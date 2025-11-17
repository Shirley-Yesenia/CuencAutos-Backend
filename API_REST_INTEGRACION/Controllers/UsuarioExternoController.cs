using System;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;

namespace API_REST_INTEGRACION.Controllers
{
    public class UsuarioExternoController : ApiController
    {
        [HttpPost]
        [Route("api/v1/integracion/autos/usuarios/externo")]
        public IHttpActionResult Post(HttpRequestMessage request)
        {
            try
            {
                var contenido = request.Content.ReadAsStringAsync().Result;
                if (string.IsNullOrWhiteSpace(contenido))
                    return BadRequest("El cuerpo del request está vacío o mal formateado.");

                var nuevoUsuario = JsonConvert.DeserializeObject<UsuarioExternoDto>(contenido);
                if (nuevoUsuario == null)
                    return BadRequest("No se pudo interpretar el JSON.");

                var usuarioCreado = new
                {
                    IdUsuario = new Random().Next(1000, 9999),
                    Nombre = $"{nuevoUsuario.Nombre} {nuevoUsuario.Apellido}",
                    Email = nuevoUsuario.Email,
                    Estado = "Creado correctamente ✅"
                };

                return Ok(usuarioCreado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

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
