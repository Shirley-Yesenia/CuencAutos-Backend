using AccesoDatos.DTO;
using Logica;
using Datos;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Routing;
using API_REST_GESTION.Hateoas.Builders;

namespace API_REST_GESTION.Controllers
{
    [RoutePrefix("api/v1/vehiculos")]
    public class VehiculoController : ApiController
    {
        private readonly VehiculoLogica _logica = new VehiculoLogica();
        private readonly ImagenVehiculoDatos _imgDatos = new ImagenVehiculoDatos();
        private readonly VehiculoHateoas _hateoas = new VehiculoHateoas();

        // ================================================================
        // GET: api/v1/vehiculos
        // ================================================================
        
        [HttpGet, Route("", Name = "GetVehiculos")]
        public IHttpActionResult ObtenerVehiculos()
        {
            try
            {
                var vehiculos = _logica.ListarVehiculos();

                var url = new UrlHelper(Request);

                foreach (var v in vehiculos)
                {
                    var img = _imgDatos.ListarPorVehiculo(v.IdVehiculo).FirstOrDefault();
                    v.UrlImagen = img?.UriImagen;

                    _hateoas.GenerarLinks(v, url);
                }

                return Ok(vehiculos);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // ================================================================
        // GET: api/v1/vehiculos/{id}
        // ================================================================
        
        [HttpGet, Route("{id:int}", Name = "GetVehiculoById")]
        public IHttpActionResult ObtenerVehiculoPorId(int id)
        {
            try
            {
                var vehiculo = _logica.ObtenerVehiculoPorId(id);
                if (vehiculo == null) return NotFound();

                var img = _imgDatos.ListarPorVehiculo(id).FirstOrDefault();
                vehiculo.UrlImagen = img?.UriImagen;

                var url = new UrlHelper(Request);
                _hateoas.GenerarLinks(vehiculo, url);

                return Ok(vehiculo);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // ================================================================
        // POST: api/v1/vehiculos
        // ================================================================
        
        [HttpPost, Route("", Name = "CreateVehiculo")]
        public IHttpActionResult CrearVehiculo([FromBody] VehiculoDto vehiculo)
        {
            try
            {
                if (vehiculo == null)
                    return BadRequest("El objeto 'vehiculo' es requerido.");

                int id = _logica.CrearVehiculo(vehiculo);
                var nuevo = _logica.ObtenerVehiculoPorId(id);

                var url = new UrlHelper(Request);
                _hateoas.GenerarLinks(nuevo, url);

                return Created($"api/vehiculos/{id}", nuevo);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // ================================================================
        // PUT: api/v1/vehiculos/{id}
        // ================================================================
        
        [HttpPut, Route("{id:int}", Name = "UpdateVehiculo")]
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

                var actualizadoDto = _logica.ObtenerVehiculoPorId(id);

                var url = new UrlHelper(Request);
                _hateoas.GenerarLinks(actualizadoDto, url);

                return Ok(actualizadoDto);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // ================================================================
        // DELETE: api/v1/vehiculos/{id}
        // ================================================================
       
        [HttpDelete, Route("{id:int}", Name = "DeleteVehiculo")]
        public IHttpActionResult EliminarVehiculo(int id)
        {
            try
            {
                bool eliminado = _logica.EliminarVehiculo(id);

                return Ok(new { mensaje = "Vehículo eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // ================================================================
        // GET: api/v1/vehiculos/buscar
        // ================================================================
       
        [HttpGet, Route("buscar", Name = "BuscarVehiculos")]
        public IHttpActionResult BuscarVehiculos(string categoria = null, string transmision = null, string estado = null)
        {
            try
            {
                var vehiculos = _logica.BuscarVehiculos(categoria, transmision, estado);

                var url = new UrlHelper(Request);

                foreach (var v in vehiculos)
                {
                    var img = _imgDatos.ListarPorVehiculo(v.IdVehiculo).FirstOrDefault();
                    v.UrlImagen = img?.UriImagen;

                    _hateoas.GenerarLinks(v, url);
                }

                return Ok(vehiculos);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // ================================================================
        // GET: api/v1/vehiculos/{id}/imagenes
        // ================================================================
        
        [HttpGet, Route("{id:int}/imagenes", Name = "GetImagenesPorVehiculo")]
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
