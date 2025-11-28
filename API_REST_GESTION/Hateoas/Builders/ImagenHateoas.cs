using System;

namespace API_REST_GESTION.Hateoas.Builders
{
    public class ImagenHateoas
    {
        private readonly string _baseUrl;

        public ImagenHateoas(string baseUrl)
        {
            _baseUrl = baseUrl.TrimEnd('/');
        }

        public object LinksColeccion()
        {
            return new
            {
                self = $"{_baseUrl}/api/v1/imagenes",
                crear = $"{_baseUrl}/api/v1/imagenes"
            };
        }

        public object LinksImagen(int idImagen)
        {
            return new
            {
                self = $"{_baseUrl}/api/v1/imagenes/{idImagen}",
                actualizar = $"{_baseUrl}/api/v1/imagenes/{idImagen}",
                eliminar = $"{_baseUrl}/api/v1/imagenes/{idImagen}",
                listar = $"{_baseUrl}/api/v1/imagenes"
            };
        }

        public object LinksPorVehiculo(int idVehiculo)
        {
            return new
            {
                self = $"{_baseUrl}/api/v1/imagenes/vehiculo/{idVehiculo}",
                principal = $"{_baseUrl}/api/v1/imagenes/vehiculo/{idVehiculo}/principal",
                listar = $"{_baseUrl}/api/v1/imagenes"
            };
        }

        public object LinksPrincipal(int idVehiculo)
        {
            return new
            {
                self = $"{_baseUrl}/api/v1/imagenes/vehiculo/{idVehiculo}/principal",
                imagenesVehiculo = $"{_baseUrl}/api/v1/imagenes/vehiculo/{idVehiculo}",
                listar = $"{_baseUrl}/api/v1/imagenes"
            };
        }
    }
}
