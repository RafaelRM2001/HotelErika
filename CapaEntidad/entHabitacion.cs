using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CapaEntidad
{
    public class entHabitacion
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El número de habitación es obligatorio.")]
        [StringLength(10, ErrorMessage = "Máximo 10 caracteres.")]
        public string Numero { get; set; }

        [Required(ErrorMessage = "El piso es obligatorio.")]
        [Range(1, 100, ErrorMessage = "El piso debe estar entre 1 y 100.")]
        public int Piso { get; set; }

        [Required(ErrorMessage = "El tipo de habitación es obligatorio.")]
        [StringLength(50, ErrorMessage = "Máximo 50 caracteres.")]
        public string Tipo { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(1, 10000, ErrorMessage = "El precio debe ser mayor que 0.")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        [StringLength(50, ErrorMessage = "Máximo 50 caracteres.")]
        public string Estado { get; set; }

        [StringLength(500, ErrorMessage = "Máximo 500 caracteres.")]
        public string Descripcion { get; set; }

        public DateTime FechaRegistro { get; set; }

        public DateTime? FechaActualizacion { get; set; }

        public bool Activo { get; set; }

        public string DescripcionExtendida => $"{Numero} - {Tipo} - S/ {Precio}";
    }
}