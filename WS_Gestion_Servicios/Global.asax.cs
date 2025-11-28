using System;
using System.Web;

namespace WS_Gestion_Servicios
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // Obtener el origen de la petición
            string origin = Request.Headers["Origin"];

            // Permitir CORS
            if (!string.IsNullOrEmpty(origin))
            {
                Response.AddHeader("Access-Control-Allow-Origin", origin);
            }
            else
            {
                Response.AddHeader("Access-Control-Allow-Origin", "*");
            }

            Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
            Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, SOAPAction, Authorization, Accept, X-Requested-With");
            Response.AddHeader("Access-Control-Allow-Credentials", "true");
            Response.AddHeader("Access-Control-Max-Age", "86400");

            // Manejar peticiones OPTIONS (preflight)
            if (Request.HttpMethod == "OPTIONS")
            {
                Response.StatusCode = 200;
                Response.StatusDescription = "OK";
                Response.End();
            }
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            // Código de inicio de la aplicación
        }
    }
}