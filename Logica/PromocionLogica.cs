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
    public class PromocionLogica
    {
        private readonly PromocionDatos datos = new PromocionDatos();

        // ============================================================
        // 🔵 LISTAR TODAS LAS PROMOCIONES
        // ============================================================
        public List<PromocionDto> ListarPromociones()
        {
            return datos.ListarPromociones();
        }

        // ============================================================
        // 🟣 LISTAR PROMOCIONES ACTIVAS
        // ============================================================
        public List<PromocionDto> ListarPromocionesActivas()
        {
            return datos.ListarPromocionesActivas();
        }

        // ============================================================
        // 🔍 OBTENER PROMOCIÓN POR ID
        // ============================================================
        public PromocionDto ObtenerPromocionPorId(int idPromocion)
        {
            if (idPromocion <= 0)
                throw new ArgumentException("El ID de la promoción no es válido.");

            return datos.ObtenerPorId(idPromocion);
        }

        // ============================================================
        // 🟢 CREAR NUEVA PROMOCIÓN
        // ============================================================
        public int CrearPromocion(PromocionDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "Los datos de la promoción no pueden ser nulos.");

            if (string.IsNullOrWhiteSpace(dto.Nombre))
                throw new Exception("Debe ingresar un nombre para la promoción.");

            if (dto.PorcentajeDescuento <= 0 || dto.PorcentajeDescuento > 100)
                throw new Exception("El porcentaje de descuento debe estar entre 1 y 100.");

            if (dto.FechaInicio >= dto.FechaFin)
                throw new Exception("La fecha de inicio debe ser anterior a la de finalización.");

            // ✅ Verificar que no haya superposición de fechas con otras promociones activas
            var activas = datos.ListarPromocionesActivas();
            bool superpone = activas.Any(p =>
                (dto.FechaInicio <= p.FechaFin && dto.FechaFin >= p.FechaInicio));

            if (superpone)
                throw new Exception("Ya existe una promoción activa en el rango de fechas seleccionado.");

            // Crear entidad
            var entidad = new Promocion
            {
                nombre = dto.Nombre,
                descripcion = dto.Descripcion,
                porcentaje_descuento = dto.PorcentajeDescuento,
                fecha_inicio = dto.FechaInicio,
                fecha_fin = dto.FechaFin
            };

            return datos.CrearPromocion(entidad);
        }

        // ============================================================
        // 🟠 ACTUALIZAR PROMOCIÓN EXISTENTE
        // ============================================================
        public bool ActualizarPromocion(PromocionDto dto)
        {
            if (dto == null || dto.IdPromocion <= 0)
                throw new Exception("Datos inválidos para actualizar la promoción.");

            if (dto.PorcentajeDescuento <= 0 || dto.PorcentajeDescuento > 100)
                throw new Exception("El porcentaje de descuento debe estar entre 1 y 100.");

            if (dto.FechaInicio >= dto.FechaFin)
                throw new Exception("La fecha de inicio debe ser anterior a la de finalización.");

            var entidad = new Promocion
            {
                id_promocion = dto.IdPromocion,
                nombre = dto.Nombre,
                descripcion = dto.Descripcion,
                porcentaje_descuento = dto.PorcentajeDescuento,
                fecha_inicio = dto.FechaInicio,
                fecha_fin = dto.FechaFin
            };

            return datos.ActualizarPromocion(entidad);
        }

        // ============================================================
        // 🔴 ELIMINAR PROMOCIÓN
        // ============================================================
        public bool EliminarPromocion(int idPromocion)
        {
            if (idPromocion <= 0)
                throw new Exception("ID inválido para eliminar la promoción.");

            return datos.EliminarPromocion(idPromocion);
        }

        // ============================================================
        // 🧮 EXTRA - Calcular descuento sobre un monto
        // ============================================================
        public decimal AplicarDescuento(decimal montoOriginal, int idPromocion)
        {
            if (montoOriginal <= 0)
                throw new Exception("El monto original debe ser mayor que 0.");

            var promo = datos.ObtenerPorId(idPromocion);
            if (promo == null)
                throw new Exception("No se encontró la promoción.");

            DateTime hoy = DateTime.Now;
            if (hoy < promo.FechaInicio || hoy > promo.FechaFin)
                throw new Exception("La promoción no está vigente actualmente.");

            decimal descuento = montoOriginal * (promo.PorcentajeDescuento / 100);
            return montoOriginal - descuento;
        }
    }
}
