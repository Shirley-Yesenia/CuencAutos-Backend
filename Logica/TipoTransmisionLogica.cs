using AccesoDatos;
using AccesoDatos.DTO;
using Datos;
using System;
using System.Collections.Generic;


namespace Logica
{
    public class TipoTransmisionLogica
    {
        private readonly TipoTransmisionDatos _datos = new TipoTransmisionDatos();

        // 🔹 Listar todas las transmisiones
        public List<TipoTransmision> Listar()
        {
            return _datos.Listar();
        }

        // 🔹 Buscar por ID
        public TipoTransmision BuscarPorId(int id)
        {
            return _datos.BuscarPorId(id);
        }

        // 🔹 Insertar nueva transmisión
        public bool Insertar(TipoTransmision tipo)
        {
            if (string.IsNullOrWhiteSpace(tipo.nombre))
                return false;

            return _datos.Insertar(tipo);
        }

        // 🔹 Actualizar transmisión existente
        public bool Actualizar(TipoTransmision tipo)
        {
            return _datos.Actualizar(tipo);
        }

        // 🔹 Eliminar transmisión
        public bool Eliminar(int id)
        {
            return _datos.Eliminar(id);
        }
    }
}
