using System.Collections.Generic;
using System.Linq;
using AccesoDatos; // Este namespace es el que tiene tu contexto db31809Entities

namespace Datos
{
    /// <summary>
    /// Capa de acceso a datos para la entidad TipoTransmision.
    /// </summary>
    public class TipoTransmisionDatos
    {
        // Contexto de la base de datos (Entity Framework)
        private readonly db31808Entities1 _context = new db31808Entities1();

        // ======================================================
        // 🔹 READ: Listar todos los tipos de transmisión
        // ======================================================
        public List<TipoTransmision> Listar()
        {
            return _context.TipoTransmision.ToList();
        }

        // ======================================================
        // 🔹 READ: Buscar por ID
        // ======================================================
        public TipoTransmision BuscarPorId(int id)
        {
            return _context.TipoTransmision.Find(id);
        }

        // ======================================================
        // 🔹 CREATE: Insertar nuevo tipo de transmisión
        // ======================================================
        public bool Insertar(TipoTransmision tipo)
        {
            _context.TipoTransmision.Add(tipo);
            _context.SaveChanges();
            return true;
        }

        // ======================================================
        // 🔹 UPDATE: Actualizar un tipo existente
        // ======================================================
        public bool Actualizar(TipoTransmision tipo)
        {
            var obj = _context.TipoTransmision.Find(tipo.id_transmision);
            if (obj == null) return false;

            obj.nombre = tipo.nombre;
            obj.descripcion = tipo.descripcion;

            _context.SaveChanges();
            return true;
        }

        // ======================================================
        // 🔹 DELETE: Eliminar un tipo de transmisión
        // ======================================================
        public bool Eliminar(int id)
        {
            var obj = _context.TipoTransmision.Find(id);
            if (obj == null) return false;

            _context.TipoTransmision.Remove(obj);
            _context.SaveChanges();
            return true;
        }
    }
}
