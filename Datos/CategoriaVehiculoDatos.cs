using AccesoDatos;
using AccesoDatos.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class CategoriaVehiculoDatos
    {
        // 🔹 Instancia del contexto de Entity Framework
        private readonly db31808Entities1 _context = new db31808Entities1();

        // ============================================================
        // 🟢 CREATE - Crear una nueva categoría
        // ============================================================
        public int Crear(CategoriaVehiculo nueva)
        {
            _context.CategoriaVehiculo.Add(nueva); // Agrega la entidad al contexto
            _context.SaveChanges();                 // Guarda cambios en la BD
            return nueva.id_categoria;              // Retorna el ID generado
        }

        // ============================================================
        // 🔵 READ - Listar todas las categorías
        // ============================================================
        public List<CategoriaVehiculoDto> Listar()
        {
            // Proyección a DTO para no exponer entidades del modelo
            return _context.CategoriaVehiculo.Select(c => new CategoriaVehiculoDto
            {
                IdCategoria = c.id_categoria,
                Nombre = c.nombre,
                Descripcion = c.descripcion
            }).ToList();
        }

        // ============================================================
        // 🔍 READ (por ID) - Obtener una categoría específica
        // ============================================================
        public CategoriaVehiculoDto ObtenerPorId(int id)
        {
            var categoria = _context.CategoriaVehiculo.Find(id);

            if (categoria == null)
                return null;

            return new CategoriaVehiculoDto
            {
                IdCategoria = categoria.id_categoria,
                Nombre = categoria.nombre,
                Descripcion = categoria.descripcion
            };
        }

        // ============================================================
        // 🟠 UPDATE - Actualizar una categoría existente
        // ============================================================
        public bool Actualizar(CategoriaVehiculo mod)
        {
            var categoria = _context.CategoriaVehiculo.Find(mod.id_categoria);
            if (categoria == null) return false;

            categoria.nombre = mod.nombre;
            categoria.descripcion = mod.descripcion;

            _context.SaveChanges(); // Aplica los cambios en la BD
            return true;
        }

        // ============================================================
        // 🔴 DELETE - Eliminar una categoría
        // ============================================================
        public bool Eliminar(int id)
        {
            var categoria = _context.CategoriaVehiculo.Find(id);
            if (categoria == null) return false;

            _context.CategoriaVehiculo.Remove(categoria);
            _context.SaveChanges();
            return true;
        }
    }
}
