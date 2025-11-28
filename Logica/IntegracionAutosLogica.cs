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
            if (req == null)
                throw new ArgumentNullException("request");

            if (!int.TryParse(req.IdVehiculo, out var idVeh))
                throw new ArgumentException("IdVehiculo debe ser numerico.");

            if (req.FechaInicio >= req.FechaFin)
                throw new ArgumentException("Rango de fechas invalido.");

            var ttl = req.DuracionHoldSegundos ?? 600;

            // -------------------------------------------
            // 1️⃣ Verificar que el vehículo existe
            // -------------------------------------------
            if (!datos.ExisteVehiculo(req.IdVehiculo))
                throw new Exception("Vehiculo no existe.");

            // -------------------------------------------
            // 2️⃣ Verificar si YA existe un hold activo
            //    que se solapa con las fechas
            // -------------------------------------------
            bool existeSolape = datos.ExisteHoldActivo(
                idVeh,
                req.FechaInicio,
                req.FechaFin
            );

            if (existeSolape)
                throw new Exception("Ya existe una pre-reserva activa para este vehículo en ese rango de fechas.");

            // -------------------------------------------
            // 3️⃣ Crear el hold
            // -------------------------------------------
            var idHold = datos.CrearHold(idVeh, req.FechaInicio, req.FechaFin, ttl);

            // -------------------------------------------
            // 4️⃣ Respuesta final
            // -------------------------------------------
            return new PreReservaAutoResponseDto
            {
                IdHold = idHold.ToString(),
                FechaExpiracion = req.FechaInicio.AddSeconds(ttl)
            };
        }
    }
}
