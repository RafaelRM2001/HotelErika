using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class logReserva
    {
        private static readonly logReserva _instancia = new logReserva();
        public static logReserva Instancia => _instancia;

        public List<entReserva> ListarReservas()
        {
            return datReserva.Instancia.Listar();
        }

        public bool InsertarReserva(entReserva r)
        {
            r.FechaRegistro = DateTime.Now;
            return datReserva.Instancia.Insertar(r);
        }

        public bool AplazarReserva(int id, DateTime nuevaIngreso, DateTime nuevaSalida)
        {
            return datReserva.Instancia.AplazarReserva(id, nuevaIngreso, nuevaSalida);
        }

        public entReserva ObtenerReservaPorId(int id)
        {
            return datReserva.Instancia.ObtenerPorId(id);
        }
    }
}
