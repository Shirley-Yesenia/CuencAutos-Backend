using System;
using System.Collections.Generic;
using System.Web.Services;
using AccesoDatos.DTO;
using Logica;

namespace WS_Gestion_Servicios
{
    [WebService(Namespace = "http://rentautos.com.ec/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class WS_Promocion : WebService
    {
        private readonly PromocionLogica logica = new PromocionLogica();

        // ==========================================================
        // GET: Lista todas las promociones
        // ==========================================================
        [WebMethod(Description = "Lista todas las promociones registradas.")]
        public List<PromocionDto> ObtenerPromociones()
        {
            try
            {
                return logica.ListarPromociones();
            }
            catch
            {
                return null;
            }
        }

        // ==========================================================
        // GET BY ID
        // ==========================================================
        [WebMethod(Description = "Obtiene una promoción por su ID.")]
        public PromocionDto ObtenerPromocionPorId(int idPromocion)
        {
            try
            {
                return logica.ObtenerPromocionPorId(idPromocion);
            }
            catch
            {
                return null;
            }
        }

        // ==========================================================
        // POST: Crear promoción
        // ==========================================================
        [WebMethod(Description = "Crea una nueva promoción.")]
        public PromocionDto CrearPromocion(PromocionDto dto)
        {
            if (dto == null)
                return null;

            try
            {
                int idNuevo = logica.CrearPromocion(dto);

                if (idNuevo <= 0)
                    return null;

                dto.IdPromocion = idNuevo;
                return dto;
            }
            catch
            {
                return null;
            }
        }

        // ==========================================================
        // PUT: Actualizar promoción
        // ==========================================================
        [WebMethod(Description = "Actualiza una promoción existente.")]
        public bool ActualizarPromocion(int idPromocion, PromocionDto dto)
        {
            if (dto == null)
                return false;

            try
            {
                dto.IdPromocion = idPromocion;
                return logica.ActualizarPromocion(dto);
            }
            catch
            {
                return false;
            }
        }

        // ==========================================================
        // DELETE
        // ==========================================================
        [WebMethod(Description = "Elimina una promoción por su ID.")]
        public bool EliminarPromocion(int idPromocion)
        {
            try
            {
                return logica.EliminarPromocion(idPromocion);
            }
            catch
            {
                return false;
            }
        }

        // ==========================================================
        // PING
        // ==========================================================
        [WebMethod(Description = "Verifica si el servicio SOAP está operativo.")]
        public string Ping()
        {
            return "Servicio SOAP de Promociones operativo ✔";
        }
    }
}
