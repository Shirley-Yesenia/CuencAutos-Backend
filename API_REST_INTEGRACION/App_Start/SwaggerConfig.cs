using System.Web.Http;
using WebActivatorEx;
using API_REST_INTEGRACION;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace API_REST_INTEGRACION
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    // ============================================
                    // 🔥 MULTIPLE VERSIONES: v1 → v6
                    // ============================================
                    c.MultipleApiVersions(
                        (apiDesc, version) =>
                        {
                            // Detecta la versión leyendo el prefijo del path
                            // Ejemplo: /api/v3/integracion/autos/search
                            var path = apiDesc.RelativePath.ToLower();

                            return path.Contains($"/{version}/");
                        },
                        info =>
                        {
                            info.Version("v1", "API_REST_INTEGRACION v1 - Estable");
                            info.Version("v2", "API_REST_INTEGRACION v2 - Mejoras");
                            info.Version("v3", "API_REST_INTEGRACION v3");
                            info.Version("v4", "API_REST_INTEGRACION v4");
                            info.Version("v5", "API_REST_INTEGRACION v5");
                            info.Version("v6", "API_REST_INTEGRACION v6 - Experimental");
                        }
                    );

                    // El resto lo dejamos igual o limpio
                    // c.IncludeXmlComments(GetXmlCommentsPath());
                })
                .EnableSwaggerUi(c =>
                {
                    // 🔥 Selector visual para cambiar entre versiones
                    c.EnableDiscoveryUrlSelector();
                });
        }
    }
}
