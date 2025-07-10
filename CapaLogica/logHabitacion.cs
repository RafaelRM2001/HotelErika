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

        /// <summary>
        /// Listar todas las habitaciones activas.
        /// </summary>
        public List<entHabitacion> ListarHabitaciones()
        {
            return datHabitacion.Instancia.Listar();
        }

        /// <summary>
        /// Obtener una habitación por su ID.
        /// </summary>
        public entHabitacion ObtenerHabitacionPorId(int id)
        {
            return datHabitacion.Instancia.Buscar(id); // Usa el método correcto de datHabitacion
        }

        /// <summary>
        /// Insertar una nueva habitación.
        /// </summary>
        public bool InsertarHabitacion(entHabitacion habitacion)
        {
            // El campo FechaRegistro lo maneja SQL con GETDATE()
            habitacion.Activo = true; // Se asegura de activarla al insertar
            return datHabitacion.Instancia.Insertar(habitacion);
        }

        /// <summary>
        /// Editar una habitación existente.
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



        public bool EliminarHabitacion(int id)
        {
            return datHabitacion.Instancia.Eliminar(id);
        }

    }
}
