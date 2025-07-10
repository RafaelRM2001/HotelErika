using System;
using System.Data;
using System.Data.SqlClient;
using CapaEntidad;

namespace CapaDatos
{
    public class datReserva
    {
        private static readonly datReserva instancia = new datReserva();
        public static datReserva Instancia => instancia;

        public List<entReserva> Listar()
        {
            List<entReserva> lista = new List<entReserva>();
            using (SqlConnection cn = conexion.Instancia.Conectar())
            {
                SqlCommand cmd = new SqlCommand("spListarReservas", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new entReserva
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        ClienteId = Convert.ToInt32(dr["ClienteId"]),
                        NombreCliente = dr["NombreCliente"].ToString(),
                        HabitacionId = Convert.ToInt32(dr["HabitacionId"]),
                        NumeroHabitacion = dr["NumeroHabitacion"].ToString(),
                        FechaReserva = Convert.ToDateTime(dr["FechaReserva"]),
                        FechaIngreso = Convert.ToDateTime(dr["FechaIngreso"]),
                        FechaSalida = Convert.ToDateTime(dr["FechaSalida"]),
                        Estado = dr["Estado"].ToString(),
                        FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"]),
                        FechaActualizacion = dr["FechaActualizacion"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(dr["FechaActualizacion"])
                    });
                }
            }
            return lista;
        }

        public bool Insertar(entReserva r)
        {
            using (SqlConnection cn = conexion.Instancia.Conectar())
            {
                SqlCommand cmd = new SqlCommand("spInsertarReserva", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClienteId", r.ClienteId);
                cmd.Parameters.AddWithValue("@HabitacionId", r.HabitacionId);
                cmd.Parameters.AddWithValue("@FechaIngreso", r.FechaIngreso);
                cmd.Parameters.AddWithValue("@FechaSalida", r.FechaSalida);
                cmd.Parameters.AddWithValue("@Estado", r.Estado);
                cn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public entReserva ObtenerPorId(int id)
        {
            entReserva r = null;
            using (SqlConnection cn = conexion.Instancia.Conectar())
            {
                SqlCommand cmd = new SqlCommand("spObtenerReservaPorId", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    r = new entReserva
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        ClienteId = Convert.ToInt32(dr["ClienteId"]),
                        HabitacionId = Convert.ToInt32(dr["HabitacionId"]),
                        FechaIngreso = Convert.ToDateTime(dr["FechaIngreso"]),
                        FechaSalida = Convert.ToDateTime(dr["FechaSalida"]),
                        Estado = dr["Estado"].ToString(),
                        FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"]),
                        FechaActualizacion = dr["FechaActualizacion"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(dr["FechaActualizacion"])
                    };
                }
            }
            return r;
        }

        public bool AplazarReserva(int id, DateTime nuevaFechaIngreso, DateTime nuevaFechaSalida)
        {
            using (SqlConnection cn = conexion.Instancia.Conectar())
            {
                SqlCommand cmd = new SqlCommand("spActualizarReserva", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@NuevaFechaIngreso", nuevaFechaIngreso);
                cmd.Parameters.AddWithValue("@NuevaFechaSalida", nuevaFechaSalida);
                cn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
