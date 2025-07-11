using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class entComprobante
    {
        public int Id { get; set; }
        public int ReservaId { get; set; }
        public string MetodoPago { get; set; } = "";
        public decimal Monto { get; set; }
        public DateTime FechaEmision { get; set; }
        public string NombreCliente { get; set; }
        public string ApellidoCliente { get; set; }
    }
}
