using AccesoDatos;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace Datos
{
    public class PreReservaDatos
    {
        private readonly db31808Entities1 _context = new db31808Entities1();

        public bool ExisteVehiculo(string idVehiculo)
        {
            if (!int.TryParse(idVehiculo, out var id)) return false;

            var n = _context.Database.SqlQuery<int>(
                "SELECT COUNT(1) FROM Vehiculo WHERE id_vehiculo = @v",
                new SqlParameter("@v", id)
            ).FirstOrDefault();

            return n > 0;
        }

        // 🔥 AQUI SE AGREGA LA FUNCIÓN QUE TE FALTABA
        public bool ExisteHoldActivo(int idVehiculo, DateTime fechaInicio, DateTime fechaFin)
        {
            var sql = @"
                SELECT COUNT(1)
                FROM Hold
                WHERE id_vehiculo = @veh
                  AND estado = 'Activo'
                  AND fecha_expiracion > GETDATE()
                  AND (
                        (fecha_inicio <= @fin AND fecha_expiracion >= @ini)
                      )
            ";

            var n = _context.Database.SqlQuery<int>(
                sql,
                new SqlParameter("@veh", idVehiculo),
                new SqlParameter("@ini", fechaInicio),
                new SqlParameter("@fin", fechaFin)
            ).FirstOrDefault();

            return n > 0;
        }

        public int CrearHold(int idVehiculo, DateTime fechaInicio, DateTime fechaFin, int duracionSeg)
        {
            if (duracionSeg <= 0) duracionSeg = 600;

            var ahora = DateTime.Now;
            var expira = ahora.AddSeconds(duracionSeg);

            var precioDia = _context.Database.SqlQuery<decimal>(
                "SELECT precio_dia FROM Vehiculo WHERE id_vehiculo=@v",
                new SqlParameter("@v", idVehiculo)
            ).FirstOrDefault();

            var dias = Math.Max(1, (fechaFin.Date - fechaInicio.Date).Days);
            var monto = precioDia * dias;

            var idUsuario = 1; // placeholder pruebas

            var sql = @"
                INSERT INTO Hold (id_usuario, id_vehiculo, fecha_inicio, fecha_expiracion, monto_bloqueado, referencia_banco, estado)
                VALUES (@usr, @veh, @ini, @exp, @monto, NULL, 'Activo');
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            var idHold = _context.Database.SqlQuery<int>(
                sql,
                new SqlParameter("@usr", idUsuario),
                new SqlParameter("@veh", idVehiculo),
                new SqlParameter("@ini", fechaInicio),
                new SqlParameter("@exp", expira),
                new SqlParameter("@monto", monto)
            ).First();

            return idHold;
        }
    }
}
