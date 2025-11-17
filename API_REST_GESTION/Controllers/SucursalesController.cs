using System.Web.Http;
using System.Web.Http.Cors;
using Logica;
using AccesoDatos.DTO;

namespace API_REST_GESTION.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/v1/sucursales")]
    public class SucursalesController : ApiController
    {
        private readonly SucursalLogica _ln = new SucursalLogica();

        [HttpGet, Route("")]
        public IHttpActionResult GetAll()
        {
            return Ok(_ln.ListarSucursales());
        }

        [HttpGet, Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            var sucursal = _ln.ObtenerSucursalPorId(id);
            if (sucursal == null) return NotFound();
            return Ok(sucursal);
        }

        [HttpPost, Route("")]
        public IHttpActionResult Post([FromBody] SucursalDto sucursal)
        {
            var newId = _ln.CrearSucursal(sucursal);
            return Ok(_ln.ObtenerSucursalPorId(newId));
        }

        [HttpPut, Route("{id:int}")]
        public IHttpActionResult Put(int id, [FromBody] SucursalDto sucursal)
        {
            sucursal.IdSucursal = id;
            var ok = _ln.ActualizarSucursal(sucursal);
            if (!ok) return BadRequest("No se pudo actualizar.");
            return Ok(_ln.ObtenerSucursalPorId(id));
        }

        [HttpDelete, Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            var ok = _ln.EliminarSucursal(id);
            return Ok(ok);
        }
    }
}
