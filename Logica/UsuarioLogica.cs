using AccesoDatos;
using AccesoDatos.DTO;
using Datos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class UsuarioLogica
    {
        private readonly UsuarioDatos datos = new UsuarioDatos();

        // 🔵 Añadido para creación automática del carrito
        private readonly CarritoLogica carritoLogica = new CarritoLogica();


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
        // 🟢 CREAR NUEVO USUARIO (Completo, versión normal)
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

            // 1️⃣ Crear usuario
            int idUsuario = datos.Crear(entidad);

            // 2️⃣ Crear carrito automáticamente
            carritoLogica.CrearCarritoParaUsuario(idUsuario);

            // 3️⃣ Retornar ID
            return idUsuario;
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

        public UsuarioExternoDto2 ObtenerUsuarioPorEmail(string email)
        {
            using (var db = new db31808Entities1())
            {
                var usuario = db.Usuario
                                .Where(u => u.email == email)
                                .Select(u => new UsuarioExternoDto2
                                {
                                    Nombre = u.nombre,
                                    Apellido = u.apellido,
                                    Email = u.email
                                })
                                .FirstOrDefault();

                return usuario;
            }
        }

        // ============================================================
        // 🆕 CREAR USUARIO EXTERNO (Booking / Integraciones)
        // ============================================================
        public UsuarioDto CrearUsuarioExterno(UsuarioExternoDto2 req)
        {
            if (req == null)
                throw new ArgumentNullException("Los datos del usuario no pueden ser nulos.");

            if (string.IsNullOrWhiteSpace(req.Email))
                throw new Exception("Debe proporcionar un email válido.");

            var existente = datos.ObtenerDtoPorEmail(req.Email);
            if (existente != null)
            {
                return existente;
            }

            var nuevo = new Usuario
            {
                nombre = req.Nombre ?? "",
                apellido = req.Apellido ?? "",
                email = req.Email,
                contrasena = null,
                direccion = null,
                pais = req.Pais ?? "",
                edad = null,
                tipo_identificacion = null,
                identificacion = null,
                rol = "cliente_externo"
            };

            datos.Crear(nuevo);

            return datos.ObtenerDtoPorEmail(req.Email);
        }
    }
}
