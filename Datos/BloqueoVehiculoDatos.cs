using AccesoDatos;
using AccesoDatos.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Datos
{
    public class BloqueoVehiculoDatos
    {
        private readonly db31808Entities1 _context = new db31808Entities1();

        // 🔵 LISTAR BLOQUEOS POR VEHÍCULO
        public List<BloqueoVehiculoDto> ListarBloqueosPorVehiculo(int idVehiculo)
        {
            return _context.Hold
                .Where(h => h.id_vehiculo == idVehiculo)
                .Select(h => new BloqueoVehiculoDto
                {
                    IdHold = h.id_hold,
                    IdUsuario = h.id_usuario,
                    IdVehiculo = h.id_vehiculo,
                    FechaInicio = h.fecha_inicio,
                    FechaExpiracion = h.fecha_expiracion,
                    MontoBloqueado = h.monto_bloqueado,
                    ReferenciaBanco = h.referencia_banco,
                    Estado = h.estado
                })
                .ToList();
        }

        // 🟢 CREAR BLOQUEO
        public bool CrearBloqueo(BloqueoVehiculoDto dto)
        {
            var entidad = new Hold
            {
                id_usuario = dto.IdUsuario,
                id_vehiculo = dto.IdVehiculo,
                fecha_inicio = dto.FechaInicio == DateTime.MinValue ? DateTime.Now : dto.FechaInicio,
                fecha_expiracion = dto.FechaExpiracion,
                monto_bloqueado = dto.MontoBloqueado,
                referencia_banco = dto.ReferenciaBanco,
                estado = string.IsNullOrEmpty(dto.Estado) ? "Activo" : dto.Estado
            };

            _context.Hold.Add(entidad);
            _context.SaveChanges();
            return true;
        }

        // 🔴 ELIMINAR BLOQUEO
        public bool EliminarBloqueo(int idHold)
        {
            var bloque = _context.Hold.Find(idHold);
            if (bloque == null) return false;

            _context.Hold.Remove(bloque);
            _context.SaveChanges();
            return true;
        }
        // 🔍 OBTENER BLOQUEO POR ID
        public BloqueoVehiculoDto ObtenerBloqueo(int idHold)
        {
            var h = _context.Hold.FirstOrDefault(x => x.id_hold == idHold);
            if (h == null) return null;

            return new BloqueoVehiculoDto
            {
                IdHold = h.id_hold,
                IdUsuario = h.id_usuario,
                IdVehiculo = h.id_vehiculo,
                FechaInicio = h.fecha_inicio,
                FechaExpiracion = h.fecha_expiracion,
                MontoBloqueado = h.monto_bloqueado,
                ReferenciaBanco = h.referencia_banco,
                Estado = h.estado
            };
        }

    }
}
