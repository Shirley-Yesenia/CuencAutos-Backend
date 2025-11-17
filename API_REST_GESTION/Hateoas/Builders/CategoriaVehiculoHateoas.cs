using System;
using System.Collections.Generic;
using System.Web.Http.Routing;
using AccesoDatos.DTO;

namespace API_REST_GESTION.Hateoas.Builders
{
    public class CategoriaVehiculoHateoas
    {
        private readonly UrlHelper _urlHelper;

        public CategoriaVehiculoHateoas(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public dynamic Build(CategoriaVehiculoDto dto)
        {
            return new
            {
                dto.IdCategoria,
                dto.Nombre,
                dto.Descripcion,

                _links = new List<object>
                {
                    new {
                        rel = "self",
                        href = _urlHelper.Link("GetCategoriaById", new { id = dto.IdCategoria }),
                        method = "GET"
                    },
                    new {
                        rel = "update",
                        href = _urlHelper.Link("UpdateCategoria", new { id = dto.IdCategoria }),
                        method = "PUT"
                    },
                    new {
                        rel = "delete",
                        href = _urlHelper.Link("DeleteCategoria", new { id = dto.IdCategoria }),
                        method = "DELETE"
                    },
                    new {
                        rel = "all",
                        href = _urlHelper.Link("GetAllCategorias", null),
                        method = "GET"
                    }
                }
            };
        }
    }
}
