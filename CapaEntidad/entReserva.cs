using System;
using System.ComponentModel.DataAnnotations;

namespace CapaEntidad
{
    public class entReserva
    {
        public int Id { get; set; }

        // Datos del cliente
        [Required(ErrorMessage = "El DNI o RUC es obligatorio.")]
        public string DNI_RUC { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre del cliente es obligatorio.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido del cliente es obligatorio.")]
        public string Apellido { get; set; } = string.Empty;

        // Datos de la habitación reservada
        [Required(ErrorMessage = "El tipo de habitación es obligatorio.")]
        public string TipoHabitacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El número de habitación es obligatorio.")]
        public string NumeroHabitacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser un valor positivo.")]
        public decimal Precio { get; set; }

        // Fechas de la reserva
        [Required(ErrorMessage = "La fecha de entrada es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime FechaEntrada { get; set; }

        [Required(ErrorMessage = "La fecha de salida es obligatoria.")]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(entReserva), nameof(ValidarFechas))]
        public DateTime FechaSalida { get; set; }

        // Fecha en que se registró la reserva
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        // Para lógica de cancelación o baja sin eliminar
        public bool Activo { get; set; } = true;

        public int UsuarioId { get; set; }  // 🔑 Identificador del usuario que hizo la reserva

        /// Validación custom para asegurar que FechaSalida > FechaEntrada.
        /// </summary>
        public static ValidationResult? ValidarFechas(DateTime fechaSalida, ValidationContext context)
        {
            var instancia = (entReserva)context.ObjectInstance;
            if (fechaSalida <= instancia.FechaEntrada)
            {
                return new ValidationResult("La fecha de salida debe ser posterior a la fecha de entrada.", new[] { nameof(FechaSalida) });
            }
            return ValidationResult.Success;
        }
    }
}
