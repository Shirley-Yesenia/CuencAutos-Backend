using System;
using System.Web.Services;
using AccesoDatos.DTO;
using Logica;

namespace WS_Gestion_Servicios
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WS_CarritoDetalle : WebService
    {
        private readonly CarritoLogica _logica = new CarritoLogica();

        // ============================================================
        // 🔵 OBTENER DETALLE DEL CARRITO
        // SOAP: GetDetalle
        // ============================================================
        [WebMethod(Description = "Obtiene el detalle completo del carrito.")]
        public CarritoDetalleRespuestaDto ObtenerCarrito(int idCarrito)

        {
            var carrito = _logica.ObtenerCarritoConItems(idCarrito);
            return carrito; // SOAP devuelve el objeto directamente
        }

        // ============================================================
        // 🟢 AGREGAR VEHÍCULO AL CARRITO
        // SOAP: AgregarVehiculo
        // ============================================================
        [WebMethod(Description = "Agrega un vehículo al carrito.")]
        public string AgregarVehiculo(int idUsuario, int idVehiculo, DateTime fechaInicio, DateTime fechaFin)
        {
            bool ok = _logica.AgregarVehiculo(idUsuario, idVehiculo, fechaInicio, fechaFin);

            return ok ? "Vehículo agregado correctamente" :
                        "No se pudo agregar el vehículo al carrito.";
        }

        // ============================================================
        // 🟠 ACTUALIZAR FECHAS DE UN ITEM
        // SOAP: ActualizarItem
        // ============================================================
        [WebMethod(Description = "Actualiza las fechas de un item del carrito.")]
        public string ActualizarItem(int idItem, DateTime fechaInicio, DateTime fechaFin)
        {
            bool ok = _logica.ActualizarItem(idItem, fechaInicio, fechaFin);

            return ok ? "Item actualizado correctamente" :
                        "No se pudo actualizar el item.";
        }

        // ============================================================
        // 🔴 ELIMINAR ITEM
        // SOAP: DeleteItem
        // ============================================================
        [WebMethod(Description = "Elimina un item del carrito.")]
        public string DeleteItem(int idItem)
        {
            bool ok = _logica.EliminarItem(idItem);

            return ok ? "Item eliminado correctamente" :
                        "No se pudo eliminar el item.";
        }
    }
}
