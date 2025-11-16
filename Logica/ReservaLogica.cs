using AccesoDatos;
using AccesoDatos.DTO;
using Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class ReservaLogica
    {
        private readonly ReservaDatos datos = new ReservaDatos();

        // ============================================================
        // 🔵 LISTAR TODAS LAS RESERVAS
        // ============================================================
        public List<ReservaDto> ListarReservas()
        {
            return datos.Listar();
        }

        // ============================================================
        // 🔍 OBTENER RESERVA POR ID
        // ============================================================
        public ReservaDto ObtenerReservaPorId(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID de la reserva no es válido.");

            return datos.ObtenerPorId(id);
        }

        // ============================================================
        // 🟣 LISTAR RESERVAS POR USUARIO
        // ============================================================
        public List<ReservaDto> ListarReservasPorUsuario(int idUsuario)
        {
            if (idUsuario <= 0)
                throw new ArgumentException("El ID del usuario no es válido.");

            return datos.ListarPorUsuario(idUsuario);
        }

        // ============================================================
        // 🟢 CREAR NUEVA RESERVA
        // ============================================================
        public int CrearReserva(ReservaDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "Los datos de la reserva no pueden ser nulos.");

            if (dto.IdUsuario <= 0)
                throw new Exception("Debe especificar un usuario válido.");

            if (dto.IdVehiculo <= 0)
                throw new Exception("Debe especificar un vehículo válido.");

            if (dto.FechaInicio >= dto.FechaFin)
                throw new Exception("La fecha de inicio debe ser anterior a la de finalización.");

            // Validar disponibilidad antes de crear
            bool disponible = datos.ValidarDisponibilidad(dto.IdVehiculo, dto.FechaInicio, dto.FechaFin);
            if (!disponible)
                throw new Exception("El vehículo no está disponible en las fechas seleccionadas.");

            var entidad = new Reserva
            {
                id_usuario = dto.IdUsuario,
                id_vehiculo = dto.IdVehiculo,
                fecha_inicio = dto.FechaInicio,
                fecha_fin = dto.FechaFin,
                total = dto.Total,
                estado = dto.Estado ?? "Confirmada",
                fecha_reserva = DateTime.Now
            };

            return datos.Crear(entidad);
        }

        // ============================================================
        // 🟠 ACTUALIZAR RESERVA EXISTENTE
        // ============================================================
        public bool ActualizarReserva(ReservaDto dto)
        {
            if (dto == null || dto.IdReserva <= 0)
                throw new Exception("Datos inválidos para actualizar la reserva.");

            if (dto.FechaInicio >= dto.FechaFin)
                throw new Exception("La fecha de inicio debe ser anterior a la de finalización.");

            var entidad = new Reserva
            {
                id_reserva = dto.IdReserva,
                id_usuario = dto.IdUsuario,
                id_vehiculo = dto.IdVehiculo,
                fecha_inicio = dto.FechaInicio,
                fecha_fin = dto.FechaFin,
                total = dto.Total,
                estado = dto.Estado,
                fecha_reserva = dto.FechaReserva
            };

            return datos.Actualizar(entidad);
        }

        // ============================================================
        // 🔴 ELIMINAR RESERVA
        // ============================================================
        public bool EliminarReserva(int id)
        {
            if (id <= 0)
                throw new Exception("ID inválido para eliminar la reserva.");

            return datos.Eliminar(id);
        }

        // ============================================================
        // 🧩 VALIDAR DISPONIBILIDAD (REUTILIZA LA CAPA DE DATOS)
        // ============================================================
        public bool ValidarDisponibilidad(int idVehiculo, DateTime inicio, DateTime fin)
        {
            if (idVehiculo <= 0)
                throw new ArgumentException("Debe indicar un vehículo válido.");

            if (inicio >= fin)
                throw new Exception("El rango de fechas no es válido.");

            return datos.ValidarDisponibilidad(idVehiculo, inicio, fin);
        }

        // ============================================================
        // 🧾 MÉTODO EXTRA: CAMBIAR ESTADO DE RESERVA
        // ============================================================
        public bool CambiarEstadoReserva(int idReserva, string nuevoEstado)
        {
            if (idReserva <= 0)
                throw new Exception("ID de reserva inválido.");

            if (string.IsNullOrWhiteSpace(nuevoEstado))
                throw new Exception("Debe indicar un nuevo estado.");

            var reserva = datos.ObtenerPorId(idReserva);
            if (reserva == null)
                throw new Exception("No se encontró la reserva.");

            reserva.Estado = nuevoEstado;
            return ActualizarReserva(reserva);
        }
    }
}
