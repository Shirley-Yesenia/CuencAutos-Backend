using System;
using System.Collections.Generic;
using System.Web.Http.Routing;
using AccesoDatos.DTO;

namespace API_REST_GESTION.Hateoas.Builders
{
    public class BloqueoVehiculosHateoas
    {
        private readonly UrlHelper _urlHelper;
        public BloqueoVehiculosHateoas(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public dynamic Build(BloqueoVehiculoDto bloqueo)
        {
            return new
            {
                bloqueo.IdHold,
                bloqueo.IdUsuario,
                bloqueo.IdVehiculo,
                bloqueo.FechaInicio,
                bloqueo.FechaExpiracion,
                bloqueo.MontoBloqueado,
                bloqueo.ReferenciaBanco,
                bloqueo.Estado,

                _links = new List<object>
                {
                    new {
                        rel = "self",
                        href = _urlHelper.Link("GetBloqueoById", new { idHold = bloqueo.IdHold }),
                        method = "GET"
                    },
                    new {
                        rel = "delete",
                        href = _urlHelper.Link("DeleteBloqueo", new { idHold = bloqueo.IdHold }),
                        method = "DELETE"
                    },
                    new {
                        rel = "vehiculo",
                        href = _urlHelper.Link("GetBloqueosPorVehiculo", new { idVehiculo = bloqueo.IdVehiculo }),
                        method = "GET"
                    }
                }
            };
        }
    }
}
