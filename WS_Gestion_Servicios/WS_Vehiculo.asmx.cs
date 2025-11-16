using AccesoDatos.DTO;
using Logica;
using Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace WS_Gestion_Servicios
{
    [WebService(Namespace = "http://rentaautos.ec/gestion")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WS_Vehiculo : WebService
    {
        private readonly VehiculoLogica _logic = new VehiculoLogica();
        private readonly ImagenVehiculoDatos _imgDatos = new ImagenVehiculoDatos();

        // ============================================================
        // 🟢 GET: Lista todos los vehículos
        // ============================================================
        [WebMethod(Description = "Lista todos los vehículos disponibles/registrados.")]
        public List<VehiculoDto> obtenerVehiculos()
        {
            var vehiculos = _logic.ListarVehiculos();

            // Agregar la URL de imagen correspondiente
            foreach (var v in vehiculos)
            {
                var img = _imgDatos.ListarPorVehiculo(v.IdVehiculo).FirstOrDefault();
                v.UrlImagen = img?.UriImagen; // si no tiene imagen, queda null
            }

            return vehiculos;
        }

        // ============================================================
        // 🔍 GET: Detalle por ID
        // ============================================================
        [WebMethod(Description = "Obtiene el detalle de un vehículo por su ID.")]
        public VehiculoDto obtenerVehiculoPorId(int idVehiculo)
        {
            var vehiculo = _logic.ObtenerVehiculoPorId(idVehiculo);
            if (vehiculo == null) return null;

            // Agregar la URL de imagen
            var img = _imgDatos.ListarPorVehiculo(idVehiculo).FirstOrDefault();
            vehiculo.UrlImagen = img?.UriImagen;

            return vehiculo;
        }

        // ============================================================
        // 🟡 POST: Crea un vehículo y retorna el ID generado
        // ============================================================
        [WebMethod(Description = "Crea un nuevo vehículo y retorna el ID generado.")]
        public int crearVehiculo(VehiculoDto vehiculo)
        {
            if (vehiculo == null)
                throw new SoapException("El objeto 'vehiculo' es requerido.", System.Xml.XmlQualifiedName.Empty);

            return _logic.CrearVehiculo(vehiculo);
        }

        // ============================================================
        // 🟠 PUT: Actualiza un vehículo existente
        // ============================================================
        [WebMethod(Description = "Actualiza los datos de un vehículo existente.")]
        public bool actualizarVehiculo(int idVehiculo, VehiculoDto vehiculo)
        {
            if (vehiculo == null)
                throw new SoapException("El objeto 'vehiculo' es requerido.", System.Xml.XmlQualifiedName.Empty);

            vehiculo.IdVehiculo = idVehiculo;
            return _logic.ActualizarVehiculo(vehiculo);
        }

        // ============================================================
        // 🔴 DELETE: Elimina un vehículo por ID
        // ============================================================
        [WebMethod(Description = "Elimina (o marca inactivo) un vehículo por ID.")]
        public bool eliminarVehiculo(int idVehiculo)
        {
            return _logic.EliminarVehiculo(idVehiculo);
        }

        // ============================================================
        // 🔎 OPCIONAL: Buscar por filtros (categoría, transmisión, estado)
        // ============================================================
        [WebMethod(Description = "Busca vehículos por filtros opcionales: categoria, transmision, estado.")]
        public List<VehiculoDto> buscarVehiculos(string categoria, string transmision, string estado)
        {
            var vehiculos = _logic.BuscarVehiculos(categoria, transmision, estado);

            // Agregar imágenes
            foreach (var v in vehiculos)
            {
                var img = _imgDatos.ListarPorVehiculo(v.IdVehiculo).FirstOrDefault();
                v.UrlImagen = img?.UriImagen;
            }

            return vehiculos;
        }
    }
}