using AccesoDatos;
using AccesoDatos.DTO;
using System.Collections.Generic;

namespace Logica
{
    public class HoldLogica
    {
        private readonly HoldDatos datos = new HoldDatos();

        public HoldDto ObtenerPorId(int idHold)
        {
            var entidad = datos.ObtenerPorId(idHold);
            if (entidad == null) return null;

            return new HoldDto
            {
                IdHold = entidad.id_hold,
                IdUsuario = entidad.id_usuario,
                IdVehiculo = entidad.id_vehiculo,
                FechaInicio = entidad.fecha_inicio,
                FechaExpiracion = entidad.fecha_expiracion,
                MontoBloqueado = entidad.monto_bloqueado,
                ReferenciaBanco = entidad.referencia_banco,
                Estado = entidad.estado
            };
        }

        public bool CambiarEstado(int idHold, string nuevoEstado)
        {
            return datos.CambiarEstado(idHold, nuevoEstado);
        }

        public List<HoldDto> ListarActivos()
        {
            var lista = datos.ListarActivos();
            var resultado = new List<HoldDto>();

            foreach (var h in lista)
            {
                resultado.Add(new HoldDto
                {
                    IdHold = h.id_hold,
                    IdUsuario = h.id_usuario,
                    IdVehiculo = h.id_vehiculo,
                    FechaInicio = h.fecha_inicio,
                    FechaExpiracion = h.fecha_expiracion,
                    MontoBloqueado = h.monto_bloqueado,
                    ReferenciaBanco = h.referencia_banco,
                    Estado = h.estado
                });
            }

            return resultado;
        }
    }
}
