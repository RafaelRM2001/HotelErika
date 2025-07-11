using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CapaEntidad;

namespace CapaDatos
{
    public class datHabitacion
    {
        private static readonly datHabitacion _instancia = new datHabitacion();
        public static datHabitacion Instancia => _instancia;

        public List<entHabitacion> Listar()
        {
            List<entHabitacion> lista = new List<entHabitacion>();

            using (SqlConnection con = Conexion.Instancia.Conectar())
            {
                SqlCommand cmd = new SqlCommand("spListarHabitaciones", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new entHabitacion
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Numero = dr["Numero"].ToString(),
                        Piso = Convert.ToInt32(dr["Piso"]),
                        Tipo = dr["Tipo"].ToString(),
                        Precio = Convert.ToDecimal(dr["Precio"]),
                        Estado = dr["Estado"].ToString(),
                        Descripcion = dr["Descripcion"].ToString(),
                        FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"]),
                        FechaActualizacion = dr["FechaActualizacion"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(dr["FechaActualizacion"]),
                        Activo = Convert.ToBoolean(dr["Activo"])
                    });
                }
            }

            return lista;
        }

        public List<entHabitacion> ListarHabitacionesDisponibles()
        {
            List<entHabitacion> lista = new List<entHabitacion>();

            using (SqlConnection cn = Conexion.Instancia.Conectar())
            {
                SqlCommand cmd = new SqlCommand("spListarHabitacionesDisponibles", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new entHabitacion
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Numero = dr["Numero"].ToString(),
                        Tipo = dr["Tipo"].ToString(),
                        Precio = Convert.ToDecimal(dr["Precio"]),
                        Estado = dr["Estado"].ToString(),
                        Descripcion = dr["Descripcion"].ToString()
                    });
                }
            }

            return lista;
        }

        public entHabitacion Buscar(int id)
        {
            entHabitacion hab = null;

            using (SqlConnection con = Conexion.Instancia.Conectar())
            {
                SqlCommand cmd = new SqlCommand("spObtenerHabitacionPorId", con) // corregido
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    hab = new entHabitacion
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Numero = dr["Numero"].ToString(),
                        Piso = Convert.ToInt32(dr["Piso"]),
                        Tipo = dr["Tipo"].ToString(),
                        Precio = Convert.ToDecimal(dr["Precio"]),
                        Estado = dr["Estado"].ToString(),
                        Descripcion = dr["Descripcion"].ToString(),
                        FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"]),
                        FechaActualizacion = dr["FechaActualizacion"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(dr["FechaActualizacion"]),
                        Activo = Convert.ToBoolean(dr["Activo"])
                    };
                }
            }

            return hab;
        }

        public bool Insertar(entHabitacion h)
        {
            using (SqlConnection con = Conexion.Instancia.Conectar())
            {
                SqlCommand cmd = new SqlCommand("spInsertarHabitacion", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@Numero", h.Numero);
                cmd.Parameters.AddWithValue("@Piso", h.Piso);
                cmd.Parameters.AddWithValue("@Tipo", h.Tipo);
                cmd.Parameters.AddWithValue("@Precio", h.Precio);
                cmd.Parameters.AddWithValue("@Estado", h.Estado);
                cmd.Parameters.AddWithValue("@Descripcion", string.IsNullOrEmpty(h.Descripcion) ? (object)DBNull.Value : h.Descripcion);
                cmd.Parameters.AddWithValue("@Activo", h.Activo);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Editar(entHabitacion h)
        {
            using (SqlConnection con = Conexion.Instancia.Conectar())
            {
                SqlCommand cmd = new SqlCommand("spActualizarHabitacion", con) // corregido
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@Id", h.Id);
                cmd.Parameters.AddWithValue("@Numero", h.Numero);
                cmd.Parameters.AddWithValue("@Piso", h.Piso);
                cmd.Parameters.AddWithValue("@Tipo", h.Tipo);
                cmd.Parameters.AddWithValue("@Precio", h.Precio);
                cmd.Parameters.AddWithValue("@Estado", h.Estado);
                cmd.Parameters.AddWithValue("@Descripcion", string.IsNullOrEmpty(h.Descripcion) ? (object)DBNull.Value : h.Descripcion);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Eliminar(int id)
        {
            using (SqlConnection con = Conexion.Instancia.Conectar())
            {
                SqlCommand cmd = new SqlCommand("spEliminarHabitacion", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        public bool ExisteNumeroHabitacion(string numero)
        {
            using (SqlConnection con = Conexion.Instancia.Conectar())
            {
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Habitaciones WHERE Numero = @Numero", con);
                cmd.Parameters.AddWithValue("@Numero", numero);

                con.Open();
                int cantidad = (int)cmd.ExecuteScalar();
                return cantidad > 0;
            }
        }

        public bool CambiarEstado(string numeroHabitacion, string nuevoEstado)
        {
            using (SqlConnection con = Conexion.Instancia.Conectar())
            {
                SqlCommand cmd = new SqlCommand("UPDATE Habitaciones SET Estado = @Estado WHERE Numero = @Numero", con);
                cmd.Parameters.AddWithValue("@Estado", nuevoEstado);
                cmd.Parameters.AddWithValue("@Numero", numeroHabitacion);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public decimal ObtenerPrecioPorNumero(string numeroHabitacion)
        {
            decimal precio = 0;
            using (SqlConnection cn = Conexion.Instancia.Conectar())
            {
                SqlCommand cmd = new SqlCommand("SELECT Precio FROM Habitaciones WHERE Numero = @Numero", cn);
                cmd.Parameters.AddWithValue("@Numero", numeroHabitacion);
                cn.Open();
                var resultado = cmd.ExecuteScalar();
                if (resultado != null)
                    precio = Convert.ToDecimal(resultado);
            }
            return precio;
        }
    }
}
