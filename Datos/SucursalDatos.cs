using AccesoDatos;
using AccesoDatos.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class SucursalDatos
    {
        // 🔹 Instancia del contexto de Entity Framework
        private readonly db31808Entities1 _context = new db31808Entities1();

        // ============================================================
        // 🟢 CREATE - Crear una nueva sucursal
        // ============================================================
        public int Crear(Sucursal nueva)
        {
            _context.Sucursal.Add(nueva);   // Agrega la entidad al contexto
            _context.SaveChanges();          // Guarda los cambios en la BD
            return nueva.id_sucursal;        // Devuelve el ID generado
        }

        // ============================================================
        // 🔵 READ - Listar todas las sucursales
        // ============================================================
        public List<SucursalDto> Listar()
        {
            // Proyecta los datos del modelo hacia el DTO
            return _context.Sucursal.Select(s => new SucursalDto
            {
                IdSucursal = s.id_sucursal,
                Nombre = s.nombre,
                Ciudad = s.ciudad,
                Pais = s.pais,
                Direccion = s.direccion
            }).ToList();
        }

        // ============================================================
        // 🔍 READ (por ID) - Obtener una sucursal específica
        // ============================================================
        public SucursalDto ObtenerPorId(int id)
        {
            var s = _context.Sucursal.Find(id);
            if (s == null) return null;

            return new SucursalDto
            {
                IdSucursal = s.id_sucursal,
                Nombre = s.nombre,
                Ciudad = s.ciudad,
                Pais = s.pais,
                Direccion = s.direccion
            };
        }

        // ============================================================
        // 🟠 UPDATE - Actualizar una sucursal existente
        // ============================================================
        public bool Actualizar(Sucursal mod)
        {
            var s = _context.Sucursal.Find(mod.id_sucursal);
            if (s == null) return false;

            s.nombre = mod.nombre;
            s.ciudad = mod.ciudad;
            s.pais = mod.pais;
            s.direccion = mod.direccion;

            _context.SaveChanges(); // Aplica los cambios
            return true;
        }

        // ============================================================
        // 🔴 DELETE - Eliminar una sucursal
        // ============================================================
        public bool Eliminar(int id)
        {
            var s = _context.Sucursal.Find(id);
            if (s == null) return false;

            _context.Sucursal.Remove(s);
            _context.SaveChanges();
            return true;
        }

        // ============================================================
        // 🔎 MÉTODO EXTRA: Buscar sucursales por ciudad
        // ============================================================
        public List<SucursalDto> BuscarPorCiudad(string ciudad)
        {
            return _context.Sucursal
                .Where(s => s.ciudad.ToLower().Contains(ciudad.ToLower()))
                .Select(s => new SucursalDto
                {
                    IdSucursal = s.id_sucursal,
                    Nombre = s.nombre,
                    Ciudad = s.ciudad,
                    Pais = s.pais,
                    Direccion = s.direccion
                }).ToList();
        }
    }
}
