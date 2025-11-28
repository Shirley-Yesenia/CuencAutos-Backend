using System.Collections.Generic;

namespace API_REST_GESTION.Hateoas.Builders
{
    public class UsuariosHateoas
    {
        private readonly string _baseUrl;

        public UsuariosHateoas(string baseUrl)
        {
            _baseUrl = baseUrl.TrimEnd('/');
        }

        public object Build(object data, int? id = null)
        {
            var links = new List<object>
            {
                new { rel = "self", href = $"{_baseUrl}/api/v1/usuarios" }
            };

            if (id.HasValue)
            {
                links.Add(new { rel = "detalle", href = $"{_baseUrl}/api/v1/usuarios/{id}" });
                links.Add(new { rel = "actualizar", href = $"{_baseUrl}/api/v1/usuarios/{id}" });
                links.Add(new { rel = "eliminar", href = $"{_baseUrl}/api/v1/usuarios/{id}" });
            }
            else
            {
                links.Add(new { rel = "crear", href = $"{_baseUrl}/api/v1/usuarios" });
                links.Add(new { rel = "login", href = $"{_baseUrl}/api/v1/usuarios/login" });
            }

            return new
            {
                data,
                links
            };
        }
    }
}
