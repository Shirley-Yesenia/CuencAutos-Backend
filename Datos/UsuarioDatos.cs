using AccesoDatos;
using AccesoDatos.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Datos
{
    public class UsuarioDatos
    {
        private readonly db31808Entities1 _context = new db31808Entities1();

        // ============================================================
        // 🟢 CREATE - Crear un nuevo usuario + carrito automático
        // ============================================================
        public int Crear(Usuario nuevo)
        {
            // Crear usuario
            _context.Usuario.Add(nuevo);
            _context.SaveChanges(); // Genera id_usuario

            // Crear carrito automático (SIN estado)
            var carrito = new Carrito
            {
                id_usuario = nuevo.id_usuario,
                fecha_creacion = DateTime.Now
            };

            _context.Carrito.Add(carrito);
            _context.SaveChanges();

            return nuevo.id_usuario;
        }

        // ============================================================
        // 🔵 READ - Listar todos los usuarios
        // ============================================================
        public List<UsuarioDto> Listar()
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
                Rol = u.rol
            }).ToList();
        }

        // ============================================================
        // 🔍 READ - Obtener un usuario por ID
        // ============================================================
        public UsuarioDto ObtenerPorId(int id)
        {
            return Listar().FirstOrDefault(u => u.IdUsuario == id);
        }

        // ============================================================
        // 🔍 READ - Obtener usuario por EMAIL
        // ============================================================
        public Usuario ObtenerPorEmail(string email)
        {
            return _context.Usuario.FirstOrDefault(u => u.email == email);
        }

        public UsuarioDto ObtenerDtoPorEmail(string email)
        {
            var u = _context.Usuario.FirstOrDefault(x => x.email == email);
            if (u == null) return null;

            return new UsuarioDto
            {
                IdUsuario = u.id_usuario,
                Nombre = u.nombre,
                Apellido = u.apellido,
                Email = u.email,
                Direccion = u.direccion,
                Pais = u.pais,
                Edad = u.edad,
                TipoIdentificacion = u.tipo_identificacion,
                Identificacion = u.identificacion,
                Rol = u.rol
            };
        }

        // ============================================================
        // 🟠 UPDATE - Actualizar usuario
        // ============================================================
        public bool Actualizar(Usuario mod)
        {
            var u = _context.Usuario.Find(mod.id_usuario);
            if (u == null) return false;

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

            _context.SaveChanges();
            return true;
        }

        // ============================================================
        // 🔴 DELETE - Eliminar usuario
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
        // 🧩 Login simple
        // ============================================================
        public UsuarioDto Login(string email, string contrasena)
        {
            var usuario = _context.Usuario.FirstOrDefault(u =>
                u.email == email && u.contrasena == contrasena);

            if (usuario == null) return null;

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
        // 🧩 Alias adicional
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
