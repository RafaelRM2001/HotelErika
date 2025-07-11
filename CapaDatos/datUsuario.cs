using System;
using System.Data;
using System.Data.SqlClient;
using CapaEntidad;

namespace CapaDatos
{
    public class datUsuario
    {
        // Singleton
        private static readonly datUsuario instancia = new datUsuario();
        public static datUsuario Instancia => instancia;

        // LOGIN
        public entUsuario LoginUsuario(string email, string password)
        {
            SqlConnection cn = null;
            SqlCommand cmd = null;
            entUsuario usuario = null;

            try
            {
                cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand(@"
                    SELECT U.*, R.Nombre AS RolNombre 
                    FROM Usuarios U 
                    INNER JOIN Roles R ON U.RolId = R.Id
                    WHERE U.Email = @Email AND U.Password = @Password AND U.Activo = 1", cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    usuario = new entUsuario
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Nombre = dr["Nombre"].ToString(),
                        Apellido = dr["Apellido"].ToString(),
                        Email = dr["Email"].ToString(),
                        Password = dr["Password"].ToString(),
                        RolId = Convert.ToInt32(dr["RolId"]),
                        RolNombre = dr["RolNombre"].ToString(),
                        FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"]),
                        FechaActualizacion = dr["FechaActualizacion"] == DBNull.Value ? null : (DateTime?)dr["FechaActualizacion"],
                        Activo = Convert.ToBoolean(dr["Activo"])
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al iniciar sesión: " + ex.Message);
            }
            finally
            {
                if (cn != null) cn.Close();
            }

            return usuario;
        }

        // REGISTRAR USUARIO
        public bool InsertarUsuario(entUsuario u)
        {
            SqlConnection cn = null;
            SqlCommand cmd = null;
            bool registrado = false;

            try
            {
                cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spRegistrarUsuario", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Nombre", u.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", u.Apellido);
                cmd.Parameters.AddWithValue("@Email", u.Email);
                cmd.Parameters.AddWithValue("@Password", u.Password);
                cmd.Parameters.AddWithValue("@RolId", u.RolId);
                cmd.Parameters.AddWithValue("@DNI_RUC", u.DNI_RUC);
                cmd.Parameters.AddWithValue("@FechaRegistro", u.FechaRegistro);
                cmd.Parameters.AddWithValue("@Activo", u.Activo);

                cn.Open();
                int filas = cmd.ExecuteNonQuery();
                registrado = filas > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar usuario: " + ex.Message);
            }
            finally
            {
                if (cn != null) cn.Close();
            }

            return registrado;
        }

        // OBTENER ROL ID POR NOMBRE
        public int ObtenerRolIdPorNombre(string nombreRol)
        {
            SqlConnection cn = null;
            SqlCommand cmd = null;
            int rolId = 0;

            try
            {
                cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("spObtenerRolIdPorNombre", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Nombre", nombreRol);

                SqlParameter outputId = new SqlParameter("@RolId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputId);

                cn.Open();
                cmd.ExecuteNonQuery();

                rolId = (int)outputId.Value;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el ID del rol: " + ex.Message);
            }
            finally
            {
                if (cn != null) cn.Close();
            }

            return rolId;
        }

        // OBTENER DATOS DEL USUARIO POR ID
        public entUsuario ObtenerUsuarioPorId(int id)
        {
            entUsuario u = null;
            using (SqlConnection cn = Conexion.Instancia.Conectar())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Usuarios WHERE Id = @Id", cn);
                cmd.Parameters.AddWithValue("@Id", id);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    u = new entUsuario
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Nombre = dr["Nombre"].ToString(),
                        Apellido = dr["Apellido"].ToString(),
                        Email = dr["Email"].ToString(),
                        DNI_RUC = dr["DNI_RUC"].ToString(), 
                        RolId = Convert.ToInt32(dr["RolId"]),
                        FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"]),
                        FechaActualizacion = dr["FechaActualizacion"] == DBNull.Value ? null : Convert.ToDateTime(dr["FechaActualizacion"]),
                        Activo = Convert.ToBoolean(dr["Activo"])
                    };
                }
            }
            return u;
        }
    }
}
