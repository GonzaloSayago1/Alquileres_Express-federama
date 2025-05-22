using System;

using Alquileres_Express.Aplicacion.Entidades;
namespace Alquileres_Express.Aplicacion.Interfaces;

public interface IRepositorioInmueble
{
    List<Usuario> GetUsuarios();
    void RegistrarUsuario(Usuario u);
}