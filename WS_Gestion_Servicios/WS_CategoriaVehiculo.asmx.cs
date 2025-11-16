using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using System.Web.Services.Protocols;
using Logica;
using AccesoDatos.DTO;

namespace WS_Gestion_Servicios
{
    [WebService(Namespace = "http://rentaautos.ec/gestion")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WS_CategoriaVehiculo : WebService
    {
        private readonly CategoriaVehiculoLogica ln = new CategoriaVehiculoLogica();

        private SoapException Fault(string m, Exception ex = null)
        {
            return new SoapException(m, SoapException.ClientFaultCode, ex);
        }

        [WebMethod(Description = "Lista categorias")]
        public List<CategoriaVehiculoDto> obtenerCategoriasVehiculo()
        {
            try { return ln.ListarCategorias(); }
            catch (Exception ex) { throw Fault("No se pudo listar categorias", ex); }
        }

        [WebMethod(Description = "Crea una categoria")]
        public CategoriaVehiculoDto crearCategoriaVehiculo(CategoriaVehiculoDto categoria)
        {
            try
            {
                return ln.CrearCategoria(categoria);
            }
            catch (Exception ex)
            {
                throw Fault("No se pudo crear la categoria: " + ex.Message, ex);
            }
        }


        [WebMethod(Description = "Actualiza una categoria")]
        public CategoriaVehiculoDto actualizarCategoriaVehiculo(int idCategoria, CategoriaVehiculoDto categoria)
        {
            try
            {
                if (categoria == null) throw new ArgumentException("categoria requerida");
                categoria.IdCategoria = idCategoria;
                var ok = ln.ActualizarCategoria(categoria);
                if (!ok) throw new Exception("No se pudo actualizar la categoria");
                return ln.ObtenerPorId(idCategoria);
            }
            catch (Exception ex) { throw Fault("No se pudo actualizar la categoria", ex); }
        }

        [WebMethod(Description = "Elimina una categoria")]
        public bool eliminarCategoriaVehiculo(int idCategoria)
        {
            try { return ln.EliminarCategoria(idCategoria); }
            catch (Exception ex) { throw Fault("No se pudo eliminar la categoria", ex); }
        }

        [WebMethod(Description = "Lista transmisiones")]
        public List<string> listarTransmisiones()
        {
            try { return new List<string> { "Manual", "Automatico", "CVT" }; }
            catch (Exception ex) { throw Fault("No se pudo listar transmisiones", ex); }
        }

        [WebMethod(Description = "Lista combustibles")]
        public List<string> listarCombustibles()
        {
            try { return new List<string> { "Gasolina", "Diesel", "Hibrido", "Electrico" }; }
            catch (Exception ex) { throw Fault("No se pudo listar combustibles", ex); }
        }
    }
}