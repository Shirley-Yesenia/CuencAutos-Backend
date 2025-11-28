using System.Web.Http;
using System.Web.Http.Cors;
using Logica;
using AccesoDatos.DTO;
using API_REST_GESTION.Hateoas.Builders;

namespace API_REST_GESTION.Controllers
{
    [RoutePrefix("api/v1/sucursales")]
    public class SucursalesController : ApiController
    {
        private readonly SucursalLogica _ln = new SucursalLogica();
        private readonly SucursalesHateoas _hateoas;

        public SucursalesController()
        {
            // Puedes cambiarlo si usas otra ruta base
            _hateoas = new SucursalesHateoas("http://localhost:5000");
        }

        // ==========================================================
        // GET: /api/v1/sucursales
        // ==========================================================
        [HttpGet, Route("")]
        public IHttpActionResult GetAll()
        {
            var lista = _ln.ListarSucursales();
            return Ok(_hateoas.Build(lista));
        }

        // ==========================================================
        // GET: /api/v1/sucursales/{id}
        // ==========================================================
        [HttpGet, Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            var sucursal = _ln.ObtenerSucursalPorId(id);
            if (sucursal == null) return NotFound();

            return Ok(_hateoas.Build(sucursal, id));
        }

        // ==========================================================
        // POST: /api/v1/sucursales
        // ==========================================================
        [HttpPost, Route("")]
        public IHttpActionResult Post([FromBody] SucursalDto sucursal)
        {
            var newId = _ln.CrearSucursal(sucursal);
            var data = _ln.ObtenerSucursalPorId(newId);

            return Ok(_hateoas.Build(data, newId));
        }

        // ==========================================================
        // PUT: /api/v1/sucursales/{id}
        // ==========================================================
        [HttpPut, Route("{id:int}")]
        public IHttpActionResult Put(int id, [FromBody] SucursalDto sucursal)
        {
            sucursal.IdSucursal = id;

            var updated = _ln.ActualizarSucursal(sucursal);
            if (!updated) return BadRequest("No se pudo actualizar.");

            var data = _ln.ObtenerSucursalPorId(id);
            return Ok(_hateoas.Build(data, id));
        }

        // ==========================================================
        // DELETE: /api/v1/sucursales/{id}
        // ==========================================================
        [HttpDelete, Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            var ok = _ln.EliminarSucursal(id);
            return Ok(_hateoas.Build(ok, id));
        }
    }
}
