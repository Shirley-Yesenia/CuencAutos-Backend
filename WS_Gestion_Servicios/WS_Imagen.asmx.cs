using System;
using System.Collections.Generic;
using System.Web.Services;
using Logica;
using AccesoDatos.DTO;

namespace WS_Gestion_Servicios   // ✅ Debe coincidir con el .asmx
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WS_Imagen : WebService
    {
        private readonly ImagenVehiculoLogica logicaImagen = new ImagenVehiculoLogica();

        [WebMethod(Description = "Lista todas las imágenes registradas en el sistema.")]
        public List<ImagenVehiculoDto> ListarImagenes()
        {
            return logicaImagen.ListarImagenes();
        }

        [WebMethod(Description = "Obtiene una imagen específica por su ID.")]
        public ImagenVehiculoDto ObtenerPorId(int idImagen)
        {
            return logicaImagen.ObtenerPorId(idImagen);
        }

        [WebMethod(Description = "Obtiene todas las imágenes asociadas a un vehículo.")]
        public List<ImagenVehiculoDto> ListarPorVehiculo(int idVehiculo)
        {
            return logicaImagen.ListarPorVehiculo(idVehiculo);
        }

        [WebMethod(Description = "Crea una nueva imagen asociada a un vehículo.")]
        public bool CrearImagen(ImagenVehiculoDto dto)
        {
            return logicaImagen.CrearImagen(dto);
        }

        [WebMethod(Description = "Actualiza los datos de una imagen existente.")]
        public bool ActualizarImagen(ImagenVehiculoDto dto)
        {
            return logicaImagen.ActualizarImagen(dto);
        }

        [WebMethod(Description = "Elimina una imagen registrada por su ID.")]
        public bool EliminarImagen(int idImagen)
        {
            return logicaImagen.EliminarImagen(idImagen);
        }

        [WebMethod(Description = "Devuelve la imagen principal (primera) asociada a un vehículo.")]
        public ImagenVehiculoDto ObtenerPrincipal(int idVehiculo)
        {
            return logicaImagen.ObtenerPrincipal(idVehiculo);
        }
    }
}