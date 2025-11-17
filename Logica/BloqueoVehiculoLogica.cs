using AccesoDatos.DTO;
using Datos;
using System;
using System.Collections.Generic;

namespace Logica
{
    public class BloqueoVehiculoLogica
    {
        private readonly BloqueoVehiculoDatos datos = new BloqueoVehiculoDatos();

        public List<BloqueoVehiculoDto> ListarBloqueosPorVehiculo(int idVehiculo)
        {
            if (idVehiculo <= 0)
                throw new ArgumentException("El ID del vehículo debe ser válido.");

            return datos.ListarBloqueosPorVehiculo(idVehiculo);
        }

        public bool CrearBloqueo(BloqueoVehiculoDto dto)
        {
            if (dto.IdVehiculo <= 0 || dto.IdUsuario <= 0)
                throw new ArgumentException("El ID de usuario o vehículo no puede ser 0.");

            return datos.CrearBloqueo(dto);
        }

        public bool EliminarBloqueo(int idHold)
        {
            if (idHold <= 0)
                throw new ArgumentException("El ID del bloqueo debe ser válido.");

            return datos.EliminarBloqueo(idHold);
        }

        // 🔵 MÉTODO FALTANTE
        public BloqueoVehiculoDto ObtenerBloqueo(int idHold)
        {
            if (idHold <= 0)
                throw new ArgumentException("El ID del bloqueo debe ser válido.");

            return datos.ObtenerBloqueo(idHold);
        }
    }
}
