using System;
using System.Linq;
using System.Web.Services;
using Logica;
using AccesoDatos.DTO;

namespace WS_Gestion_Servicios   // 👈 Debe coincidir con Class="WS_Gestion_Servicios.Sucursales"
{
    [WebService(Namespace = "http://integracion.rentaautos.com/soap")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Sucursales : WebService   // 👈 Nombre de clase igual al del .asmx
    {
        private readonly SucursalLogica _ln = new SucursalLogica();

        [WebMethod(Description = "Lista puntos de recogida/entrega (ciudad, dirección, horarios).")]
        public SucursalDto[] obtenerSucursales()
        {
            var lista = _ln.ListarSucursales();
            return lista?.ToArray();
        }

        [WebMethod(Description = "Detalle de una sucursal.")]
        public SucursalDto obtenerSucursalPorId(int idSucursal)
        {
            return _ln.ObtenerSucursalPorId(idSucursal);
        }

        [WebMethod(Description = "Crea una sucursal de alquiler.")]
        public SucursalDto crearSucursal(SucursalDto sucursal)
        {
            var newId = _ln.CrearSucursal(sucursal);
            return _ln.ObtenerSucursalPorId(newId);
        }

        [WebMethod(Description = "Actualiza datos de la sucursal.")]
        public SucursalDto actualizarSucursal(int idSucursal, SucursalDto sucursal)
        {
            sucursal.IdSucursal = idSucursal;
            var ok = _ln.ActualizarSucursal(sucursal);
            if (!ok) throw new Exception("No se pudo actualizar la sucursal.");
            return _ln.ObtenerSucursalPorId(idSucursal);
        }

        [WebMethod(Description = "Elimina una sucursal.")]
        public bool eliminarSucursal(int idSucursal)
        {
            return _ln.EliminarSucursal(idSucursal);
        }
    }
}