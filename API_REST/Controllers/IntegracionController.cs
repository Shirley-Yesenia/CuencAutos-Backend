using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Logica;
using AccesoDatos.DTO;
using AccesoDatos;

namespace API_REST.Controllers
{
    [RoutePrefix("api/integracion/autos")]
    public class IntegracionController : ApiController
    {
        private readonly VehiculoLogica _logic = new VehiculoLogica();
        // 🟢 Declaramos el contexto del Entity Framework
        private readonly db31808Entities1 _context = new db31808Entities1();

        // =====================================================
        // GET: /api/integracion/autos/search?categoria=1&transmision=2
        // =====================================================
        [HttpGet]
        [Route("search")]
        public IHttpActionResult BuscarAutos(int? categoria = null, int? transmision = null)
        {
            try
            {
                // 🟢 Traemos la lista de vehículos desde la capa lógica
                List<VehiculoDto> autos = _logic.ListarVehiculos();

                // 🔹 Filtros dinámicos
                if (categoria.HasValue)
                    autos = autos.Where(a => a.IdCategoria == categoria.Value).ToList();

                if (transmision.HasValue)
                    autos = autos.Where(a => a.IdTransmision == transmision.Value).ToList();

                if (autos.Count == 0)
                    return NotFound(); foreach (var v in autos)
                {
                    v.UrlImagen = new Logica.ImagenVehiculoLogica()
                        .ListarPorVehiculo(v.IdVehiculo)
                        .FirstOrDefault()?.UriImagen;
                }

                return Ok(autos);
            }
            catch (System.Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/catalogos/transmisiones")]
        public IHttpActionResult GetTransmisiones()
        {

            var lista = _context.TipoTransmision
                .Select(t => new { t.id_transmision, t.nombre })
                .ToList();

            return Ok(lista);
        }

    }
}