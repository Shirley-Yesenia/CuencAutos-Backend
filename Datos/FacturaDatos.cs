using AccesoDatos;
using AccesoDatos.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class FacturaDatos
    {
        private readonly db31808Entities1 _context = new db31808Entities1();

        // ============================================================
        // 🟢 CREATE - Generar una nueva factura
        // ============================================================
        public int CrearFactura(Factura nueva)
        {
            _context.Factura.Add(nueva);
            _context.SaveChanges();
            return nueva.id_factura;  // Retorna el ID generado
        }

        // ============================================================
        // 🔵 READ - Listar todas las facturas
        // ============================================================
        public List<FacturaDto> ListarFacturas()
        {
            return _context.Factura
                .Select(f => new FacturaDto
                {
                    IdFactura = f.id_factura,
                    IdReserva = f.id_reserva,
                    UriFactura = f.uri_factura,
                    FechaEmision = f.fecha_emision,
                    ValorTotal = f.valor_total
                })
                .ToList();
        }

        // ============================================================
        // 🔍 READ - Obtener una factura específica por ID
        // ============================================================
        public FacturaDto ObtenerPorId(int id)
        {
            var f = _context.Factura.Find(id);
            if (f == null) return null;

            return new FacturaDto
            {
                IdFactura = f.id_factura,
                IdReserva = f.id_reserva,
                UriFactura = f.uri_factura,
                FechaEmision = f.fecha_emision,
                ValorTotal = f.valor_total
            };
        }

        // ============================================================
        // 🟠 UPDATE - Actualizar una factura existente
        // ============================================================
        public bool ActualizarFactura(Factura facturaModificada)
        {
            var factura = _context.Factura.Find(facturaModificada.id_factura);
            if (factura == null) return false;

            factura.uri_factura = facturaModificada.uri_factura;
            factura.fecha_emision = facturaModificada.fecha_emision;
            factura.valor_total = facturaModificada.valor_total;

            _context.SaveChanges();
            return true;
        }

        // ============================================================
        // 🔴 DELETE - Eliminar una factura
        // ============================================================
        public bool EliminarFactura(int idFactura)
        {
            var factura = _context.Factura.Find(idFactura);
            if (factura == null) return false;

            _context.Factura.Remove(factura);
            _context.SaveChanges();
            return true;
        }

        // ============================================================
        // ⚙️ EXTRA - Buscar facturas por usuario (JOIN con Reserva)
        // ============================================================
        public List<FacturaDto> ListarPorUsuario(int idUsuario)
        {
            return _context.Factura
                .Where(f => f.Reserva.id_usuario == idUsuario)
                .Select(f => new FacturaDto
                {
                    IdFactura = f.id_factura,
                    IdReserva = f.id_reserva,
                    UriFactura = f.uri_factura,
                    FechaEmision = f.fecha_emision,
                    ValorTotal = f.valor_total
                })
                .ToList();
        }

    }
}
