using AccesoDatos;
using AccesoDatos.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class PagoDatos
    {
        private readonly db31808Entities1 _context = new db31808Entities1();

        // ============================================================
        // 🟢 CREATE - Registrar un nuevo pago
        // ============================================================
        public int CrearPago(Pago nuevo)
        {
            _context.Pago.Add(nuevo);
            _context.SaveChanges();
            return nuevo.id_pago; // Retorna el ID generado
        }

        // ============================================================
        // 🔵 READ - Listar todos los pagos
        // ============================================================
        public List<PagoDto> ListarPagos()
        {
            return _context.Pago
                .Select(p => new PagoDto
                {
                    IdPago = p.id_pago,
                    IdReserva = p.id_reserva,
                    Metodo = p.metodo,
                    Monto = p.monto,
                    FechaPago = p.fecha_pago ?? DateTime.MinValue, // en caso de ser nullable
                    ReferenciaExterna = p.referencia_externa,
                    Estado = p.estado
                })
                .ToList();
        }

        // ============================================================
        // 🔍 READ - Obtener un pago por su ID
        // ============================================================
        public PagoDto ObtenerPorId(int idPago)
        {
            var p = _context.Pago.Find(idPago);
            if (p == null) return null;

            return new PagoDto
            {
                IdPago = p.id_pago,
                IdReserva = p.id_reserva,
                Metodo = p.metodo,
                Monto = p.monto,
                FechaPago = p.fecha_pago ?? DateTime.MinValue,
                ReferenciaExterna = p.referencia_externa,
                Estado = p.estado
            };
        }

        // ============================================================
        // 🟠 UPDATE - Actualizar un pago existente
        // ============================================================
        public bool ActualizarPago(Pago pagoEditado)
        {
            var existente = _context.Pago.Find(pagoEditado.id_pago);
            if (existente == null) return false;

            existente.metodo = pagoEditado.metodo;
            existente.monto = pagoEditado.monto;
            existente.fecha_pago = pagoEditado.fecha_pago;
            existente.referencia_externa = pagoEditado.referencia_externa;
            existente.estado = pagoEditado.estado;

            _context.SaveChanges();
            return true;
        }

        // ============================================================
        // 🔴 DELETE - Eliminar un pago
        // ============================================================
        public bool EliminarPago(int idPago)
        {
            var pago = _context.Pago.Find(idPago);
            if (pago == null) return false;

            _context.Pago.Remove(pago);
            _context.SaveChanges();
            return true;
        }

        // ============================================================
        // ⚙️ EXTRA - Listar pagos por reserva
        // ============================================================
        public List<PagoDto> ListarPorReserva(int idReserva)
        {
            return _context.Pago
                .Where(p => p.id_reserva == idReserva)
                .Select(p => new PagoDto
                {
                    IdPago = p.id_pago,
                    IdReserva = p.id_reserva,
                    Metodo = p.metodo,
                    Monto = p.monto,
                    FechaPago = p.fecha_pago ?? DateTime.MinValue,
                    ReferenciaExterna = p.referencia_externa,
                    Estado = p.estado
                })
                .ToList();
        }
    }
}
