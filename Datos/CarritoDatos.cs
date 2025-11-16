using AccesoDatos;
using AccesoDatos.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class CarritoDatos
    {
        // Contexto de Entity Framework
        private readonly db31808Entities1 _context = new db31808Entities1();

        // ============================================================
        // 🟢 CREATE - Crear un nuevo carrito
        // ============================================================
        public int Crear(Carrito nuevo)
        {
            _context.Carrito.Add(nuevo);      // Agrega la entidad al contexto
            _context.SaveChanges();           // Guarda los cambios
            return nuevo.id_carrito;          // Retorna el ID generado
        }

        // ============================================================
        // 🔵 READ - Listar todos los carritos
        // ============================================================
        public List<CarritoDto> Listar()
        {
            return _context.Carrito
                .Select(c => new CarritoDto
                {
                    IdCarrito = c.id_carrito,
                    IdUsuario = c.id_usuario,
                    FechaCreacion = c.fecha_creacion
                }).ToList();
        }

        // ============================================================
        // 🔍 READ (por ID) - Obtener un carrito específico
        // ============================================================
        public CarritoDto ObtenerPorId(int id)
        {
            var c = _context.Carrito.Find(id);
            if (c == null) return null;

            return new CarritoDto
            {
                IdCarrito = c.id_carrito,
                IdUsuario = c.id_usuario,
                FechaCreacion = c.fecha_creacion
            };
        }

        // ============================================================
        // 🟣 READ (por Usuario) - Obtener el carrito activo del usuario
        // ============================================================
        public CarritoDto ObtenerPorUsuario(int idUsuario)
        {
            var c = _context.Carrito
                .Where(x => x.id_usuario == idUsuario)
                .OrderByDescending(x => x.fecha_creacion)
                .FirstOrDefault();

            if (c == null) return null;

            return new CarritoDto
            {
                IdCarrito = c.id_carrito,
                IdUsuario = c.id_usuario,
                FechaCreacion = c.fecha_creacion
            };
        }

        // ============================================================
        // 🟠 UPDATE - Actualizar datos del carrito (raro, pero posible)
        // ============================================================
        public bool Actualizar(Carrito mod)
        {
            var c = _context.Carrito.Find(mod.id_carrito);
            if (c == null) return false;

            c.id_usuario = mod.id_usuario;
            c.fecha_creacion = mod.fecha_creacion;

            _context.SaveChanges();
            return true;
        }

        // ============================================================
        // 🔴 DELETE - Eliminar un carrito y sus items asociados
        // ============================================================
        public bool Eliminar(int id)
        {
            var c = _context.Carrito.Find(id);
            if (c == null) return false;

            // 🔸 Elimina los items del carrito antes (si hay FK)
            var items = _context.CarritoItem.Where(i => i.id_carrito == id).ToList();
            _context.CarritoItem.RemoveRange(items);

            _context.Carrito.Remove(c);
            _context.SaveChanges();
            return true;
        }

        // ============================================================
        // 🧩 MÉTODO EXTRA - Crear carrito si no existe
        // ============================================================
        public int ObtenerOCrear(int idUsuario)
        {
            var existente = _context.Carrito.FirstOrDefault(c => c.id_usuario == idUsuario);
            if (existente != null)
                return existente.id_carrito;

            var nuevo = new Carrito
            {
                id_usuario = idUsuario,
                fecha_creacion = DateTime.Now
            };
            _context.Carrito.Add(nuevo);
            _context.SaveChanges();

            return nuevo.id_carrito;
        }
    }
}
