using AccesoDatos.DTO;
using Logica;
using System;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace Servicios
{
    /// <summary>
    /// Servicio SOAP para gestión de reservas.
    /// </summary>
    [WebService(Namespace = "http://rentaautos.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class WS_Reserva : WebService
    {
        private readonly ReservaLogica logica = new ReservaLogica();

        // ============================================================
        // 🔵 LISTAR TODAS LAS RESERVAS
        // ============================================================
        [WebMethod(Description = "Obtiene la lista completa de reservas registradas.")]
        public List<ReservaDto> ObtenerReservas()
        {
            try
            {
                return logica.ListarReservas();
            }
            catch (Exception ex)
            {
                throw new SoapException("Error al obtener reservas: " + ex.Message, SoapException.ServerFaultCode);
            }
        }

        // ============================================================
        // 🔍 OBTENER RESERVA POR ID
        // ============================================================
        [WebMethod(Description = "Obtiene el detalle de una reserva por su ID.")]
        public ReservaDto ObtenerReservaPorId(int idReserva)
        {
            try
            {
                return logica.ObtenerReservaPorId(idReserva);
            }
            catch (Exception ex)
            {
                throw new SoapException("Error al obtener la reserva: " + ex.Message, SoapException.ClientFaultCode);
            }
        }

        // ============================================================
        // 🟢 CREAR NUEVA RESERVA
        // ============================================================
        [WebMethod(Description = "Crea una nueva reserva con los datos proporcionados.")]
        public int CrearReserva(ReservaDto reserva)
        {
            try
            {
                return logica.CrearReserva(reserva);
            }
            catch (Exception ex)
            {
                throw new SoapException("Error al crear la reserva: " + ex.Message, SoapException.ClientFaultCode);
            }
        }

        // ============================================================
        // 🟠 ACTUALIZAR RESERVA EXISTENTE
        // ============================================================
        [WebMethod(Description = "Actualiza una reserva existente con la información enviada.")]
        public bool ActualizarReserva(ReservaDto reserva)
        {
            try
            {
                return logica.ActualizarReserva(reserva);
            }
            catch (Exception ex)
            {
                throw new SoapException("Error al actualizar la reserva: " + ex.Message, SoapException.ClientFaultCode);
            }
        }

        // ============================================================
        // 🔴 ELIMINAR RESERVA
        // ============================================================
        [WebMethod(Description = "Elimina una reserva existente por su ID.")]
        public bool EliminarReserva(int idReserva)
        {
            try
            {
                return logica.EliminarReserva(idReserva);
            }
            catch (Exception ex)
            {
                throw new SoapException("Error al eliminar la reserva: " + ex.Message, SoapException.ClientFaultCode);
            }
        }

        // ============================================================
        // 🧩 MÉTODO OPCIONAL: LISTAR POR USUARIO
        // ============================================================
        [WebMethod(Description = "Obtiene todas las reservas realizadas por un usuario específico.")]
        public List<ReservaDto> ObtenerReservasPorUsuario(int idUsuario)
        {
            try
            {
                return logica.ListarReservasPorUsuario(idUsuario);
            }
            catch (Exception ex)
            {
                throw new SoapException("Error al listar las reservas del usuario: " + ex.Message, SoapException.ClientFaultCode);
            }
        }

        // ============================================================
        // 🧾 MÉTODO OPCIONAL: CAMBIAR ESTADO
        // ============================================================
        [WebMethod(Description = "Cambia el estado de una reserva existente.")]
        public bool CambiarEstadoReserva(int idReserva, string nuevoEstado)
        {
            try
            {
                return logica.CambiarEstadoReserva(idReserva, nuevoEstado);
            }
            catch (Exception ex)
            {
                throw new SoapException("Error al cambiar el estado: " + ex.Message, SoapException.ClientFaultCode);
            }
        }
    }
}
