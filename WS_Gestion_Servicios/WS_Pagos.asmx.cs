using System;
using System.Web.Services;
using AccesoDatos.DTO;
using Logica;
using IntegracionBanco;
using IntegracionBanco.bancoDto;

namespace WS_Gestion_Servicios
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WS_Pagos : WebService
    {
        private readonly PagoLogica pagoLogica = new PagoLogica();

        // ===========================================================
        // LISTAR PAGOS
        // ===========================================================
        [WebMethod(Description = "Lista todos los pagos.")]
        public PagoDto[] ListarPagos()
        {
            var lista = pagoLogica.ListarPagos();
            return lista?.ToArray();
        }

        // ===========================================================
        // OBTENER PAGO POR ID
        // ===========================================================
        [WebMethod(Description = "Obtiene un pago por su ID.")]
        public PagoDto ObtenerPagoPorId(int idPago)
        {
            return pagoLogica.ObtenerPagoPorId(idPago);
        }

        // ===========================================================
        // LISTAR PAGOS POR RESERVA
        // ===========================================================
        [WebMethod(Description = "Lista los pagos asociados a una reserva.")]
        public PagoDto[] ListarPagosPorReserva(int idReserva)
        {
            var lista = pagoLogica.ListarPagosPorReserva(idReserva);
            return lista?.ToArray();
        }

        // ===========================================================
        // CREAR PAGO (CON INTEGRACIÓN BANCO)
        // ===========================================================
        [WebMethod(Description = "Crea un pago con integración bancaria.")]
        public PagoRespuestaSoapDto CrearPago(PagoRequestDto body)
        {
            if (body == null)
                return new PagoRespuestaSoapDto { Mensaje = "Debe enviar información del pago." };

            try
            {
                // 1️⃣ Preparar DTO para el banco
                var transaccionDto = new transaccionDto
                {
                    cuenta_origen = body.CuentaCliente,
                    cuenta_destino = body.CuentaComercio,
                    monto = body.Monto
                };

                // 2️⃣ Ejecutar transacción con el banco (SOAP → no await)
                string respuestaBanco = bancoConsumer.transaccionUnitaria(transaccionDto).Result;
                bool aprobado = respuestaBanco == "OK";

                // 3️⃣ Crear registro interno del pago
                var pagoDto = new PagoDto
                {
                    IdReserva = body.IdReserva,
                    Metodo = "Transaccion",
                    Monto = body.Monto,
                    ReferenciaExterna = respuestaBanco,
                    Estado = aprobado ? "Exitoso" : "Fallido",
                    FechaPago = DateTime.Now
                };

                int idPago = pagoLogica.CrearPago(pagoDto);

                // 4️⃣ Devolver respuesta SOAP clara
                return new PagoRespuestaSoapDto
                {
                    Mensaje = "Pago procesado",
                    Aprobado = aprobado,
                    RespuestaBanco = respuestaBanco,
                    IdPago = idPago
                };
            }
            catch (Exception ex)
            {
                return new PagoRespuestaSoapDto
                {
                    Mensaje = "Error: " + ex.Message,
                    Aprobado = false
                };
            }
        }

        // ===========================================================
        // ELIMINAR PAGO
        // ===========================================================
        [WebMethod(Description = "Elimina un pago.")]
        public string EliminarPago(int idPago)
        {
            return pagoLogica.EliminarPago(idPago)
                ? "Pago eliminado correctamente."
                : "No se pudo eliminar el pago.";
        }

        // ===========================================================
        // ACTUALIZAR PAGO
        // ===========================================================
        [WebMethod(Description = "Actualiza un pago.")]
        public string ActualizarPago(PagoDto body)
        {
            return pagoLogica.ActualizarPago(body)
                ? "Pago actualizado correctamente."
                : "No se pudo actualizar el pago.";
        }
    }

    // =========================
    // DTO específico para SOAP
    // =========================
    public class PagoRespuestaSoapDto
    {
        public string Mensaje { get; set; }
        public bool Aprobado { get; set; }
        public string RespuestaBanco { get; set; }
        public int IdPago { get; set; }
    }
}
