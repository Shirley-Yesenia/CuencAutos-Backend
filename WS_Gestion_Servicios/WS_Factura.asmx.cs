using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using AccesoDatos.DTO;
using AccesoDatos;
using Datos;         
using Logica;

namespace WS_RentaAutos
{
    /// <summary>
    /// Servicio SOAP para gestión de Facturas (con Entity Framework).
    /// </summary>
    [WebService(Namespace = "http://rentaautos.com.ec/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class WS_Factura : System.Web.Services.WebService
    {
        // ============================================================
        // 🧱 Contexto de base de datos (Entity Framework)
        // ============================================================
        private readonly db31808Entities1 _context = new db31808Entities1();

        // ============================================================
        // 🧩 Lógica de negocio (usa datos y DTO)
        // ============================================================
        private readonly FacturaLogica _logica = new FacturaLogica();

        // ============================================================
        // 🔵 GET /api/facturas
        // ============================================================
        [WebMethod(Description = "Obtiene la lista de todas las facturas emitidas.")]
        public List<FacturaDto> obtenerFacturas()
        {
            try
            {
                // Puedes llamar directo a la lógica o al contexto según prefieras
                return _logica.ListarFacturas();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las facturas: " + ex.Message);
            }
        }

        // ============================================================
        // 🔍 GET /api/facturas/{idFactura}
        // ============================================================
        [WebMethod(Description = "Obtiene el detalle de una factura por su ID.")]
        public FacturaDto obtenerFacturaPorId(int idFactura)
        {
            try
            {
                return _logica.ObtenerFacturaPorId(idFactura);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la factura: " + ex.Message);
            }
        }

        // ============================================================
        // 🟢 POST /api/facturas
        // ============================================================
        [WebMethod(Description = "Crea una nueva factura asociada a una reserva confirmada.")]
        public int crearFactura(FacturaDto factura)
        {
            try
            {
                return _logica.CrearFactura(factura);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear la factura: " + ex.Message);
            }
        }

        // ============================================================
        // 🟠 PUT /api/facturas/{idFactura}
        // ============================================================
        [WebMethod(Description = "Actualiza los datos de una factura existente.")]
        public bool actualizarFactura(int idFactura, FacturaDto factura)
        {
            try
            {
                if (factura == null)
                    throw new Exception("Debe proporcionar los datos de la factura.");

                factura.IdFactura = idFactura;
                return _logica.ActualizarFactura(factura);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar la factura: " + ex.Message);
            }
        }

        // ============================================================
        // 🔴 DELETE /api/facturas/{idFactura}
        // ============================================================
        [WebMethod(Description = "Elimina una factura por su ID.")]
        public bool eliminarFactura(int idFactura)
        {
            try
            {
                return _logica.EliminarFactura(idFactura);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar la factura: " + ex.Message);
            }
        }
    }
}
