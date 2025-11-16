using AccesoDatos;
using AccesoDatos.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class ReservaDatos
    {
        // 🔹 Contexto de Entity Framework
        private readonly db31808Entities1 _context = new db31808Entities1();

        // ============================================================
        // 🟢 CREATE - Crear una nueva reserva
        // ============================================================
        public int Crear(Reserva nueva)
        {
            _context.Reserva.Add(nueva);     // Agrega la reserva al contexto
            _context.SaveChanges();          // Guarda en la BD
            return nueva.id_reserva;         // Retorna el ID generado
        }

        // ============================================================
        // 🔵 READ - Listar todas las reservas
        // ============================================================
        public List<ReservaDto> Listar()
        {
            var query = from r in _context.Reserva
                        join u in _context.Usuario on r.id_usuario equals u.id_usuario
                        join v in _context.Vehiculo on r.id_vehiculo equals v.id_vehiculo
                        select new ReservaDto
                        {
                            IdReserva = r.id_reserva,
                            IdUsuario = u.id_usuario,
                            NombreUsuario = u.nombre + " " + u.apellido,
                            IdVehiculo = v.id_vehiculo,
                            VehiculoNombre = v.marca + " " + v.modelo,
                            FechaInicio = r.fecha_inicio,
                            FechaFin = r.fecha_fin,
                            Total = r.total,
                            Estado = r.estado,
                            FechaReserva = r.fecha_reserva
                        };

            return query.ToList();
        }

        // ============================================================
        // 🔍 READ (por ID) - Obtener una reserva específica
        // ============================================================
        public ReservaDto ObtenerPorId(int id)
        {
            return Listar().FirstOrDefault(r => r.IdReserva == id);
        }

        // ============================================================
        // 🟣 READ (por Usuario) - Listar reservas de un usuario
        // ============================================================
        public List<ReservaDto> ListarPorUsuario(int idUsuario)
        {
            return Listar().Where(r => r.IdUsuario == idUsuario).ToList();
        }

        // ============================================================
        // 🟠 UPDATE - Actualizar una reserva existente
        // ============================================================
        public bool Actualizar(Reserva mod)
        {
            var r = _context.Reserva.Find(mod.id_reserva);
            if (r == null) return false;

            r.id_usuario = mod.id_usuario;
            r.id_vehiculo = mod.id_vehiculo;
            r.fecha_inicio = mod.fecha_inicio;
            r.fecha_fin = mod.fecha_fin;
            r.total = mod.total;
            r.estado = mod.estado;
            r.fecha_reserva = mod.fecha_reserva;

            _context.SaveChanges(); // Aplica los cambios
            return true;
        }

        // ============================================================
        // 🔴 DELETE - Eliminar una reserva
        // ============================================================
        public bool Eliminar(int id)
        {
            var r = _context.Reserva.Find(id);
            if (r == null) return false;

            _context.Reserva.Remove(r);
            _context.SaveChanges();
            return true;
        }


        // ============================================================
        // 🧩 MÉTODO EXTRA - Validar disponibilidad
        // ============================================================
        public bool ValidarDisponibilidad(int idVehiculo, DateTime inicio, DateTime fin)
        {
            // Busca si existe una reserva activa que choque con el rango de fechas
            bool hayConflicto = _context.Reserva.Any(r =>
                r.id_vehiculo == idVehiculo &&
                r.estado != "Cancelada" &&
                r.fecha_inicio < fin &&
                inicio < r.fecha_fin);

            // Si hay conflicto, retorna false (no disponible)
            return !hayConflicto;
        }
        // ============================================================
        // 🧩 Alias para compatibilidad con controladores REST
        // ============================================================
        public List<ReservaDto> ListarReservas()
        {
            var query = from r in _context.Reserva
                        join u in _context.Usuario on r.id_usuario equals u.id_usuario
                        join v in _context.Vehiculo on r.id_vehiculo equals v.id_vehiculo
                        select new ReservaDto
                        {
                            IdReserva = r.id_reserva,
                            IdUsuario = u.id_usuario,
                            NombreUsuario = u.nombre + " " + u.apellido,
                            IdVehiculo = v.id_vehiculo,
                            VehiculoNombre = v.marca + " " + v.modelo,
                            FechaInicio = r.fecha_inicio,
                            FechaFin = r.fecha_fin,
                            Total = r.total,
                            Estado = r.estado,
                            FechaReserva = r.fecha_reserva
                        };

            return query.ToList();
        }


    }
}
