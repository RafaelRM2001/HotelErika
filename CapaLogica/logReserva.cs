using System;
using System.Collections.Generic;
using CapaEntidad;
using CapaDatos;

namespace CapaLogica
{
    public class logReserva
    {
        #region Singleton
        private static readonly logReserva _instancia = new logReserva();
        public static logReserva Instancia => _instancia;
        #endregion

        #region Métodos Auxiliares

        /// <summary>
        /// Busca un cliente activo por su DNI o RUC.
        /// </summary>
        public entReserva BuscarClientePorDNI(string dni)
        {
            return datReserva.Instancia.BuscarClientePorDNI(dni);
        }

        public List<string> ListarTiposHabitacion()
        {
            return datReserva.Instancia.ListarTiposHabitacion();
        }

        public List<entReserva> ListarHabitacionesPorTipo(string tipo)
        {
            return datReserva.Instancia.ListarHabitacionesPorTipo(tipo);
        }

        public decimal ObtenerPrecioHabitacion(string numero)
        {
            return datReserva.Instancia.ObtenerPrecioHabitacion(numero);
        }

        #endregion

        #region CRUD Reserva

        public List<entReserva> ListarReserva()
        {
            return datReserva.Instancia.ListarReserva();
        }

        public bool InsertarReserva(entReserva r)
        {
            return datReserva.Instancia.InsertarReserva(r);
        }

        public entReserva ObtenerReservaPorId(int id)
        {
            return datReserva.Instancia.ObtenerReservaPorId(id);
        }

        public bool CancelarReserva(int id)
        {
            return datReserva.Instancia.CancelarReserva(id);
        }

        public List<entReserva> ObtenerReservasPorUsuario(int usuarioId)
        {
            return datReserva.Instancia.ObtenerReservasPorUsuario(usuarioId);
        }
        public bool ReprogramarReserva(int id, string numeroHabitacion, DateTime fechaEntrada, DateTime fechaSalida)
        {
            return datReserva.Instancia.ReprogramarReserva(id, numeroHabitacion, fechaEntrada, fechaSalida);
        }

        #endregion
    }
}
