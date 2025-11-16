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
    public class CarritoItemLogica
    {
        private readonly CarritoItemDatos itemDatos = new CarritoItemDatos();

        // ============================================================
        // 🟢 CREATE - Agregar un nuevo ítem al carrito
        // ============================================================
        public bool AgregarItem(CarritoItemDto dto)
        {
            try
            {
                if (dto == null)
                    throw new ArgumentNullException("El objeto CarritoItemDto no puede ser nulo.");

                // Validaciones básicas
                if (dto.IdCarrito <= 0 || dto.IdVehiculo <= 0)
                    throw new Exception("El carrito y el vehículo deben ser válidos.");

                if (dto.FechaInicio >= dto.FechaFin)
                    throw new Exception("La fecha de inicio debe ser menor que la fecha de fin.");

                // Mapeo DTO → Entidad
                var entidad = new CarritoItem
                {
                    id_carrito = dto.IdCarrito,
                    id_vehiculo = dto.IdVehiculo,
                    fecha_inicio = dto.FechaInicio,
                    fecha_fin = dto.FechaFin,
                    subtotal = dto.Subtotal
                };

                // Llamada a la capa de datos
                return itemDatos.Agregar(entidad);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al agregar item: {ex.Message}");
                return false;
            }
        }

        // ============================================================
        // 🔵 READ - Listar todos los ítems de un carrito
        // ============================================================
        public List<CarritoItemDto> ListarPorCarrito(int idCarrito)
        {
            if (idCarrito <= 0)
                throw new ArgumentException("El ID del carrito debe ser válido.");

            return itemDatos.ListarPorCarrito(idCarrito);
        }

        // ============================================================
        // 🔍 READ - Obtener un ítem específico
        // ============================================================
        public CarritoItemDto ObtenerPorId(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID del ítem debe ser válido.");

            return itemDatos.ObtenerPorId(id);
        }

        // ============================================================
        // 🟠 UPDATE - Modificar un ítem existente
        // ============================================================
        public bool Actualizar(CarritoItemDto dto)
        {
            if (dto == null || dto.IdItem <= 0)
                throw new ArgumentException("Datos del ítem inválidos.");

            var entidad = new CarritoItem
            {
                id_item = dto.IdItem,
                id_carrito = dto.IdCarrito,
                id_vehiculo = dto.IdVehiculo,
                fecha_inicio = dto.FechaInicio,
                fecha_fin = dto.FechaFin,
                subtotal = dto.Subtotal
            };

            return itemDatos.Actualizar(entidad);
        }

        // ============================================================
        // 🔴 DELETE - Eliminar un ítem
        // ============================================================
        public bool Eliminar(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID del ítem debe ser válido.");

            return itemDatos.Eliminar(id);
        }

        // ============================================================
        // ⚙️ EXTRA - Vaciar todo el carrito
        // ============================================================
        public bool VaciarCarrito(int idCarrito)
        {
            if (idCarrito <= 0)
                throw new ArgumentException("El ID del carrito debe ser válido.");

            return itemDatos.VaciarCarrito(idCarrito);
        }

        // ============================================================
        // 🧮 EXTRA - Calcular subtotal total de un carrito
        // ============================================================
        public decimal CalcularTotalCarrito(int idCarrito)
        {
            var items = itemDatos.ListarPorCarrito(idCarrito);
            if (items == null || items.Count == 0) return 0;

            return items.Sum(i => i.Subtotal);
        }
    }
}
