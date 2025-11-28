using AccesoDatos.DTO;
using Logica;
using System;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace WS_Gestion_Servicios
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WS_Usuarios : WebService
    {
        private readonly UsuarioLogica logica = new UsuarioLogica();

        // ============================================================
        // 🔵 LISTAR USUARIOS
        // ============================================================
        [WebMethod]
        public List<UsuarioDto> ListarUsuarios()
        {
            return logica.ListarUsuarios();
        }

        // ============================================================
        // 🔍 OBTENER USUARIO POR ID
        // ============================================================
        [WebMethod]
        public UsuarioDto ObtenerUsuarioPorId(int id)
        {
            try
            {
                return logica.ObtenerUsuarioPorId(id);
            }
            catch (Exception ex)
            {
                throw new SoapException(ex.Message, SoapException.ClientFaultCode);
            }
        }

        // ============================================================
        // 🟢 CREAR USUARIO  (carrito se crea automáticamente)
        // ============================================================
        [WebMethod]
        public int CrearUsuario(
            string nombre,
            string apellido,
            string email,
            string contrasena,
            string direccion,
            string pais,
            int edad,
            string tipoIdentificacion,
            string identificacion,
            string rol)
        {
            try
            {
                var dto = new UsuarioDto
                {
                    Nombre = nombre,
                    Apellido = apellido,
                    Email = email,
                    Contrasena = contrasena,
                    Direccion = direccion,
                    Pais = pais,
                    Edad = edad,
                    TipoIdentificacion = tipoIdentificacion,
                    Identificacion = identificacion,
                    Rol = string.IsNullOrEmpty(rol) ? "Cliente" : rol
                };

                return logica.CrearUsuario(dto);
                // 👆 Aquí ya se crea el carrito automáticamente gracias a UsuarioLogica
            }
            catch (Exception ex)
            {
                throw new SoapException(ex.Message, SoapException.ClientFaultCode);
            }
        }

        // ============================================================
        // 🟠 ACTUALIZAR USUARIO
        // ============================================================
        [WebMethod]
        public bool ActualizarUsuario(
            int idUsuario,
            string nombre,
            string apellido,
            string email,
            string contrasena,
            string direccion,
            string pais,
            int edad,
            string tipoIdentificacion,
            string identificacion,
            string rol)
        {
            try
            {
                var dto = new UsuarioDto
                {
                    IdUsuario = idUsuario,
                    Nombre = nombre,
                    Apellido = apellido,
                    Email = email,
                    Contrasena = contrasena,
                    Direccion = direccion,
                    Pais = pais,
                    Edad = edad,
                    TipoIdentificacion = tipoIdentificacion,
                    Identificacion = identificacion,
                    Rol = rol
                };

                return logica.ActualizarUsuario(dto);
            }
            catch (Exception ex)
            {
                throw new SoapException(ex.Message, SoapException.ClientFaultCode);
            }
        }

        // ============================================================
        // 🔴 ELIMINAR USUARIO
        // ============================================================
        [WebMethod]
        public bool EliminarUsuario(int idUsuario)
        {
            try
            {
                return logica.EliminarUsuario(idUsuario);
            }
            catch (Exception ex)
            {
                throw new SoapException(ex.Message, SoapException.ClientFaultCode);
            }
        }

        // ============================================================
        // 🔐 LOGIN
        // ============================================================
        [WebMethod]
        public UsuarioDto Login(string email, string contrasena)
        {
            try
            {
                return logica.ValidarLogin(email, contrasena);
            }
            catch
            {
                throw new SoapException("Credenciales incorrectas.", SoapException.ClientFaultCode);
            }
        }
    }
}
