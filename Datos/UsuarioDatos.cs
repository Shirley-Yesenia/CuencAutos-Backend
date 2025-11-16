using AccesoDatos;
using AccesoDatos.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class UsuarioDatos
    {
        // 🔹 Instancia del contexto de Entity Framework
        private readonly db31808Entities1 _context = new db31808Entities1();

        // ============================================================
        // 🟢 CREATE - Crear un nuevo usuario
        // ============================================================
        public int Crear(Usuario nuevo)
        {
            _context.Usuario.Add(nuevo);   // Agrega al contexto
            _context.SaveChanges();        // Guarda en la base de datos
            return nuevo.id_usuario;       // Devuelve el ID generado
        }

        // ============================================================
        // 🔵 READ - Listar todos los usuarios
        // ============================================================
        public List<UsuarioDto> Listar()
        {
            // Proyección a DTO para no exponer las entidades directamente
            return _context.Usuario.Select(u => new UsuarioDto
            {
                IdUsuario = u.id_usuario,
                Nombre = u.nombre,
                Apellido = u.apellido,
                Email = u.email,
                Contrasena = u.contrasena,
                Direccion = u.direccion,
                Pais = u.pais,
                Edad = u.edad,
                TipoIdentificacion = u.tipo_identificacion,
                Identificacion = u.identificacion,
                Rol = u.rol
            }).ToList();
        }

        // ============================================================
        // 🔍 READ (por ID) - Obtener un usuario específico
        // ============================================================
        public UsuarioDto ObtenerPorId(int id)
        {
            // Retorna el primer usuario con ese ID o null si no existe
            return Listar().FirstOrDefault(u => u.IdUsuario == id);
        }

        // ============================================================
        // 🟠 UPDATE - Actualizar un usuario existente
        // ============================================================
        public bool Actualizar(Usuario mod)
        {
            // Busca al usuario en la base
            var u = _context.Usuario.Find(mod.id_usuario);
            if (u == null) return false; // Si no existe, retorna false

            // Actualiza los campos
            u.nombre = mod.nombre;
            u.apellido = mod.apellido;
            u.email = mod.email;
            u.contrasena = mod.contrasena;
            u.direccion = mod.direccion;
            u.pais = mod.pais;
            u.edad = mod.edad;
            u.tipo_identificacion = mod.tipo_identificacion;
            u.identificacion = mod.identificacion;
            u.rol = mod.rol;

            _context.SaveChanges(); // Guarda los cambios
            return true;
        }

        // ============================================================
        // 🔴 DELETE - Eliminar un usuario
        // ============================================================
        public bool Eliminar(int id)
        {
            var u = _context.Usuario.Find(id);
            if (u == null) return false;

            _context.Usuario.Remove(u);
            _context.SaveChanges();
            return true;
        }

        // ============================================================
        // 🧩 MÉTODO EXTRA: Autenticación básica
        // ============================================================
        public UsuarioDto Login(string email, string contrasena)
        {
            // Busca un usuario con las credenciales
            var usuario = _context.Usuario.FirstOrDefault(u =>
                u.email == email && u.contrasena == contrasena);

            // Si no se encuentra, retorna null
            if (usuario == null) return null;

            // Devuelve el usuario en formato DTO
            return new UsuarioDto
            {
                IdUsuario = usuario.id_usuario,
                Nombre = usuario.nombre,
                Apellido = usuario.apellido,
                Email = usuario.email,
                Rol = usuario.rol
            };
        }
        // ============================================================
        // 🧩 Alias para compatibilidad con controladores REST
        // ============================================================
        public List<UsuarioDto> ListarUsuarios()
        {
            return _context.Usuario.Select(u => new UsuarioDto
            {
                IdUsuario = u.id_usuario,
                Nombre = u.nombre,
                Apellido = u.apellido,
                Email = u.email,
                Contrasena = u.contrasena,
                Direccion = u.direccion,
                Pais = u.pais,
                Edad = u.edad,
                TipoIdentificacion = u.tipo_identificacion,
                Identificacion = u.identificacion,
                Rol = u.rol,
                UsuarioCorreo = u.email
            }).ToList();
        }


    }
}
