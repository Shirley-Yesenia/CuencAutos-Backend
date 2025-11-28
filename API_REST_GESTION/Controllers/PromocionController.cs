using System;
using System.Collections.Generic;
using System.Web.Http;
using Logica;
using AccesoDatos.DTO;
using API_REST_GESTION.Hateoas.Builders;

namespace API_REST_GESTION.Controllers
{
    [RoutePrefix("api/v1/promociones")]
    public class PromocionController : ApiController
    {
        private readonly PromocionLogica logica = new PromocionLogica();

        // ================================
        // GET: /api/v1/promociones
        // ================================
        [HttpGet]
        [Route("", Name = "ObtenerPromociones")]
        public IHttpActionResult ObtenerPromociones()
        {
            try
            {
                var lista = logica.ListarPromociones();
                if (lista == null || lista.Count == 0)
                    return NotFound();

                var hateoas = new List<object>();
                var hbuilder = new PromocionHateoas(Url);

                foreach (var item in lista)
                    hateoas.Add(hbuilder.Build(item));

                return Ok(hateoas);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // ================================
        // GET BY ID
        // ================================
        [HttpGet]
        [Route("{idPromocion:int}", Name = "GetPromocionById")]
        public IHttpActionResult ObtenerPromocionPorId(int idPromocion)
        {
            try
            {
                var dto = logica.ObtenerPromocionPorId(idPromocion);
                if (dto == null)
                    return NotFound();

                var hbuilder = new PromocionHateoas(Url);
                return Ok(hbuilder.Build(dto));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // ================================
        // POST
        // ================================
        [HttpPost]
        [Route("", Name = "CrearPromocion")]
        public IHttpActionResult CrearPromocion([FromBody] PromocionDto dto)
        {
            if (dto == null)
                return BadRequest("Debe enviar los datos de la promoción.");

            try
            {
                int resultado = logica.CrearPromocion(dto);
                if (resultado <= 0)
                    return BadRequest("No se pudo crear la promoción.");

                dto.IdPromocion = resultado;

                var hbuilder = new PromocionHateoas(Url);
                return Ok(hbuilder.Build(dto));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // ================================
        // PUT
        // ================================
        [HttpPut]
        [Route("{idPromocion:int}", Name = "UpdatePromocion")]
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

                var hbuilder = new PromocionHateoas(Url);
                return Ok(hbuilder.Build(dto));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // ================================
        // DELETE
        // ================================
        [HttpDelete]
        [Route("{idPromocion:int}", Name = "DeletePromocion")]
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

        // ================================
        // PING
        // ================================
        [HttpGet]
        [Route("ping")]
        public IHttpActionResult Ping()
        {
            return Ok("Servicio REST de Promociones operativo ✅");
        }
    }
}
