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
    public class VehiculoLogica
    {
        private readonly VehiculoDatos datos = new VehiculoDatos();

        // ============================================================
        // 🔵 LISTAR TODOS LOS VEHÍCULOS
        // ============================================================
        public List<VehiculoDto> ListarVehiculos()
        {
            return datos.Listar();
        }

        // ============================================================
        // 🔍 OBTENER VEHÍCULO POR ID
        // ============================================================
        public VehiculoDto ObtenerVehiculoPorId(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID de vehículo no es válido.");

            return datos.ObtenerPorId(id);
        }

        // ============================================================
        // 🟢 CREAR NUEVO VEHÍCULO
        // ============================================================
        public int CrearVehiculo(VehiculoDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "Los datos del vehículo no pueden ser nulos.");

            if (string.IsNullOrWhiteSpace(dto.Marca))
                throw new Exception("El campo 'Marca' es obligatorio.");

            if (string.IsNullOrWhiteSpace(dto.Modelo))
                throw new Exception("El campo 'Modelo' es obligatorio.");

            if (dto.PrecioDia <= 0)
                throw new Exception("El precio por día debe ser mayor que cero.");

            var entidad = new Vehiculo
            {
                marca = dto.Marca,
                modelo = dto.Modelo,
                anio = dto.Anio,
                id_categoria = dto.IdCategoria,
                id_transmision = dto.IdTransmision,
                capacidad = dto.Capacidad,
                precio_dia = dto.PrecioDia,
                estado = dto.Estado ?? "Disponible",
                descripcion = dto.Descripcion,
                id_sucursal = dto.IdSucursal
            };

            return datos.Crear(entidad);
        }

        // ============================================================
        // 🟠 ACTUALIZAR VEHÍCULO EXISTENTE
        // ============================================================
        public bool ActualizarVehiculo(VehiculoDto dto)
        {
            if (dto == null || dto.IdVehiculo <= 0)
                throw new Exception("Datos inválidos para actualizar el vehículo.");

            var entidad = new Vehiculo
            {
                id_vehiculo = dto.IdVehiculo,
                marca = dto.Marca,
                modelo = dto.Modelo,
                anio = dto.Anio,
                id_categoria = dto.IdCategoria,
                id_transmision = dto.IdTransmision,
                capacidad = dto.Capacidad,
                precio_dia = dto.PrecioDia,
                estado = dto.Estado,
                descripcion = dto.Descripcion,
                id_sucursal = dto.IdSucursal
            };

            return datos.Actualizar(entidad);
        }

        // ============================================================
        // 🔴 ELIMINAR VEHÍCULO
        // ============================================================
        public bool EliminarVehiculo(int id)
        {
            if (id <= 0)
                throw new Exception("ID inválido para eliminar vehículo.");

            return datos.Eliminar(id);
        }

        // ============================================================
        // 🔍 FILTROS OPCIONALES (para REST)
        // ============================================================
        public List<VehiculoDto> BuscarVehiculos(string categoria = null, string transmision = null, string estado = null)
        {
            // 1️⃣ Obtiene todos los vehículos desde la capa de datos
            var lista = datos.Listar() ?? new List<VehiculoDto>();

            // 2️⃣ Normaliza (elimina espacios, acentos y pasa a minúsculas)
            categoria = Normalizar(categoria);
            transmision = Normalizar(transmision);
            estado = Normalizar(estado);

            // 3️⃣ Aplica filtros dinámicos
            if (!string.IsNullOrEmpty(categoria))
                lista = lista.Where(v => Normalizar(v.CategoriaNombre).Contains(categoria)).ToList();

            if (!string.IsNullOrEmpty(transmision))
                lista = lista.Where(v => Normalizar(v.TransmisionNombre).Contains(transmision)).ToList();

            if (!string.IsNullOrEmpty(estado))
                lista = lista.Where(v => Normalizar(v.Estado).Contains(estado)).ToList();

            // 4️⃣ Devuelve la lista (vacía o con resultados)
            return lista;
        }

        /// <summary>
        /// Elimina espacios, acentos y pasa a minúsculas para comparar sin errores.
        /// </summary>
        private static string Normalizar(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) return "";
            var normalized = texto.Trim().ToLowerInvariant();
            normalized = normalized.Replace("á", "a")
                                   .Replace("é", "e")
                                   .Replace("í", "i")
                                   .Replace("ó", "o")
                                   .Replace("ú", "u");
            return normalized;
        }

    }

}