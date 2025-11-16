using System;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace WS_Integracion_Servicios
{
    [WebService(Namespace = "http://rentaautos.com/aerolinea")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class WS_UsuarioExterno : WebService
    {
        [WebMethod(Description = "Crea un cliente externo proveniente del sistema Booking Bus o integración externa.")]
        public UsuarioCreado CrearUsuarioExterno(UsuarioExternoDto nuevoUsuario)
        {
            try
            {
                if (string.IsNullOrEmpty(nuevoUsuario.Email))
                    throw new Exception("El correo electrónico es obligatorio.");

                var usuario = new UsuarioCreado
                {
                    IdUsuario = new Random().Next(1000, 9999),
                    Nombre = nuevoUsuario.Nombre,
                    Email = nuevoUsuario.Email,
                    Estado = "Creado correctamente ✅"
                };

                return usuario;
            }
            catch (Exception ex)
            {
                throw new SoapException("Error al crear usuario externo: " + ex.Message, SoapException.ClientFaultCode);
            }
        }
    }

    public class UsuarioExternoDto
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Pais { get; set; }
    }

    public class UsuarioCreado
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Estado { get; set; }
    }
}
