using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;

namespace CapaDatos
{
    public class datCliente
    {
        #region singleton
        private static readonly datCliente UnicaInstancia = new datCliente();
        public static datCliente Instancia
        {
            get
            {
                return datCliente.UnicaInstancia;
            }
        }
        #endregion singleton

        #region metodos

        public List<entCliente> ListarCliente()
        {
            SqlCommand cmd = null;
            List<entCliente> lista = new List<entCliente>();
            try
            {
                SqlConnection cn = conexion.Instancia.Conectar();
                cmd = new SqlCommand("spListarCliente", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entCliente c = new entCliente();
                    c.Id = Convert.ToInt32(dr["Id"]);
                    c.Nombre = dr["Nombre"].ToString();
                    c.Apellido = dr["Apellido"].ToString();
                    c.Email = dr["Email"].ToString();
                    c.Telefono = dr["Telefono"].ToString();
                    c.Direccion = dr["Direccion"].ToString();
                    c.DNI_RUC = dr["DNI_RUC"].ToString();
                    c.FechaNacimiento = dr["FechaNacimiento"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["FechaNacimiento"]);
                    c.TipoCliente = dr["TipoCliente"].ToString();
                    c.FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"]);
                    c.FechaActualizacion = dr["FechaActualizacion"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["FechaActualizacion"]);
                    c.Activo = Convert.ToBoolean(dr["Activo"]);
                    lista.Add(c);
                }
            }
            catch (SqlException e)
            {
                throw e;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return lista;
        }

        public Boolean InsertarCliente(entCliente c)
        {
            SqlCommand cmd = null;
            Boolean inserta = false;
            try
            {
                SqlConnection cn = conexion.Instancia.Conectar();
                cmd = new SqlCommand("spInsertarCliente", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Nombre", c.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", c.Apellido);
                cmd.Parameters.AddWithValue("@Email", c.Email);
                cmd.Parameters.AddWithValue("@Telefono", c.Telefono);
                cmd.Parameters.AddWithValue("@Direccion", c.Direccion);
                cmd.Parameters.AddWithValue("@DNI_RUC", c.DNI_RUC);
                cmd.Parameters.AddWithValue("@FechaNacimiento", (object)c.FechaNacimiento ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@TipoCliente", c.TipoCliente);
                cmd.Parameters.AddWithValue("@FechaRegistro", c.FechaRegistro);
                cmd.Parameters.AddWithValue("@Activo", c.Activo);

                cn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    inserta = true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return inserta;
        }

        public entCliente ObtenerClientePorId(int id)
        {
            SqlConnection cn = conexion.Instancia.Conectar();
            SqlCommand cmd = new SqlCommand("sp_ObtenerClientePorId", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);

            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            entCliente c = null;
            if (dr.Read())
            {
                c = new entCliente
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    Nombre = dr["Nombre"].ToString(),
                    Apellido = dr["Apellido"].ToString(),
                    Email = dr["Email"].ToString(),
                    Telefono = dr["Telefono"].ToString(),
                    Direccion = dr["Direccion"].ToString(),
                    DNI_RUC = dr["DNI_RUC"].ToString(),
                    FechaNacimiento = dr["FechaNacimiento"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["FechaNacimiento"]),
                    TipoCliente = dr["TipoCliente"].ToString(),
                    FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"]),
                    FechaActualizacion = dr["FechaActualizacion"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["FechaActualizacion"]),
                    Activo = Convert.ToBoolean(dr["Activo"])
                };
            }
            cn.Close();
            return c;
        }

        public bool EliminarCliente(int id)
        {
            SqlConnection cn = conexion.Instancia.Conectar();
            SqlCommand cmd = new SqlCommand("sp_EliminarCliente", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);
            cn.Open();
            int filasAfectadas = cmd.ExecuteNonQuery();
            cn.Close();
            return filasAfectadas > 0;
        }

        public bool EditarCliente(entCliente c)
        {
            SqlCommand cmd = null;
            bool actualizado = false;
            try
            {
                SqlConnection cn = conexion.Instancia.Conectar();
                cmd = new SqlCommand("sp_EditarCliente", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", c.Id);
                cmd.Parameters.AddWithValue("@Nombre", c.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", c.Apellido);
                cmd.Parameters.AddWithValue("@Email", c.Email);
                cmd.Parameters.AddWithValue("@Telefono", c.Telefono);
                cmd.Parameters.AddWithValue("@Direccion", c.Direccion);
                cmd.Parameters.AddWithValue("@DNI_RUC", c.DNI_RUC);
                cmd.Parameters.AddWithValue("@FechaNacimiento", (object)c.FechaNacimiento ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@TipoCliente", c.TipoCliente);
                cmd.Parameters.AddWithValue("@FechaActualizacion", (object)c.FechaActualizacion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Activo", c.Activo);

                cn.Open();
                int filas = cmd.ExecuteNonQuery();
                actualizado = filas > 0;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return actualizado;
        }

        #endregion metodos
    }
}
