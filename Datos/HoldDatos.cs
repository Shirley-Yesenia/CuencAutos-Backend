using System;
using System.Collections.Generic;
using System.Linq;

namespace AccesoDatos
{
    public class HoldDatos
    {
        public Hold ObtenerPorId(int idHold)
        {
            using (var db = new db31808Entities1())
            {
                return db.Hold.FirstOrDefault(h => h.id_hold == idHold);
            }
        }

        public List<Hold> ListarActivos()
        {
            using (var db = new db31808Entities1())
            {
                return db.Hold.Where(h => h.estado == "Activo").ToList();
            }
        }

        public bool Crear(Hold nuevo)
        {
            using (var db = new db31808Entities1())
            {
                db.Hold.Add(nuevo);
                db.SaveChanges();
                return true;
            }
        }

        public bool CambiarEstado(int idHold, string nuevoEstado)
        {
            using (var db = new db31808Entities1())
            {
                var hold = db.Hold.FirstOrDefault(h => h.id_hold == idHold);
                if (hold == null) return false;

                hold.estado = nuevoEstado;
                db.SaveChanges();
                return true;
            }
        }
    }
}

