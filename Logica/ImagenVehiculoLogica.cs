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
    public class ImagenVehiculoLogica
    {
        private readonly ImagenVehiculoDatos imagenDatos = new ImagenVehiculoDatos();

        // ============================================================
        // 🟢 CREATE - Registrar una nueva imagen
        // ============================================================
        public bool CrearImagen(ImagenVehiculoDto dto)
        {
            try
            {
                if (dto == null)
                    throw new ArgumentNullException("El DTO de imagen no puede ser nulo.");

                if (dto.IdVehiculo <= 0)
                    throw new Exception("El vehículo asociado debe ser válido.");

                if (string.IsNullOrWhiteSpace(dto.UriImagen))
                    throw new Exception("La URL de la imagen es obligatoria.");

                // DTO → Entidad
                var entidad = new ImagenVehiculo
                {
                    id_vehiculo = dto.IdVehiculo,
                    uri_imagen = dto.UriImagen
                };

                imagenDatos.CrearImagen(entidad);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al crear imagen: {ex.Message}");
                return false;
            }
        }

        // ============================================================
        // 🔵 READ - Listar todas las imágenes
        // ============================================================
        public List<ImagenVehiculoDto> ListarImagenes()
        {
            return imagenDatos.ListarImagenes();
        }

        // ============================================================
        // 🔍 READ - Obtener imagen por ID
        // ============================================================
        public ImagenVehiculoDto ObtenerPorId(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID de la imagen debe ser válido.");

            return imagenDatos.ObtenerPorId(id);
        }

        // ============================================================
        // 🟣 READ - Listar imágenes por vehículo
        // ============================================================
        public List<ImagenVehiculoDto> ListarPorVehiculo(int idVehiculo)
        {
            if (idVehiculo <= 0)
                throw new ArgumentException("El ID del vehículo debe ser válido.");

            return imagenDatos.ListarPorVehiculo(idVehiculo);
        }

        // ============================================================
        // 🟠 UPDATE - Actualizar una imagen existente
        // ============================================================
        public bool ActualizarImagen(ImagenVehiculoDto dto)
        {
            if (dto == null || dto.IdImagen <= 0)
                throw new ArgumentException("Datos de imagen inválidos.");

            var entidad = new ImagenVehiculo
            {
                id_imagen = dto.IdImagen,
                id_vehiculo = dto.IdVehiculo,
                uri_imagen = dto.UriImagen
            };

            return imagenDatos.ActualizarImagen(entidad);
        }

        // ============================================================
        // 🔴 DELETE - Eliminar una imagen
        // ============================================================
        public bool EliminarImagen(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID de la imagen debe ser válido.");

            return imagenDatos.EliminarImagen(id);
        }

        // ============================================================
        // ⚙️ EXTRA - Obtener la imagen principal del vehículo (primera)
        // ============================================================
        public ImagenVehiculoDto ObtenerPrincipal(int idVehiculo)
        {
            var lista = imagenDatos.ListarPorVehiculo(idVehiculo);
            return lista.FirstOrDefault();
        }
    }
}
