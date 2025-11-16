using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using Logica;
using AccesoDatos.DTO;

namespace API_REST_GESTION.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/categoriasvehiculo")]
    public class CategoriaVehiculoController : ApiController
    {
        private readonly CategoriaVehiculoLogica ln = new CategoriaVehiculoLogica();

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetCategorias()
        {
            try
            {
                var categorias = ln.ListarCategorias();
                if (categorias == null || categorias.Count == 0)
                    return NotFound();

                return Ok(categorias);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult CrearCategoria([FromBody] CategoriaVehiculoDto categoria)
        {
            if (categoria == null)
                return BadRequest("El objeto 'categoria' no puede ser nulo.");

            try
            {
                var creada = ln.CrearCategoria(categoria);
                return Ok(creada);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Route("{idCategoria:int}")]
        public IHttpActionResult ActualizarCategoria(int idCategoria, [FromBody] CategoriaVehiculoDto categoria)
        {
            if (categoria == null)
                return BadRequest("El objeto 'categoria' no puede ser nulo.");

            try
            {
                categoria.IdCategoria = idCategoria;
                bool actualizado = ln.ActualizarCategoria(categoria);

                if (!actualizado)
                    return BadRequest("No se pudo actualizar la categoría.");

                var actualizada = ln.ObtenerPorId(idCategoria);
                return Ok(actualizada);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        [Route("{idCategoria:int}")]
        public IHttpActionResult EliminarCategoria(int idCategoria)
        {
            try
            {
                bool eliminado = ln.EliminarCategoria(idCategoria);
                if (eliminado)
                    return Ok(new { mensaje = "Categoría eliminada correctamente." });
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("transmisiones")]
        public IHttpActionResult ListarTransmisiones()
        {
            try
            {
                var transmisiones = new List<string> { "Manual", "Automático", "CVT" };
                return Ok(transmisiones);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("combustibles")]
        public IHttpActionResult ListarCombustibles()
        {
            try
            {
                var combustibles = new List<string> { "Gasolina", "Diésel", "Híbrido", "Eléctrico" };
                return Ok(combustibles);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("ping")]
        public IHttpActionResult Ping()
        {
            return Ok("Servicio REST de CategoríaVehículo operativo ✅");
        }
    }
}
