using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AccesoDatos.DTO;
using Logica;
using Datos;

namespace API_REST.Controllers
{
    [RoutePrefix("api/vehiculos")]
    public class VehiculosController : ApiController
    {
        private readonly VehiculoLogica _logic = new VehiculoLogica();
        private readonly ImagenVehiculoDatos _imgDatos = new ImagenVehiculoDatos();

        // ============================================================
        // 🟢 GET: Lista todos los vehículos
        // ============================================================
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll()
        {
            var lista = _logic.ListarVehiculos();

            // Adjuntar URL de imagen a cada vehículo
            foreach (var v in lista)
                v.UrlImagen = _imgDatos.ListarPorVehiculo(v.IdVehiculo).FirstOrDefault()?.UriImagen;

            return Ok(lista);
        }

        // ============================================================
        // 🔍 GET: Detalle por ID
        // ============================================================
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            var v = _logic.ObtenerVehiculoPorId(id);
            if (v == null) return NotFound();

            v.UrlImagen = _imgDatos.ListarPorVehiculo(id).FirstOrDefault()?.UriImagen;
            return Ok(v);
        }

        // ============================================================
        // 🔎 GET: Buscar por filtros
        // ============================================================
        [HttpGet]
        [Route("search")]
        public IHttpActionResult Search(string categoria = null, string transmision = null, string estado = null)
        {
            var lista = _logic.BuscarVehiculos(categoria, transmision, estado);

            foreach (var v in lista)
                v.UrlImagen = _imgDatos.ListarPorVehiculo(v.IdVehiculo).FirstOrDefault()?.UriImagen;

            return Ok(lista);
        }

        // ============================================================
        // 🟡 POST: Crear un nuevo vehículo
        // ============================================================
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody] VehiculoDto dto)
        {
            if (dto == null) return BadRequest("Body vacío o formato incorrecto.");

            var id = _logic.CrearVehiculo(dto);
            return Created($"{Request.RequestUri}/{id}", new { IdVehiculo = id });
        }

        // ============================================================
        // 🟠 PUT: Actualizar un vehículo
        // ============================================================
        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult Put(int id, [FromBody] VehiculoDto dto)
        {
            if (dto == null) return BadRequest("Body vacío o formato incorrecto.");
            dto.IdVehiculo = id;

            var ok = _logic.ActualizarVehiculo(dto);
            if (!ok) return NotFound();

            return Ok(new { actualizado = true });
        }

        // ============================================================
        // 🔴 DELETE: Eliminar un vehículo
        // ============================================================
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            var ok = _logic.EliminarVehiculo(id);
            if (!ok) return NotFound();

            return Ok(new { eliminado = true });
        }
    }
}