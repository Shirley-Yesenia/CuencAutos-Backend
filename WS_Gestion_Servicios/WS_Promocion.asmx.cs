using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        [WebMethod(Description = "Lista promociones o descuentos vigentes.")]
        public List<PromocionDto> ObtenerPromociones()
        {
            return logica.ListarPromociones();
        }

        [WebMethod(Description = "Crea una nueva promoción.")]
        public bool CrearPromocion(PromocionDto dto)
        {
            int resultado = logica.CrearPromocion(dto);
            return resultado > 0; // devuelve true si se insertó al menos una fila
        }

        [WebMethod(Description = "Actualiza una promoción existente.")]
        public bool ActualizarPromocion(PromocionDto dto)
        {
            return logica.ActualizarPromocion(dto);
        }

        [WebMethod(Description = "Elimina una promoción por su ID.")]
        public bool EliminarPromocion(int idPromocion)
        {
            return logica.EliminarPromocion(idPromocion);
        }
    }
}