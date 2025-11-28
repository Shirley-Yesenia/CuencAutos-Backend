using Logica;
using AccesoDatos.DTO;
using System;
using System.Web.Http;
using API_REST_GESTION.Hateoas.Builders;

namespace API_REST_GESTION.Controllers
{
    [RoutePrefix("api/v1/usuarios")]
    public class UsuariosController : ApiController
    {
        private readonly UsuarioLogica _logica = new UsuarioLogica();
        private readonly UsuariosHateoas _hateoas;

        public UsuariosController()
        {
            _hateoas = new UsuariosHateoas("http://localhost:5000");
        }

        // ================================================================
        // GET: api/v1/usuarios
        // ================================================================
        [HttpGet, Route("")]
        public IHttpActionResult ListarUsuarios()
        {
            var lista = _logica.ListarUsuarios();
            return Ok(_hateoas.Build(lista));
        }

        // ================================================================
        // GET: api/v1/usuarios/{id}
        // ================================================================
        [HttpGet, Route("{id:int}")]
        public IHttpActionResult ObtenerUsuario(int id)
        {
            var usuario = _logica.ObtenerUsuarioPorId(id);
            if (usuario == null)
                return NotFound();

            return Ok(_hateoas.Build(usuario, id));
        }

        // ================================================================
        // POST: api/v1/usuarios
        // ================================================================
        [HttpPost, Route("")]
        public IHttpActionResult CrearUsuario([FromBody] UsuarioDto dto)
        {
            try
            {
                var idNuevo = _logica.CrearUsuario(dto);
                var creado = _logica.ObtenerUsuarioPorId(idNuevo);

                return Ok(_hateoas.Build(creado, idNuevo));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ================================================================
        // PUT: api/v1/usuarios/{id}
        // ================================================================
        [HttpPut, Route("{id:int}")]
        public IHttpActionResult ActualizarUsuario(int id, [FromBody] UsuarioDto dto)
        {
            try
            {
                dto.IdUsuario = id;

                var actualizado = _logica.ActualizarUsuario(dto);
                if (!actualizado)
                    return BadRequest("No se pudo actualizar el usuario.");

                var data = _logica.ObtenerUsuarioPorId(id);
                return Ok(_hateoas.Build(data, id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ================================================================
        // DELETE: api/v1/usuarios/{id}
        // ================================================================
        [HttpDelete, Route("{id:int}")]
        public IHttpActionResult EliminarUsuario(int id)
        {
            try
            {
                var eliminado = _logica.EliminarUsuario(id);
                if (!eliminado)
                    return BadRequest("No se pudo eliminar el usuario.");

                // Para delete usamos HATEOAS aplicando tu mismo estilo:
                return Ok(_hateoas.Build(
                    new { mensaje = "Usuario eliminado correctamente", id },
                    id
                ));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ================================================================
        // POST: api/v1/usuarios/login
        // ================================================================
        public class LoginRequest
        {
            public string Email { get; set; }
            public string Contrasena { get; set; }
        }

        [HttpPost, Route("login")]
        public IHttpActionResult Login([FromBody] LoginRequest req)
        {
            try
            {
                var usuario = _logica.ValidarLogin(req.Email, req.Contrasena);
                return Ok(_hateoas.Build(usuario));
            }
            catch
            {
                return Unauthorized();
            }
        }
    }
}
