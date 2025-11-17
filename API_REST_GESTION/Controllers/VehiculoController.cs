using AccesoDatos.DTO;
using Logica;
using Datos;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace API_REST_GESTION.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/v1/vehiculos")]
    public class VehiculoController : ApiController
    {
        private readonly VehiculoLogica _logica = new VehiculoLogica();
        private readonly ImagenVehiculoDatos _imgDatos = new ImagenVehiculoDatos();

        [HttpGet]
        [Route("", Name = "GetVehiculos")]
        public IHttpActionResult ObtenerVehiculos()
        {
            try
            {
                var vehiculos = _logica.ListarVehiculos();

                foreach (var v in vehiculos)
                {
                    var img = _imgDatos.ListarPorVehiculo(v.IdVehiculo).FirstOrDefault();
                    v.UrlImagen = img?.UriImagen;
                }

                return Ok(vehiculos);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetVehiculoById")]
        public IHttpActionResult ObtenerVehiculoPorId(int id)
        {
            try
            {
                var vehiculo = _logica.ObtenerVehiculoPorId(id);
                if (vehiculo == null) return NotFound();

                var img = _imgDatos.ListarPorVehiculo(id).FirstOrDefault();
                vehiculo.UrlImagen = img?.UriImagen;

                return Ok(vehiculo);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("", Name = "CreateVehiculo")]
        public IHttpActionResult CrearVehiculo([FromBody] VehiculoDto vehiculo)
        {
            try
            {
                if (vehiculo == null)
                    return BadRequest("El objeto 'vehiculo' es requerido.");

                int id = _logica.CrearVehiculo(vehiculo);
                var nuevo = _logica.ObtenerVehiculoPorId(id);

                return Created($"api/vehiculos/{id}", nuevo);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Route("{id:int}", Name = "UpdateVehiculo")]
        public IHttpActionResult ActualizarVehiculo(int id, [FromBody] VehiculoDto vehiculo)
        {
            try
            {
                if (vehiculo == null)
                    return BadRequest("El objeto 'vehiculo' es requerido.");

                vehiculo.IdVehiculo = id;
                bool actualizado = _logica.ActualizarVehiculo(vehiculo);

                if (!actualizado)
                    return NotFound();

                return Ok(_logica.ObtenerVehiculoPorId(id));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        [Route("{id:int}", Name = "DeleteVehiculo")]
        public IHttpActionResult EliminarVehiculo(int id)
        {
            try
            {
                bool eliminado = _logica.EliminarVehiculo(id);
                if (!eliminado)
                    return NotFound();

                return Ok(new { mensaje = "Vehículo eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("buscar", Name = "BuscarVehiculos")]
        public IHttpActionResult BuscarVehiculos(string categoria = null, string transmision = null, string estado = null)
        {
            try
            {
                var vehiculos = _logica.BuscarVehiculos(categoria, transmision, estado);

                foreach (var v in vehiculos)
                {
                    var img = _imgDatos.ListarPorVehiculo(v.IdVehiculo).FirstOrDefault();
                    v.UrlImagen = img?.UriImagen;
                }

                return Ok(vehiculos);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // ruta necesaria para HATEOAS (las imágenes)
        [HttpGet]
        [Route("{id:int}/imagenes", Name = "GetImagenesPorVehiculo")]
        public IHttpActionResult ObtenerImagenesVehiculo(int id)
        {
            try
            {
                var imagenes = _imgDatos.ListarPorVehiculo(id);
                return Ok(imagenes);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
