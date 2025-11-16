using AccesoDatos;
using AccesoDatos.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class PromocionDatos
    {
        private readonly db31808Entities1 _context = new db31808Entities1();

        // ============================================================
        // 🟢 CREATE - Registrar una nueva promoción
        // ============================================================
        public int CrearPromocion(Promocion nueva)
        {
            _context.Promocion.Add(nueva);
            _context.SaveChanges();
            return nueva.id_promocion; // Retorna el ID generado
        }

        // ============================================================
        // 🔵 READ - Listar todas las promociones
        // ============================================================
        public List<PromocionDto> ListarPromociones()
        {
            return _context.Promocion
                .Select(p => new PromocionDto
                {
                    IdPromocion = p.id_promocion,
                    Nombre = p.nombre,
                    Descripcion = p.descripcion,
                    PorcentajeDescuento = p.porcentaje_descuento ?? 0, // Manejo seguro de nullables
                    FechaInicio = p.fecha_inicio ?? DateTime.MinValue,
                    FechaFin = p.fecha_fin ?? DateTime.MinValue
                })
                .ToList();
        }

        // ============================================================
        // 🔍 READ - Obtener una promoción por su ID
        // ============================================================
        public PromocionDto ObtenerPorId(int idPromocion)
        {
            var p = _context.Promocion.Find(idPromocion);
            if (p == null) return null;

            return new PromocionDto
            {
                IdPromocion = p.id_promocion,
                Nombre = p.nombre,
                Descripcion = p.descripcion,
                PorcentajeDescuento = p.porcentaje_descuento ?? 0,
                FechaInicio = p.fecha_inicio ?? DateTime.MinValue,
                FechaFin = p.fecha_fin ?? DateTime.MinValue
            };
        }

        // ============================================================
        // 🟠 UPDATE - Actualizar una promoción existente
        // ============================================================
        public bool ActualizarPromocion(Promocion promoEditada)
        {
            var existente = _context.Promocion.Find(promoEditada.id_promocion);
            if (existente == null) return false;

            existente.nombre = promoEditada.nombre;
            existente.descripcion = promoEditada.descripcion;
            existente.porcentaje_descuento = promoEditada.porcentaje_descuento;
            existente.fecha_inicio = promoEditada.fecha_inicio;
            existente.fecha_fin = promoEditada.fecha_fin;

            _context.SaveChanges();
            return true;
        }

        // ============================================================
        // 🔴 DELETE - Eliminar una promoción
        // ============================================================
        public bool EliminarPromocion(int idPromocion)
        {
            var promo = _context.Promocion.Find(idPromocion);
            if (promo == null) return false;

            _context.Promocion.Remove(promo);
            _context.SaveChanges();
            return true;
        }

        // ============================================================
        // ⚙️ EXTRA - Listar promociones activas (vigentes hoy)
        // ============================================================
        public List<PromocionDto> ListarPromocionesActivas()
        {
            DateTime hoy = DateTime.Now;

            return _context.Promocion
                .Where(p => p.fecha_inicio <= hoy && p.fecha_fin >= hoy)
                .Select(p => new PromocionDto
                {
                    IdPromocion = p.id_promocion,
                    Nombre = p.nombre,
                    Descripcion = p.descripcion,
                    PorcentajeDescuento = p.porcentaje_descuento ?? 0,
                    FechaInicio = p.fecha_inicio ?? DateTime.MinValue,
                    FechaFin = p.fecha_fin ?? DateTime.MinValue
                })
                .ToList();
        }
    }
}
