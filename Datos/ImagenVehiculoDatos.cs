using AccesoDatos;
using AccesoDatos.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class ImagenVehiculoDatos
    {
        private readonly db31808Entities1 _context = new db31808Entities1();

        // ============================================================
        // 🟢 CREATE - Registrar una nueva imagen de vehículo
        // ============================================================
        public int CrearImagen(ImagenVehiculo nueva)
        {
            _context.ImagenVehiculo.Add(nueva);
            _context.SaveChanges();
            return nueva.id_imagen; // Retorna el ID generado
        }

        // ============================================================
        // 🔵 READ - Listar todas las imágenes
        // ============================================================
        public List<ImagenVehiculoDto> ListarImagenes()
        {
            return _context.ImagenVehiculo
                .Select(i => new ImagenVehiculoDto
                {
                    IdImagen = i.id_imagen,
                    IdVehiculo = i.id_vehiculo,
                    UriImagen = i.uri_imagen
                })
                .ToList();
        }

        // ============================================================
        // 🔍 READ - Obtener una imagen específica por ID
        // ============================================================
        public ImagenVehiculoDto ObtenerPorId(int idImagen)
        {
            var i = _context.ImagenVehiculo.Find(idImagen);
            if (i == null) return null;

            return new ImagenVehiculoDto
            {
                IdImagen = i.id_imagen,
                IdVehiculo = i.id_vehiculo,
                UriImagen = i.uri_imagen
            };
        }

        // ============================================================
        // 🟠 UPDATE - Actualizar una imagen existente
        // ============================================================
        public bool ActualizarImagen(ImagenVehiculo imagenEditada)
        {
            var existente = _context.ImagenVehiculo.Find(imagenEditada.id_imagen);
            if (existente == null) return false;

            existente.id_vehiculo = imagenEditada.id_vehiculo;
            existente.uri_imagen = imagenEditada.uri_imagen;

            _context.SaveChanges();
            return true;
        }

        // ============================================================
        // 🔴 DELETE - Eliminar una imagen
        // ============================================================
        public bool EliminarImagen(int idImagen)
        {
            var img = _context.ImagenVehiculo.Find(idImagen);
            if (img == null) return false;

            _context.ImagenVehiculo.Remove(img);
            _context.SaveChanges();
            return true;
        }

        // ============================================================
        // ⚙️ EXTRA - Listar imágenes por vehículo
        // ============================================================
        public List<ImagenVehiculoDto> ListarPorVehiculo(int idVehiculo)
        {
            return _context.ImagenVehiculo
                .Where(i => i.id_vehiculo == idVehiculo)
                .Select(i => new ImagenVehiculoDto
                {
                    IdImagen = i.id_imagen,
                    IdVehiculo = i.id_vehiculo,
                    UriImagen = i.uri_imagen
                })
                .ToList();
        }
    }
}
