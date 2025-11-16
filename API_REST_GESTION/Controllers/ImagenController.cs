using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using Logica;
using AccesoDatos.DTO;

namespace API_REST_GESTION.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/imagenes")]
    public class ImagenController : ApiController
    {
        private readonly ImagenVehiculoLogica logica = new ImagenVehiculoLogica();

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetImagenes()
        {
            try
            {
                var imgs = logica.ListarImagenes();
                if (imgs == null || imgs.Count == 0)
                    return NotFound();

                return Ok(imgs);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("{idImagen:int}")]
        public IHttpActionResult GetImagen(int idImagen)
        {
            try
            {
                var img = logica.ObtenerPorId(idImagen);
                if (img == null)
                    return NotFound();

                return Ok(img);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("vehiculo/{idVehiculo:int}")]
        public IHttpActionResult GetPorVehiculo(int idVehiculo)
        {
            try
            {
                var imgs = logica.ListarPorVehiculo(idVehiculo);
                if (imgs == null || imgs.Count == 0)
                    return NotFound();

                return Ok(imgs);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("vehiculo/{idVehiculo:int}/principal")]
        public IHttpActionResult GetPrincipal(int idVehiculo)
        {
            try
            {
                var img = logica.ObtenerPrincipal(idVehiculo);
                if (img == null)
                    return NotFound();

                return Ok(img);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

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

                return Ok(new { mensaje = "Imagen creada correctamente." });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

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

                return Ok(new { mensaje = "Imagen actualizada correctamente." });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        [Route("{idImagen:int}")]
        public IHttpActionResult EliminarImagen(int idImagen)
        {
            try
            {
                bool eliminado = logica.EliminarImagen(idImagen);
                if (!eliminado)
                    return NotFound();

                return Ok(new { mensaje = "Imagen eliminada correctamente." });
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
