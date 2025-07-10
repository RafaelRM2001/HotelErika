using System;
using CapaEntidad;
using CapaDatos;

namespace CapaLogica
{
    public class logUsuario
    {
        // Singleton
        private static readonly logUsuario instancia = new logUsuario();
        public static logUsuario Instancia => instancia;

        // LOGIN
        public entUsuario LoginUsuario(string email, string password)
        {
            return datUsuario.Instancia.LoginUsuario(email, password);
        }

        // REGISTRO
        public bool RegistrarUsuario(entUsuario usuario)
        {
            // Verifica que el rol sea válido (debe venir desde el controlador ya validado)
            if (usuario.RolId <= 0)
                throw new ArgumentException("Rol no válido.");

            return datUsuario.Instancia.InsertarUsuario(usuario);
        }

        // Obtener el Id del rol por su nombre
        public int ObtenerRolIdPorNombre(string nombreRol)
        {
            if (string.IsNullOrWhiteSpace(nombreRol))
                throw new ArgumentException("El nombre del rol no puede estar vacío.");

            return datUsuario.Instancia.ObtenerRolIdPorNombre(nombreRol.Trim());
        }
    }
}
