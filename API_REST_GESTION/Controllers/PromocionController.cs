using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using Logica;
using AccesoDatos.DTO;

namespace API_REST_GESTION.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/promociones")]
    public class PromocionController : ApiController
    {
        private readonly PromocionLogica logica = new PromocionLogica();

        [HttpGet]
        [Route("")]
        public IHttpActionResult ObtenerPromociones()
        {
            try
            {
                var lista = logica.ListarPromociones();
                if (lista == null || lista.Count == 0)
                    return NotFound();

                return Ok(lista);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult CrearPromocion([FromBody] PromocionDto dto)
        {
            if (dto == null)
                return BadRequest("Debe enviar los datos de la promoción.");

            try
            {
                int resultado = logica.CrearPromocion(dto);
                if (resultado <= 0)
                    return BadRequest("No se pudo crear la promoción.");

                return Ok(new { mensaje = "Promoción creada correctamente.", idGenerado = resultado });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Route("{idPromocion:int}")]
        public IHttpActionResult ActualizarPromocion(int idPromocion, [FromBody] PromocionDto dto)
        {
            if (dto == null)
                return BadRequest("Debe enviar los datos de la promoción.");

            try
            {
                dto.IdPromocion = idPromocion;
                bool actualizado = logica.ActualizarPromocion(dto);

                if (!actualizado)
                    return BadRequest("No se pudo actualizar la promoción.");

                return Ok(new { mensaje = "Promoción actualizada correctamente." });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        [Route("{idPromocion:int}")]
        public IHttpActionResult EliminarPromocion(int idPromocion)
        {
            try
            {
                bool eliminado = logica.EliminarPromocion(idPromocion);
                if (!eliminado)
                    return NotFound();

                return Ok(new { mensaje = "Promoción eliminada correctamente." });
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
            return Ok("Servicio REST de Promociones operativo ✅");
        }
    }
}
