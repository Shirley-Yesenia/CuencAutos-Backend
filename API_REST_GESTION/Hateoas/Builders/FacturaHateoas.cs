using System;
using System.Collections.Generic;

namespace API_REST_GESTION.Hateoas.Builders
{
    public class FacturaHateoas
    {
        private readonly string _baseUrl;

        public FacturaHateoas(string baseUrl)
        {
            _baseUrl = baseUrl.TrimEnd('/');
        }

        public object ConstruirLinksFactura(int idFactura)
        {
            return new
            {
                self = $"{_baseUrl}/api/v1/facturas/{idFactura}",
                actualizar = $"{_baseUrl}/api/v1/facturas/{idFactura}",
                eliminar = $"{_baseUrl}/api/v1/facturas/{idFactura}",
                listar = $"{_baseUrl}/api/v1/facturas"
            };
        }

        public object ConstruirLinksColeccion()
        {
            return new
            {
                self = $"{_baseUrl}/api/v1/facturas",
                crear = $"{_baseUrl}/api/v1/facturas"
            };
        }
    }
}
