using System.Collections.Generic;

namespace API_REST_GESTION.Hateoas.Builders
{
    public class SucursalesHateoas
    {
        private readonly string _baseUrl;

        public SucursalesHateoas(string baseUrl)
        {
            _baseUrl = baseUrl.TrimEnd('/');
        }

        public object Build(object data, int? id = null)
        {
            var links = new List<object>
            {
                new { rel = "self", href = $"{_baseUrl}/api/v1/sucursales" }
            };

            if (id.HasValue)
            {
                links.Add(new { rel = "detalle", href = $"{_baseUrl}/api/v1/sucursales/{id}" });
                links.Add(new { rel = "actualizar", href = $"{_baseUrl}/api/v1/sucursales/{id}" });
                links.Add(new { rel = "eliminar", href = $"{_baseUrl}/api/v1/sucursales/{id}" });
            }
            else
            {
                links.Add(new { rel = "crear", href = $"{_baseUrl}/api/v1/sucursales" });
            }

            return new
            {
                data,
                links
            };
        }
    }
}
