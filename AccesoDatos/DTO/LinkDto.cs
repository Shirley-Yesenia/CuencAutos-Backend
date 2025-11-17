using System;

namespace AccesoDatos.DTO
{
    /// <summary>
    /// Representa un enlace HATEOAS simple.
    /// </summary>
    public class LinkDto
    {
        public LinkDto() { }

        public LinkDto(string rel, string href, string method = "GET")
        {
            Rel = rel;
            Href = href;
            Method = string.IsNullOrWhiteSpace(method) ? "GET" : method.ToUpperInvariant();
        }

        public string Rel { get; set; }
        public string Href { get; set; }
        public string Method { get; set; } = "GET";
    }
}
