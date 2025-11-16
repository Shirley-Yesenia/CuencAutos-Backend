using Newtonsoft.Json;
using System.Web.Http;
using System.Web.UI.WebControls;

namespace API_REST
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Rutas por atributo
            config.MapHttpAttributeRoutes();

            // Ruta por defecto: /api/{controller}/{id}
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = System.Web.Http.RouteParameter.Optional }

            );

            // JSON por defecto + camelCase
            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            json.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;

            // Opcional: deshabilitar XML
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Opcional: CORS
            // config.EnableCors();
        }
    }
}