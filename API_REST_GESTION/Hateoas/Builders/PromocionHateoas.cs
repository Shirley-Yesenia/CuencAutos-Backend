using System.Collections.Generic;
using System.Web.Http.Routing;
using AccesoDatos.DTO;

namespace API_REST_GESTION.Hateoas.Builders
{
    public class PromocionHateoas
    {
        private readonly UrlHelper _urlHelper;

        public PromocionHateoas(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public dynamic Build(PromocionDto dto)
        {
            return new
            {
                dto.IdPromocion,
                dto.Nombre,
                dto.PorcentajeDescuento,
                dto.FechaInicio,
                dto.FechaFin,
                dto.Descripcion,

                _links = new List<object>
                {
                    new {
                        rel = "self",
                        href = _urlHelper.Link("GetPromocionById", new { idPromocion = dto.IdPromocion }),
                        method = "GET"
                    },
                    new {
                        rel = "update",
                        href = _urlHelper.Link("UpdatePromocion", new { idPromocion = dto.IdPromocion }),
                        method = "PUT"
                    },
                    new {
                        rel = "delete",
                        href = _urlHelper.Link("DeletePromocion", new { idPromocion = dto.IdPromocion }),
                        method = "DELETE"
                    },
                    new {
                        rel = "all",
                        href = _urlHelper.Link("ObtenerPromociones", null),
                        method = "GET"
                    }
                }
            };
        }
    }
}
