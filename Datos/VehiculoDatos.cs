using AccesoDatos;
using AccesoDatos.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class VehiculoDatos
    {
        // 🔹 Conexión al contexto de base de datos generado por el EDMX
        private readonly db31808Entities1 _context = new db31808Entities1();

        // ============================================================
        // 🟢 CREATE - Crear un nuevo vehículo
        // ============================================================
        public int Crear(Vehiculo nuevo)
        {
            _context.Vehiculo.Add(nuevo);     // Agrega el nuevo objeto al contexto
            _context.SaveChanges();            // Guarda los cambios en la base de datos
            return nuevo.id_vehiculo;          // Retorna el ID generado automáticamente
        }

        // ============================================================
        // 🔵 READ - Listar todos los vehículos
        // ============================================================
        // 🔹 Listar vehículos con JOIN a TipoTransmision, CategoriaVehiculo y Sucursal
        public List<VehiculoDto> Listar()
        {
            var query = from v in _context.Vehiculo
                        join c in _context.CategoriaVehiculo on v.id_categoria equals c.id_categoria into catJoin
                        from c in catJoin.DefaultIfEmpty()

                        join t in _context.TipoTransmision on v.id_transmision equals t.id_transmision into transJoin
                        from t in transJoin.DefaultIfEmpty()

                        join s in _context.Sucursal on v.id_sucursal equals s.id_sucursal into sucJoin
                        from s in sucJoin.DefaultIfEmpty()

                        select new VehiculoDto
                        {
                            IdVehiculo = v.id_vehiculo,
                            Marca = v.marca,
                            Modelo = v.modelo,
                            Anio = v.anio,

                            IdCategoria = v.id_categoria,
                            CategoriaNombre = c != null ? c.nombre : "Sin categoría",

                            IdTransmision = v.id_transmision ?? 0,
                            TransmisionNombre = t != null ? t.nombre : "No definido",

                            Capacidad = v.capacidad ?? 0,
                            PrecioDia = v.precio_dia,
                            Estado = v.estado,
                            Descripcion = v.descripcion,

                            IdSucursal = v.id_sucursal ?? 0,
                            SucursalNombre = s != null ? s.nombre : "Sin sucursal",

                            UrlImagen = null // se llena luego desde la capa de lógica si usas Cloudinary
                        };

            return query.ToList();
        }
        // ============================================================
        // 🔍 READ (por ID) - Obtener un vehículo específico
        // ============================================================
        public VehiculoDto ObtenerPorId(int id)
        {
            // Busca el vehículo en la lista de DTOs por ID
            return Listar().FirstOrDefault(v => v.IdVehiculo == id);
        }

        // ============================================================
        // 🟠 UPDATE - Modificar los datos de un vehículo existente
        // ============================================================
        public bool Actualizar(Vehiculo mod)
        {
            // Busca el vehículo original en la base
            var veh = _context.Vehiculo.Find(mod.id_vehiculo);
            if (veh == null) return false; // Si no existe, retorna false

            // Actualiza los campos
            veh.marca = mod.marca;
            veh.modelo = mod.modelo;
            veh.anio = mod.anio;
            veh.id_categoria = mod.id_categoria;
            veh.id_transmision = mod.id_transmision;
            veh.capacidad = mod.capacidad;
            veh.precio_dia = mod.precio_dia;
            veh.estado = mod.estado;
            veh.descripcion = mod.descripcion;
            veh.id_sucursal = mod.id_sucursal;

            _context.SaveChanges(); // Guarda los cambios
            return true; // Retorna éxito
        }

        // ============================================================
        // 🔴 DELETE - Eliminar un vehículo
        // ============================================================
        public bool Eliminar(int id)
        {
            // Busca el vehículo por ID
            var veh = _context.Vehiculo.Find(id);
            if (veh == null) return false;

            // Lo elimina del contexto y guarda cambios
            _context.Vehiculo.Remove(veh);
            _context.SaveChanges();
            return true;
        }
        // ============================================================
        // 🧩 Alias para compatibilidad con controladores REST
        // ============================================================
        public List<VehiculoDto> ListarVehiculos()
        {
            var query = from v in _context.Vehiculo
                        join c in _context.CategoriaVehiculo on v.id_categoria equals c.id_categoria into catJoin
                        from c in catJoin.DefaultIfEmpty()
                        join t in _context.TipoTransmision on v.id_transmision equals t.id_transmision into transJoin
                        from t in transJoin.DefaultIfEmpty()
                        join s in _context.Sucursal on v.id_sucursal equals s.id_sucursal into sucJoin
                        from s in sucJoin.DefaultIfEmpty()
                        select new VehiculoDto
                        {
                            IdVehiculo = v.id_vehiculo,
                            Marca = v.marca,
                            Modelo = v.modelo,
                            Anio = v.anio,

                            IdCategoria = v.id_categoria,
                            CategoriaNombre = c != null ? c.nombre : "Sin categoría",

                            IdTransmision = v.id_transmision ?? 0,
                            TransmisionNombre = t != null ? t.nombre : "No definido",

                            Capacidad = v.capacidad ?? 0,
                            PrecioDia = v.precio_dia,
                            Estado = v.estado,
                            Descripcion = v.descripcion,

                            IdSucursal = v.id_sucursal ?? 0,
                            SucursalNombre = s != null ? s.nombre : "Sin sucursal",
                            Matricula = v.matricula ?? "Sin matrícula",
                            // ✅ AGREGA ESTA LÍNEA
                            UrlImagen = null              // (puede quedar igual)
                        };


            return query.ToList();
        }



    }
}