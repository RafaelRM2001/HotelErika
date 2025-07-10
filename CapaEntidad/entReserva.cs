using System;
using System.ComponentModel.DataAnnotations;

namespace CapaEntidad
{
    public class entReserva
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string NombreCliente { get; set; } // Solo lectura
        public int HabitacionId { get; set; }
        public string NumeroHabitacion { get; set; } // Solo lectura
        public DateTime FechaReserva { get; set; }
        public DateTime FechaIngreso { get; set; }
        public DateTime FechaSalida { get; set; }
        public string Estado { get; set; } // Reservado, Completado, Aplazado
        public DateTime FechaRegistro { get; set; }
        public DateTime? FechaActualizacion { get; set; }

    }
}
