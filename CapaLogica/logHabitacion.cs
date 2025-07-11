using System;
using System.Collections.Generic;
using CapaDatos;
using CapaEntidad;

namespace CapaLogica
{
    public class logHabitacion
    {
        private static readonly logHabitacion _instancia = new logHabitacion();
        public static logHabitacion Instancia => _instancia;


        /// Listar todas las habitaciones activas.
        public List<entHabitacion> ListarHabitaciones()
        {
            return datHabitacion.Instancia.Listar();
        }


        /// Obtener una habitación por su ID.

        public entHabitacion ObtenerHabitacionPorId(int id)
        {
            return datHabitacion.Instancia.Buscar(id); // Usa el método correcto de datHabitacion
        }


        /// </summary>
        public bool InsertarHabitacion(entHabitacion habitacion)
        {
            // El campo FechaRegistro lo maneja SQL con GETDATE()
            habitacion.Activo = true; // Se asegura de activarla al insertar
            return datHabitacion.Instancia.Insertar(habitacion);
        }


        /// </summary>
        public bool EditarHabitacion(entHabitacion habitacion)
        {
            habitacion.FechaActualizacion = DateTime.Now;
            return datHabitacion.Instancia.Editar(habitacion);
        }


        public bool ExisteNumeroHabitacion(string numero)
        {
            return datHabitacion.Instancia.ExisteNumeroHabitacion(numero);
        }

        public List<entHabitacion> ListarHabitacionesDisponibles()
        {
            // Asume que el método ya existe en la capa datos
            return datHabitacion.Instancia.ListarHabitacionesDisponibles();
        }

        public bool EliminarHabitacion(int id)
        {
            return datHabitacion.Instancia.Eliminar(id);
        }

        public bool CambiarEstadoHabitacion(string numeroHabitacion, string nuevoEstado)
        {
            return datHabitacion.Instancia.CambiarEstado(numeroHabitacion, nuevoEstado);
        }

    }
}