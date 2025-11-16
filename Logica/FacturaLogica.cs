using AccesoDatos;
using AccesoDatos.DTO;
using Datos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class FacturaLogica
    {
        private readonly FacturaDatos datos = new FacturaDatos();
        private readonly ReservaDatos reservaDatos = new ReservaDatos();
        private readonly PagoDatos pagoDatos = new PagoDatos();

        // ============================================================
        // 🔵 LISTAR TODAS LAS FACTURAS
        // ============================================================
        public List<FacturaDto> ListarFacturas()
        {
            return datos.ListarFacturas();
        }

        // ============================================================
        // 🔍 OBTENER FACTURA POR ID
        // ============================================================
        public FacturaDto ObtenerFacturaPorId(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID de la factura no es válido.");

            return datos.ObtenerPorId(id);
        }

        // ============================================================
        // 🟣 LISTAR FACTURAS POR USUARIO
        // ============================================================
        public List<FacturaDto> ListarFacturasPorUsuario(int idUsuario)
        {
            if (idUsuario <= 0)
                throw new ArgumentException("Debe indicar un ID de usuario válido.");

            return datos.ListarPorUsuario(idUsuario);
        }

        // ============================================================
        // 🟢 CREAR NUEVA FACTURA (después de pago)
        // ============================================================
        public int CrearFactura(FacturaDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "Los datos de la factura no pueden ser nulos.");

            if (dto.IdReserva <= 0)
                throw new Exception("Debe asociar una reserva válida a la factura.");

            // ✅ Validar que la reserva exista
            var reserva = reservaDatos.ObtenerPorId(dto.IdReserva);
            if (reserva == null)
                throw new Exception("No se encontró la reserva especificada.");

            // ✅ Validar que exista al menos un pago asociado
            var pagos = pagoDatos.ListarPorReserva(dto.IdReserva);
            if (pagos == null || pagos.Count == 0)
                throw new Exception("No se ha registrado ningún pago para esta reserva.");

            // ✅ Calcular el total (si no viene del DTO)
            if (dto.ValorTotal <= 0)
                dto.ValorTotal = reserva.Total;

            // ✅ Simular generación del archivo PDF (en entorno local)
            string rutaFactura = GenerarFacturaPdf(dto, reserva);

            // Crear entidad
            var entidad = new Factura
            {
                id_reserva = dto.IdReserva,
                uri_factura = rutaFactura,
                fecha_emision = DateTime.Now,
                valor_total = dto.ValorTotal
            };

            return datos.CrearFactura(entidad);
        }

        // ============================================================
        // 🟠 ACTUALIZAR FACTURA
        // ============================================================
        public bool ActualizarFactura(FacturaDto dto)
        {
            if (dto == null || dto.IdFactura <= 0)
                throw new Exception("Datos inválidos para actualizar la factura.");

            var entidad = new Factura
            {
                id_factura = dto.IdFactura,
                id_reserva = dto.IdReserva,
                uri_factura = dto.UriFactura,
                fecha_emision = dto.FechaEmision,
                valor_total = dto.ValorTotal
            };

            return datos.ActualizarFactura(entidad);
        }

        // ============================================================
        // 🔴 ELIMINAR FACTURA
        // ============================================================
        public bool EliminarFactura(int idFactura)
        {
            if (idFactura <= 0)
                throw new Exception("ID inválido para eliminar la factura.");

            return datos.EliminarFactura(idFactura);
        }

        // ============================================================
        // 🧩 MÉTODO EXTRA - GENERAR ARCHIVO PDF SIMULADO
        // ============================================================
        private string GenerarFacturaPdf(FacturaDto factura, ReservaDto reserva)
        {
            try
            {
                string carpeta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Facturas");
                if (!Directory.Exists(carpeta))
                    Directory.CreateDirectory(carpeta);

                string nombreArchivo = $"Factura_{factura.IdReserva}_{DateTime.Now:yyyyMMddHHmmss}.txt";
                string rutaArchivo = Path.Combine(carpeta, nombreArchivo);

                // Contenido de la factura simulada (podrías reemplazar por un PDF real más adelante)
                string contenido = $@"
================ FACTURA DE RESERVA ================
Número de Factura: [Generada]
Fecha de Emisión: {DateTime.Now:dd/MM/yyyy HH:mm}
----------------------------------------------------
Cliente: {reserva.NombreUsuario}
Vehículo: {reserva.VehiculoNombre}
Periodo: {reserva.FechaInicio:dd/MM/yyyy} a {reserva.FechaFin:dd/MM/yyyy}
Total: ${factura.ValorTotal:F2}
Estado: Emitida
====================================================
Gracias por preferir RentaAutos.
";

                File.WriteAllText(rutaArchivo, contenido);
                return rutaArchivo;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar el archivo de factura: " + ex.Message);
            }
        }
    }
}
