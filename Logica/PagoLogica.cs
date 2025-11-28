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
    public class PagoLogica
    {
        private readonly PagoDatos datos = new PagoDatos();
        private readonly ReservaDatos reservaDatos = new ReservaDatos(); // para validar reserva existente

        // ============================================================
        // 🔵 LISTAR TODOS LOS PAGOS
        // ============================================================
        public List<PagoDto> ListarPagos()
        {
            return datos.ListarPagos();
        }

        // ============================================================
        // 🔍 OBTENER PAGO POR ID
        // ============================================================
        public PagoDto ObtenerPagoPorId(int idPago)
        {
            if (idPago <= 0)
                throw new ArgumentException("El ID del pago no es válido.");

            return datos.ObtenerPorId(idPago);
        }

        // ============================================================
        // 🟣 LISTAR PAGOS POR RESERVA
        // ============================================================
        public List<PagoDto> ListarPagosPorReserva(int idReserva)
        {
            if (idReserva <= 0)
                throw new ArgumentException("Debe indicar un ID de reserva válido.");

            return datos.ListarPorReserva(idReserva);
        }

        // ============================================================
        // 🟢 CREAR NUEVO PAGO
        // ============================================================
        public int CrearPago(PagoDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "Los datos del pago no pueden ser nulos.");

            if (dto.IdReserva <= 0)
                throw new Exception("Debe especificar una reserva válida.");

            if (dto.Monto <= 0)
                throw new Exception("El monto del pago debe ser mayor a 0.");

            // ✅ Validar que la reserva exista
            var reserva = reservaDatos.ObtenerPorId(dto.IdReserva);
            if (reserva == null)
                throw new Exception("La reserva indicada no existe.");

            // 🏦 Validación del método
            if (string.IsNullOrWhiteSpace(dto.Metodo))
                dto.Metodo = "Transaccion"; // valor por defecto si viene vacío

            // Crea la entidad
            var entidad = new Pago
            {
                id_reserva = dto.IdReserva,
                metodo = dto.Metodo,
                monto = dto.Monto,
                fecha_pago = dto.FechaPago == default ? DateTime.Now : dto.FechaPago,
                referencia_externa = dto.ReferenciaExterna,
                estado = dto.Estado ?? "Exitoso"
            };

            return datos.CrearPago(entidad);
        }

        // ============================================================
        // 🟠 ACTUALIZAR PAGO EXISTENTE
        // ============================================================
        public bool ActualizarPago(PagoDto dto)
        {
            if (dto == null || dto.IdPago <= 0)
                throw new Exception("Datos inválidos para actualizar el pago.");

            var entidad = new Pago
            {
                id_pago = dto.IdPago,
                id_reserva = dto.IdReserva,
                metodo = dto.Metodo,
                monto = dto.Monto,
                fecha_pago = dto.FechaPago,
                referencia_externa = dto.ReferenciaExterna,
                estado = dto.Estado
            };

            return datos.ActualizarPago(entidad);
        }

        // ============================================================
        // 🔴 ELIMINAR PAGO
        // ============================================================
        public bool EliminarPago(int idPago)
        {
            if (idPago <= 0)
                throw new Exception("ID inválido para eliminar el pago.");

            return datos.EliminarPago(idPago);
        }

        // ============================================================
        // ⚙️ EXTRA - CONFIRMAR PAGO CON BANCO EXTERNO
        // ============================================================
        public bool ConfirmarPagoBanco(PagoDto pagoBanco)
        {
            // Simulación de integración con API del banco
            if (pagoBanco == null)
                throw new Exception("No se recibieron datos del pago del banco.");

            if (string.IsNullOrWhiteSpace(pagoBanco.ReferenciaExterna))
                throw new Exception("Falta el identificador de transacción del banco.");

            // Buscar si ya existe un pago con esa referencia
            var existentes = datos.ListarPagos();
            bool yaExiste = existentes.Exists(p => p.ReferenciaExterna == pagoBanco.ReferenciaExterna);
            if (yaExiste)
                throw new Exception("El pago con esta referencia ya fue registrado.");

            // Guardar en la base de datos
            return CrearPago(pagoBanco) > 0;
        }
    }
}
