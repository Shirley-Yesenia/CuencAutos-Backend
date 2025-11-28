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
    public class CarritoLogica
    {
        private readonly CarritoDatos carritoDatos = new CarritoDatos();
        private readonly CarritoItemDatos itemDatos = new CarritoItemDatos();
        private readonly VehiculoDatos vehiculoDatos = new VehiculoDatos();
        private readonly ReservaDatos reservaDatos = new ReservaDatos();

        public CarritoDetalleRespuestaDto ObtenerCarritoConItems(int idCarrito)
        {
            var carrito = carritoDatos.ObtenerPorId(idCarrito);
            if (carrito == null)
                return null;

            var items = itemDatos.ListarPorCarrito(idCarrito);

            return new CarritoDetalleRespuestaDto
            {
                IdCarrito = carrito.IdCarrito,
                IdUsuario = carrito.IdUsuario,
                FechaCreacion = carrito.FechaCreacion,
                Items = items
            };
        }


        // ============================================================
        // 🔵 LISTAR TODOS LOS CARRITOS
        // ============================================================
        public List<CarritoDto> ListarCarritos()
        {
            return carritoDatos.Listar();
        }

        // ============================================================
        // 🔍 OBTENER CARRITO POR ID
        // ============================================================
        public CarritoDto ObtenerCarritoPorId(int idCarrito)
        {
            if (idCarrito <= 0)
                throw new ArgumentException("El ID del carrito no es válido.");

            return carritoDatos.ObtenerPorId(idCarrito);
        }

        // ============================================================
        // 🧍 OBTENER CARRITO POR USUARIO (y crearlo si no existe)
        // ============================================================
        public CarritoDto ObtenerOCrearCarrito(int idUsuario)
        {
            if (idUsuario <= 0)
                throw new ArgumentException("El ID de usuario no es válido.");

            int idCarrito = carritoDatos.ObtenerOCrear(idUsuario);
            return carritoDatos.ObtenerPorId(idCarrito);
        }

        // ============================================================
        // 🟢 AGREGAR VEHÍCULO AL CARRITO
        // ============================================================
        public bool AgregarVehiculo(int idUsuario, int idVehiculo, DateTime inicio, DateTime fin)
        {
            if (idUsuario <= 0 || idVehiculo <= 0)
                throw new Exception("Datos inválidos.");

            if (inicio >= fin)
                throw new Exception("La fecha de inicio debe ser anterior a la de fin.");

            // ✅ Validar disponibilidad del vehículo
            bool disponible = reservaDatos.ValidarDisponibilidad(idVehiculo, inicio, fin);
            if (!disponible)
                throw new Exception("El vehículo no está disponible en las fechas seleccionadas.");

            // ✅ Obtener el carrito del usuario (lo crea si no existe)
            int idCarrito = carritoDatos.ObtenerOCrear(idUsuario);

            // ✅ Obtener el vehículo y calcular subtotal
            var veh = vehiculoDatos.ObtenerPorId(idVehiculo);
            if (veh == null)
                throw new Exception("No se encontró el vehículo seleccionado.");

            int dias = (int)(fin - inicio).TotalDays;
            if (dias <= 0) dias = 1; // mínimo 1 día

            decimal subtotal = veh.PrecioDia * dias;

            // Crear el item
            var item = new CarritoItem
            {
                id_carrito = idCarrito,
                id_vehiculo = idVehiculo,
                fecha_inicio = inicio,
                fecha_fin = fin,
                subtotal = subtotal
            };

            return itemDatos.Agregar(item);
        }

        // ============================================================
        // 🟣 OBTENER TODOS LOS ITEMS DE UN CARRITO
        // ============================================================
        public List<CarritoItemDto> ListarItems(int idCarrito)
        {
            if (idCarrito <= 0)
                throw new ArgumentException("El ID del carrito no es válido.");

            return itemDatos.ListarPorCarrito(idCarrito);
        }

        // ============================================================
        // 🟠 ACTUALIZAR FECHAS DE UN ITEM DEL CARRITO
        // ============================================================
        public bool ActualizarItem(int idItem, DateTime nuevaInicio, DateTime nuevaFin)
        {
            if (nuevaInicio >= nuevaFin)
                throw new Exception("La fecha de inicio debe ser anterior a la de fin.");

            var item = itemDatos.ObtenerPorId(idItem);
            if (item == null)
                throw new Exception("No se encontró el ítem del carrito.");

            // Recalcular subtotal
            int dias = (int)(nuevaFin - nuevaInicio).TotalDays;
            if (dias <= 0) dias = 1;

            var vehiculo = vehiculoDatos.ObtenerPorId(item.IdVehiculo);
            if (vehiculo == null)
                throw new Exception("No se pudo obtener información del vehículo.");

            item.FechaInicio = nuevaInicio;
            item.FechaFin = nuevaFin;
            item.Subtotal = vehiculo.PrecioDia * dias;

            // Convertir de DTO a entidad
            var entidad = new CarritoItem
            {
                id_item = idItem,
                id_carrito = item.IdCarrito,
                id_vehiculo = item.IdVehiculo,
                fecha_inicio = nuevaInicio,
                fecha_fin = nuevaFin,
                subtotal = item.Subtotal
            };

            return itemDatos.Actualizar(entidad);
        }

        // ============================================================
        // 🔴 ELIMINAR ITEM DEL CARRITO
        // ============================================================
        public bool EliminarItem(int idItem)
        {
            if (idItem <= 0)
                throw new ArgumentException("ID inválido del item.");

            return itemDatos.Eliminar(idItem);
        }

        // ============================================================
        // 🔴 ELIMINAR CARRITO COMPLETO
        // ============================================================
        public bool EliminarCarrito(int idCarrito)
        {
            if (idCarrito <= 0)
                throw new ArgumentException("El ID del carrito no es válido.");

            return carritoDatos.Eliminar(idCarrito);
        }

        // ============================================================
        // 💰 CALCULAR TOTAL DEL CARRITO
        // ============================================================
        public decimal CalcularTotal(int idCarrito)
        {
            var items = itemDatos.ListarPorCarrito(idCarrito);
            return items.Sum(i => i.Subtotal);
        }
        // ============================================================
        // 🆕 Crear carrito para usuario (usado por UsuarioLogica)
        // ============================================================
        public int CrearCarritoParaUsuario(int idUsuario)
        {
            if (idUsuario <= 0)
                throw new ArgumentException("ID de usuario inválido.");

            // Usa el método existente que ya crea el carrito si no existe
            return carritoDatos.ObtenerOCrear(idUsuario);
        }


        // ============================================================
        // 🧩 CONVERTIR CARRITO A RESERVA (checkout)
        // ============================================================
        public int GenerarReservaDesdeCarrito(int idCarrito)
        {
            var carrito = carritoDatos.ObtenerPorId(idCarrito);
            if (carrito == null)
                throw new Exception("No se encontró el carrito.");

            var items = itemDatos.ListarPorCarrito(idCarrito);
            if (items.Count == 0)
                throw new Exception("El carrito está vacío.");

            // Tomamos el primer vehículo del carrito (para este modelo simple)
            var item = items.First();
            var reserva = new Reserva
            {
                id_usuario = carrito.IdUsuario,
                id_vehiculo = item.IdVehiculo,
                fecha_inicio = item.FechaInicio,
                fecha_fin = item.FechaFin,
                total = item.Subtotal,
                estado = "Confirmada",
                fecha_reserva = DateTime.Now
            };

            // Guardar reserva
            return reservaDatos.Crear(reserva);
        }
    }
}
