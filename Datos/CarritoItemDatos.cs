using AccesoDatos;
using AccesoDatos.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class CarritoItemDatos
    {
        // Contexto de Entity Framework
        private readonly db31808Entities1 _context = new db31808Entities1();

        // ============================================================
        // 🟢 CREATE - Agregar un nuevo item al carrito
        // ============================================================
        public bool Agregar(CarritoItem nuevo)
        {
            try
            {
                _context.CarritoItem.Add(nuevo);
                _context.SaveChanges();
                return true; // ✅ Éxito
            }
            catch (Exception)
            {
                return false; // ❌ Error controlado
            }
        }

        // ============================================================
        // 🔵 READ - Listar todos los items del carrito
        // ============================================================
        public List<CarritoItemDto> ListarPorCarrito(int idCarrito)
        {
            return _context.CarritoItem
                .Where(i => i.id_carrito == idCarrito)
                .Select(i => new CarritoItemDto
                {
                    IdItem = i.id_item,
                    IdCarrito = i.id_carrito,
                    IdVehiculo = i.id_vehiculo,
                    VehiculoNombre = i.Vehiculo.marca + " " + i.Vehiculo.modelo, // unión con Vehículo
                    FechaInicio = i.fecha_inicio,
                    FechaFin = i.fecha_fin,
                    Subtotal = i.subtotal
                })
                .ToList();
        }

        // ============================================================
        // 🔍 READ - Obtener un item específico por su ID
        // ============================================================
        public CarritoItemDto ObtenerPorId(int id)
        {
            var i = _context.CarritoItem.Find(id);
            if (i == null) return null;

            return new CarritoItemDto
            {
                IdItem = i.id_item,
                IdCarrito = i.id_carrito,
                IdVehiculo = i.id_vehiculo,
                VehiculoNombre = i.Vehiculo.marca + " " + i.Vehiculo.modelo,
                FechaInicio = i.fecha_inicio,
                FechaFin = i.fecha_fin,
                Subtotal = i.subtotal
            };
        }

        // ============================================================
        // 🟠 UPDATE - Modificar datos de un item existente
        // ============================================================
        public bool Actualizar(CarritoItem mod)
        {
            var item = _context.CarritoItem.Find(mod.id_item);
            if (item == null) return false;

            item.id_vehiculo = mod.id_vehiculo;
            item.fecha_inicio = mod.fecha_inicio;
            item.fecha_fin = mod.fecha_fin;
            item.subtotal = mod.subtotal;

            _context.SaveChanges();
            return true;
        }

        // ============================================================
        // 🔴 DELETE - Eliminar un item del carrito
        // ============================================================
        public bool Eliminar(int id)
        {
            var item = _context.CarritoItem.Find(id);
            if (item == null) return false;

            _context.CarritoItem.Remove(item);
            _context.SaveChanges();
            return true;
        }

        // ============================================================
        // ⚙️ EXTRA - Eliminar todos los items de un carrito
        // ============================================================
        public bool VaciarCarrito(int idCarrito)
        {
            var items = _context.CarritoItem.Where(i => i.id_carrito == idCarrito).ToList();
            if (!items.Any()) return false;

            _context.CarritoItem.RemoveRange(items);
            _context.SaveChanges();
            return true;
        }
    }
}
