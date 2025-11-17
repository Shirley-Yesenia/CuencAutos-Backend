using AccesoDatos.DTO;
using System;

public class ReservaDto : HateoasResource
{
    public int IdReserva { get; set; }
    public int IdUsuario { get; set; }
    public string NombreUsuario { get; set; }
    public string CorreoUsuario { get; set; }      // nuevo
    public int IdVehiculo { get; set; }
    public string VehiculoNombre { get; set; }
    public string VehiculoMatricula { get; set; }  // nuevo
    public string CategoriaNombre { get; set; }    // nuevo
    public string TransmisionNombre { get; set; }  // nuevo
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public decimal Total { get; set; }
    public string Estado { get; set; }
    public DateTime? FechaReserva { get; set; }
    public string UriFactura { get; set; }
    public string UsuarioCorreo { get; set; }
    // nuevo

}

