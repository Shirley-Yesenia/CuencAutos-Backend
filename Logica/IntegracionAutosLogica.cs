using System;
using AccesoDatos.DTO;
using Datos;

namespace Logica
{
    public class IntegracionAutosLogica
    {
        private readonly PreReservaDatos datos = new PreReservaDatos();

        public PreReservaAutoResponseDto CrearPreReservaAuto(PreReservaAutoRequestDto req)
        {
            if (req == null) throw new ArgumentNullException("request");
            if (!int.TryParse(req.IdVehiculo, out var idVeh)) throw new ArgumentException("IdVehiculo debe ser numerico.");
            if (req.FechaInicio >= req.FechaFin) throw new ArgumentException("Rango de fechas invalido.");

            var ttl = req.DuracionHoldSegundos ?? 600;

            if (!datos.ExisteVehiculo(req.IdVehiculo))
                throw new Exception("Vehiculo no existe.");

            var idHold = datos.CrearHold(idVeh, req.FechaInicio, req.FechaFin, ttl);

            return new PreReservaAutoResponseDto
            {
                IdHold = idHold.ToString(),              // devolvemos el identity como string
                FechaExpiracion = DateTime.Now.AddSeconds(ttl)
            };
        }
    }
}