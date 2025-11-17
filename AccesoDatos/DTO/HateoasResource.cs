using System.Collections.Generic;
using System.Linq;

namespace AccesoDatos.DTO
{
    /// <summary>
    /// Clase base para DTOs que soportan HATEOAS.
    /// Provee manejo de la colección de enlaces (Links) y helpers Add/Clear.
    /// </summary>
    public abstract class HateoasResource
    {
        public List<LinkDto> Links { get; set; } = new List<LinkDto>();

        /// <summary>
        /// Agrega un link de forma segura.
        /// </summary>
        /// <param name="rel">Relación (self, update, delete, ...)</param>
        /// <param name="href">URL</param>
        /// <param name="method">Método HTTP (GET, POST, ...). Opcional, default GET</param>
        public void AddLink(string rel, string href, string method = "GET")
        {
            if (string.IsNullOrWhiteSpace(rel) || string.IsNullOrWhiteSpace(href))
                return;

            // Evita duplicados por rel+href+method
            var exists = Links.Any(l =>
                string.Equals(l.Rel, rel, System.StringComparison.OrdinalIgnoreCase) &&
                string.Equals(l.Href, href, System.StringComparison.OrdinalIgnoreCase) &&
                string.Equals(l.Method, method, System.StringComparison.OrdinalIgnoreCase)
            );

            if (!exists)
                Links.Add(new LinkDto(rel, href, method));
        }

        /// <summary>
        /// Elimina todos los links actuales.
        /// </summary>
        public void ClearLinks()
        {
            Links = new List<LinkDto>();
        }
    }
}
