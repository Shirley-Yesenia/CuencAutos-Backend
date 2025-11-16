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
    public class SucursalLogica
    {
        private readonly SucursalDatos datos = new SucursalDatos();

        // ============================================================
        // 🔵 LISTAR TODAS LAS SUCURSALES
        // ============================================================
        public List<SucursalDto> ListarSucursales()
        {
            return datos.Listar();
        }

        // ============================================================
        // 🔍 OBTENER SUCURSAL POR ID
        // ============================================================
        public SucursalDto ObtenerSucursalPorId(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID de sucursal no es válido.");

            return datos.ObtenerPorId(id);
        }

        // ============================================================
        // 🟢 CREAR NUEVA SUCURSAL
        // ============================================================
        public int CrearSucursal(SucursalDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "Los datos de la sucursal no pueden ser nulos.");

            if (string.IsNullOrWhiteSpace(dto.Nombre))
                throw new Exception("El nombre de la sucursal es obligatorio.");

            if (string.IsNullOrWhiteSpace(dto.Ciudad))
                throw new Exception("Debe especificar la ciudad.");

            if (string.IsNullOrWhiteSpace(dto.Pais))
                throw new Exception("Debe especificar el país.");

            var entidad = new Sucursal
            {
                nombre = dto.Nombre,
                ciudad = dto.Ciudad,
                pais = dto.Pais,
                direccion = dto.Direccion
            };

            return datos.Crear(entidad);
        }

        // ============================================================
        // 🟠 ACTUALIZAR SUCURSAL
        // ============================================================
        public bool ActualizarSucursal(SucursalDto dto)
        {
            if (dto == null || dto.IdSucursal <= 0)
                throw new Exception("Datos inválidos para actualizar la sucursal.");

            var entidad = new Sucursal
            {
                id_sucursal = dto.IdSucursal,
                nombre = dto.Nombre,
                ciudad = dto.Ciudad,
                pais = dto.Pais,
                direccion = dto.Direccion
            };

            return datos.Actualizar(entidad);
        }

        // ============================================================
        // 🔴 ELIMINAR SUCURSAL
        // ============================================================
        public bool EliminarSucursal(int id)
        {
            if (id <= 0)
                throw new Exception("ID inválido para eliminar la sucursal.");

            return datos.Eliminar(id);
        }

        // ============================================================
        // 🔎 BUSCAR SUCURSALES POR CIUDAD
        // ============================================================
        public List<SucursalDto> BuscarSucursalesPorCiudad(string ciudad)
        {
            if (string.IsNullOrWhiteSpace(ciudad))
                throw new Exception("Debe ingresar una ciudad para la búsqueda.");

            return datos.BuscarPorCiudad(ciudad);
        }
    }
}
