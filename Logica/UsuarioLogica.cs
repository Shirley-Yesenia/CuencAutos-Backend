using AccesoDatos;
using AccesoDatos.DTO;
using Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class UsuarioLogica
    {
        private readonly UsuarioDatos datos = new UsuarioDatos();

        // ============================================================
        // 🔵 LISTAR TODOS LOS USUARIOS
        // ============================================================
        public List<UsuarioDto> ListarUsuarios()
        {
            return datos.Listar();
        }

        // ============================================================
        // 🔍 OBTENER USUARIO POR ID
        // ============================================================
        public UsuarioDto ObtenerUsuarioPorId(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID de usuario no es válido.");

            return datos.ObtenerPorId(id);
        }

        // ============================================================
        // 🟢 CREAR NUEVO USUARIO
        // ============================================================
        public int CrearUsuario(UsuarioDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "Los datos del usuario no pueden ser nulos.");

            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new Exception("El correo electrónico es obligatorio.");

            if (string.IsNullOrWhiteSpace(dto.Contrasena))
                throw new Exception("La contraseña no puede estar vacía.");

            var entidad = new Usuario
            {
                nombre = dto.Nombre,
                apellido = dto.Apellido,
                email = dto.Email,
                contrasena = dto.Contrasena,
                direccion = dto.Direccion,
                pais = dto.Pais,
                edad = dto.Edad,
                tipo_identificacion = dto.TipoIdentificacion,
                identificacion = dto.Identificacion,
                rol = dto.Rol ?? "Cliente"
            };

            return datos.Crear(entidad);
        }

        // ============================================================
        // 🟠 ACTUALIZAR USUARIO
        // ============================================================
        public bool ActualizarUsuario(UsuarioDto dto)
        {
            if (dto == null || dto.IdUsuario <= 0)
                throw new Exception("Datos inválidos para actualizar el usuario.");

            var entidad = new Usuario
            {
                id_usuario = dto.IdUsuario,
                nombre = dto.Nombre,
                apellido = dto.Apellido,
                email = dto.Email,
                contrasena = dto.Contrasena,
                direccion = dto.Direccion,
                pais = dto.Pais,
                edad = dto.Edad,
                tipo_identificacion = dto.TipoIdentificacion,
                identificacion = dto.Identificacion,
                rol = dto.Rol
            };

            return datos.Actualizar(entidad);
        }

        // ============================================================
        // 🔴 ELIMINAR USUARIO
        // ============================================================
        public bool EliminarUsuario(int id)
        {
            if (id <= 0)
                throw new Exception("ID inválido para eliminar usuario.");

            return datos.Eliminar(id);
        }

        // ============================================================
        // 🧩 VALIDAR LOGIN
        // ============================================================
        public UsuarioDto ValidarLogin(string email, string contrasena)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(contrasena))
                throw new Exception("Correo y contraseña son obligatorios.");

            var usuario = datos.Login(email, contrasena);
            if (usuario == null)
                throw new Exception("Credenciales incorrectas.");

            return usuario;
        }
    }
}
