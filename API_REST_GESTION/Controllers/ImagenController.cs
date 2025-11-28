using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using Logica;
using AccesoDatos.DTO;
using API_REST_GESTION.Hateoas.Builders;

namespace API_REST_GESTION.Controllers
{
    [RoutePrefix("api/v1/imagenes")]
    public class ImagenController : ApiController
    {
        private readonly ImagenVehiculoLogica logica = new ImagenVehiculoLogica();
        private readonly ImagenHateoas hateoas;

        public ImagenController()
        {
            var baseUrl = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            hateoas = new ImagenHateoas(baseUrl);
        }

        // =========================================================
        // GET /api/v1/imagenes
        // =========================================================
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetImagenes()
        {
            try
            {
                var imgs = logica.ListarImagenes();
                if (imgs == null || imgs.Count == 0)
                    return NotFound();

                return Ok(new
                {
                    data = imgs,
                    _links = hateoas.LinksColeccion()
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // =========================================================
        // GET /api/v1/imagenes/{idImagen}
        // =========================================================
        [HttpGet]
        [Route("{idImagen:int}")]
        public IHttpActionResult GetImagen(int idImagen)
        {
            try
            {
                var img = logica.ObtenerPorId(idImagen);
                if (img == null)
                    return NotFound();

                return Ok(new
                {
                    data = img,
                    _links = hateoas.LinksImagen(idImagen)
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // =========================================================
        // GET /api/v1/imagenes/vehiculo/{idVehiculo}
        // =========================================================
        [HttpGet]
        [Route("vehiculo/{idVehiculo:int}")]
        public IHttpActionResult GetPorVehiculo(int idVehiculo)
        {
            try
            {
                var imgs = logica.ListarPorVehiculo(idVehiculo);
                if (imgs == null || imgs.Count == 0)
                    return NotFound();

                return Ok(new
                {
                    data = imgs,
                    _links = hateoas.LinksPorVehiculo(idVehiculo)
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // =========================================================
        // GET /api/v1/imagenes/vehiculo/{idVehiculo}/principal
        // =========================================================
        [HttpGet]
        [Route("vehiculo/{idVehiculo:int}/principal")]
        public IHttpActionResult GetPrincipal(int idVehiculo)
        {
            try
            {
                var img = logica.ObtenerPrincipal(idVehiculo);
                if (img == null)
                    return NotFound();

                return Ok(new
                {
                    data = img,
                    _links = hateoas.LinksPrincipal(idVehiculo)
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // =========================================================
        // POST /api/v1/imagenes
        // =========================================================
        [HttpPost]
        [Route("")]
        public IHttpActionResult CrearImagen([FromBody] ImagenVehiculoDto dto)
        {
            if (dto == null)
                return BadRequest("Debe enviar los datos de la imagen.");

            try
            {
                bool ok = logica.CrearImagen(dto);
                if (!ok)
                    return BadRequest("No se pudo crear la imagen.");

                return Ok(new
                {
                    mensaje = "Imagen creada correctamente.",
                    _links = hateoas.LinksColeccion()
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // =========================================================
        // PUT /api/v1/imagenes/{idImagen}
        // =========================================================
        [HttpPut]
        [Route("{idImagen:int}")]
        public IHttpActionResult ActualizarImagen(int idImagen, [FromBody] ImagenVehiculoDto dto)
        {
            if (dto == null)
                return BadRequest("Debe enviar los datos de la imagen.");

            try
            {
                dto.IdImagen = idImagen;
                bool ok = logica.ActualizarImagen(dto);

                if (!ok)
                    return BadRequest("No se pudo actualizar la imagen.");

                return Ok(new
                {
                    mensaje = "Imagen actualizada correctamente.",
                    _links = hateoas.LinksImagen(idImagen)
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // =========================================================
        // DELETE /api/v1/imagenes/{idImagen}
        // =========================================================
        [HttpDelete]
        [Route("{idImagen:int}")]
        public IHttpActionResult EliminarImagen(int idImagen)
        {
            try
            {
                bool eliminado = logica.EliminarImagen(idImagen);
                if (!eliminado)
                    return NotFound();

                return Ok(new
                {
                    mensaje = "Imagen eliminada correctamente.",
                    _links = hateoas.LinksColeccion()
                });
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
            return Ok("Servicio REST de Imágenes operativo ✅");
        }
    }
}
