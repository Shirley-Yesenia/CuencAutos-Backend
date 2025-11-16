using AccesoDatos;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace Datos
{
    public class PreReservaDatos
    {
        // Usa el nombre real de tu DbContext (lo viste en Model1.Context.cs)
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

        public int CrearHold(int idVehiculo, DateTime fechaInicio, DateTime fechaFin, int duracionSeg)
        {
            if (duracionSeg <= 0) duracionSeg = 600;

            var ahora = DateTime.Now;
            var expira = ahora.AddSeconds(duracionSeg);

            // Precio/día del vehículo
            var precioDia = _context.Database.SqlQuery<decimal>(
                "SELECT precio_dia FROM Vehiculo WHERE id_vehiculo=@v",
                new SqlParameter("@v", idVehiculo)
            ).FirstOrDefault();

            var dias = Math.Max(1, (fechaFin.Date - fechaInicio.Date).Days);
            var monto = precioDia * dias;

            // Usuario placeholder para pruebas (ajusta si tienes sesión)
            var idUsuario = 1;

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