using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Routing;
using Logica;
using AccesoDatos.DTO;
using API_REST_GESTION.Hateoas.Builders;

namespace API_REST_GESTION.Controllers
{
   
    [RoutePrefix("api/v1/categoriasvehiculo")]
    public class CategoriaVehiculoController : ApiController
    {
        private readonly CategoriaVehiculoLogica ln = new CategoriaVehiculoLogica();

        private CategoriaVehiculoHateoas GetBuilder()
        {
            return new CategoriaVehiculoHateoas(new UrlHelper(Request));
        }

        // ============================================================
        // GET ALL (con HATEOAS)
        // ============================================================
        [HttpGet]
        
        [Route("", Name = "GetAllCategorias")]
        public IHttpActionResult GetCategorias()
        {
            try
            {
                var categorias = ln.ListarCategorias();
                if (categorias == null || categorias.Count == 0)
                    return NotFound();

                var builder = GetBuilder();
                var resultado = new List<object>();

                foreach (var c in categorias)
                    resultado.Add(builder.Build(c));

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // ============================================================
        // POST crear categoría
        // ============================================================
        [HttpPost]
        
        [Route("", Name = "CreateCategoria")]
        public IHttpActionResult CrearCategoria([FromBody] CategoriaVehiculoDto categoria)
        {
            if (categoria == null)
                return BadRequest("El objeto 'categoria' no puede ser nulo.");

            try
            {
                var creada = ln.CrearCategoria(categoria);

                var builder = GetBuilder();
                return Ok(builder.Build(creada));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // ============================================================
        // PUT actualizar
        // ============================================================
        [HttpPut]
        
        [Route("{idCategoria:int}", Name = "UpdateCategoria")]
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

                var builder = GetBuilder();
                return Ok(builder.Build(actualizada));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // ============================================================
        // DELETE eliminar
        // ============================================================
        [HttpDelete]
        
        [Route("{idCategoria:int}", Name = "DeleteCategoria")]
        public IHttpActionResult EliminarCategoria(int idCategoria)
        {
            try
            {
                bool eliminado = ln.EliminarCategoria(idCategoria);
                if (!eliminado)
                    return NotFound();

                return Ok(new { mensaje = "Categoría eliminada correctamente." });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // ============================================================
        // GET transmisiones (sin hateoas)
        // ============================================================
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

        // ============================================================
        // GET combustibles (sin hateoas)
        // ============================================================
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

        // ============================================================
        // PING
        // ============================================================
        [HttpGet]
        
        [Route("ping")]
        public IHttpActionResult Ping()
        {
            return Ok("Servicio REST de CategoríasVehículo operativo ✅");
        }
    }
}
