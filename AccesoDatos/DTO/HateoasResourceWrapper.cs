using System.Collections.Generic;

namespace AccesoDatos.DTO
{
    /// <summary>
    /// Envoltorio HATEOAS que permite devolver un objeto de datos + enlaces.
    /// Este wrapper NO afecta los DTO originales (Swagger no ensucia los modelos).
    /// </summary>
    public class HateoasResourceWrapper<T>
    {
        public T Data { get; set; }
        public List<LinkDto> Links { get; set; } = new List<LinkDto>();

        public HateoasResourceWrapper() { }

        public HateoasResourceWrapper(T data)
        {
            Data = data;
        }

        public void AddLink(string rel, string href, string method = "GET")
        {
            Links.Add(new LinkDto(rel, href, method));
        }
    }
}
