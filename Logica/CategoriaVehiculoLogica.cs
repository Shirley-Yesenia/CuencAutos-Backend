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
    public class CategoriaVehiculoLogica
    {
        private readonly CategoriaVehiculoDatos categoriaDatos = new CategoriaVehiculoDatos();

        // ============================================================
        // 🟢 CREATE - Registrar una nueva categoría
        // ============================================================
        public CategoriaVehiculoDto CrearCategoria(CategoriaVehiculoDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException("dto");
            if (string.IsNullOrWhiteSpace(dto.Nombre))
                throw new ArgumentException("El nombre de la categoria es obligatorio.");

            var entidad = new CategoriaVehiculo
            {
                nombre = dto.Nombre.Trim(),
                descripcion = dto.Descripcion?.Trim()
            };

            var idNuevo = categoriaDatos.Crear(entidad);     // devuelve el ID identity
            var creado = categoriaDatos.ObtenerPorId(idNuevo);
            if (creado == null) throw new Exception("La categoria se creo pero no pudo recuperarse.");
            return creado;
        }


        // ============================================================
        // 🔵 READ - Listar todas las categorías
        // ============================================================
        public List<CategoriaVehiculoDto> ListarCategorias()
        {
            return categoriaDatos.Listar();
        }

        // ============================================================
        // 🔍 READ - Obtener una categoría por ID
        // ============================================================
        public CategoriaVehiculoDto ObtenerPorId(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID debe ser mayor a 0.");

            return categoriaDatos.ObtenerPorId(id);
        }

        // ============================================================
        // 🟠 UPDATE - Actualizar una categoría existente
        // ============================================================
        public bool ActualizarCategoria(CategoriaVehiculoDto dto)
        {
            if (dto == null || dto.IdCategoria <= 0)
                throw new ArgumentException("Datos inválidos para actualizar la categoría.");

            var entidad = new CategoriaVehiculo
            {
                id_categoria = dto.IdCategoria,
                nombre = dto.Nombre?.Trim(),
                descripcion = dto.Descripcion?.Trim()
            };

            return categoriaDatos.Actualizar(entidad);
        }

        // ============================================================
        // 🔴 DELETE - Eliminar una categoría
        // ============================================================
        public bool EliminarCategoria(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID debe ser válido.");

            return categoriaDatos.Eliminar(id);
        }

        // ============================================================
        // ⚙ EXTRA - Buscar categorías por nombre parcial
        // ============================================================
        public List<CategoriaVehiculoDto> BuscarPorNombre(string texto)
        {
            var categorias = categoriaDatos.Listar();

            if (string.IsNullOrWhiteSpace(texto))
                return categorias;

            return categorias.FindAll(c =>
                c.Nombre.ToLower().Contains(texto.ToLower()));
        }
    }
}