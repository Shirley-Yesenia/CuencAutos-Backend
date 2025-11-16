using System;
using System.Collections.Generic;
using System.Web.Services;
using AccesoDatos.DTO;
using Logica;

namespace WS_Gestion_Servicios
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class WS_BloqueosVehiculo : WebService
    {
        private readonly BloqueoVehiculoLogica logica = new BloqueoVehiculoLogica();

        public WS_BloqueosVehiculo()
        {
            this.Server.ScriptTimeout = 600; // ⏱ tiempo máximo de espera: 600 segundos
        }

        // ============================================================
        // 🔵 LISTAR BLOQUEOS POR VEHÍCULO
        // ============================================================
        [WebMethod(Description = "Lista todos los bloqueos asociados a un vehículo.")]
        public List<BloqueoVehiculoDto> ListarBloqueosVehiculo(int idVehiculo)
        {
            return logica.ListarBloqueosPorVehiculo(idVehiculo);
        }

        // ============================================================
        // 🟢 CREAR BLOQUEO
        // ============================================================
        [WebMethod(Description = "Crea un nuevo bloqueo de disponibilidad para un vehículo.")]
        public bool CrearBloqueoVehiculo(BloqueoVehiculoDto bloqueo)
        {
            return logica.CrearBloqueo(bloqueo);
        }

        // ============================================================
        // 🔴 ELIMINAR BLOQUEO
        // ============================================================
        [WebMethod(Description = "Elimina un bloqueo activo de un vehículo.")]
        public bool EliminarBloqueoVehiculo(int idHold)
        {
            return logica.EliminarBloqueo(idHold);
        }
    }
}