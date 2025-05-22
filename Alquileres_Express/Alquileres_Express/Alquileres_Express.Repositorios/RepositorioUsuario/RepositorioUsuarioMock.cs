using System;

using Alquileres_Express.Aplicacion.Interfaces;
using Alquileres_Express.Aplicacion.Entidades;
using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore.Metadata;
namespace Alquileres_Express.Repositorios;

public class RepositorioUsuarioMock : IRepositorioUsuario
{
    private readonly List<Usuario> _listaUsuarios = new List<Usuario>()
    {
        new Usuario(){ID = 1,
            correo = "maria@gmail.com",
            contraseña = "123456",
            nombre = "Maria",
            apellido = "Torres",
            direccion = "La plata",
            fechaNacimiento = new DateTime(2001, 01, 01)}
    };

    static int _proximoId = 2;
    private Usuario Clonar(Usuario u) //Explicar uso del clonar
    {
        return new Usuario()
        {
            ID = u.ID,
            correo = u.correo,
            contraseña = u.contraseña,
            nombre = u.nombre,
            apellido = u.apellido,
            direccion = u.direccion,
            fechaNacimiento = u.fechaNacimiento
        };
    }
    public void RegistrarUsuario(Usuario u)
{
    
    if (_listaUsuarios.Any(aux => aux.correo == u.correo))
        throw new InvalidOperationException("El correo ya está registrado por otro usuario.");

    if (u.contraseña.Length < 6)
        throw new InvalidOperationException("La contraseña debe tener 6 o más dígitos.");
    //Calcular edad
    var today = DateTime.Today;
    int edad = today.Year - u.fechaNacimiento.Year;
    if (u.fechaNacimiento > today.AddYears(-edad))
        edad--;

    if (edad < 18)
        throw new InvalidOperationException("El usuario debe ser mayor de edad.");

    u.ID = _proximoId++;
    u.contraseña = hash(u.contraseña);
    _listaUsuarios.Add(Clonar(u));
}

    private string hash(string Contraseña)
    {
        using var sha256 = SHA256.Create();
        byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(Contraseña));
        var sb = new StringBuilder();
        for (int i = 0; i < bytes.Length; i++)
        {
            sb.Append(bytes[i].ToString("x2"));
        }
        return sb.ToString();
    }

    public List<Usuario> GetUsuarios()//Devuelve una copia de la lista de usuarios
    {
        return _listaUsuarios.Select(u => Clonar(u)).ToList();
    }
}