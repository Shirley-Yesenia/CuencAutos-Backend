using System.Web.Http;
using WebActivatorEx;
using API_REST_GESTION;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace API_REST_GESTION
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    // ================================
                    // ?? 6 versiones de la API
                    // ================================
                    c.MultipleApiVersions(
                        (apiDesc, version) =>
                        {
                            // Detecta la versión leyendo el RoutePrefix (ej: api/v1/...)
                            var path = apiDesc.RelativePath.ToLower();

                            return path.Contains($"/{version}/");
                        },
                        info =>
                        {
                            info.Version("v1", "API_REST_GESTION v1 - Estable");
                            info.Version("v2", "API_REST_GESTION v2 - Mejoras");
                            info.Version("v3", "API_REST_GESTION v3");
                            info.Version("v4", "API_REST_GESTION v4");
                            info.Version("v5", "API_REST_GESTION v5");
                            info.Version("v6", "API_REST_GESTION v6 - Experimental");
                        }
                    );

                    // =============================================
                    // ?? Otras configuraciones (opcionales)
                    // =============================================
                    // c.IncludeXmlComments(GetXmlCommentsPath());
                })
                .EnableSwaggerUi(c =>
                {
                    // ?? Permite seleccionar versión en la UI
                    c.EnableDiscoveryUrlSelector();
                });
        }
    }
}
